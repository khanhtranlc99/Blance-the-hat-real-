using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using System;

public class UIMainScreen : MonoBehaviour
{
    [SerializeField] ScrollRect scroll;
    [SerializeField] Button btnInGame;
    [SerializeField] private ButtonSelect btnSelect;
    public int numbersOfItem;
    public UIAnimStatus Status => anim.Status;
    private UIAnimation anim;

    private ItemsAsset items => DataManager.ItemsAsset;

    private void Awake()
    {
        anim = GetComponent<UIAnimation>();

        for (int i = 0; i < items.list.Count; i++)
        {
            int index = i;
            var btn = Instantiate(btnInGame, scroll.content);
            btn.GetComponentInChildren<Image>().sprite = items.list[i].thumbnail;
            btn.transform.localScale = new Vector3(1, 1, 1);
            btn.onClick.AddListener(() => 
            {
                btnSelect.SetButton(items.list[index]);
            });
        }    
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
    public void _OnClickButton()
    {
   
    }
}
