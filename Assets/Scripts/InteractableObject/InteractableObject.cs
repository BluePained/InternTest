using UnityEngine;

public abstract class InteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] private Collider interactableCollider;

    public void AssignCollider(Collider coll)
    {
        interactableCollider = coll;
    }
    
    public virtual void Interact()
    {
        
    }
}
