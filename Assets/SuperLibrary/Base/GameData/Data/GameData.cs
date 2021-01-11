﻿using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public UserData user = new UserData();
    public List<StageData> stages = new List<StageData>();
}
