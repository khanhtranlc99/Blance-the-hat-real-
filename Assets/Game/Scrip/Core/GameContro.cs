using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GameContro : MonoBehaviour
{
    public static GameContro instance;
    public GameObject itemInGameContro;
    public GameObject EnemyInGameContro;
    public float speedRunTime;
    public int coutnTime;
    public int plusScore;
    public bool runTime;
    public CloneItem clone;
    public LogicItem item;
    [SerializeField] private GameObject main;  
    [SerializeField] private CloneItem[] cloneItem;
    [SerializeField] private Text textTime; 
    [SerializeField] private Text textScore;


    Coroutine timeCoroutine = null;
    bool isPause = false;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        //_LoadLogicItem();
    }
    private IEnumerator _Runtime()
    {
        Debug.Log("RunTime");
        while (runTime == true)
        {
            yield return new WaitForSeconds(speedRunTime);
            if (!isPause)
            {
                coutnTime += 1;
                textTime.text = "" + coutnTime;
            }
        }
    }
     private void _LoadLogicItem()
    {
        for( int i = 0; i < item.listLogic.Count; i ++)
        {
            item.listLogic[i].cloneItem.friction = item.listLogic[i].friction;
            item.listLogic[i].cloneItem.bounciness = item.listLogic[i].bounciness;
            item.listLogic[i].cloneItem.mass = item.listLogic[i].mass;
            item.listLogic[i].cloneItem.itemCanJump = item.listLogic[i].itemCanJump;
            item.listLogic[i].cloneItem.wasJump = item.listLogic[i].wasJump;
            item.listLogic[i].cloneItem.moveXJump = item.listLogic[i].moveXJump;
            item.listLogic[i].cloneItem.moveYJump = item.listLogic[i].moveYJump;
            item.listLogic[i].cloneItem.timeLoopJump = item.listLogic[i].timeLoopJump;
            item.listLogic[i].cloneItem._LoadData();
            Debug.Log("LoadDataItem");
        }
    }
    public void _StopTime()
    {
        runTime = false;
        isPause = false;
        StopCoroutine(_Runtime());
        MakeEnemy.make._PauseSpawn();
    }
   
    public void _Pause()
    {
        UI.uI.ChangeUI(UI.MenuUI.pause);
        UI.uI.keyObject.SetActive(false);
        isPause = true;
        clone._StopMoving();
        MakeEnemy.make._PauseSpawn();
    }
    public void _Resume()
    {
        isPause = false;
        clone._Moving();
        UI.uI.ChangeUI(UI.MenuUI.gamePlay);
        UI.uI.keyObject.SetActive(true);
        MakeEnemy.make._ResuameSpawn();
    }
    public void _Reset()
    {
        main.transform.DORotate(new Vector3(0, 0, 0), 0.1f);
        runTime = true;
        isPause = false;
        coutnTime = 0;
        textTime.text = "" + coutnTime;
        if (timeCoroutine != null)
        {
            StopCoroutine(timeCoroutine);
            timeCoroutine = null;
        }
        timeCoroutine = StartCoroutine(_Runtime()); ;
        MakeEnemy.make._ResuameSpawn();
    }
    public void _PlusScore()
    {
        plusScore += 3;
        textScore.text = "" + plusScore;
    }    
    
}
