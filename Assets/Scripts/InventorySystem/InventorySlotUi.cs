using System;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUi : MonoBehaviour
{
    [field: SerializeField] public int SlotNumber { get; private set; }
    [SerializeField] private ControlGroup controlGroup;
    private Button _slotButton;
    
    private void Start()
    {
        if(controlGroup == null)
            controlGroup = GameObject.Find("ControlGroup").GetComponent<ControlGroup>();
        
        _slotButton = GetComponent<Button>();

        _slotButton.onClick.AddListener((OnButtonClick));
    }

    public void AssignIndex(int slotNumber)
    {
        SlotNumber = slotNumber;
    }

    private void OnButtonClick()
    {
        if (InventoryManager.Instance.IndexExistInInventory(SlotNumber))
        {
            controlGroup.gameObject.SetActive(false);
            controlGroup.OnCall(SlotNumber, this.transform);
        }
    }
}
