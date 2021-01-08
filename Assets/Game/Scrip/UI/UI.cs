using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI uI;
    public UIGamePlay uiGamePlay;
    public UIMenu uiMenu;
    public UIPause uiPause;
    public GameObject keyObject;
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
        uiPause.Show(menuUI == MenuUI.pause);
    }


    public enum MenuUI
    {

        gamePlay,
        menu,
        pause

    }
}
