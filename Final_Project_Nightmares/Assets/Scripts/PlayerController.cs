using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Rigidbody rb;
    private InputAction moveAction;
    public float speed;

    //public float jumpVelocity;
    // six  can idle, walk, run, push, crouchwalk, climb wall
    void Start()
    {
        moveAction = playerInput.actions["NewMove"];
    }

    private void OnEnable()
    {
        moveAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
    }

    void Update()
    {
     
        //transform.Translate(moveInput.x * speed * Time.deltaTime, 0f, moveInput.y * speed * Time.deltaTime, Space.World);
    }

    private void FixedUpdate()
    {
        Vector3 moveInput = moveAction.ReadValue<Vector3>();



        // move left and right
        Vector3 moveDir = new Vector3(moveInput.x * speed, 0f, moveInput.z * speed);
        rb.AddForce(moveDir);

        //jump
        rb.AddForce(new Vector3(0f, moveInput.y), ForceMode.Impulse);

        //Quaternion faceDirMovement =
        rb.MoveRotation(moveDir);

        // todo: prevent double-jumping
    }


}
