using UnityEngine;

public enum ItemType
{
    ToolItem,
    CraftedItem,
    SeedItem,
    ResourcesItem
}

public abstract class Item : ScriptableObject
{
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public Sprite OnUseIcon { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public int StackSize { get; private set; }
    public abstract ItemType Type { get;}

}
