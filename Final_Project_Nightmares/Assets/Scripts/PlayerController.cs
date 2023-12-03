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
    private bool isClimbing;

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

    void Update()
    {
     
        //transform.Translate(moveInput.x * speed * Time.deltaTime, 0f, moveInput.y * speed * Time.deltaTime, Space.World);
    }

    private void FixedUpdate()
    {
        Vector3 moveInput = moveAction.ReadValue<Vector3>();

        if (moveInput != Vector3.zero)
        {
            rb.freezeRotation = false;
            if (dustPFX != null)
            {
                if (!dustPFX.isPlaying)
                {
                    // attempt to rotate pfx to trail behind character
                    //Quaternion targetRot = Quaternion.Inverse(new Quaternion(0, transform.rotation.y, 0, 0));
                    //dustPFX.transform.rotation = targetRot;
                    //Debug.Log(targetRot + " " + dustPFX.transform.position);
                    dustPFX.Play();
                }
                
            }
            // get angle and rotation
            float angle = Mathf.Atan2(moveInput.x, moveInput.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, angle, 0);

            //rotate in direction of movement
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotateSpeed * Time.deltaTime));

            // move left and right
            Vector3 moveDir = new Vector3(moveInput.x * speed * speedMultiplier, 0f, moveInput.z * speed * speedMultiplier);
            rb.AddForce(moveDir);
            if (animator != null) animator.SetFloat("speed", speed * speedMultiplier);

            //jump
            // todo: prevent double-jumping
            rb.AddForce(new Vector3(0f, moveInput.y), ForceMode.Impulse);
            if (moveInput.y > 0f)
            {
                if (animator != null) animator.SetTrigger("jump");
                GameManager.ShakeTheCamera(.03f, .03f);
            }
            if (animator != null) animator.ResetTrigger("jump");

        }
        else
        {
            if (animator != null) animator.SetFloat("speed", 0f);
            rb.freezeRotation = true;
        } 

    
    }

    private bool CheckClimbable()
    {
        isClimbable = Physics.SphereCast(transform.position, sphereCastRadius, transform.forward, out raycast, castLength, layerMask);
        climbableLookAtAngle = Vector3.Angle(transform.forward, -raycast.normal);
        return (isClimbable && climbableLookAtAngle < 30f);
    }
    public void OnClimb(InputAction.CallbackContext aContext)
    {
        Vector3 moveInput = moveAction.ReadValue<Vector3>();
        // climb - hold h + arrow key to navigate
        if (CheckClimbable())
        {
            rb.AddForce(moveInput.x * speed, moveInput.y * speed, moveInput.z * speed);
            isClimbing = true;
        }

    }

    public void OnRun(InputAction.CallbackContext aContext)
    {
        speedMultiplier = 3f;
        if (animator != null) animator.SetBool("isRuning", aContext.performed);
    }
}
