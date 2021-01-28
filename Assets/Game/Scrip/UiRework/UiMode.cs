using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiMode : ControUI
{
    [SerializeField] private Button normalButton;
    [SerializeField] private Button plusButton;

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
        if (DataManager.StagesAsset.list.Count > 0)
            DataManager.CurrentStage = DataManager.StagesAsset.list[0];
        normalButton.transform.localScale = new Vector2(1.5f, 1.5f);
        plusButton.transform.localScale = new Vector2(1, 1);
    }

    public void _OnPlusMode()
    {
        if (DataManager.StagesAsset.list.Count > 1)
            DataManager.CurrentStage = DataManager.StagesAsset.list[1];
           normalButton.transform.localScale = new Vector2(1, 1);
           plusButton.transform.localScale = new Vector2(1.5f, 1.5f);
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

    private void _PlusCoin()
    {
    }
}