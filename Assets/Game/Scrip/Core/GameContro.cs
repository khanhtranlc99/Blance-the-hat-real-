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
    public ObjectConfig item;
    [SerializeField] private GameObject main;  
    [SerializeField] private CloneItem[] cloneItem;
    [SerializeField] private Text textTime; 
    [SerializeField] private Text textScore;
    Coroutine timeCoroutine = null;
    bool isPause = false;
    public PointEffector2D boom;
    public Rigidbody2D water;
    public Rigidbody2D ballSilicol;
    public float angerFirst;
   
    private void OnEnable()
    {
     
    }
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
     public void _LoadLogicItem()
    {
        for( int i = 0; i < item.listLogicItems.Count; i ++)
        {
            item.listLogicItems[i].cloneItem.friction = item.listLogicItems[i].friction;
            item.listLogicItems[i].cloneItem.bounciness = item.listLogicItems[i].bounciness;
            item.listLogicItems[i].cloneItem.mass = item.listLogicItems[i].mass;
            item.listLogicItems[i].cloneItem.itemCanJump = item.listLogicItems[i].itemCanJump;
            item.listLogicItems[i].cloneItem.wasJump = item.listLogicItems[i].wasJump;
            item.listLogicItems[i].cloneItem.moveXJump = item.listLogicItems[i].moveXJump;
            item.listLogicItems[i].cloneItem.moveYJump = item.listLogicItems[i].moveYJump;
            item.listLogicItems[i].cloneItem.timeLoopJump = item.listLogicItems[i].timeLoopJump;
            //item.listLogicItems[i].cloneItem.waterForce = item.listLogicItems[i].obtacles[0].force;
            //item.listLogicItems[i].cloneItem.boomForce = item.listLogicItems[i].obtacles[1].force;
            //item.listLogicItems[i].cloneItem.ballForce = item.listLogicItems[i].obtacles[2].force;
            item.listLogicItems[i].cloneItem._LoadData();
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
        MakeEnemy.make._ModeNormal();
    }
    public void _Reset()
    {
        main.transform.DORotate(new Vector3(0, 0, angerFirst), 0.1f);
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
        //MakeEnemy.make._ResuameSpawn();
    }
    public void _PlusScore()
    {
        plusScore += 3;
        textScore.text = "" + plusScore;
    }    
    public void _LoadLogicEnemy()
    {
        water.mass = clone.waterForce;
        boom.forceMagnitude = clone.boomForce;
        ballSilicol.mass = clone.ballForce;
        Debug.Log("LoadEnemy");
    }    

    private void ABCD()
    {
        
        for( int i = 0; i < item.listLogicItems.Count; i++)
        {
         
        }    
            

    }
    
}

