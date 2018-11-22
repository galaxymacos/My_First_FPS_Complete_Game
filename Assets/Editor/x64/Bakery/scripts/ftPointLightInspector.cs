
using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.Rendering;

[CustomEditor(typeof(BakeryPointLight))]
[CanEditMultipleObjects]
public class ftPointLightInspector : UnityEditor.Editor
{
    SerializedProperty ftraceLightColor;
    SerializedProperty ftraceLightIntensity;
    SerializedProperty ftraceLightShadowSpread;
    SerializedProperty ftraceLightCutoff;
    SerializedProperty ftraceLightSamples;
    SerializedProperty ftraceLightProj;
    SerializedProperty ftraceLightTexture;
    SerializedProperty ftraceLightTexture2D;
    SerializedProperty ftraceLightAngle;
    SerializedProperty ftraceLightIES;
    SerializedProperty ftraceLightBitmask;
    SerializedProperty ftraceLightBakeToIndirect;
    SerializedProperty ftraceLightRealisticFalloff;
    SerializedProperty ftraceLightShadowmask;
    SerializedProperty ftraceLightIndirectIntensity;

    UnityEngine.Object spotCookieTexture;

    static string[] selStrings = new string[] {"0","1","2","3","4","5","6","7","8","9","10","11","12","13","14","15","16",
                                                "17","18","19","20","21","22","23","24","25","26","27","28","29","30"};//,"31"};

    void InitSerializedProperties(SerializedObject obj)
    {
        ftraceLightColor = obj.FindProperty("color");
        ftraceLightIntensity = obj.FindProperty("intensity");
        ftraceLightIndirectIntensity = obj.FindProperty("indirectIntensity");
        ftraceLightShadowSpread = obj.FindProperty("shadowSpread");
        ftraceLightCutoff = obj.FindProperty("cutoff");
        ftraceLightAngle = obj.FindProperty("angle");
        ftraceLightSamples = obj.FindProperty("samples");
        ftraceLightProj = obj.FindProperty("projMode");
        ftraceLightTexture = obj.FindProperty("cubemap");
        ftraceLightTexture2D = obj.FindProperty("cookie");
        ftraceLightIES = obj.FindProperty("iesFile");
        ftraceLightBitmask = obj.FindProperty("bitmask");
        ftraceLightBakeToIndirect = obj.FindProperty("bakeToIndirect");
        ftraceLightRealisticFalloff = obj.FindProperty("realisticFalloff");
        ftraceLightShadowmask = obj.FindProperty("shadowmask");
    }

    void OnEnable()
    {
        InitSerializedProperties(serializedObject);

        if (spotCookieTexture == null)
        {
            spotCookieTexture = AssetDatabase.LoadAssetAtPath("Assets/Bakery/ftUnitySpotTexture.bmp", typeof(Texture2D));
        }
    }

    void GetLinearLightParameters(Light light, out float lightR, out float lightG, out float lightB, out float lightInt)
    {
        if (PlayerSettings.colorSpace != ColorSpace.Linear)
        {
            lightInt = light.intensity;
            lightR = light.color.r;
            lightG = light.color.g;
            lightB = light.color.b;
            return;
        }

        if (!GraphicsSettings.lightsUseLinearIntensity)
        {
            lightR = Mathf.Pow(light.color.r * light.intensity, 2.2f);
            lightG = Mathf.Pow(light.color.g * light.intensity, 2.2f);
            lightB = Mathf.Pow(light.color.b * light.intensity, 2.2f);
            lightInt = Mathf.Max(Mathf.Max(lightR, lightG), lightB);
            lightR /= lightInt;
            lightG /= lightInt;
            lightB /= lightInt;
        }
        else
        {
            lightInt = light.intensity;
            lightR = light.color.linear.r;
            lightG = light.color.linear.g;
            lightB = light.color.linear.b;
        }
    }

    public override void OnInspectorGUI() {
        //if (showFtrace)
        {
            OnEnable();

            serializedObject.Update();

            EditorGUILayout.PropertyField(ftraceLightColor, new GUIContent("Color", "Color of the light"));
            EditorGUILayout.PropertyField(ftraceLightIntensity, new GUIContent("Intensity", "Color multiplier"));
            EditorGUILayout.PropertyField(ftraceLightShadowSpread, new GUIContent("Shadow spread", "Controls shadow blurriness from 0 to 1"));
            EditorGUILayout.PropertyField(ftraceLightRealisticFalloff, new GUIContent("Physical falloff", "Use inverse-squared falloff instead of Unity falloff"));
            EditorGUILayout.PropertyField(ftraceLightCutoff, new GUIContent("Range", "Lighting distance limit. When 'Physical falloff' is on, for maximum corectness set to a very high value. Using smaller values is useful for faster render times and to match real-time lights. Bakery uses Skyforge falloff to maintain balance between correct inverse-squared attenuation and practical limits (https://habr.com/company/mailru/blog/248873/)"));
            EditorGUILayout.PropertyField(ftraceLightSamples, new GUIContent("Samples", "The amount of sample points generated on the surface of this light. Point light shadows are traced towards points on a sphere (with radius = shadowSpread) around the light. "));
            EditorGUILayout.PropertyField(ftraceLightProj, new GUIContent("Projection mask", "Determines additional light masking mode."));

            switch(ftraceLightProj.enumValueIndex)
            {
                case (int)BakeryPointLight.ftLightProjectionMode.Cookie:
                    EditorGUILayout.PropertyField(ftraceLightTexture2D, new GUIContent("Cookie texture", "Texture"));
                    EditorGUILayout.Slider(ftraceLightAngle, 1, 179, new GUIContent("Angle", "Angle of projection (like in spotlights)."));
                    break;
                case (int)BakeryPointLight.ftLightProjectionMode.Cubemap:
                    EditorGUILayout.PropertyField(ftraceLightTexture, new GUIContent("Projected cubemap", "Cubemap"));
                    break;
                case (int)BakeryPointLight.ftLightProjectionMode.IES:
                    ftraceLightIES.objectReferenceValue = EditorGUILayout.ObjectField("IES file", ftraceLightIES.objectReferenceValue, typeof(UnityEngine.Object), false);
                    if (ftraceLightIES.objectReferenceValue != null)
                    {
                        var path = AssetDatabase.GetAssetPath(ftraceLightIES.objectReferenceValue);
                        if (path.Length < 4 || path.Substring(path.Length - 4).ToLower() != ".ies")
                        {
                            EditorUtility.DisplayDialog("Bakery", "File must have IES extension.", "OK");
                            ftraceLightIES.objectReferenceValue = null;
                        }
                    }
                    break;
                default:
                    break;
            }

            int prevVal = ftraceLightBitmask.intValue;
            int newVal = EditorGUILayout.MaskField(new GUIContent("Bitmask", "Lights only affect renderers with overlapping bits"), ftraceLightBitmask.intValue, selStrings);
            if (prevVal != newVal) ftraceLightBitmask.intValue = newVal;

            EditorGUILayout.PropertyField(ftraceLightBakeToIndirect, new GUIContent("Bake to indirect", "Add direct contribution from this light to indirect-only lightmaps"));
            if (ftraceLightBakeToIndirect.boolValue && ftraceLightShadowmask.boolValue) ftraceLightShadowmask.boolValue = false;

            EditorGUILayout.PropertyField(ftraceLightShadowmask, new GUIContent("Shadowmask", "Enable mixed lighting. Static shadows from this light will be baked, and real-time light will cast shadows from dynamic objects."));
            if (ftraceLightBakeToIndirect.boolValue && ftraceLightShadowmask.boolValue) ftraceLightBakeToIndirect.boolValue = false;

            EditorGUILayout.PropertyField(ftraceLightIndirectIntensity, new GUIContent("Indirect intensity", "Non-physical GI multiplier for this light"));

            serializedObject.ApplyModifiedProperties();
        }


        bool showWarningCant = false;
        bool showError = false;
        string why = "";

        bool shadowmaskNoDynamicLight = false;

        foreach(BakeryPointLight selectedLight in targets)
        {
            bool match = true;
            //string why = "";
            var light = selectedLight.GetComponent<Light>();
            if (light == null)
            {
                if (ftraceLightShadowmask.boolValue) shadowmaskNoDynamicLight = true;
                continue;
            }
            if (!light.enabled)
            {
                if (ftraceLightShadowmask.boolValue) shadowmaskNoDynamicLight = true;
            }

            var so = new SerializedObject(selectedLight);
            InitSerializedProperties(so);

            if (ftraceLightIndirectIntensity.floatValue != light.bounceIntensity)
            {
                match = false;
                why = "indirect intensity doesn't match";
            }

            if (ftraceLightProj.enumValueIndex == (int)BakeryPointLight.ftLightProjectionMode.IES)
            {
                showWarningCant = true;
            }
            else if (ftraceLightProj.enumValueIndex == (int)BakeryPointLight.ftLightProjectionMode.Omni)
            {
                if (light.type != LightType.Point)
                {
                    match = false;
                    why = "real-time light is not point";
                }
                else if (light.cookie != null)
                {
                    match = false;
                    why = "real-time light has cookie";
                }
            }
            else if (ftraceLightProj.enumValueIndex == (int)BakeryPointLight.ftLightProjectionMode.Cubemap)
            {
                if (light.type != LightType.Point)
                {
                    match = false;
                    why = "real-time light is not point";
                }
                else if (light.cookie == null)
                {
                    match = false;
                    why = "real-time light has no cookie";
                }
            }
            else if (ftraceLightProj.enumValueIndex == (int)BakeryPointLight.ftLightProjectionMode.Cookie)
            {
                if (light.type != LightType.Spot)
                {
                    match = false;
                    why = "real-time light is not spot";
                }
                else if (light.cookie == null && ftraceLightTexture2D.objectReferenceValue != spotCookieTexture)
                {
                    match = false;
                    why = "wrong cookie texture";
                }
                else if (light.cookie != null && ftraceLightTexture2D.objectReferenceValue != light.cookie)
                {
                    match = false;
                    why = "wrong cookie texture";
                }
                else if (light.spotAngle != ftraceLightAngle.floatValue)
                {
                    match = false;
                    why = "spot angle doesn't match";
                }
            }

            var clr = ftraceLightColor.colorValue;
            float eps = 1.0f / 255.0f;
            float lightR, lightG, lightB, lightInt;
            float fr, fg, fb;
            float fintensity = ftraceLightIntensity.floatValue;
            if (PlayerSettings.colorSpace == ColorSpace.Linear)
            {
                fr = clr.linear.r;// * fintensity;
                fg = clr.linear.g;// * fintensity;
                fb = clr.linear.b;// * fintensity;
            }
            else
            {
                fr = clr.r;
                fg = clr.g;
                fb = clr.b;
            }
            GetLinearLightParameters(light, out lightR, out lightG, out lightB, out lightInt);

            if (GraphicsSettings.lightsUseLinearIntensity || PlayerSettings.colorSpace != ColorSpace.Linear)
            {
                if (Mathf.Abs(lightR - fr) > eps || Mathf.Abs(lightG - fg) > eps || Mathf.Abs(lightB - fb) > eps)
                {
                    match = false;
                    why = "color doesn't match";
                }
                else if (Mathf.Abs(lightInt - fintensity) > eps)
                {
                    match = false;
                    why = "intensity doesn't match";
                }
            }
            else
            {
                eps *= Mathf.Max(lightInt, fintensity);
                if (Mathf.Abs(lightR*lightInt - fr*fintensity) > eps ||
                    Mathf.Abs(lightG*lightInt - fg*fintensity) > eps ||
                    Mathf.Abs(lightB*lightInt - fb*fintensity) > eps)
                {
                    match = false;
                    why = "intensity doesn't match";
                }
            }

            if (Mathf.Abs(light.range - ftraceLightCutoff.floatValue) > 0.001f)
            {
                match = false;
                why = "range doesn't match";
            }

            if (!match)
            {
                showError = true;
            }
        }

        if (shadowmaskNoDynamicLight)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Warning: shadowmask needs enabled real-time light to work");
        }

        if (showWarningCant)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Warning: real-time light can't match baked IES light");
            EditorGUILayout.Space();
        }

        if (showError)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Real-time light doesn't match lightmap: " + why);

            if (GUILayout.Button("Match lightmapped to real-time"))
            {
                foreach(BakeryPointLight selectedLight in targets)
                {
                    var light = selectedLight.GetComponent<Light>();
                    if (light == null) continue;
                    //if (!light.enabled) continue;
                    var so = new SerializedObject(selectedLight);
                    InitSerializedProperties(so);

                    if (PlayerSettings.colorSpace != ColorSpace.Linear)
                    {
                        ftraceLightColor.colorValue = light.color;
                        ftraceLightIntensity.floatValue = light.intensity;
                    }
                    else if (!GraphicsSettings.lightsUseLinearIntensity)
                    {
                        float lightR, lightG, lightB, lightInt;
                        GetLinearLightParameters(light, out lightR, out lightG, out lightB, out lightInt);
                        ftraceLightColor.colorValue = new Color(lightR, lightG, lightB);
                        ftraceLightIntensity.floatValue = lightInt;
                    }
                    else
                    {
                        ftraceLightColor.colorValue = light.color;
                        ftraceLightIntensity.floatValue = light.intensity;
                    }
                    ftraceLightCutoff.floatValue = light.range;
                    ftraceLightAngle.floatValue = light.spotAngle;

                    if (light.type == LightType.Point)
                    {
                        if (light.cookie == null)
                        {
                            ftraceLightProj.enumValueIndex = (int)BakeryPointLight.ftLightProjectionMode.Omni;
                            ftraceLightTexture.objectReferenceValue = null;
                        }
                        else
                        {
                            ftraceLightProj.enumValueIndex = (int)BakeryPointLight.ftLightProjectionMode.Cubemap;
                            ftraceLightTexture.objectReferenceValue = light.cookie;
                        }
                    }
                    else if (light.type == LightType.Spot)
                    {
                        ftraceLightProj.enumValueIndex = (int)BakeryPointLight.ftLightProjectionMode.Cookie;
                        if (light.cookie == null)
                        {
                            ftraceLightTexture2D.objectReferenceValue = spotCookieTexture;
                        }
                        else
                        {
                            ftraceLightTexture2D.objectReferenceValue = light.cookie;
                        }
                    }
                    ftraceLightIndirectIntensity.floatValue = light.bounceIntensity;

                    so.ApplyModifiedProperties();
                }
            }
            if (GUILayout.Button("Match real-time to lightmapped"))
            {
                foreach(BakeryPointLight selectedLight in targets)
                {
                    var light = selectedLight.GetComponent<Light>();
                    if (light == null) continue;
                    //if (!light.enabled) continue;
                    var so = new SerializedObject(selectedLight);
                    InitSerializedProperties(so);

                    Undo.RecordObject(light, "Change light");
                    if (PlayerSettings.colorSpace != ColorSpace.Linear)
                    {
                        light.color = ftraceLightColor.colorValue;
                        light.intensity = ftraceLightIntensity.floatValue;
                    }
                    else if (!GraphicsSettings.lightsUseLinearIntensity)
                    {
                        var clr = ftraceLightColor.colorValue;
                        float fintensity = ftraceLightIntensity.floatValue;
                        float fr = clr.linear.r;// * fintensity;
                        float fg = clr.linear.g;// * fintensity;
                        float fb = clr.linear.b;// * fintensity;

                        fr = Mathf.Pow(fr * fintensity, 1.0f / 2.2f);
                        fg = Mathf.Pow(fg * fintensity, 1.0f / 2.2f);
                        fb = Mathf.Pow(fb * fintensity, 1.0f / 2.2f);
                        float fint = Mathf.Max(Mathf.Max(fr, fg), fb);
                        fr /= fint;
                        fg /= fint;
                        fb /= fint;
                        light.color = new Color(fr, fg, fb);
                        light.intensity = fint;
                    }
                    else
                    {
                        light.color = ftraceLightColor.colorValue;
                        light.intensity = ftraceLightIntensity.floatValue;
                    }
                    light.range = ftraceLightCutoff.floatValue;
                    light.spotAngle = ftraceLightAngle.floatValue;

                    if (ftraceLightProj.enumValueIndex == (int)BakeryPointLight.ftLightProjectionMode.Omni)
                    {
                        light.type = LightType.Point;
                        light.cookie = null;
                    }
                    else if (ftraceLightProj.enumValueIndex == (int)BakeryPointLight.ftLightProjectionMode.Cubemap)
                    {
                        light.type = LightType.Point;
                        light.cookie = ftraceLightTexture.objectReferenceValue as Cubemap;
                    }
                    else if (ftraceLightProj.enumValueIndex == (int)BakeryPointLight.ftLightProjectionMode.Cookie)
                    {
                        light.type = LightType.Spot;
                        light.cookie = ftraceLightTexture.objectReferenceValue == spotCookieTexture ? null : (ftraceLightTexture.objectReferenceValue as Texture2D);
                    }
                    light.bounceIntensity = ftraceLightIndirectIntensity.floatValue;
                }
            }
        }

        if (PlayerSettings.colorSpace == ColorSpace.Linear)
        {
            if (!GraphicsSettings.lightsUseLinearIntensity)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Warning: project is not set up to use linear light intensity.");
                EditorGUILayout.LabelField("GraphicsSettings.lightsUseLinearIntensity should be TRUE.");
                if (GUILayout.Button("Fix"))
                {
                    GraphicsSettings.lightsUseLinearIntensity = true;
                }
            }
            else
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Project is using linear light intensity. This is nice.");
                if (GUILayout.Button("Change to non-linear"))
                {
                    GraphicsSettings.lightsUseLinearIntensity = false;
                }
            }
        }
    }
}


