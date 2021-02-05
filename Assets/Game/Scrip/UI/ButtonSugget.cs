using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSugget : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] Image image;
    [SerializeField] private Text textPrite;

    int random;
    private void OnEnable()
    {
        SetItem();
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
            if (selectItem.unlockType == UnlockType.Ads)
            {
                image.sprite = selectItem.thumbnail;
                textPrite.text = "" + selectItem.unlockPrice;
                button.onClick.AddListener(() =>
                {
                    AdsManager.ShowVideoReward((s) =>
                    {
                        if (s == AdEvent.Success)
                        {
                         
                                selectItem.unlockPrice -= 1;
                                textPrite.text = "" + selectItem.unlockPrice;
                            if (selectItem.unlockPrice ==  0)
                            {
                                selectItem.isUnlocked = true;
                                UiEndGame.ui._SuggetOff();
                            }

                        }
                    }, "Sugget_Item", "sugget_Item_" + DataManager.GameConfig.coinAdsReward);
                });
            }
            else
            {
                UiEndGame.ui._SuggetOff();
            }
        }
    }
    public void SetSkin()
    {
        var selectItem = DataManager.SkinsAsset.list.FirstOrDefault(_ => _.isUnlocked == false);
        if (selectItem == null)
        {
            UiEndGame.ui._SuggetOff();
        }
        else
        {
            image.sprite = selectItem.thumbnail;
            textPrite.text = "" + selectItem.unlockPrice;

            button.onClick.AddListener(() =>
            {
                AdsManager.ShowVideoReward((s) =>
                {
                    if (s == AdEvent.Success)
                    {
                        selectItem.unlockPrice -= 1;
                        textPrite.text = "" + selectItem.unlockPrice;
                        if (selectItem.unlockPrice == 0)
                        {
                            selectItem.isUnlocked = true;
                            UiEndGame.ui._SuggetOff();
                        }
                    }
                }, "Select_Skin", "select_Skin_" + DataManager.GameConfig.coinAdsReward);

            });
        }
    }
    private void _Random()
    {
        var a = Random.Range(0, 2);
        if (a == 0)
        {
            SetItem();
        }
        else
        {
            SetSkin();
        }
    }
}
