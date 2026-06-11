using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private float speed;

    private Action<InputAction.CallbackContext> _movePerformed;
    private Vector2 _inputVector;
    private readonly List<IInteractable> _interactables = new();
    
    private void Awake()
    {
        if(playerRb == null)
            playerRb = GetComponent<Rigidbody2D>();
        
        InputManager.ToggleActionMap(InputManager.InputActions.Player);
        
        _movePerformed = ctx => _inputVector = ctx.ReadValue<Vector2>();
    }

    private void OnEnable()
    {
        InputManager.InputActions.Player.Move.performed += _movePerformed;
        InputManager.InputActions.Player.Move.canceled += _movePerformed;
        InputManager.InputActions.Player.Interact.performed += OnInteract;
    }

    private void OnDisable()
    {
        InputManager.InputActions.Player.Move.performed -= _movePerformed;
        InputManager.InputActions.Player.Move.canceled -= _movePerformed;
        InputManager.InputActions.Player.Interact.performed -= OnInteract;
    }
    
    private void FixedUpdate()
    {
        playerRb.MovePosition(playerRb.position + _inputVector * (speed * Time.fixedDeltaTime));
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (_interactables.Count > 0)
            _interactables[^1].Interact();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable))
            _interactables.Add(interactable);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable))
            _interactables.Remove(interactable);
    }
    
    
}
