using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPause : ControUI
{
    public override void Show(bool value)
    {
        base.Show(value);
    }
    public void _ChangePause()
    {
        UI.uI.ChangeUI(UI.MenuUI.pause);
    }
}