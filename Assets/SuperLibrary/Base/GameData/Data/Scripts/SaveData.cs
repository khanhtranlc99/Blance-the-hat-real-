﻿using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class SaveData
{
    [Header("Data")]
    public string name;
    public string id;
    [HideInInspector]
    public string idThumb;
    public int index;
    public string description;
    public UnlockType unlockType = UnlockType.Star;
    public int unlockPrice = 1;
    public bool isUnlocked;
    [HideInInspector]
    public bool isSelected;
    [HideInInspector]
    public int count = 0;
    [HideInInspector]
    public int countHit = 0;
    [HideInInspector]
    public int unlockPay = 0;
    [HideInInspector]
    public int score;
    public bool isCanUnlock => this.IsCanUnlock();

    public SaveData()
    {

    }

    public SaveData(int index, string id, string name, string description = "", bool isSelected = false, bool isUnlocked = false, int unlockPrice = 1, UnlockType unlockType = UnlockType.Star)
    {
        this.index = index;
        this.name = name;
        this.description = description;
        this.id = id;
        this.isUnlocked = isUnlocked;
        this.isSelected = isSelected;
        this.unlockPrice = unlockPrice;
        this.unlockType = unlockType;
    }
}

public static class ItemExtend
{
    public static int totalCoin => DataManager.UserData.totalCoin;
    public static int totalDiamond => DataManager.UserData.totalDiamond;
    public static int userLevel => DataManager.UserData.level;
    public static int totalStar => DataManager.StagesAsset.list.Sum(x => x.star);

    public static bool IsCanUnlock(this SaveData item)
    {
        switch (item.unlockType)
        {
            case UnlockType.Ads:
                return true;
            case UnlockType.Gold:
                return item.unlockPrice <= totalCoin;
            case UnlockType.Star:
                return item.unlockPrice <= totalStar;
            default:
                return false;
        }
    }

    public static SaveData Clone(this SaveData temp)
    {
        return new SaveData
        {
            index = temp.index,
            name = temp.name,
            isUnlocked = temp.isUnlocked,
            isSelected = temp.isSelected,
            unlockPrice = temp.unlockPrice,
            unlockPay = temp.unlockPay,
            unlockType = temp.unlockType,
        };
    }
}

[Serializable]
public enum UnlockType
{
    None,
    Gold,
    Gem,
    Ads,
    Star,
    All = 9999
}

