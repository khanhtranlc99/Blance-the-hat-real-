using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

[CreateAssetMenu(fileName = "StagesAsset", menuName = "DataAsset/StagesAsset")]
public class StagesAsset : BaseAsset<StageData>
{
    public List<StageData> stageSaveList
    {
        get => list.Where(x => x.isUnlocked || x.unlockPay > 0).Select(x => x as StageData).ToList();
    }
    public StageData Next()
    {
        var nextStage = list.FirstOrDefault(x => x.isUnlocked && x.index > Current.index) as StageData;
        if (nextStage != null)
            Current = nextStage;
        return Current;
    }

    public override void ResetData()
    {
        base.ResetData();

        for (int i = 0; i < list.Count; i++)
        {
            list[i].totalComplete = 0;
            list[i].totalFail = 0;
            list[i].totalPlay = 0;
            list[i].totalReborn = 0;
            list[i].totalRestart = 0;
            list[i].totalTimePlay = 0;
            list[i].process = 0;
            list[i].score = 0;
            list[i].star = 0;
            list[i].combo = 0;
            list[i].time = 0;
        }
    }

    public void AddLevel(int maxLevel)
    {
        for (int i = list.Count + 1; i <= maxLevel; i++)
        {
            var d = new StageData();
            d.name = "Stage " + i;
            d.id = "Stage_" + i;
            d.index = i;
            d.totalComplete = 0;
            d.totalFail = 0;
            d.totalPlay = 0;
            d.totalReborn = 0;
            d.totalRestart = 0;
            d.totalTimePlay = 0;
            d.process = 0;
            d.score = 0;
            d.star = 0;
            d.combo = 0;
            d.time = 0;
            list.Add(d);
        }
    }
}

[Serializable]
public class StageData : SaveData
{
    [Header("StageData")]
    public int totalPlay = 0;
    public GameMode gameMode = GameMode.Normal;

    [HideInInspector]
    public int star;
    [HideInInspector]
    public int process;
    [HideInInspector]
    public int score;
    [HideInInspector]
    public int combo;
    [HideInInspector]
    public float time;
    [HideInInspector]
    public long totalTimePlay = 0;
    [HideInInspector]
    public int totalRestart = 0;
    [HideInInspector]
    public int totalReborn = 0;
    [HideInInspector]
    public int totalPass = 0;
    [HideInInspector]
    public int totalFail = 0;
    [HideInInspector]
    public int totalComplete = 0;
    [HideInInspector]
    public UnlockType unlockBy = UnlockType.Star;
}

[Serializable]
public class StageCloud
{
    public int index = 1;
    public string name;
    public string id;
    public string description;
    public bool publish = true;
    public int unlockPrice = 1;
    [HideInInspector]
    public UnlockType unlockType = UnlockType.Star;
}

[Serializable]
public enum GameMode
{
    Normal = 0,
    Wood = 1,
}

