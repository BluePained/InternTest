using System;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    [field: SerializeField] public Item ItemData { get; private set; }
    [field: SerializeField] public int CurrentAmount { get; private set; }
    private void Start()
    {
        if(CurrentAmount <= 0)
        {
            CurrentAmount = 1;
        }
    }
}
