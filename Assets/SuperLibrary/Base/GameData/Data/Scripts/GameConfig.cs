using System;

[Serializable]
public class GameConfig
{
    public int timePlayToShowAds = 30;
    public int timePlayReduceToShowAds = 15;
    public RebornType rebornType = RebornType.Continue;
    public RebornBy rebornBy = RebornBy.Ads;
    public int suggestUpdateVersion = 1909090100;
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