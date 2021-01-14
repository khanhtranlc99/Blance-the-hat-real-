using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiMode : ControUI
{
    [SerializeField] Button btnNormal;
    [SerializeField] Button btnPlusMode;
    [SerializeField] Button btnPlusCoin;

    public override void Show(bool value)
    {
        base.Show(value);
    }
    private void Awake()
    {
        btnNormal.onClick.AddListener(() => _NormalMode());
    }
    private void _NormalMode()
    {
        GameStateManager.LoadGame(null);
        UIcontro.uIcontro.ChangeUI(UIcontro.MenuUI.Home);
    }
}
