using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiHomee : ControUI
{
    [SerializeField] private Image image;
    public override void Show(bool value)
    {
        base.Show(value);
    }
    private void Awake()
    {
         image.sprite = DataManager.CurrentItem.thumbnail;
        image.SetNativeSize();
    }

}
