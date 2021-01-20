using System;
using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemScroll : EnhancedScrollerCellView
{
    [SerializeField] private Image itemImage;
    [SerializeField] private Button button;
    [SerializeField] private Text text;

    private UnityAction onClickAction;

    private void OnEnable()
    {
        this.RegisterListener((int) EventID.ItemScrollSelect, OnItemSelectHandler);
    }

    public void SetData(ItermScrollData data)
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

    public void SetText(string value)
    {
        text.text = value;
    }

    public void ActiveText(bool isActive)
    {
        text.gameObject.SetActive(isActive);
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

    private void OnItemSelectHandler(object param)
    {
        var item = (ItemScroll) param;
        if (param.Equals(this))
        {
            this.transform.localScale = new Vector2(1.5f, 1.5f);
        }
        else
        {
            this.transform.localScale = Vector2.one;
        }
    }

    private void OnDisable()
    {
        EventDispatcher.Instance?.RemoveListener((int) EventID.ItemScrollSelect, OnItemSelectHandler);
    }
}
