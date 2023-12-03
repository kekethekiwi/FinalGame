using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private ParticleSystem dustPFX;
    private Animator animator;
    private InputAction moveAction;
    private InputAction runAction;
    public float speed;
    public float rotateSpeed = 2f;
    private float speedMultiplier = 1f;

    //private Quaternion lastRotation = Quaternion.identity;
    //public float jumpVelocity;
    // six  can idle, walk, run, push, crouchwalk, climb wall
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
                //Debug.Log("playpfx");
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
            //Debug.Log(angle + " " + Quaternion.Euler(0, angle, 0));

            // move left and right
            Vector3 moveDir = new Vector3(moveInput.x * speed * speedMultiplier, 0f, moveInput.z * speed * speedMultiplier);
            rb.AddForce(moveDir);
            if (animator != null) animator.SetFloat("speed", speed * speedMultiplier);

            //jump
            rb.AddForce(new Vector3(0f, moveInput.y), ForceMode.Impulse);
        }
        else
        {
            if (animator != null) animator.SetFloat("speed", 0f);
            rb.freezeRotation = true;
        } 

        // todo: prevent double-jumping
    }

    public void OnRun(InputAction.CallbackContext aContext)
    {
        speedMultiplier = 3f;
        if (animator != null) animator.SetBool("IsRuning", aContext.performed);
    }
}
