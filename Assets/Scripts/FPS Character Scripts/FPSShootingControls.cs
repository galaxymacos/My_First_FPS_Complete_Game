using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FPSShootingControls : MonoBehaviour {
    [SerializeField] private Camera mainCam;

    private float nextTimeToFire;

    private FPSController FpsController;

 

    [SerializeField] private GameObject concreteImpact;
    [SerializeField] private GameObject bloodParticle;
    private AudioSource audioSource;
    internal Vector3 lastHitPosition;


    private void Start()
    {
        FpsController = GetComponent<FPSController>();
        audioSource = GetComponentsInChildren<AudioSource>()[1];
    }
    // Update is called once per frame
    void FixedUpdate() {
        Shoot();
        
    }

    private void Shoot() {
        if (FpsController.currentWeapon.bulletLeft <= 0) {
        }
        else {
            if ((FpsController.currentWeapon.name == "deagle"
                ? Input.GetMouseButtonDown(0)
                : Input.GetMouseButton(0)) && Time.time > nextTimeToFire && !FpsController.isReloading && FpsController.currentWeapon.bulletLeft >0) {
                FpsController.BulletDown();
                nextTimeToFire = Time.time + 1f / FpsController.currentWeapon.fireRate;
                RaycastHit hit;
                if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit,Mathf.Infinity)) {
  
//                    Debug.DrawRay(mainCam.transform.position, mainCam.transform.forward,Color.red,1);
                    
                    GameObject hitObject = hit.collider.transform.gameObject;
                    print("Hit: "+hitObject.name);

                    if (hitObject.CompareTag("Board")) {
                        hit.transform.gameObject.GetComponent<Animator>().SetTrigger("Hit");
                        switch (hitObject.name) {
                            case "RobotBoard":
                                LoadRobotScene();
                                break;
                            case "BossFightBoard":
                                LoadFinalBossScene();
                                break;
                        }
                    }

                    if (hitObject.CompareTag("Trophy")) {
                        hitObject.GetComponent<TrophyBehavior>().PlayTrophy();
                    }

                    EnemyTarget targetScript;
                    if (hitObject.name == "Head") {
                        print(hit.transform.gameObject.name);
                        targetScript = hit.transform.GetComponentsInParent<EnemyTarget>()[0];
                        GameObject BloodGameObject = Instantiate(bloodParticle, hit.point, Quaternion.LookRotation(hit.normal));
                        targetScript.TakeDamage(FpsController.currentWeapon.damage*2);
                        if (FpsController.currentHandsWeapon.gameObject.name == "deagle") {
                            audioSource.Play();
                        }
                    }
                    else {
                        targetScript = hit.transform.gameObject.GetComponent<EnemyTarget>();
                        if (targetScript != null)
                        {
                            Instantiate(bloodParticle, hit.point, Quaternion.LookRotation(hit.normal));
                            targetScript.TakeDamage(FpsController.currentWeapon.damage);
                        }
                        else
                        {
                            Instantiate(concreteImpact, hit.point, Quaternion.LookRotation(hit.normal));
                        }
                    }

                    
                }
            }
        }
    }

    private void LoadFinalBossScene() {
        StartCoroutine(LoadScene(2));
    }

    private void LoadRobotScene() {
        StartCoroutine(LoadScene(1));
    }

    IEnumerator LoadScene(int index) {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(index);
    }
}