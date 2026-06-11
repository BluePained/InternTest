using System;
using UnityEngine;

[RequireComponent(typeof(ItemInfo))]
public class ItemSprite : MonoBehaviour
{
    private ItemInfo _itemInfo;
    private SpriteRenderer _spriteRenderer;

    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (Application.isPlaying) return;
        
        _itemInfo = GetComponent<ItemInfo>();
        _spriteRenderer = this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();

        if (_itemInfo == null || _spriteRenderer == null || _itemInfo.ItemData == null) return;

        _spriteRenderer.sprite = _itemInfo.ItemData.Icon != null ? _itemInfo.ItemData.Icon : _spriteRenderer.sprite;
    }
    #endif
}
