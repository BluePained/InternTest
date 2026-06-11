using UnityEngine;

[CreateAssetMenu(fileName = "Seed Item", menuName = "Item/Seed Item")]
public class SeedItem : Item
{
    public override ItemType Type => ItemType.SeedItem;
}
