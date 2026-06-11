using System;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    [field: SerializeField] public Item ItemData { get; private set; }
    [field: SerializeField] public int CurrentAmount { get; private set; }
    private void Awake()
    {
        if(CurrentAmount <= 0)
        {
            CurrentAmount = 1;
        }
    }

    public void OnItemAdded()
    {
        int amount = InventoryManager.Instance.AddItemToInventory(this);
        
        switch (amount)
        {
            case < 0:
                return;
            case 0:
                Destroy(this.gameObject);
                break;
            default:
                CurrentAmount = amount;
                break;
        }
    }
}
