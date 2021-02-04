using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSugget : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] Image image;
    int random;
    private void OnEnable()
    {
        _Random();
    }
    public void SetItem()
    {
        var selectItem = DataManager.ItemsAsset.list.FirstOrDefault(_ => _.isUnlocked == false);
        if (selectItem == null)
        {
            UiEndGame.ui._SuggetOff();
        }
        else
        {
            image.sprite = selectItem.thumbnail; ;
            button.onClick.AddListener(() =>
            {
                AdsManager.ShowVideoReward((s) =>
                {
                    if (s == AdEvent.Success)
                    {
                        selectItem.isUnlocked = true;
                        UiEndGame.ui._SuggetOff();
                    }
                }, "Suggest_Item", "suggest_item_" + selectItem.id);
            });
        }
    }
    public void SetSkin()
    {
        var selectItem = DataManager.SkinsAsset.list.FirstOrDefault(_ => _.isUnlocked == false);
        if ( selectItem == null)
        {
            UiEndGame.ui._SuggetOff();
        }
        else
        {
            image.sprite = selectItem.thumbnail; ;
            button.onClick.AddListener(() =>
            {
                AdsManager.ShowVideoReward((s) =>
                {
                    if (s == AdEvent.Success)
                    {
                        selectItem.isUnlocked = true;
                        UiEndGame.ui._SuggetOff();
                    }
                }, "Select_Skin", "suggest_skin_" + selectItem.id);
            });
        }

       


    }    
    private void _Random()
    {
        var a = Random.Range(0, 2);
        if( a == 0)
        {
            SetItem();
        }
        else
        {
            SetSkin();
        }
    }
}
