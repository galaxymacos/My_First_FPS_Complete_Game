using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSPlayerAnimation : MonoBehaviour {

    private Animator anim;

    private string MOVE = "Move";
    private string VELOCITY_Y = "VelocityY";
    private string CROUCH = "Crouch";
    private string CROUCH_WALK = "CrouchWalk";

    private string STAND_SHOOT = "StandShoot";
    private string CROUCH_SHOOT = "CrouchShoot";
    private string RELOAD = "Reload";

    // Switch guns
    public RuntimeAnimatorController animControllerPistol, animControllerMachineGun; 
    
    void Awake() {
        anim = GetComponent<Animator>();
    }

	public void Movement(float magnitude) {
		anim.SetFloat(MOVE,magnitude);
	}

	public void PlayerJump(float velocity) {
		anim.SetFloat(VELOCITY_Y,velocity);
	}

	public void PlayerCrouch(bool isCrouching) {
		anim.SetBool(CROUCH,isCrouching);
	}

	public void PlayerCrouchWalk(float magnitude) {
		anim.SetFloat(CROUCH_WALK,magnitude);
	}

	public void Shoot(bool isStanding) {
		anim.SetTrigger(isStanding ? STAND_SHOOT : CROUCH_SHOOT);
	}

	public void ReloadGun() {
		anim.SetTrigger(RELOAD);
	}

	public void ChangeController(bool isPistol) {
		anim.runtimeAnimatorController = isPistol ? animControllerPistol : animControllerMachineGun;
	}
}
