﻿using System;
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
        DataManager.OnLoaded += (d) =>
        {
            itemsSprite = DataManager.ItemsAsset.list.Select(_ => _.thumbnail).ToArray();
            SetHomeImage();
        };
    }

    private void OnEnable()
    {
        SetHomeImage();
    }

    private void SetHomeImage()
    {
        if (itemsSprite == null || itemsSprite.Length <= 0) return;
        
        if (PlayerPrefs.GetInt(Constant.IS_RANDOM_ITEM_PREFS, 0) == 1)
        {
            if (!this.gameObject.activeSelf) return;
            StartCoroutine(ChangeItemSpriteRandomly(itemsSprite));
        }
        else
        {
            var currentItem = DataManager.CurrentItem;

            if (currentItem.index == 0)
            {
                if (!this.gameObject.activeSelf) return;
                StartCoroutine(ChangeItemSpriteRandomly(itemsSprite));
                PlayerPrefs.SetInt(Constant.IS_RANDOM_ITEM_PREFS, 1);
            }
            else
            {
                image.sprite = DataManager.CurrentItem.thumbnail;
                image.SetNativeSize();
            }
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