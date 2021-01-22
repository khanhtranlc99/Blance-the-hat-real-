using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UiHomee : ControUI
{
    [SerializeField] private Image image;

    private Sprite[] itemsSprite;
    

    private void Awake()
    {
        itemsSprite = DataManager.ItemsAsset.list.Select(_ => _.thumbnail).ToArray();
    }

    private void OnEnable()
    {
        SetHomeImage();
    }

    private void SetHomeImage()
    {
        if (PlayerPrefs.GetInt(Constant.IS_RANDOM_ITEM_PREFS, 0) == 1)
        {
            StartCoroutine(ChangeItemSpriteRandomly(itemsSprite));
        }
        else
        {
            image.sprite = DataManager.CurrentItem.thumbnail;
            image.SetNativeSize();
        }
    }

    private IEnumerator ChangeItemSpriteRandomly(Sprite[] sprites)
    {
        int i = 0;
        while (true)
        {
            image.sprite = sprites[i % sprites.Length];
            image.SetNativeSize();
            i++;
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}