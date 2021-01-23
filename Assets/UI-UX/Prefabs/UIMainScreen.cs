using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using System;

public class UIMainScreen : MonoBehaviour
{
    public UIAnimStatus Status => anim.Status;
    private UIAnimation anim;

    private void Awake()
    {
        anim = GetComponent<UIAnimation>();

    
    }

    public void Show(TweenCallback onStart = null, TweenCallback onCompleted = null)
    {
        InitView();
        anim.Show(onStart, onCompleted);
    }

    private void InitView()
    {


    }

    public void Hide()
    {
        anim.Hide();
    }
  public void _ShowItem()
    {
        // for (int i = 0; i < items.list.Count; i++)
        // {
        //     int index = i;
        //     var btn = Instantiate(btnInGame, scroll.content);
        //     btn.GetComponentInChildren<Image>().sprite = items.list[i].thumbnail;
        //     btn.transform.localScale = new Vector3(1, 1, 1);
        //     btn.onClick.AddListener(() =>
        //     {
        //         btnSelect.SetButton(items.list[index]);
        //     });
        // }
    }    
   public void _BackToSelecItemButAfterThatWeGotoLoadGame()
    {
        UIcontro.uIcontro.backtoLoadgame = true;
    }    

}
