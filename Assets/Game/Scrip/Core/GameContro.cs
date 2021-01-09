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
    public CloneItem clone;
    [SerializeField] private GameObject main;  
    [SerializeField] private CloneItem[] cloneItem;
    [SerializeField] private Text textTime;
    public bool runTime;
    
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
    }

     private IEnumerator _Runtime()
    {
        while (runTime == true)
        {
            yield return new WaitForSeconds(speedRunTime);
            coutnTime += 1;
            textTime.text = "" + coutnTime;
        }
    }
    public void _StopTime()
    {
        runTime = false;
        StopCoroutine(_Runtime());
    }
   
    public void _Pause()
    {
        UI.uI.ChangeUI(UI.MenuUI.pause);
        UI.uI.keyObject.SetActive(false);
        runTime = false;
        StopCoroutine(_Runtime());
        clone._StopMoving();
        MakeEnemy.make._PauseSpawn();
    }
    public void _Resume()
    {
        runTime = true;
        StartCoroutine(_Runtime());
        clone._Moving();
        UI.uI.ChangeUI(UI.MenuUI.gamePlay);
        UI.uI.keyObject.SetActive(true);
        MakeEnemy.make._ResuameSpawn();

    }
    public void _Reset()
    {
        main.transform.DORotate(new Vector3(0, 0, 34.876f), 0.1f);
        runTime = true;
        coutnTime = 0;
        textTime.text = "" + coutnTime;
        StartCoroutine(_Runtime());     
      
    }

}
