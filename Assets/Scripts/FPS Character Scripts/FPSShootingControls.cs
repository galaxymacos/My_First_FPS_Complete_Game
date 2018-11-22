using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FPSShootingControls : MonoBehaviour {
    [SerializeField] private Camera mainCam;

    private float nextTimeToFire;

    private FPSController FpsController;

    private void Start() {
        FpsController = GetComponent<FPSController>();
    }

    [SerializeField] private GameObject concreteImpact;

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
                : Input.GetMouseButton(0)) && Time.time > nextTimeToFire && !FpsController.isReloading) {
                FpsController.BulletDown();
                nextTimeToFire = Time.time + 1f / FpsController.currentWeapon.fireRate;
                RaycastHit hit;
                if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit,Mathf.Infinity)) {
  
//                    Debug.DrawRay(mainCam.transform.position, mainCam.transform.forward,Color.red,1);
                    Instantiate(concreteImpact, hit.point, Quaternion.LookRotation(hit.normal));
                    GameObject hitObject = hit.transform.gameObject;
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

                    var targetScript = hit.transform.gameObject.GetComponent<EnemyTarget>();
                    if (targetScript != null) {
                        targetScript.TakeDamage(FpsController.currentWeapon.damage);
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