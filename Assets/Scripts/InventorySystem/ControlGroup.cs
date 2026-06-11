using System;
using UnityEngine;
using UnityEngine.UI;

public class ControlGroup : MonoBehaviour
{
    [SerializeField] private Button[] controlGroupButtons;
    [SerializeField] private Vector3 offset;

    private void Start()
    {
        gameObject.SetActive(false);

        InventoryManager.Instance.OnInventorySlotUpdate += DisablePanel;
        ClearListener();
    }

    private void OnEnable()
    {
        if(InventoryManager.Instance != null)
            InventoryManager.Instance.OnInventorySlotUpdate += DisablePanel;
    }
    
    private void OnDisable()
    {
        InventoryManager.Instance.OnInventorySlotUpdate -= DisablePanel;
        ClearListener();
    }

    public void OnCall(int index, Transform position)
    {
        this.transform.localPosition = position.localPosition + offset;
        this.gameObject.SetActive(true);
        ClearListener();
        
        controlGroupButtons[0].onClick.AddListener(() => OnRemoveItem(index, 1, RemoveType.Throw));
        controlGroupButtons[1].onClick.AddListener(() => OnRemoveItem(index, -1, RemoveType.Throw));
        controlGroupButtons[2].onClick.AddListener(() => OnRemoveItem(index, -1, RemoveType.Discard));
    }

    private void OnRemoveItem(int index, int amount, RemoveType removeType)
    {
        InventoryManager.Instance.RemoveItemFromInventory(index, amount, removeType);
        gameObject.SetActive(false);
    }
    
    private void DisablePanel(InventorySlot[] slot)
    {
        gameObject.SetActive(false);
        ClearListener();
    }
    
    private void ClearListener()
    {
        foreach (var button in controlGroupButtons)
        {
            button.onClick.RemoveAllListeners();
        }
    }
}
