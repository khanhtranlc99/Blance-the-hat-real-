using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiMode : ControUI
{
    [SerializeField] private Button[] selectButton;
  

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
   
    //    BoomReject = 2,
    //    RubberBalls = 3,
    //    Shake = 4,
    //    WaterDrop = 5
    public void _NormalMode()
    {
        if (DataManager.StagesAsset.list.Count > 0)
            DataManager.CurrentStage = DataManager.StagesAsset.list[0];
          _ScaleButton();
        selectButton[0].transform.localScale = new Vector2(1.5f, 1.5f);
    }

    public void _OnPlusMode()
    {
        if (DataManager.StagesAsset.list.Count > 1)
            DataManager.CurrentStage = DataManager.StagesAsset.list[1];
        _ScaleButton();
        selectButton[1].transform.localScale = new Vector2(1.5f, 1.5f);
    }
    public void _OnBoomReject()
    {
        if (DataManager.StagesAsset.list.Count > 2)
            DataManager.CurrentStage = DataManager.StagesAsset.list[2];
        _ScaleButton();
        selectButton[2].transform.localScale = new Vector2(1.5f, 1.5f);
    }
    public void _OnRubberBalls()
    {
        if (DataManager.StagesAsset.list.Count > 3)
            DataManager.CurrentStage = DataManager.StagesAsset.list[3];
        _ScaleButton();
        selectButton[3].transform.localScale = new Vector2(1.5f, 1.5f);
    }
    public void _OnShake()  // nho sua
    {
        _ScaleButton();
        selectButton[4].transform.localScale = new Vector2(1.5f, 1.5f);

    }
    public void _OnWaterDrop()
    {
        if (DataManager.StagesAsset.list.Count > 5)
            DataManager.CurrentStage = DataManager.StagesAsset.list[5];
           _ScaleButton();
           selectButton[4].transform.localScale = new Vector2(1.5f, 1.5f);
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
}