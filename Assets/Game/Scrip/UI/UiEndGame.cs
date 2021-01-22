﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiEndGame : MonoBehaviour
{
    [SerializeField] private Text textTimeEndGame;
    [SerializeField] private Image imageItem;
    [SerializeField] private Text textHightScore;
    private void Awake()
    {
       
    }
    public void _PrinTime()
    {   
        textTimeEndGame.text = "" + GameCoreManager.coreManager.coutnTime + "s";
        textHightScore.text = "" + DataManager.CurrentItem.score;
        imageItem.sprite = DataManager.CurrentItem.thumbnail;
        imageItem.SetNativeSize();
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
