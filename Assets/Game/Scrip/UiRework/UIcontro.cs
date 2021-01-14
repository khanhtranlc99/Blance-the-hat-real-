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
    [SerializeField] Button btnSelecItem;
    [SerializeField] Button btnMode;
    [SerializeField] Button btnHome;
    private void Awake()
    {
        uIcontro = this;
        ChangeUI(MenuUI.Home);
        btnSelecItem.onClick.AddListener(() => _BtnSeclectOnClick());
        btnMode.onClick.AddListener(() => _BtnModeOnClick());
        btnHome.onClick.AddListener(() => _BtnHomeOnClick());
    }
    private void Start()
    {
    
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
}
