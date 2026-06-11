using UnityEngine;

public abstract class ReactObject : InteractableObject
{
    private bool _reacted;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_reacted) return;

        if (collision.CompareTag("Player"))
        {
            _reacted = true;
            React();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            _reacted = false;
    }
    
    protected abstract void React();
}
