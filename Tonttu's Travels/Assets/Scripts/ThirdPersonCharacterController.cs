using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCharacterController : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public float moveSpeed = 6f;
    public float turnTime = 0.1f;
    public float jumpHeight = 10f;
    public Vector3 playerVelocity;


    private float stamina = 6f;
    private bool isSprinting = false;
    float turnSmoothVelocity;

    // Start is called before the first frame update
    void Start() {
        //rb = GetComponent<Rigidbody>();
        //animator = this.GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //volume = GameObject.FindGameObjectWithTag("Camera").GetComponent<PostProcessVolume>();
        // Alert = GameObject.FindGameObjectWithTag("Alert").GetComponent<AudioSource>();
        //dashIcon.SetActive(true);
        //focusVisionIcon.SetActive(true);
        //sprintIcon.SetActive(true);
        //
    }

    // Update is called once per frame
    void Update() {
        //if (GameManager.isPaused) {
        //    return;
        //}

        PlayerMovement();


        //if (Input.GetKeyDown(KeyCode.LeftShift) && (stamina > 0)) {
        //    speed = 12;
        //    isSprinting = true;
        //    //sprintIcon.SetActive(false);

        //    //animator.SetBool("Sprint", isSprinting);
        //} else if (Input.GetKeyUp(KeyCode.LeftShift)) {
        //    speed = 7;
        //    isSprinting = false;

        //    //animator.SetBool("Sprint", isSprinting = false);
        //}

        //if (DashCooldownTime <= 0) {
        //    DashOnCooldown = false;
        //    //dashIcon.SetActive(true);
        //} else {
        //    DashCooldownTime -= Time.deltaTime;
        //    DashOnCooldown = true;
        //}

        //if (isSprinting == true) {
        //    stamina -= Time.deltaTime;
        //} else {
        //    stamina += Time.deltaTime;
        //}

        //if (stamina > 6) {
        //    stamina = 6;
        //    //sprintIcon.SetActive(true);
        //}

        //if (stamina < 0) {
        //    Speed = 7;
        //    isSprinting = false;
        //}


        //if (walkingBackwards) {
        //    Speed = 4;
        //} else if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKey("w")) {
        //    Speed = 7;
        //}

        //CheckForJump();
    }

    //void CheckForJump() {
    //    if (Input.GetKeyDown("space") && (onGround || MAX_JUMP > currentJump)) {
    //        applyJumpForce = true;
    //        onGround = false;

    //        if (currentJump < MAX_JUMP && currentJump > 0) {
    //            //animator.SetBool("Double Jump", true);
    //        }

    //        currentJump++;

    //    }
    //    if (currentJump <= 1) {
    //        //animator.SetBool("Double Jump", false);
    //    }
    //}

    //void FixedUpdate() {
    //    if (applyJumpForce) {
    //        applyJumpForce = false;
    //        rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
    //    }
    //}

    void PlayerMovement() {
        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");


        //var playerVelocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        //if (controller.isGrounded) {
        //    playerVelocity = this.gameObject.transform.TransformDirection(playerVelocity);
        //    if (playerVelocity.y > 0) {
        //        playerVelocity.y = 0;
        //    }

        //    if (Input.GetButtonDown("Jump")) {
        //        playerVelocity.y = jumpHeight;
        //    }
        //    controller.Move(moveSpeed * Time.deltaTime * playerVelocity);
        //}

        //playerVelocity.y += Physics.gravity.y * Time.deltaTime;
        //controller.Move(moveSpeed * Time.deltaTime * playerVelocity);

        var inputDir = new Vector3(hor, 0f, ver).normalized;
        if (inputDir.magnitude >= 0.1f) {
            float inputAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, inputAngle, ref turnSmoothVelocity, turnTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, inputAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }

        if (controller.isGrounded) {
            //if (inputDir.y > 0) {
            //    inputDir.y = 0;
            //}

            if (Input.GetButtonDown("Jump")) {
                inputDir.y = jumpHeight;
            }
            controller.Move(moveSpeed * Time.deltaTime * inputDir);
        } else {
            var fall = Vector3.zero;
            //fall.y = 
        }

    }

    //void OnCollisionEnter(Collision collision) {
    //    if (collision.gameObject.CompareTag("ground")) {
    //        onGround = true;
    //        currentJump = 0;
    //    }
    //}
}