using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI uI;
    public UIGamePlay uiGamePlay;
    public UIMenu uiMenu;
    private void Awake()
    {
        uI = this;
    }

    private void Start()
    {
        ChangeUI(MenuUI.menu);
    }
    public void ChangeUI(MenuUI menuUI)
    {
        uiGamePlay.Show(menuUI == MenuUI.gamePlay);
        uiMenu.Show(menuUI == MenuUI.menu);
    }


    public enum MenuUI
    {

        gamePlay,
        menu

    }
}
