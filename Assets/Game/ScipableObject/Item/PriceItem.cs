using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MapData", menuName = "Data/PriceItem", order = 0)]

public class PriceItem : ScriptableObject
{
    public string name;
    public int prive;
    public bool wasBought;
}
