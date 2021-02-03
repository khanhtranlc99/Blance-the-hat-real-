using System;
using EnhancedUI.EnhancedScroller;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemSkinScroll : EnhancedScrollerCellView
{
    [SerializeField] private Image itemImage;
    [SerializeField] private Button button;

    private UnityAction onClickAction;

    private void OnEnable()
    {
        this.RegisterListener((int) EventID.ItemScrollSelect, OnItemSelectHandler);
    }

    private void OnItemSelectHandler(object param)
    {
        var skin = (ItemSkinScroll) param;
        if (skin.Equals(this))
        {
            this.transform.localScale = new Vector2(1.5f, 1.5f);
        }
        else
        {
            this.transform.localScale = Vector2.one;
        }
    }

    public void SetData(ItemSkinScrollData data)
    {
        // update the cell view's UI
        if (data.sprite == null)
        {
            // this is a blank slot, so set the background color to no alpha
            itemImage.color = new Color(0, 0, 0, 0);
        }
        else
        {
            // this slot has an image so set its sprite
            itemImage.sprite = data.sprite;
        }
    }

    public void SetAction(UnityAction action)
    {
        if (onClickAction != null)
        {
            button.onClick.RemoveListener(onClickAction);
        }

        onClickAction = action;
        button.onClick.AddListener(onClickAction);
    }
}