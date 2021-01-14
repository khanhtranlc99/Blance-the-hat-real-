using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MapData", menuName = "Data/Map", order = 0)]
public class ObjectConfig : ScriptableObject
{
    public List<GameItem> listLogic; // đổi tên cái này thành items
}
[System.Serializable]
public struct GameItem
{
    public CloneItem cloneItem;
    public List<GameObtacle> obtacles;
    public float friction; //ok
    public float bounciness; //ok
    //public float mass; // ok
    public bool itemCanJump;  //ok
    public bool wasJump; //ok
    public float moveXJump; //ok
    public float moveYJump; //ok
    public float timeLoopJump; //ok
   
}
[System.Serializable]
public struct GameObtacle
{
    public string name;
    public float force;
}

