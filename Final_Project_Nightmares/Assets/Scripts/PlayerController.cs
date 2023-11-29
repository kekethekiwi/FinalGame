using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    
    [SerializeField] private PlayerInput playerInput;
    private InputAction moveAction;

    //public void OnMove(InputAction.CallbackContext context)
    //{
    //private Vector2 move;
    //    move = context.ReadValue<Vector2>();
    //}

    void Start()
    {
        moveAction = playerInput.actions["move"];
    }


    void Update()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Debug.Log(moveInput);
        transform.Translate(moveInput.x * speed * Time.deltaTime, 0f, moveInput.y * speed * Time.deltaTime, Space.World);
    }

}
