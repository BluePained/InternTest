using UnityEngine;

[CreateAssetMenu(fileName = "Resource Item", menuName = "Item/Resource Item")]
public class ResourceItem : Item
{
    public override ItemType Type => ItemType.ResourcesItem;
}
