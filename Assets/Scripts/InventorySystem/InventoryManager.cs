using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum RemoveType
{
    Throw,
    Discard,
    Crafting,
    Used
}

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

    public void RemoveItem(int amount)
    {
        Amount -= amount;

        //Remove all if -1
        if (Amount <= 0 || amount == -1)
        {
            Amount = 0;
            ItemData = null;
        }
    }
    
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    [SerializeField] private InventorySlot[] inventorySlots;
    
    public event Action<InventorySlot[]> OnInventorySlotUpdate;
    
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
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnResetScene;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnResetScene;
    }

    public void NotifyUi()
    {
        OnInventorySlotUpdate?.Invoke(inventorySlots);
    }

    public int AddItemToInventory(ItemInfo itemData)
    {
        //If it returned -1, then something goes wrong
        //If it returned 0, then it should fill the slot properly
        //If it returned something else then it's inventory full (No negative number ofc)
        
        if (itemData.ItemData == null) return -1;
        if (itemData.CurrentAmount <= 0) return -1;

        bool isInventoryChanged = false;
        
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
                isInventoryChanged = true;
                amountToAdd = 0;
                break;
            }
            
            //But if it's not
            slot.AddExistItem(remainingSlot);
            amountToAdd -= remainingSlot;
            isInventoryChanged = true;
        }
        
        //if Item doesn't exist or still remain from above
        foreach (var slot in inventorySlots)
        {
            if (amountToAdd <= 0)
                break;
            
            //If slot isn't empty
            if (slot.ItemData != null)
                continue;
            
            //if it's empty
            if (amountToAdd <= stackSize)
            {
                slot.AddItem(itemData.ItemData, amountToAdd);
                isInventoryChanged = true;
                amountToAdd = 0;
                break;
            }
            
            slot.AddItem(itemData.ItemData, stackSize);
            amountToAdd -= stackSize;
            isInventoryChanged = true;
        }
        
        if(isInventoryChanged)
            OnInventorySlotUpdate?.Invoke(inventorySlots);

        return amountToAdd;
    }

    public void RemoveItemFromInventory(int index, int amount, RemoveType removeType)
    {
        var slot = inventorySlots[index];
        switch (removeType)
        {
            case RemoveType.Throw:
                if (amount == -1)
                    amount = inventorySlots[index].Amount;
                ItemSpawner.Instance.SpawnItem(slot.ItemData, amount, ItemSpawner.Instance.GetPlayerSpawnOffset());
                
                slot.RemoveItem(amount);
                break;
            case RemoveType.Discard:
                slot.RemoveItem(amount);
                break;
            case RemoveType.Crafting:
                break;
            case RemoveType.Used:
                break;
            default:
                break;
        }
        
        OnInventorySlotUpdate?.Invoke(inventorySlots);
    }

    public void SortInventory(InventorySlot[] slot)
    {
        inventorySlots = slot;
        OnInventorySlotUpdate?.Invoke(inventorySlots);
    }

    public InventorySlot[] GetInventorySlots()
    {
        return inventorySlots;
    }

    public bool IndexExistInInventory(int index)
    {
        return inventorySlots[index].ItemData != null;
    }
    
    #region Reset Scene

    private void OnResetScene(Scene scene, LoadSceneMode mode)
    {
        
    }

    #endregion
    
}
