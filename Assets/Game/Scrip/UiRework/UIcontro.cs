using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIcontro : MonoBehaviour
{
    public static UIcontro uIcontro;
    public UiMode uiMode;
    public UiSelectItem uiSelectItem;
    public UiHomee uiHomee;
    public UiEndGame uiEndGame;
    public bool backtoLoadgame;
    [SerializeField] Button btnSelecItem;
    [SerializeField] Button btnMode;
    [SerializeField] Button btnHome;
    [SerializeField] GameObject postMouse;
    [SerializeField] Camera camUi;
    [SerializeField] GameObject buttonSetting;
    private void OnEnable()
    {
        this.RegisterListener((int) EventID.GameLose, OnGameLoseHandler);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance?.RemoveListener((int) EventID.GameLose, OnGameLoseHandler);
    }

    private void Awake()
    {
        uIcontro = this;
        ChangeUI(MenuUI.Home);
        btnSelecItem.onClick.AddListener(() => _BtnSeclectOnClick());
        btnMode.onClick.AddListener(() => _BtnModeOnClick());
        //btnHome.onClick.AddListener(() => _BtnHomeOnClick());
    }
    private void Start()
    {
    
    }
    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
     
            SoundManager.Play("taptap");
            var post = camUi.ScreenToViewportPoint(Input.mousePosition);
 
        }

    }
    public void ChangeUI(MenuUI menuUI)
    {
        uiMode.Show(menuUI == MenuUI.Mode);
        uiSelectItem.Show(menuUI == MenuUI.SelectItem);
        uiHomee.Show(menuUI == MenuUI.Home);
    }

    public enum MenuUI
    {
        Mode,
        SelectItem,
        Home
    }
    public void _BtnHomeOnClick()
    {
        ChangeUI(MenuUI.Home);
        Debug.Log("Home");
    }
    public void _BtnModeOnClick()
    {
        ChangeUI(MenuUI.Mode);
        Debug.Log("Mode");
    }
    public void _BtnSeclectOnClick()
    {
        ChangeUI(MenuUI.SelectItem);
        Debug.Log("Seclect0");
    }

    private void OnGameLoseHandler(object param)
    {
        uIcontro.uiEndGame._PrinTime();
    }
    public void _SettingOff()
    {
        if( buttonSetting.activeSelf == true)
        {
            buttonSetting.SetActive(false);
        }
        else
        {
            buttonSetting.SetActive(true);
        }
    }
}
