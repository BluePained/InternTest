using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private float speed;

    private InputSystem_Actions _inputActions;
    private Vector2 _inputVector;
    
    private void Awake()
    {
        if(playerRb == null)
            playerRb = GetComponent<Rigidbody2D>();
        
        _inputActions = new InputSystem_Actions();
        _inputActions.Enable();
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
       
    }

    private void Update()
    {
        _inputVector = _inputActions.Player.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        playerRb.MovePosition(playerRb.position + _inputVector * (speed * Time.fixedDeltaTime));
    }
    
}
