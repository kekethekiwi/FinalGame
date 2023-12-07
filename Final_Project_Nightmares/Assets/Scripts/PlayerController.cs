using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private new CapsuleCollider collider;
    [SerializeField] private ParticleSystem dustPFX;
    public float speed;
    public float rotateSpeed = 2f;
    public float jumpVelocity = 6f;
    public float climbSpeed = 3f;
    public float firstWait;
    public float secondWait;

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
    private Quaternion lastRotation = Quaternion.identity;
    private bool isGrounded = true;
    private Vector3 lastClimbPos = Vector3.zero;
    private IEnumerator climbCoroutine;

    private Vector3 lastPosition;
    //public float jumpVelocity;
    // six  can *idle, *jump, *walk, run*, push, crouchwalk*, climb wall
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        moveAction = playerInput.actions["NewMove"];
        runAction = playerInput.actions["Run"];
        lastPosition = rb.position;
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

        //Vector3 moveInputt = moveAction.ReadValue<Vector3>();
        //Debug.Log($"x = {moveInputt.x}, y = {moveInputt.y}, z = {moveInputt.z}");

        switch (currentState)
        {
            case state.idle:
                {
                    Vector3 moveInput = moveAction.ReadValue<Vector3>();
                    if (animator != null) animator.enabled = true;
                    if (moveInput != Vector3.zero)
                    {
                        //rb.freezeRotation = false;
                        currentState = moveInput.y > 0f ? state.jump : state.move;
                    }
                    else
                    {
                        if (animator != null) animator.SetFloat("speed", 0f);
                        rb.rotation = lastRotation;
                        //rb.position = lastPosition;
                    }

                    break;
                }

            case state.move:
                {
                    Vector3 moveInput = moveAction.ReadValue<Vector3>().normalized;
                    if (moveInput == Vector3.zero)
                    {
                        //lastPosition = rb.position;
                        currentState = state.idle;
                    }
                    else
                    {
                        // get angle and rotation
                        float angle = Mathf.Atan2(moveInput.x, moveInput.z) * Mathf.Rad2Deg;
                        Quaternion targetRotation = Quaternion.Euler(0, angle, 0);

                        //rotate in direction of movement
                        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotateSpeed * Time.deltaTime));
                        lastRotation = rb.rotation;

                        // move left and right
                        //ManageSpeed();
                        Vector3 moveDir = new Vector3(moveInput.x * speed * speedMultiplier, 0f, moveInput.z * speed * speedMultiplier);
                        rb.AddForce(moveDir);
                        //lastPosition = rb.position;
                        if (animator != null) 
                        { 
                            animator.SetFloat("speed", speed * speedMultiplier);
                            //animator.SetBool("isRuning",  speedMultiplier > 1f && moveInput != Vector3.zero);
                            Debug.Log($"Animator should run = {speedMultiplier > 1f && moveInput != Vector3.zero}");

                        }

                        // pfx
                        PlayPFX(dustPFX);

                        // switch to jump
                        if (moveInput.y > 0f) currentState = state.jump;
                    }
                    break;
                }
            case state.jump:
                {
                    CheckifGrounded();
                    Vector3 moveInput = moveAction.ReadValue<Vector3>();
                    if (moveInput.y > 0f && isGrounded == true)
                    {
                        rb.AddForce(new Vector3(0f, moveInput.y * jumpVelocity), ForceMode.Impulse);
                        if (animator != null) animator.SetTrigger("jump");

                        //GameManager.ShakeTheCamera(.03f, .03f);
                    }
                    else if (moveInput.x > 0f || moveInput.y > 0f && isGrounded == true)
                    {
                        if (animator != null) animator.ResetTrigger("jump");
                        //lastPosition = rb.position;
                        currentState = state.move;

                    }
                    else if (isGrounded == true)
                    {
                        if (animator != null) animator.ResetTrigger("jump");
                        //lastPosition = rb.position;
                        currentState = state.idle;
                    }
                    //if (animator != null) animator.ResetTrigger("jump");
                    break;
                }
            case state.climb:
                {
                    if (!Input.GetKey(KeyCode.H) && !Input.GetKeyDown(KeyCode.H))
                    {
                        ResetClimb();
                    }
                    else
                    {
                        //if (animator != null) animator.SetBool("isClimbing", true);
                        //rb.useGravity = false;
                        //Vector3 moveInput = moveAction.ReadValue<Vector3>().normalized;
                        //Vector3 vertClimb = new Vector3(0f, moveInput.z * climbSpeed * Time.fixedDeltaTime, 0f);
                        //Vector3 horizClimb = new Vector3(moveInput.x * climbSpeed * Time.fixedDeltaTime, 0f, 0f);
                        //rb.MovePosition(rb.position + vertClimb + horizClimb);

                        if (climbCoroutine == null)
                        {
                            Debug.Log("create and start CoROUTINE");
                            climbCoroutine = ClimbCoroutine();
                            StartCoroutine(climbCoroutine);
                        }
                    }

                    ////// try new input system
                    //rb.useGravity = false;
                    //Vector3 newPos = raycast.point + raycast.normal * (collider.radius - .1f);
                    //rb.MovePosition(newPos);
                    //if (animator != null && animator.GetCurrentAnimatorStateInfo(0).IsName("isClimbing")) 
                    //{
                    //    animator.enabled = false;
                    //}

                    //Vector3 moveInput = moveAction.ReadValue<Vector3>().normalized;
                    //Vector3 vertClimb = new Vector3(0f, moveInput.z * climbSpeed * Time.fixedDeltaTime, 0f);
                    //Vector3 horizClimb = new Vector3(moveInput.x * climbSpeed * Time.fixedDeltaTime, 0f, 0f);
                    //Debug.Log($"moveInput = {moveInput}, vertclimb = {vertClimb}, horizClimb = {horizClimb}, final = {rb.position + vertClimb + horizClimb} ");
                    //if (animator != null && animator.GetCurrentAnimatorStateInfo(0).IsName("isClimbing"))
                    //{
                    //    animator.enabled = true;
                    //}

                    //rb.MovePosition(rb.position + vertClimb + horizClimb);
                    //isClimbing = true;

                    break;
                }
        }

    }

    private void ResetClimb()
    {
        climbCoroutine = null;
        if (animator != null)
        {
            animator.SetBool("isClimbing", false);
            animator.enabled = true;
        }
        rb.useGravity = true;

        Vector3 moveInput = moveAction.ReadValue<Vector3>().normalized;
        if (moveInput != Vector3.zero)
        {
            currentState = moveInput.y > 0f ? state.jump : state.move;
        }
        else
        {
            currentState = state.idle;
        }

    }
    IEnumerator ClimbCoroutine()
    {
        rb.useGravity = false;
        Vector3 startPos = rb.position;
        Vector3 targetPos = CalculateClimbPos();

        if (animator != null)
        {
            animator.enabled = true;
            animator.SetBool("isClimbing", true);
        }

        yield return new WaitForSeconds(firstWait);
        //if (animator != null) animator.enabled = false;

        float startTime = Time.time;
        float duration = .5f;
        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            rb.MovePosition(Vector3.Lerp(startPos, targetPos, t));
            yield return null;
        }
        //rb.MovePosition(targetPos);

        if (animator != null)
        {
            animator.enabled = false;
            animator.SetBool("isClimbing", true);
        }
        yield return new WaitForSeconds(secondWait);

        climbCoroutine = null;
    }
    Vector3 CalculateClimbPos()
    {
        // to do 
        //Debug.Log($"moveInput = {moveInput}, vertclimb = {vertClimb}, horizClimb = {horizClimb}, final = {rb.position + vertClimb + horizClimb} ");

        Physics.SphereCast(transform.position, sphereCastRadius, transform.forward, out raycast, castLength, layerMask);
        Vector3 moveInput = moveAction.ReadValue<Vector3>().normalized;
        Vector3 vertClimb = new Vector3(0f, moveInput.z * climbSpeed * Time.fixedDeltaTime, 0f);
        Vector3 horizClimb = new Vector3(moveInput.x * climbSpeed * Time.fixedDeltaTime, 0f, 0f);
        
        Vector3 movePos = rb.position + vertClimb + horizClimb;

        Vector3 targetPos = movePos; //+raycast.point + raycast.normal * (collider.radius - .1f)
        return targetPos;
    }
    private bool CheckClimbable()
    {
        isClimbable = Physics.SphereCast(transform.position, sphereCastRadius, transform.forward, out raycast, castLength, layerMask);
        climbableLookAtAngle = Vector3.Angle(transform.forward, -raycast.normal);
        Debug.Log("onClimb " + isClimbable);
        return (isClimbable && climbableLookAtAngle < 30f);
    }
    public void OnClimb(InputAction.CallbackContext aContext)
    {
        // climb - hold h + arrow key to navigate
        Vector3 moveInput = moveAction.ReadValue<Vector3>();
        if (CheckClimbable())
        {
            currentState = state.climb;
            //lastPosition = rb.position;
        }
        
    }

    public void OnRun(InputAction.CallbackContext aContext)
    {
        
        Vector3 moveInput = moveAction.ReadValue<Vector3>();
        if (aContext.performed && moveInput != Vector3.zero && currentState != state.climb)
        {
            speedMultiplier = 1.4f;
            if (animator != null) animator.SetBool("isRuning", true);
        }
        else
        {
            ManageSpeed();
            if (animator != null) animator.SetBool("isRuning", false);
        }
    }

    private void ManageSpeed()
    {
        // character not running, reset speed
        //if (!Input.GetKey(KeyCode.LeftShift | KeyCode.RightShift) && speedMultiplier > 1f)
        //{
        //    speedMultiplier = 1f;
        //}
        if (speedMultiplier > 1f)
        {
            speedMultiplier = Mathf.Lerp(speedMultiplier, 1f, 0.1f);
        }
    }
    private void PlayPFX(ParticleSystem pfx)
    {
        if (pfx != null)
        {
            if (!pfx.isPlaying)
            {
                pfx.Play();
            }
        }
    }

    private void CheckifGrounded()
    {
        //isGrounded = rb.velocity.y == 0 ? true : false;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, .1f);
    }

    // IGNORE TEST CODE - climnb
    //if (!Input.GetKeyDown(KeyCode.H))
    //{
    //    currentState = state.idle;
    //    isClimbing = false;
    //}
    // try old input system
    //playerInput.actions.FindAction("NewMove").Disable();
    //float xInput = Input.GetAxis("Horizontal");
    //float yInput = Input.GetAxis("Vertical");
    //rb.AddForce(new Vector3(xInput * climbSpeed, yInput * climbSpeed));
    //climbCoroutine = ClimbCoroutine();
    //StartCoroutine(climbCoroutine);

    // attempt to rotate pfx to trail behind character
    //Quaternion targetRot = Quaternion.Inverse(new Quaternion(0, transform.rotation.y, 0, 0));
    //dustPFX.transform.rotation = targetRot;
    //Debug.Log(targetRot + " " + dustPFX.transform.position);

}
