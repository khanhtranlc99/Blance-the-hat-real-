using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiEndGame : MonoBehaviour
{
    public static UiEndGame ui;
    [SerializeField] private Text textTimeEndGame;
    [SerializeField] private Image imageItem;
    [SerializeField] private Text textHightScore;
    [SerializeField] private GameObject[] coin;
    [SerializeField] private Text textMobilize;
    [SerializeField] private Image suggets;
    [SerializeField] private int cout;

    private void OnEnable()
    {
        _ShowCoin();
    }
    private void Awake()
    {
        ui = this; 
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

     public void _Sugget()
    {
        cout += 1;      
        if ( cout == 5)
        {
            suggets.gameObject.SetActive(true);
            cout = 0;
        }
        else
        {
            suggets.gameObject.SetActive(false);
            AdsManager.ShowVideoReward((s) =>
            {
                if (s == AdEvent.Success)
                {


                }
            }, "Ads", "Ads_EndGame" + DataManager.GameConfig.coinAdsReward);
        }    
       
        Debug.Log("" + cout);
    }
    public void _SuggetOff()
    {
       
            suggets.gameObject.SetActive(false);
    
    }

}
