using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DailyGiftData", menuName = "DataAsset/DailyGiftData")]
[System.Serializable]
public class DailyGiftData :ScriptableObject
{
    public List<DailyGift> dailysGift;
}

[System.Serializable]
public class DailyGift
{
    public string rewardID;
    public int index;
    public RewardType rewardType;
    public int amount;
    public Sprite itemSprire;
}

public enum RewardType
{
    None = 0,
    Coin = 1,
    Gem = 2,
    Skin = 3,
    Item = 4,
    RandomSkin = 5,
    RandomItem = 6,
}
