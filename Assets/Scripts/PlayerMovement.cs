using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
        
    }

    private void OnEnable()
    {
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }

    private void OnDestroy()
    {
        _inputActions.Dispose();
    }

    private void Update()
    {
        _inputVector = _inputActions.Player.Move.ReadValue<Vector2>();

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void FixedUpdate()
    {
        playerRb.MovePosition(playerRb.position + _inputVector * (speed * Time.fixedDeltaTime));
    }
    
}
