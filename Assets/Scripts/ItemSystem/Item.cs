using UnityEngine;

public abstract class Item : ScriptableObject
{
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public Sprite OnUseIcon { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public bool IsStackable { get; private set; }
    [field: SerializeField] public int StackSize { get; private set; }

    void OnValidate()
    {
        if (StackSize <= 0)
        {
            StackSize = 1;
        }
    }
}
