using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGamePlay : ControUI
{
    public override void Show(bool value)
    {
        base.Show(value);
    }
    public void _ChangeGamePlay()
    {
        UI.uI.ChangeUI(UI.MenuUI.gamePlay);
    }
}
