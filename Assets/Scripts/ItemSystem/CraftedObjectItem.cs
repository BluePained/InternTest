using UnityEngine;

[CreateAssetMenu(fileName = "Craft Item", menuName = "Item/Craft Item")]
public class CraftedObjectItem : Item
{
    public override ItemType Type => ItemType.CraftedItem;
}
