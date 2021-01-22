using System;

[Serializable]
public class GameConfig
{
    public int timePlayToShowAds = 30;
    public RebornType rebornType = RebornType.Continue;
    public RebornBy rebornBy = RebornBy.Ads;
    public int suggestUpdateVersion = 1909090100;
    public int coinPerStage = 3;
    public int coinAdsReward = 10;

    public bool autoRewardToInter = true;
    public bool autoInterToReward = true;
}

[Serializable]
public enum RebornType
{
    Continue,
    Checkpoint
}

[Serializable]
public enum RebornBy
{
    Free,
    Gold,
    Gem,
    Ads
}