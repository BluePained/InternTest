using System;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class Slot
{
    [field: SerializeField] public GameObject SlotObj { get; private set; }

    public void AssignSlot(GameObject obj)
    {
        SlotObj = obj;
    }
    
}

public class InventoryBarInitializer : MonoBehaviour
{
    [ContextMenuItem("Update Bar Size", "UpdateInventoryBarSize")]
    [Range(0, 10)][SerializeField] private int inventorySize;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private RectTransform bar;
    [SerializeField] private Slot[] slots;

#if  UNITY_EDITOR

    private void OnValidate()
    {
        if (Application.isPlaying || slotPrefab == null || bar == null) return;
        
        EditorApplication.delayCall += UpdateInventoryBarSize;
    }

    [ContextMenu("Update Bar Size")]
    private void UpdateInventoryBarSize()
    {
        if (Application.isPlaying || slotPrefab == null || bar == null) return;
        
        if(inventorySize == slots.Length) return;
        
        if(inventorySize == 0 && slots.Length > 0)
        {
            foreach (var t in slots)
            {
                if(t.SlotObj != null)
                    DestroyImmediate(t.SlotObj);
            }
            slots = Array.Empty<Slot>();
            return;
        }
        
        //is = 3 slot = 2 = 1
        //is = 3 slot = 5 = -2
        int currentSize = slots.Length;
        int diff = inventorySize - currentSize;
        switch (diff)
        {
            case > 0:
                Array.Resize(ref slots, inventorySize);

                for (int i = currentSize; i < inventorySize; i++)
                {
                    slots[i] = new Slot();
                    
                    GameObject obj = PrefabUtility.InstantiatePrefab(slotPrefab, bar) as GameObject;

                    if (obj == null)
                    {
                        print("Can't Find the prefab'");
                        return;
                    }
                    
                    slots[i].AssignSlot(obj);
                }
                
                break;
            case < 0:
            {
                for (int i = inventorySize; i < slots.Length; i++)
                {
                    DestroyImmediate(slots[i].SlotObj);
                }
                
                Array.Resize(ref slots, inventorySize);
                break;
            }
        }
        
        EditorUtility.SetDirty(this);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
    }
#endif

    
}
