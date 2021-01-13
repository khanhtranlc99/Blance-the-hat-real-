using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemsAsset", menuName = "DataAsset/ItemsAsset")]
public class ItemsAsset : BaseAsset<ItemData>
{
}

[System.Serializable]
public class ItemData : SaveData
{
    [Header("GameData")]
    public Sprite thumbnail;
    public GameObject prefab;
}
