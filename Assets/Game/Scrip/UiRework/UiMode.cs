using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiMode : ControUI
{
    [SerializeField] private Button[] selectButton;
    [SerializeField] private Button ButtonPlay;
    [SerializeField] private Button ButtonClone;
    [SerializeField] private Button ButtonWater;
  

    
 private void Start()
    {
        _NormalMode();
    }

    public override void Show(bool value)
    {
        base.Show(value);
    }

    public void _PlayButton()
    {
        GameStateManager.LoadGame(null);
        UIcontro.uIcontro.ChangeUI(UIcontro.MenuUI.Home);
    }
  
    public void _NormalMode()
    {
        ButtonPlay.gameObject.SetActive(true);
        if (DataManager.StagesAsset.list.Count > 0)
            DataManager.CurrentStage = DataManager.StagesAsset.list[0];
            _ScaleButton();
        selectButton[0].transform.localScale = new Vector2(1.5f, 1.5f);
   
    }

    public void _OnPlusMode()
    {
        ButtonPlay.gameObject.SetActive(true);
        if (DataManager.StagesAsset.list.Count > 1)
            DataManager.CurrentStage = DataManager.StagesAsset.list[1];
        _ScaleButton();
        selectButton[1].transform.localScale = new Vector2(1.5f, 1.5f);
    }
    public void _OnBoomReject()
    {  if (DataManager.StagesAsset.list[3].isUnlocked == false)
        {
            ButtonPlay.gameObject.SetActive(false);
            ButtonClone.gameObject.SetActive(true);
        }    
        else
        {
            ButtonPlay.gameObject.SetActive(true);
            ButtonClone.gameObject.SetActive(false);
        }
   
        if (DataManager.StagesAsset.list.Count > 2)
            DataManager.CurrentStage = DataManager.StagesAsset.list[2];
           _ScaleButton();
           selectButton[2].transform.localScale = new Vector2(1.5f, 1.5f);
    }
    public void _OnRubberBalls()
    {
        ButtonPlay.gameObject.SetActive(true);
        if (DataManager.StagesAsset.list.Count > 3)
            DataManager.CurrentStage = DataManager.StagesAsset.list[3];
        _ScaleButton();
        selectButton[3].transform.localScale = new Vector2(1.5f, 1.5f);
    }
    public void _OnShake()
    {
        ButtonPlay.gameObject.SetActive(true);
        if (DataManager.StagesAsset.list.Count > 4)
            DataManager.CurrentStage = DataManager.StagesAsset.list[4];
        _ScaleButton();
        selectButton[4].transform.localScale = new Vector2(1.5f, 1.5f);

    }
    public void _OnWaterDrop()
    {
        if (DataManager.StagesAsset.list[5].isUnlocked == false)
        {
            ButtonPlay.gameObject.SetActive(false);
            ButtonClone.gameObject.SetActive(false);
            ButtonWater.gameObject.SetActive(true);
        }
        else
        {
            ButtonPlay.gameObject.SetActive(true);
            ButtonClone.gameObject.SetActive(false);
            ButtonWater.gameObject.SetActive(false);
        }

        if (DataManager.StagesAsset.list.Count > 5)
            DataManager.CurrentStage = DataManager.StagesAsset.list[5];
           _ScaleButton();
           selectButton[5].transform.localScale = new Vector2(1.5f, 1.5f);
    }
    public void OnCoinAdsButtonClick()
    {
        AdsManager.ShowVideoReward((s) =>
        {
            if (s == AdEvent.Success)
            {
                CoinManager.Add(DataManager.GameConfig.coinAdsReward);
            }
        }, "Select_Mode", "select_mode_coin_" + DataManager.GameConfig.coinAdsReward);
    }

    public void _OffMode()
    {
        DataManager.CurrentStage = DataManager.StagesAsset.list[0];
    }

    private void _ScaleButton()
    {  
        for ( int i = 0; i < selectButton.Length; i ++)
        {
            selectButton[i].transform.localScale = new Vector2(1, 1);
        }
        //normalButton.transform.localScale = new Vector2(1.5f, 1.5f);
        //plusButton.transform.localScale = new Vector2(1, 1);
    }
     public void _BtnCloneOnClick()
    {
           if (CoinManager.totalCoin >= 60)
            {
                CoinManager.Add(-60);
                DataManager.StagesAsset.list[3].isUnlocked = true;
            ButtonPlay.gameObject.SetActive(true);
            ButtonClone.gameObject.SetActive(false);
            ButtonWater.gameObject.SetActive(false);
            GameStateManager.LoadGame(null);
                UIcontro.uIcontro.ChangeUI(UIcontro.MenuUI.Home);
            }
           else
        {
            Debug.Log("huhu");
        }    

    }
    public void _BtnCloneOnClickWater()
    {
        if (CoinManager.totalCoin >= 60)
        {
            CoinManager.Add(-60);
            DataManager.StagesAsset.list[5].isUnlocked = true;
            ButtonPlay.gameObject.SetActive(true);
            ButtonClone.gameObject.SetActive(false);
            ButtonWater.gameObject.SetActive(false);
            GameStateManager.LoadGame(null);
            UIcontro.uIcontro.ChangeUI(UIcontro.MenuUI.Home);
        }
        else
        {
            Debug.Log("huhu");
        }

    }
    public void _BtnNoAds()
    {
        DataManager.UserData.isRemovedAds = true;


         
    }    
}