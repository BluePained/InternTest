using System;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour, IInteractable
{
    [field: SerializeField] public Collider2D InteractableCollider { get; private set; }
    
    private void Start()
    {
        if (InteractableCollider == null)
           InteractableCollider = GetComponent<Collider2D>();
        
    }
    
    public virtual void Interact()
    {
        print("Interact");
    }

}
