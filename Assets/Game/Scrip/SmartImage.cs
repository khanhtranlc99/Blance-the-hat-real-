using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmartImage : MonoBehaviour
{
    [SerializeField] private Image[] smartImage;
    [SerializeField] private Sprite[] sprites;
    private void Start()
    {
        StartCoroutine(_RandomSprite());
    }

    private IEnumerator _RandomSprite()
    {

        yield return new WaitForSeconds(0.25f);
        var a = Random.Range(0, sprites.Length);
        for( int i = 0; i < smartImage.Length; i ++)
        {
            smartImage[i].sprite = sprites[a];
            smartImage[i].SetNativeSize();
        }
        StartCoroutine(_RandomSprite());
    }    



}
