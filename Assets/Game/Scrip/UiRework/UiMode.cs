using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiMode : ControUI
{   
    public bool wasPlusMode;
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
        wasPlusMode = false;
        normalButton.transform.localScale = new Vector2(1.5f, 1.5f);
        plusButton.transform.localScale = new Vector2(1, 1);
    }
    public void _OnPlusMode()
    {
        wasPlusMode = true;
        normalButton.transform.localScale = new Vector2(1, 1);
        plusButton.transform.localScale = new Vector2(1.5f, 1.5f);
    }

    public void OnCoinAdsButtonClick()
    {
        CoinManager.Add(DataManager.GameConfig.coinAdsReward);
    }
    public void _OffMode()
    {
        wasPlusMode = false;
        
    }
    private void _PlusCoin()
    {

    }
}
