using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RandomItemScroll : MonoBehaviour
{
    private Image itemImage;

    public Image ItemImage
    {
        get => itemImage;
        set => itemImage = value;
    }

    private void OnEnable()
    {
        var itemsSprite = DataManager.ItemsAsset.list.Select(_ => _.thumbnail).ToArray();
        StartCoroutine(ChangeItemSpriteRandomly(itemsSprite));
    }
    

    private IEnumerator ChangeItemSpriteRandomly(Sprite[] sprites)
    {
        int i = 0;
        while (true)
        {
            if (itemImage)
            {
                itemImage.sprite = sprites[i % sprites.Length];
                i++;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}