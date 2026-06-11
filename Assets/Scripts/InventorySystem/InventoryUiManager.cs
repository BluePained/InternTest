using System;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif
[Serializable]
public class InventoryComponent
{
    [field: SerializeField] public Image SlotImage { get; private set; }
    [field: SerializeField] public TMP_Text SlotText { get; private set; }

    public void AssignSlot(Image slotImage, TMP_Text slotText)
    {
        SlotImage = slotImage;
        SlotText = slotText;
    }
}

public class InventoryUiManager : MonoBehaviour
{
    [SerializeField] private Transform inventorySlotsContainer;
    [SerializeField] private InventoryComponent[] inventoryComponents;
    [SerializeField] private Button[] sortButtons;
    [SerializeField] private ControlGroup controlGroup;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Application.isPlaying || inventorySlotsContainer == null) return;

        if (inventoryComponents.Length != inventorySlotsContainer.childCount)
        {
            inventoryComponents = new InventoryComponent[inventorySlotsContainer.childCount];

            for (int i = 0; i < inventoryComponents.Length; i++)
            {
                inventoryComponents[i] = new InventoryComponent();
                Transform child = inventorySlotsContainer.GetChild(i);
                child.gameObject.GetComponent<InventorySlotUi>().AssignIndex(i);
                inventoryComponents[i].AssignSlot(child.Find("ItemImage").GetComponent<Image>(), 
                    child.Find("AmountText").GetComponent<TMP_Text>());
            }
        }
        
        EditorUtility.SetDirty(this);
        
    }
#endif

    private void Start()
    {
        InventoryManager.Instance.OnInventorySlotUpdate += UpdateUi;
        InventoryManager.Instance.NotifyUi();

        for (int i = 0; i < inventoryComponents.Length; i++)
        {
            inventoryComponents[i].SlotImage.transform.parent.GetComponent<InventorySlotUi>().AssignIndex(i);
        }
        
        sortButtons[0].onClick.AddListener(SortByName);
        sortButtons[1].onClick.AddListener(SortByAmount);
        sortButtons[2].onClick.AddListener(SortByType);
    }

    private void OnDisable()
    {
        InventoryManager.Instance.OnInventorySlotUpdate -= UpdateUi;
    }

    private void UpdateUi(InventorySlot[] inventorySlots)
    {
        for (int i = 0; i < inventoryComponents.Length; i++)
        {
            if (inventorySlots[i].ItemData == null)
            {
                inventoryComponents[i].SlotImage.color = Color.clear;
                inventoryComponents[i].SlotText.text = string.Empty;
                inventoryComponents[i].SlotText.gameObject.SetActive(false);
                continue;
            }
            
            inventoryComponents[i].SlotImage.sprite = inventorySlots[i].ItemData.Icon;
            inventoryComponents[i].SlotImage.color = Color.white;

            if (inventorySlots[i].ItemData.StackSize > 1)
            {
                inventoryComponents[i].SlotText.text = inventorySlots[i].Amount.ToString();
                inventoryComponents[i].SlotText.gameObject.SetActive(true);
            }
            else
            {
                inventoryComponents[i].SlotText.gameObject.SetActive(false);
            }
        }
    }

    private void SortByName()
    {
        InventorySlot[] sortedSlots = InventoryManager.Instance.GetInventorySlots();

        sortedSlots = sortedSlots.OrderBy(slot => slot.ItemData == null).ThenBy(slot => slot.ItemData?.name).ThenByDescending(slot => slot.Amount).ToArray();

        InventoryManager.Instance.SortInventory(sortedSlots);
    }

    private void SortByAmount()
    {
        InventorySlot[] sortedSlots = InventoryManager.Instance.GetInventorySlots();

        var totals = sortedSlots
            .Where(slot => slot.ItemData != null).GroupBy(slot => slot.ItemData).
            ToDictionary(g => g.Key, g 
                => g.Sum(slot => slot.Amount));
        
        sortedSlots = sortedSlots.OrderBy(slot => slot.ItemData == null)
            .ThenByDescending(slot => slot.ItemData == null ? 0: totals[slot.ItemData]).
            ThenByDescending(slot => slot.Amount).ToArray();

        InventoryManager.Instance.SortInventory(sortedSlots);
    }

    private void SortByType()
    {
        InventorySlot[] sortedSlots = InventoryManager.Instance.GetInventorySlots();
        
        //Type, amount, Name?
        sortedSlots = sortedSlots.OrderBy(slot => slot.ItemData == null).ThenBy(slot => slot.ItemData?.Type).ToArray();

        InventoryManager.Instance.SortInventory(sortedSlots);
    }
}
