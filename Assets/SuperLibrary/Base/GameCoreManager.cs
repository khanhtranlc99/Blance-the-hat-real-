using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

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
    public PointEffector2D boom;
    public Rigidbody2D water;
    public Rigidbody2D ballSilicol;
    public CloneItem[] allItem;
    public float angerFirst;
  
    private void Awake()
    {
        coreManager = this;
    }
    protected override void Start()
    {
        base.Start();
    }
    private void OnEnable()
    {
        this.RegisterListener((int)EventID._Start, LoadGame);
  


    }
    private void OnDisable()
    {
    }

    private void Update()
    {
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
        Debug.Log("Game Core goto InitGame");


        //Hien thi con main
        main.SetActive(true);

        //sinh ra con item
        var b = Instantiate(DataManager.CurrentItem.prefab, new Vector3(0.08f, 3.23f, 0), Quaternion.identity);
        b.transform.SetParent(itemInGameContro.transform);
        this.clone = b.GetComponent<CloneItem>();
        _LoadLogicEnemy();
        _Reset();

        /// vao game

        DOVirtual.DelayedCall(0.5f, () => GameStateManager.Ready(new MessageReadyGame { }));
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
    }

    public override void PlayGame(object data)
    {
        Debug.Log("Game Core goto PlayGame");
        //UIPerfectToast.instance.Show("LET'S GO!!!");
    }

    public override void RestartGame(object data)
    {
        Debug.Log("Game Core goto RestartGame");

    }

    public override void ResumeGame(object data)
    {
        Debug.Log("Game Core goto ResumeGame");
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
        for (int i = 0; i < item.listLogic.Count; i++)
        {
            item.listLogic[i].cloneItem.friction = item.listLogic[i].friction;
            item.listLogic[i].cloneItem.bounciness = item.listLogic[i].bounciness;
            item.listLogic[i].cloneItem.itemCanJump = item.listLogic[i].itemCanJump;
            item.listLogic[i].cloneItem.wasJump = item.listLogic[i].wasJump;
            item.listLogic[i].cloneItem.moveXJump = item.listLogic[i].moveXJump;
            item.listLogic[i].cloneItem.moveYJump = item.listLogic[i].moveYJump;
            item.listLogic[i].cloneItem.timeLoopJump = item.listLogic[i].timeLoopJump;   
            //khi nào item va chạm với obstacle thì lấy dữ liệu force từ obstacle
            item.listLogic[i].cloneItem._LoadData();
            Debug.Log("LoadDataItem");
        }
    }
    public void _Pause()
    {
        //UI.uI.ChangeUI(UI.MenuUI.pause);
        //UI.uI.keyObject.SetActive(false);
        isPause = true;
        clone._StopMoving();
        MakeEnemy.make._PauseSpawn();
    }
    public void _LoadLogicEnemy()
    {
        //water.mass = item.obtacles[0].force;
        //boom.forceMagnitude = item.obtacles[1].force;
        //ballSilicol.mass = item.obtacles[2].force;
        //Debug.Log("LoadEnemy");
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
        MakeEnemy.make._ResuameSpawn();
    }
    public void _PlusScore()
    {
        plusScore += 3;
        textScore.text = "" + plusScore;
    }
    public void _Resume()
    {
        isPause = false;
        clone._Moving();
        //UI.uI.ChangeUI(UI.MenuUI.gamePlay);
        //UI.uI.keyObject.SetActive(true);
        MakeEnemy.make._ResuameSpawn();
    }
    public void _StopTime()
    {
        runTime = false;
        isPause = false;
        StopCoroutine(_Runtime());
        MakeEnemy.make._PauseSpawn();
    }

    public void _CheckItem()
    {
    }
}


public struct MessageReadyGame
{
    public int step;
    public int stageIndex;
}