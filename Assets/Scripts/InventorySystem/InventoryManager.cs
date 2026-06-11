using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    [field: SerializeField] public Item ItemData { get; private set; }
    [field: SerializeField] public int Amount { get; private set; }

    public void AddExistItem(int amount)
    {
        Amount += amount;
    }

    public void AddItem(Item item, int amount)
    {
        ItemData = item;
        Amount = amount;
    }
    
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    [SerializeField] private InventorySlot[] inventorySlots;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public int AddItemToInventory(ItemInfo itemData)
    {
        //If it returned -1, then something goes wrong
        //If it returned 0, then it should fill the slot properly
        //If it returned something else then it's inventory full (No negative number ofc)
        
        if (itemData.ItemData == null) return -1;
        if (itemData.CurrentAmount <= 0) return -1;
        
        int amountToAdd = itemData.CurrentAmount;
        int stackSize =  itemData.ItemData.StackSize;
        
        //If Item Exist
        foreach (var slot in inventorySlots)
        {
            //How do we fix the case where the item isn't at the starting point?
            //We loop through the iteration.
            if (slot.ItemData != itemData.ItemData)
                continue;

            //Skip if the item was already maxed.
            if (slot.Amount >= stackSize)
                continue;
            
            //but if it's not /// Also means it's less than remaining slot guarantee > 0
            int remainingSlot = stackSize - slot.Amount;
            
            //if the remaining slot is enough
            if (amountToAdd <= remainingSlot)
            {
                slot.AddExistItem(amountToAdd);
                return 0;
            }
            
            //But if it's not
            slot.AddExistItem(remainingSlot);
            amountToAdd -= remainingSlot;
        }
        
        //if Item doesn't exist or still remain from above
        foreach (var slot in inventorySlots)
        {
            //If slot isn't empty
            if (slot.ItemData != null)
                continue;
            
            //if it's empty
            if (amountToAdd <= stackSize)
            {
                slot.AddItem(itemData.ItemData, amountToAdd);
                return 0;
            }
            
            slot.AddItem(itemData.ItemData, stackSize);
            amountToAdd -= stackSize;
        }
        
        //Inventory Full
        return amountToAdd;
    }
    
}
