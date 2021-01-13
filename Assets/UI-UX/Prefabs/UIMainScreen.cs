using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using System;

public class UIMainScreen : MonoBehaviour
{
    [SerializeField] Button btnPlay;
    [SerializeField] ScrollRect scroll;
    [SerializeField] Button btnInGame;
    public int numbersOfItem;






    public UIAnimStatus Status => anim.Status;

    private UIAnimation anim;

    private ItemsAsset items => DataManager.ItemsAsset;

    private void Awake()
    {
        anim = GetComponent<UIAnimation>();
        btnPlay.onClick.AddListener(() => {  GameStateManager.LoadGame(null); });// goto loading game
       

        for (int i = 0; i < items.list.Count; i++)
        {
            int index = i;
            var btn = Instantiate(btnInGame, scroll.content);
            btn.GetComponentInChildren<Image>().sprite = items.list[i].thumbnail;
            btn.transform.localScale = new Vector3(1, 1, 1);
            btn.onClick.AddListener(() => 
            {
                DataManager.CurrentItem.isSelected = false;
                DataManager.CurrentItem = items.list[index];
                DataManager.CurrentItem.isSelected = true;
                GameStateManager.LoadGame(null);
               
            });
        }    
    }


    public void Show(TweenCallback onStart = null, TweenCallback onCompleted = null)
    {
        InitView();
        anim.Show(onStart, ()=> {
        });
    }

    private void InitView()
    {


    }

    public void Hide()
    {
        anim.Hide();
    }
    public void _OnClickButton()
    {
   
    }
}
