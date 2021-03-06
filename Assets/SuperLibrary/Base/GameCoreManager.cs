﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameCoreManager : GameManagerBase<GameCoreManager> 
{
    public static GameCoreManager coreManager;
    public ObjectConfig item;
    public GameObject itemInGameContro;
    public GameObject EnemyInGameContro;
    public float speedRunTime;
    public int coutnTime;
    public int plusScore;
    public bool runTime;
    public CloneItem clone;
    public GameObject main;
    public CloneItem[] cloneItem;
    public Text textTime;
    public Text textScore;
    Coroutine timeCoroutine = null;
    bool isPause = false;
    public Explosion boom;
    public EnemyWater water;
    public EnemySilicol ballSilicol;
    public CloneItem[] allItem;
    public float angerFirst;
    public GameObject wood;
    public GameObject tutorial;
    public ParticleSystem effect;
    [SerializeField] GameObject toucPause;
    public GameObject backGround;
    public bool earthWake;
    public GameObject smoke;
    [SerializeField] GameObject postItem;
    private Coroutine itemCoroutine = null;
    protected override void Awake()
    {     
        base.Awake();
        coreManager = this;
        TouchPanelEventScript.OnPointerDownHandle += OnPointerDownHandle;
    }
    
    protected override void Start()
    {
        //_LoadLogicItem();
        base.Start();
    }
    private void OnEnable()
    {
        this.RegisterListener((int)EventID._Start, LoadGame);
        this.RegisterListener((int)EventID.GameLose, OnGameLoseHandler);


    }
    private void OnDisable()
    {
        EventDispatcher.Instance?.RemoveListener((int)EventID._Start, LoadGame);
        EventDispatcher.Instance?.RemoveListener((int)EventID.GameLose, OnGameLoseHandler);
    }

    private void Update()
    {
        if ( Input.GetKeyDown(KeyCode.Space))
        {
            _LoadLogicItem();
        }
       if(earthWake == true)
        {
            _EarthWake();
        }
        else
        {
            backGround.transform.DOMoveX(0, 0.5f);


        }

    }

    private void LateUpdate()
    {
    }

    private void OnPointerDownHandle(Vector3 p)
    {
        if (GameStateManager.CurrentState == GameState.Pause)
        {
            GameStateManager.Play(null);
            return;
        }
    }

    public override void IdleGame(object data)
    {
        Debug.Log("Game Core goto IdleGame");
        main.SetActive(false);

        if (clone != null)
        {
            Destroy(clone.gameObject);
        }
    }

    public override void InitGame(object data)
    {
        main.transform.DORotate(new Vector3(0, 0, angerFirst), 0.1f);
        DOVirtual.DelayedCall(0.5f, () => GameStateManager.Ready(new MessageReadyGame { }) );
      
    }
    public void _GotoInit() 
    {
       
        Debug.Log("Game Core goto InitGame: " + DataManager.CurrentItem.score);
        GameStatisticsManager.Score = 0;
      
        if (PlayerPrefs.GetInt(Constant.IS_RANDOM_ITEM_PREFS, 0) == 1)
        {
            DataManager.CurrentItem.isSelected = false;
            DataManager.CurrentItem = DataManager.ItemsAsset.list[UnityEngine.Random.Range(1, DataManager.ItemsAsset.list.Count)];
            DataManager.CurrentItem.isSelected = true;
        }
        var b = Instantiate(DataManager.CurrentItem.prefab, new Vector3(0.08f, 3.23f, 0), Quaternion.identity);
        b.transform.SetParent(itemInGameContro.transform);
        this.clone = b.GetComponent<CloneItem>();
        _LoadLogicEnemy();
        _Reset();
        toucPause.SetActive(false);
        /// vao game
       
    }

    public override void LoadGame(object data)
    {
        Debug.Log("Game Core goto LoadGame: " + DataManager.CurrentStage.name);
     
    }

    public override void NextGame(object data)
    {
        Debug.Log("Game Core goto NextGame");
    }

    public override void PauseGame(object data)
    {
        Debug.Log("Game Core goto PauseGame");
        toucPause.SetActive(true);
        _Pause();
    }

    public override void PlayGame(object data)
    {
        Debug.Log("Game Core goto PlayGame");

        var c = SimplePool.Spawn(postItem, new Vector2(0, 5), Quaternion.identity);
        SoundManager.Play("dropItem");
        //UIPerfectToast.instance.Show("LET'S GO!!!");
    }

    public override void RestartGame(object data)
    {
        Debug.Log("Game Core goto RestartGame");

    }

    public override void ResumeGame(object data)
    {
        Debug.Log("Game Core goto ResumeGame");


        _Resume();
        toucPause.SetActive(false);
    }

    protected override void CompleteGame(object data)
    {
        Debug.Log("Game Core goto CompleteGame");
    }

    protected override void GameOver(object data)
    {

        Debug.Log("Game Core goto GameOver");
    }

    protected override void ReadyGame(object data)
    {
        Debug.Log("Game Core goto ReadyGame");
        SoundManager.Play("statgame");
        main.SetActive(true);
        tutorial.SetActive(true);
      
        coutnTime = 0;
        textTime.text = "" + coutnTime;
   
    }

    protected override void RebornCheckPointGame(object data)
    {
        Debug.Log("Game Core goto RebornCheckPointGame");
    }

    protected override void RebornContinueGame(object data)
    {
        Debug.Log("Game Core goto RebornContinueGame");
    }

    protected override void WaitingGameComplete(object data)
    {
        GameStatisticsManager.Process = 1;
    }

    protected override void WaitingGameOver(object data)
    {
        Debug.Log("Game Core goto WaitingGameOver");
    
        _StopTime();
    }
    public void _ChageMenu(object param)
    {




    }
    public void _ChageGamePlay(object param)
    {




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
        for (int i = 0; i < item.listLogicItems.Count; i++)
        {
            item.listLogicItems[i].cloneItem.friction = item.listLogicItems[i].friction;
            item.listLogicItems[i].cloneItem.bounciness = item.listLogicItems[i].bounciness;
            item.listLogicItems[i].cloneItem.mass = item.listLogicItems[i].mass;
            item.listLogicItems[i].cloneItem.itemCanJump = item.listLogicItems[i].itemCanJump;
            item.listLogicItems[i].cloneItem.wasJump = item.listLogicItems[i].wasJump;
            item.listLogicItems[i].cloneItem.moveXJump = item.listLogicItems[i].moveXJump;
            item.listLogicItems[i].cloneItem.moveYJump = item.listLogicItems[i].moveYJump;
            item.listLogicItems[i].cloneItem.timeLoopJump = item.listLogicItems[i].timeLoopJump;   
            //khi nào item va chạm với obstacle thì lấy dữ liệu force từ obstacle
            item.listLogicItems[i].cloneItem._LoadData();
            _LoadLogicEnemy();
            Debug.Log("Chị Hà vừa LoadDataItem");
        }
    }
    public void _Pause()
    {    
        isPause = true;
        if( clone != null )
        {
            clone._StopMoving();
        }       
        MakeEnemy.make._PauseSpawn();
    }
    public void _LoadLogicEnemy()
    {

        for (int i = 0; i < item.obtacles.Count; i++)
        {
            water.forceWater = item.obtacles[0].force;
            boom.forceEplositon = item.obtacles[1].force;
            //ballSilicol.forceBalls = item.obtacles[2].force;
        }
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
        _CheckMode();
        Debug.Log("Reset");
    }
    public void _PlusScore()
    {
        plusScore += 3;
        textScore.text = "" + plusScore;
    }
    public void _Resume()
    {
        isPause = false;
        if (clone)
        {
            clone._Moving();
        }
        _CheckMode();
        if( clone == null)
        {
            GameStateManager.LoadGame(null);
        }    
    }
    public void _StopTime()
    {
        runTime = false;
        isPause = false;
        StopCoroutine(timeCoroutine);
        MakeEnemy.make._PauseSpawn();
    }

    public void _EarthWake()
    {
        var a = Random.Range(-0.69f, 0.6f);
        backGround.transform.DOMoveX( a,0.5f);
    }
    private void _CheckMode()
    {
        if (DataManager.CurrentStage.gameMode == GameMode.Wood)
        {
            MakeEnemy.make._spawnWood();
            MakeEnemy.make._ModeNormal();
            MakeEnemy.make._OffShake();
        }
        if (DataManager.CurrentStage.gameMode == GameMode.Normal)
        {
            MakeEnemy.make._ModeNormal();
            MakeEnemy.make._OffShake();
        }
        if (DataManager.CurrentStage.gameMode == GameMode.BoomReject)
        {
            MakeEnemy.make._ModeBoomReject();
            MakeEnemy.make._OffShake();
        }
        if (DataManager.CurrentStage.gameMode == GameMode.WaterDrop)
        {
            MakeEnemy.make._ModeWaterDrop();
            MakeEnemy.make._OffShake();
        }
        if (DataManager.CurrentStage.gameMode == GameMode.RubberBalls)
        {
            MakeEnemy.make._ModeRumbelBall();
            MakeEnemy.make._OffShake();
        }
        if (DataManager.CurrentStage.gameMode == GameMode.Shake)
        {
            MakeEnemy.make._ModeNormal();
            MakeEnemy.make._OnShake();
        }
    }
    private void OnGameLoseHandler(object param)
    {
        if (coreManager.wood != null)
        {
            SimplePool.Despawn(coreManager.wood.gameObject);
        }
    }
}


public struct MessageReadyGame
{
    public int step;
    public int stageIndex;
}