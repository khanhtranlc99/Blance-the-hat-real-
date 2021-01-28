using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiEndGame : MonoBehaviour
{
    [SerializeField] private Text textTimeEndGame;
    [SerializeField] private Image imageItem;
    [SerializeField] private Text textHightScore;
    [SerializeField] private GameObject[] coin;
    [SerializeField] private Text textMobilize;
    private void OnEnable()
    {
        _ShowCoin();
    }
    private void Awake()
    {
       
    }
    public void _PrinTime()
    {   
        textTimeEndGame.text = "" + GameCoreManager.coreManager.coutnTime + "s";
        if (GameCoreManager.coreManager.coutnTime > DataManager.CurrentItem.score)
        {
            textHightScore.text = "" + GameCoreManager.coreManager.coutnTime + "s";
            DataManager.CurrentItem.score = GameCoreManager.coreManager.coutnTime;
        }
        else
        {
            textHightScore.text = "" + DataManager.CurrentItem.score + "s";
          
        }  
        imageItem.sprite = DataManager.CurrentItem.thumbnail;
        imageItem.SetNativeSize();
    }
     public void _BackItem()
    {

        GameStateManager.Idle(null);
        UIcontro.uIcontro.ChangeUI(UIcontro.MenuUI.SelectItem);
   
    }
    public void _BackMode()
    {
        GameStateManager.Idle(null);
        UIcontro.uIcontro.ChangeUI(UIcontro.MenuUI.Mode);
    }


     private void _ShowCoin()
    {
       for (int i = 0; i < coin.Length; i ++)
        { 
            coin[i].SetActive(true);
        }
        _Mobilize();
     }
    private void _Mobilize()
    {
        if (GameCoreManager.coreManager.coutnTime < 10)
        {
            textMobilize.text = "You do better than " + 50 + "% player";
        }
        if (GameCoreManager.coreManager.coutnTime > 10)
        {
            textMobilize.text = "You do better than " + 70 + "% player";
        }

        if (GameCoreManager.coreManager.coutnTime > 20)
        {
            textMobilize.text = "You do better than " + 75 + "% player";
        }
    }    



}
