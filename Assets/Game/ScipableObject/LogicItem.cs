using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MapData", menuName = "Data/Map", order = 0)]
public class LogicItem : ScriptableObject
{
    public List<Logic> listLogic;

}
[System.Serializable]
public struct Logic
{
    public CloneItem cloneItem;
    public float friction; //ok
    public float bounciness; //ok
    public float mass; // ok
    public bool itemCanJump;  //ok
    public bool wasJump; //ok
    public float moveXJump; //ok
    public float moveYJump; //ok
    public float timeLoopJump; //ok
    public float waterForce;
    public float boomForce;
    public float ballForce;
   
}

