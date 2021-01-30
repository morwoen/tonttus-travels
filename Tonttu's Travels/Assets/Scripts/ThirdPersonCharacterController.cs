using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Rendering.PostProcessing;
//using UnityEngine.Audio;

public class ThirdPersonCharacterController : MonoBehaviour
{

    public float forwardSpeed = 7;
    public float sprintSpeed = 12;
    public float backwardSpeed = 4;
    private float internalSpeed = 4;
    public float dashTime = 2;
    public float DashSpeed = 2;
    public float jumpSpeed = 5;
    public float turnTime = 0.1f;
    private Rigidbody rb;
    private bool onGround = true;
    private const int MAX_JUMP = 2;
    private int currentJump = 0;
    private bool DashOnCooldown = false;
    public float dashCountdownTime = 6;
    private float dashCurrentCountdown = 6;
    //public Animator animator;
    //public GameObject stealthVisualEffect;
    private bool isSprinting = false;
    public float Stamina = 6;
    public float MaxStamina = 6;
    //public GameObject dashIcon;
    //public GameObject focusVisionIcon;
    //public GameObject sprintIcon;

    //public bool StealthActive = false;
    bool walkingBackwards = false;
    private bool applyJumpForce = false;

    public GameObject cam;

    //public PostProcessVolume volume;

    //public float Vison = 6;
   // public bool visonCooldown = false;

    public GameObject playerShot;
    public bool playerHit;
    //public AudioSource Alert;

    private float turnSmoothVelocity;

    // Start is called before the first frame update
    void Start()
    {
        dashCurrentCountdown = dashCountdownTime;
        rb = GetComponentInChildren<Rigidbody>();
        //animator = this.GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //volume = GameObject.FindGameObjectWithTag("Camera").GetComponent<PostProcessVolume>();
        //Alert = GameObject.FindGameObjectWithTag("Alert").GetComponent<AudioSource>();
        //dashIcon.SetActive(true);
        //focusVisionIcon.SetActive(true);
        //sprintIcon.SetActive(true);
        //
    }

    public void StopDashVelocity()
    {
        rb.velocity = Vector3.zero;
        Debug.Log("Dashed");
    }

    // Update is called once per frame
    void Update()
    {
        if(playerHit == true)
        {
            playerShot.SetActive(true);
        }

        //if (GameManager.isPaused)
        //{
        //    return;
        //}

        PlayerMovement();
        //Animations();

        if (Input.GetKeyDown("e") && Input.GetKey("w"))
        {
            Debug.Log("keydown");


            if (DashOnCooldown == false && onGround == true)
            {
                rb.AddForce (transform.forward * DashSpeed, ForceMode.Impulse);
                dashCurrentCountdown = dashCountdownTime;
                Invoke("StopDashVelocity", dashTime);

                Debug.Log("Dashed");
                //animator.SetTrigger("Dash");
                //dashIcon.SetActive(false);

            }
        }

        if (Input.GetKeyDown("e") && Input.GetKey("s"))
        {
            Debug.Log("keydown");


            if (DashOnCooldown == false && onGround == true)
            {
                rb.AddForce(-transform.forward * DashSpeed, ForceMode.Impulse);
                dashCurrentCountdown = dashCountdownTime;
                Invoke("StopDashVelocity", dashTime);

                Debug.Log("Dashed");
                //animator.SetTrigger("Dash");
                //dashIcon.SetActive(false);
            }
        }



        if (Input.GetKeyDown(KeyCode.LeftShift) && (Stamina > 0))
        {
            internalSpeed = sprintSpeed;
            isSprinting = true;
            //sprintIcon.SetActive(false);

            //animator.SetBool("Sprint", isSprinting);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            internalSpeed = forwardSpeed;
            isSprinting = false;

            //animator.SetBool("Sprint",isSprinting = false);
        }

        if (dashCurrentCountdown <= 0)
        {
            DashOnCooldown = false;
            //dashIcon.SetActive(true);
        }
        else
        {
            dashCurrentCountdown -= Time.deltaTime;
            DashOnCooldown = true;
        }

        if (isSprinting == true)
        {
            Stamina -= Time.deltaTime;
            
        }
        else
        {
            Stamina += Time.deltaTime;
        }

        if (Stamina > MaxStamina)
        {
            Stamina = MaxStamina;
            //sprintIcon.SetActive(true);
        }

        if(Stamina < 0)
        {
            internalSpeed = forwardSpeed;
            isSprinting = false;
            //sprintIcon.SetActive(true);
        }
        


       // print(DashCooldownTime);

        //if(Input.GetKeyDown(KeyCode.C)) 
        //{
        //    StealthActive = !StealthActive;
        //    //if (stealthVisualEffect != null)
        //        //stealthVisualEffect.SetActive(StealthActive);
        //}

       
        //if(StealthActive == true)
        //{
        //    Speed = 2;
        //}

        if(walkingBackwards)
        {
            internalSpeed = backwardSpeed;
        }
        else if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKey("w"))
        {
            internalSpeed = forwardSpeed;
        }

        CheckForJump();
    }

    void CheckForJump()
    {
        if (Input.GetKeyDown("space") && (onGround || MAX_JUMP > currentJump))
        {
            applyJumpForce = true;
            onGround = false;

            if (currentJump < MAX_JUMP && currentJump > 0)
            {
                //animator.SetBool("Double Jump", true);
            }
            
            currentJump++;

        }
        if (currentJump <= 1)
        {
            //animator.SetBool("Double Jump", false);
        }
    }

    void FixedUpdate()
    {
        if (applyJumpForce)
        {
            applyJumpForce = false;
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        }
    }

    void PlayerMovement()
    {
        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");

        if (ver < 0) walkingBackwards = true;
        else walkingBackwards = false;

        var inputDir = new Vector3(hor, 0f, ver).normalized;
        if (inputDir.magnitude >= 0.1f) {
            float inputAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, inputAngle, ref turnSmoothVelocity, turnTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, inputAngle, 0f) * Vector3.forward;
            rb.MovePosition(transform.position + moveDir.normalized * internalSpeed * Time.deltaTime);
        }
    }

    //void Animations()
    //{

    //    animator.SetBool("OnGround", onGround);
    //    bool isMoving = (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0  || Input.GetAxis("Vertical") > 0 || Input.GetAxis("Vertical") < 0);
    //    animator.SetBool("isMoving", isMoving);

    //    animator.SetFloat("VelX", Input.GetAxis("Horizontal"));
    //    animator.SetFloat("VelY", Input.GetAxis("Vertical"));

    //    animator.SetFloat("Speed", Speed);

    //    animator.SetBool("Stealth", StealthActive);

    //    /*
    //    if(rb.velocity.x > 0)
    //        animator.SetFloat("VelX", 1);
    //    else if (rb.velocity.x < 0)
    //        animator.SetFloat("VelX", -1);
    //    else
    //        animator.SetFloat("VelX", 0);

    //    if (rb.velocity.z > 0)
    //        animator.SetFloat("VelY", 1);
    //    else if (rb.velocity.z < 0)
    //        animator.SetFloat("VelY", -1);
    //    else
    //        animator.SetFloat("VelY", 0);
    //        */
    //}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            onGround = true;
            currentJump = 0;
        }
    }
}
