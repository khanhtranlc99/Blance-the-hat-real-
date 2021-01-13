using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public UserData user = new UserData();
    public List<StageData> stages = new List<StageData>();
    public List<SaveData> items = new List<SaveData>();
}
