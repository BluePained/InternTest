using System;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class InventorySlotInitializer : MonoBehaviour
{
    [SerializeField] private int inventorySize;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private RectTransform bar;
    [SerializeField] private GameObject[] slots;

#if  UNITY_EDITOR

    private void OnValidate()
    {
        if (Application.isPlaying || slotPrefab == null || bar == null) return;
        
        EditorApplication.delayCall += UpdateInventoryBarSize;
    }

    private void UpdateInventoryBarSize()
    {
        if(inventorySize == slots.Length || inventorySize < 0) return;
        
        if(inventorySize == 0 && slots.Length > 0)
        {
            foreach (var t in slots)
            {
                if(t != null)
                    DestroyImmediate(t);
            }
            slots = Array.Empty<GameObject>();
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
                    slots[i] = new GameObject();
                    
                    GameObject obj = PrefabUtility.InstantiatePrefab(slotPrefab, bar) as GameObject;

                    if (obj == null)
                    {
                        print("Can't Find the prefab'");
                        return;
                    }

                    slots[i] = obj;
                }
                
                break;
            case < 0:
            {
                for (int i = inventorySize; i < slots.Length; i++)
                {
                    DestroyImmediate(slots[i]);
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
