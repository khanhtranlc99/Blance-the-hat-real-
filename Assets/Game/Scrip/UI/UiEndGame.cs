using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiEndGame : MonoBehaviour
{

    private void Awake()
    {
       
    }
    

     public void _BackItem()
    {

        GameStateManager.Idle(null);
        UIcontro.uIcontro.ChangeUI(UIcontro.MenuUI.SelectItem);
    }
    public void _BackMode()
    {
        GameStateManager.Idle(null);
        UIcontro.uIcontro.ChangeUI(UIcontro.MenuUI.Mode);
    }







}
