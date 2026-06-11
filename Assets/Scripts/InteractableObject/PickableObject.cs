using System;
using UnityEngine;

public class PickableObject : InteractableObject
{
    private ItemInfo _itemInfo;

    private void Start()
    {
        _itemInfo = GetComponent<ItemInfo>();
    }
    
    public override void Interact()
    {
        _itemInfo.OnItemAdded();
    }
}
