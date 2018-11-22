using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Runtime;
using TMPro;
using UnityEditor;
using UnityEngine;

public class FPSController : MonoBehaviour {
    private Transform firstPersonView;

    private Transform firstPersonCamera;

    private Vector3 firstPersonViewRotation = Vector3.zero;

    [SerializeField] private float walkSpeed = 6.75f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float crouchSpeed = 4f;
    [SerializeField] private float jumpSpeed = 8f;
    [SerializeField] private float gravity = 20f;

    private float speed;

    internal bool isMoving, isGrounded, isCrouching, isReloading;

    [SerializeField] private float reloadTime = 1.5f;
    internal float reloadTimeRemain = 0f;

    private float inputX, inputY;
    private float inputXSet, inputYSet;
    private float inputModifyFactor;


    private bool limitDiagonalSpeed = true;

    private float antiBumpFactor = 0.75f;

    private CharacterController charController;
    private Vector3 moveDirection = Vector3.zero;

    public LayerMask groundLayer;
    private float rayDistance; // Detech the distance between players and ground
    private float defaultControllerHeight;
    private Vector3 defaultCamPos;
    private float camHeight;

    private FPSPlayerAnimation playerAnimation;

    [SerializeField] private TextMeshProUGUI cartridgeCapacity;
    [SerializeField] internal TextMeshProUGUI bulletLeftText;
    [SerializeField] internal TextMeshProUGUI bulletCapacity;

    private AudioSource _audioSource;

    [SerializeField] private GunCamera gunCamera;
    [SerializeField] private AudioClip[] walkingClip = new AudioClip[2];
    [SerializeField] private AudioClip runningClip;

    #region Weapon

    [SerializeField] private WeaponManager weaponManager;
    internal FPSWeapon currentWeapon;
    private float fireRate;
    private float nextTimeToFire = 0f;
    [SerializeField] private WeaponManager handsWeaponManager;
    internal FPSHandsWeapon currentHandsWeapon;

    #endregion


    // Use this for initialization
    void Start() {
        _audioSource = GetComponent<AudioSource>();
        firstPersonView = transform.Find("FPS View").transform;
        charController = GetComponent<CharacterController>();
        speed = walkSpeed;
        isMoving = false;

        rayDistance = charController.height * 0.5f + charController.radius;
        defaultControllerHeight = charController.height;
        defaultCamPos = firstPersonView.localPosition;

        playerAnimation = GetComponent<FPSPlayerAnimation>();

        weaponManager.weapons[0].SetActive(true);
        currentWeapon = weaponManager.weapons[0].GetComponent<FPSWeapon>();
        currentHandsWeapon = handsWeaponManager.weapons[0].GetComponent<FPSHandsWeapon>();
        
        handsWeaponManager.weapons[0].SetActive(true);

        _audioSource.clip = currentHandsWeapon.drawClip;
        _audioSource.Play();
    }

    // Update is called once per frame
    void Update() {
        fireRate = currentWeapon.fireRate;
        PlayerMovement();
        SelectWeapon();
     

        bulletLeftText.text = currentWeapon.bulletLeft.ToString();
        bulletLeftText.color = currentWeapon.bulletLeft <= 0 ? Color.red : Color.white;
        cartridgeCapacity.text = currentWeapon.cartridge.ToString();
        bulletCapacity.text = currentWeapon.bulletCapacity.ToString();

        if (isReloading) {
            reloadTimeRemain -= Time.deltaTime;
            if (reloadTimeRemain <= 0) {
                isReloading = false;
            }
        }

        
    }

    

    private void SelectWeapon() {
        
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            isReloading = false; // instant refresh the cooldown of gun
            if (!handsWeaponManager.weapons[0].activeInHierarchy) {
                for (int i = 0; i < handsWeaponManager.weapons.Length; i++) {
                    handsWeaponManager.weapons[i].SetActive(false);
                }

                currentHandsWeapon = null;
                handsWeaponManager.weapons[0].SetActive(true);
                currentHandsWeapon = handsWeaponManager.weapons[0].GetComponent<FPSHandsWeapon>();
                if (_audioSource.clip != currentHandsWeapon.drawClip) {
                    _audioSource.clip = currentHandsWeapon.drawClip;
                }

                _audioSource.Play();
            }

            if (!weaponManager.weapons[0].activeInHierarchy) {
                for (int i = 0; i < weaponManager.weapons.Length; i++) {
                    weaponManager.weapons[i].SetActive(false);
                }

                currentWeapon = null;
                weaponManager.weapons[0].SetActive(true);
                currentWeapon = weaponManager.weapons[0].GetComponent<FPSWeapon>();
                playerAnimation.ChangeController(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            isReloading = false; // instant refresh the cooldown of gun

            if (!handsWeaponManager.weapons[1].activeInHierarchy) {
                for (int i = 0; i < handsWeaponManager.weapons.Length; i++) {
                    handsWeaponManager.weapons[i].SetActive(false);
                }

                currentWeapon = null;
                handsWeaponManager.weapons[1].SetActive(true);
                currentHandsWeapon = handsWeaponManager.weapons[1].GetComponent<FPSHandsWeapon>();
                playerAnimation.ChangeController(false);
                if (_audioSource.clip != currentHandsWeapon.drawClip) {
                    _audioSource.clip = currentHandsWeapon.drawClip;
                }

                _audioSource.Play();
            }

            if (!weaponManager.weapons[1].activeInHierarchy) {
                for (int i = 0; i < weaponManager.weapons.Length; i++) {
                    weaponManager.weapons[i].SetActive(false);
                }

                currentWeapon = null;
                weaponManager.weapons[1].SetActive(true);
                currentWeapon = weaponManager.weapons[1].GetComponent<FPSWeapon>();
                playerAnimation.ChangeController(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            isReloading = false; // instant refresh the cooldown of gun

            if (!handsWeaponManager.weapons[2].activeInHierarchy) {
                for (int i = 0; i < handsWeaponManager.weapons.Length; i++) {
                    handsWeaponManager.weapons[i].SetActive(false);
                }

                currentHandsWeapon = null;
                handsWeaponManager.weapons[2].SetActive(true);
                currentHandsWeapon = handsWeaponManager.weapons[2].GetComponent<FPSHandsWeapon>();
                playerAnimation.ChangeController(false);
                if (_audioSource.clip != currentHandsWeapon.drawClip) {
                    _audioSource.clip = currentHandsWeapon.drawClip;
                }

                _audioSource.Play();
            }

            if (!weaponManager.weapons[2].activeInHierarchy) {
                for (int i = 0; i < weaponManager.weapons.Length; i++) {
                    weaponManager.weapons[i].SetActive(false);
                }

                currentWeapon = null;
                weaponManager.weapons[2].SetActive(true);
                currentWeapon = weaponManager.weapons[2].GetComponent<FPSWeapon>();
                playerAnimation.ChangeController(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            isReloading = true;
            reloadTimeRemain = currentWeapon.drawTime;
        }
    }

    public void BulletDown() {
        currentWeapon.bulletLeft -= 1;
    }

    enum MoveMusicState {
        None,
        PlayingFirst,
        PlayingSecond
    }

    private MoveMusicState CurrentMoveMusicState = MoveMusicState.None;
    private void PlayerMovement() {
        // Detect player key
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) {
            if (!_audioSource.isPlaying) {
                if (CurrentMoveMusicState == MoveMusicState.None) {
                    _audioSource.clip = walkingClip[0];
                    _audioSource.Play();
                    CurrentMoveMusicState = MoveMusicState.PlayingFirst;
                }
                else if (CurrentMoveMusicState == MoveMusicState.PlayingFirst)
                {
                    _audioSource.clip = walkingClip[1];
                    _audioSource.Play();
                    CurrentMoveMusicState = MoveMusicState.PlayingSecond;
                }
                else {
                    CurrentMoveMusicState = MoveMusicState.None;
                }
            }
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) {
            gunCamera.isWalking = true;
            if (Input.GetKey(KeyCode.W)) {
                inputYSet = 1f;
            }
            else {
                inputYSet = -1f;
            }
        }
        else {
            gunCamera.isWalking = false;
            inputYSet = 0f;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) {
            if (Input.GetKey(KeyCode.A)) {
                inputXSet = -1f;
            }
            else {
                inputXSet = 1f;
            }
        }
        else {
            inputXSet = 0f;
        }

        inputY = Mathf.Lerp(inputY, inputYSet, Time.deltaTime * 19f);
        inputX = Mathf.Lerp(inputX, inputXSet, Time.deltaTime * 19f);

        inputModifyFactor = Mathf.Lerp(inputModifyFactor,
            (inputYSet != 0 && inputXSet != 0 && limitDiagonalSpeed) ? 0.75f : 1.0f, Time.deltaTime * 19f);

        firstPersonViewRotation = Vector3.Lerp(firstPersonViewRotation, Vector3.zero, Time.deltaTime * 5f);
        firstPersonView.localEulerAngles = firstPersonViewRotation;

        if (isGrounded) {
            PlayerCrunchingAndSprinting();

            moveDirection = new Vector3(inputX * inputModifyFactor, -antiBumpFactor, inputY * inputModifyFactor);
            moveDirection = transform.TransformDirection(moveDirection) * speed;
            PlayerJump();
        }

        moveDirection.y -= gravity * Time.deltaTime;

        isGrounded = (CollisionFlags.Below & charController.Move(moveDirection * Time.deltaTime)) != 0;
        isMoving = charController.velocity.magnitude > 0.15f;

        HandleAnimation();
    }

    private void HandleAnimation() {
        playerAnimation.Movement(charController.velocity.magnitude);
        playerAnimation.PlayerJump(charController.velocity.y);

        if (isCrouching && charController.velocity.magnitude > 0f) {
            playerAnimation.PlayerCrouchWalk(charController.velocity.magnitude);
        }

        #region HandleShootingInformation

        if (currentWeapon.name == "deagle") {
            if (Input.GetMouseButtonDown(0) && Time.time > nextTimeToFire) {
                if (currentWeapon.bulletLeft > 0 && !isReloading) {
                    nextTimeToFire = Time.time + 1f / fireRate;
                    playerAnimation.Shoot(!isCrouching);
                    currentWeapon.Shoot();
                    currentHandsWeapon.Shoot();
                }
            }
        }
        else {
            if (Input.GetMouseButton(0) && Time.time > nextTimeToFire) {
                if (currentWeapon.bulletLeft > 0 && !isReloading) {
                    nextTimeToFire = Time.time + 1f / fireRate;
                    playerAnimation.Shoot(!isCrouching);
                    currentWeapon.Shoot();
                    currentHandsWeapon.Shoot();
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.R)) {
            if (!isReloading) {
                isReloading = true;
                reloadTimeRemain = currentWeapon.reloadTime;
                if (currentWeapon.bulletCapacity >= currentWeapon.cartridge - currentWeapon.bulletLeft) {
                    currentWeapon.bulletCapacity -= currentWeapon.cartridge - currentWeapon.bulletLeft;
                    currentWeapon.bulletLeft = currentWeapon.cartridge;
                }
                else if (currentWeapon.bulletCapacity > 0) {
                    currentWeapon.bulletLeft = currentWeapon.bulletCapacity;
                }
                else {
                    print("Ammo empty");
                }

                playerAnimation.ReloadGun();
                currentHandsWeapon.Reload();
            }
        }

        #endregion
    }

    void PlayerJump() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (isCrouching) {
                if (CanGetUp()) {
                    isCrouching = false;

                    playerAnimation.PlayerCrouch(isCrouching);
                }

                StopCoroutine(MoveCameraCrouch());
                StartCoroutine(MoveCameraCrouch());
            }
            else {
                moveDirection.y = jumpSpeed;
            }
        }
    }

    void PlayerCrunchingAndSprinting() {
        if (Input.GetKeyDown(KeyCode.C)) {
            if (!isCrouching) {
                isCrouching = true;
            }
            else {
                if (CanGetUp()) {
                    isCrouching = false;
                }
            }

            StopCoroutine(MoveCameraCrouch());
            StartCoroutine(MoveCameraCrouch());
        }

        if (isCrouching) {
            speed = crouchSpeed;
        }
        else {
            speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
            if (Input.GetKey(KeyCode.LeftShift)) {
                gunCamera.isRunning = true;
                if(_audioSource.clip!=runningClip)
                _audioSource.clip = runningClip;
                if(!_audioSource.isPlaying)
                _audioSource.Play();
            }
            else {
                if (_audioSource.clip == runningClip) {
                    _audioSource.Stop();
                    gunCamera.isRunning = false;
                }
                
            }

        }

        playerAnimation.PlayerCrouch(isCrouching);
    }

    bool CanGetUp() {
        Ray groundRay = new Ray(transform.position, transform.up);
        RaycastHit groundHit;
        if (Physics.SphereCast(groundRay, charController.radius + 0.05f, out groundHit, rayDistance, groundLayer)) {
            if (Vector3.Distance(transform.position, groundHit.point) < 2.3f) {
                return false;
            }
        }

        return true;
    }

    IEnumerator MoveCameraCrouch() {
        // Change the height of the character
        charController.height = isCrouching ? defaultControllerHeight / 1.5f : defaultControllerHeight;
        charController.center = new Vector3(0f, charController.height / 2f, 0f);

        // Change the view of the character
        camHeight = isCrouching ? defaultCamPos.y / 1.5f : defaultCamPos.y;
        while (Mathf.Abs(camHeight - firstPersonView.localPosition.y) > 0.01f) {
            firstPersonView.localPosition = Vector3.Lerp(firstPersonView.localPosition,
                new Vector3(defaultCamPos.x, camHeight, defaultCamPos.z), Time.deltaTime * 11f);
            yield return null;
        }
    }
}