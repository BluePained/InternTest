using UnityEngine;

[CreateAssetMenu(fileName = "Tool Item", menuName = "Item/Tool Item")]
public class ToolItem : Item
{
    public override ItemType Type => ItemType.ToolItem;
}
