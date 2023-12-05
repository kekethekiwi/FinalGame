using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private ParticleSystem dustPFX;
    public float speed;
    public float rotateSpeed = 2f;

    private Animator animator;
    private InputAction moveAction;
    private InputAction runAction;
    private float speedMultiplier = 1f;
    private RaycastHit raycast;
    private bool isClimbable;
    private float sphereCastRadius = .25f;
    private float castLength = .7f;
    public LayerMask layerMask;
    private float climbableLookAtAngle;
    private bool isClimbing = false;
    private state currentState = state.idle;

    //private Quaternion lastRotation = Quaternion.identity;
    //public float jumpVelocity;
    // six  can *idle, *jump, *walk, run, push, crouchwalk, climb wall
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        moveAction = playerInput.actions["NewMove"];
        runAction = playerInput.actions["Run"];
    }

    private void OnEnable()
    {
        if (moveAction!= null)
        {
            moveAction.Enable();
        }

        if (runAction != null)
        {
            runAction.Enable();
        }

    }

    private void OnDisable()
    {
        if (moveAction != null)
        {
            moveAction.Disable();
        }

        if (runAction != null)
        {
            runAction.Disable();
        }
    }

    enum state
    {
        idle,
        jump,
        move,
        climb

    }

    void Update()
    {
        // climb
        if (animator != null) animator.SetBool("isClimbing", isClimbing);

        switch (currentState) {
            case state.idle:
                {
                    Vector3 moveInput = moveAction.ReadValue<Vector3>();
                    if (moveInput != Vector3.zero)
                    {
                        rb.freezeRotation = false;
                        currentState = moveInput.y > 0f ? state.jump:state.move;
                    }
                    else
                    {
                        if (animator != null) animator.SetFloat("speed", 0f);
                        rb.freezeRotation = true;
                    }

                    break;
                }
                
            case state.move:
                {
                    Vector3 moveInput = moveAction.ReadValue<Vector3>();
                    if (moveInput == Vector3.zero)
                    {
                        currentState = state.idle;
                    }
                    else
                    {
                        // get angle and rotation
                        float angle = Mathf.Atan2(moveInput.x, moveInput.z) * Mathf.Rad2Deg;
                        Quaternion targetRotation = Quaternion.Euler(0, angle, 0);

                        //rotate in direction of movement
                        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotateSpeed * Time.deltaTime));

                        // move left and right
                        Vector3 moveDir = new Vector3(moveInput.x * speed * speedMultiplier, 0f, moveInput.z * speed * speedMultiplier);
                        rb.AddForce(moveDir);
                        if (animator != null) animator.SetFloat("speed", speed * speedMultiplier);

                        if (moveInput.y > 0f) currentState = state.jump;
                    }
                    break;
                }
            case state.jump:
                {
                    // todo: prevent double-jumping
                    Vector3 moveInput = moveAction.ReadValue<Vector3>();
                    if (moveInput.y > 0f)
                    {
                        rb.AddForce(new Vector3(0f, moveInput.y), ForceMode.Impulse);
                        if (animator != null) animator.SetTrigger("jump");
                        GameManager.ShakeTheCamera(.03f, .03f);
                    }
                    else if (moveInput.x > 0f || moveInput.y > 0f)
                    {
                        currentState = state.move;
                        
                    }
                    else
                    {
                        currentState = state.idle;
                    }
                    //if (animator != null) animator.ResetTrigger("jump");
                    break;
                }
            case state.climb:
                {
                    if (!Input.GetKeyDown(KeyCode.H))
                    {
                        currentState = state.idle;
                    }

                    // maybe make gravity unchecked
                    break;
                }


        }

        // IGNORE Test Code
        //transform.Translate(moveInput.x * speed * Time.deltaTime, 0f, moveInput.y * speed * Time.deltaTime, Space.World);
    }

    private void FixedUpdate()
    {
        //Vector3 moveInput = moveAction.ReadValue<Vector3>();

        //if (moveInput != Vector3.zero)
        //{
        //    rb.freezeRotation = false;
        //    PlayPFX(dustPFX);

        //    // get angle and rotation
        //    float angle = Mathf.Atan2(moveInput.x, moveInput.z) * Mathf.Rad2Deg;
        //    Quaternion targetRotation = Quaternion.Euler(0, angle, 0);

        //    //rotate in direction of movement
        //    rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotateSpeed * Time.deltaTime));

        //    // move left and right
        //    Vector3 moveDir = new Vector3(moveInput.x * speed * speedMultiplier, 0f, moveInput.z * speed * speedMultiplier);
        //    rb.AddForce(moveDir);
        //    if (animator != null) animator.SetFloat("speed", speed * speedMultiplier);

        //    //jump
        //    // todo: prevent double-jumping
        //    rb.AddForce(new Vector3(0f, moveInput.y), ForceMode.Impulse);
        //    if (moveInput.y > 0f)
        //    {
        //        if (animator != null) animator.SetTrigger("jump");
        //        GameManager.ShakeTheCamera(.03f, .03f);
        //    }
        //    if (animator != null) animator.ResetTrigger("jump");

        //}
        //else
        //{
        //    if (animator != null) animator.SetFloat("speed", 0f);
        //    rb.freezeRotation = true;
        //}


        // climb
        if (animator != null) animator.SetBool("isClimbing", isClimbing);

        switch (currentState)
        {
            case state.idle:
                {
                    Vector3 moveInput = moveAction.ReadValue<Vector3>();
                    if (moveInput != Vector3.zero)
                    {
                        rb.freezeRotation = false;
                        currentState = moveInput.y > 0f ? state.jump : state.move;
                    }
                    else
                    {
                        if (animator != null) animator.SetFloat("speed", 0f);
                        rb.freezeRotation = true;
                    }

                    break;
                }

            case state.move:
                {
                    Vector3 moveInput = moveAction.ReadValue<Vector3>();
                    if (moveInput == Vector3.zero)
                    {
                        currentState = state.idle;
                    }
                    else
                    {
                        // get angle and rotation
                        float angle = Mathf.Atan2(moveInput.x, moveInput.z) * Mathf.Rad2Deg;
                        Quaternion targetRotation = Quaternion.Euler(0, angle, 0);

                        //rotate in direction of movement
                        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotateSpeed * Time.deltaTime));

                        // move left and right
                        Vector3 moveDir = new Vector3(moveInput.x * speed * speedMultiplier, 0f, moveInput.z * speed * speedMultiplier);
                        rb.AddForce(moveDir);
                        if (animator != null) animator.SetFloat("speed", speed * speedMultiplier);

                        // pfx
                        PlayPFX(dustPFX);

                        if (moveInput.y > 0f) currentState = state.jump;
                    }
                    break;
                }
            case state.jump:
                {
                    // todo: prevent double-jumping
                    Vector3 moveInput = moveAction.ReadValue<Vector3>();
                    if (moveInput.y > 0f)
                    {
                        rb.AddForce(new Vector3(0f, moveInput.y), ForceMode.Impulse);
                        if (animator != null) animator.SetTrigger("jump");
                        GameManager.ShakeTheCamera(.03f, .03f);
                    }
                    else if (moveInput.x > 0f || moveInput.y > 0f)
                    {
                        currentState = state.move;

                    }
                    else
                    {
                        currentState = state.idle;
                    }
                    //if (animator != null) animator.ResetTrigger("jump");
                    break;
                }
            case state.climb:
                {
                    if (!Input.GetKeyDown(KeyCode.H))
                    {
                        currentState = state.idle;
                    }


                    break;
                }
        }

    }

    private bool CheckClimbable()
    {
        isClimbable = Physics.SphereCast(transform.position, sphereCastRadius, transform.forward, out raycast, castLength, layerMask);
        Debug.Log("onClimb " + isClimbable);
        climbableLookAtAngle = Vector3.Angle(transform.forward, -raycast.normal);
        return (isClimbable && climbableLookAtAngle < 30f);
    }
    public void OnClimb(InputAction.CallbackContext aContext)
    {
        // climb - hold h + arrow key to navigate
        Vector3 moveInput = moveAction.ReadValue<Vector3>();
        if (CheckClimbable())
        {
            Debug.Log("I'm climbing wall");
            currentState = state.climb;
            rb.AddForce(moveInput.x * speed, 200f, moveInput.z * speed);
            isClimbing = true;
        }
        isClimbing = false;
    }

    public void OnRun(InputAction.CallbackContext aContext)
    {
        speedMultiplier = 3f;
        if (animator != null) animator.SetBool("isRuning", aContext.performed);
    }

    private void PlayPFX(ParticleSystem pfx)
    {
        if (pfx != null)
        {
            if (!pfx.isPlaying)
            {
                pfx.Play();
                // attempt to rotate pfx to trail behind character
                //Quaternion targetRot = Quaternion.Inverse(new Quaternion(0, transform.rotation.y, 0, 0));
                //dustPFX.transform.rotation = targetRot;
                //Debug.Log(targetRot + " " + dustPFX.transform.position);
            }

        }
    }

    
}
