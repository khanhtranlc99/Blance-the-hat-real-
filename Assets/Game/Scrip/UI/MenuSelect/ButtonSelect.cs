﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class ButtonSelect : MonoBehaviour
{
    [SerializeField] Image imageHome;
    [SerializeField] private Button button;
    [SerializeField] private GameObject playObject;
    [SerializeField] private GameObject priceObject;
    [SerializeField] private GameObject adsObject;
    [SerializeField] private Image priceIcon;
    [SerializeField] private Text priceNumber;

    [Header("Price Icon")] [SerializeField]
    private Sprite iconAds;

    [SerializeField] private Sprite iconGold;
    [SerializeField] private Sprite iconGem;
    [SerializeField] private Sprite iconStar;

    private void Start()
    {
    }

    public void SetButton(ItemData itemData)
    {
        button.onClick.RemoveAllListeners();

        if (itemData.isUnlocked)
        {
            playObject.SetActive(true);
            priceObject.SetActive(false);
            adsObject.SetActive(false);

            button.onClick.AddListener(() =>
            {
                PlayerPrefs.SetInt(Constant.IS_RANDOM_ITEM_PREFS, 0);

                DataManager.CurrentItem.isSelected = false;
                DataManager.CurrentItem = itemData;
                DataManager.CurrentItem.isSelected = true;
                imageHome.sprite = DataManager.CurrentItem.thumbnail;
                imageHome.SetNativeSize();
                if (UIcontro.uIcontro.backtoLoadgame == true)
                {
                    GameStateManager.LoadGame(null);
                    UIcontro.uIcontro.backtoLoadgame = false;
                    UIcontro.uIcontro.ChangeUI(UIcontro.MenuUI.Home);
                }
                else
                {
                    UIcontro.uIcontro.ChangeUI(UIcontro.MenuUI.Home);
                }
            });
        }
        else
        {
            Debug.Log("Item " + itemData.name + " not unlocked");

            playObject.SetActive(false);
            priceObject.SetActive(true);
            adsObject.SetActive(false);

            int unlockRequire = 0;

            switch (itemData.unlockType)
            {
                case UnlockType.Ads:
                    priceIcon.sprite = iconAds;
                    unlockRequire = (itemData.unlockPrice - itemData.unlockPay);
                    break;
                case UnlockType.Gem:
                    priceIcon.sprite = iconGem;
                    break;
                case UnlockType.Gold:
                    priceIcon.sprite = iconGold;
                    unlockRequire = itemData.unlockPrice;
                    break;
                case UnlockType.Star:
                    priceIcon.sprite = iconStar;
                    break;
            }

            priceNumber.text = unlockRequire.ToString();

            button.onClick.AddListener(() =>
            {
                switch (itemData.unlockType)
                {
                    case UnlockType.Ads:

                        AdsManager.ShowVideoReward((s) =>
                        {
                            if (s == AdEvent.Success)
                            {
                                var unlockPlayed = itemData.unlockPay++;
                                if (unlockPlayed >= itemData.unlockPrice - 1)
                                {
                                    itemData.isUnlocked = true;
                                }

                                SetButton(itemData);
                            }
                        }, "Select_Item", "select_item_" + itemData.id);


                        break;
                    case UnlockType.Gem:
                        break;
                    case UnlockType.Gold:
                        if (CoinManager.totalCoin >= itemData.unlockPrice)
                        {
                            CoinManager.Add(-itemData.unlockPrice);
                            itemData.isUnlocked = true;
                        }
                        else
                        {
                            Debug.Log("Not enought coin");
                        }

                        SetButton(itemData);

                        break;
                    case UnlockType.Star:
                        break;
                }
            });
        }
        this.gameObject.SetActive(true);
    }

    public void SetRandomItem()
    {
        button.onClick.RemoveAllListeners();
        playObject.SetActive(true);
        priceObject.SetActive(false);
        adsObject.SetActive(false);
        button.onClick.AddListener(() =>
        {
            PlayerPrefs.SetInt(Constant.IS_RANDOM_ITEM_PREFS, 1);
            UIcontro.uIcontro.ChangeUI(UIcontro.MenuUI.Home);
        });
        this.gameObject.SetActive(true);
    }

    public void SetCoinButton()
    {
        button.onClick.RemoveAllListeners();
        playObject.SetActive(false);
        priceObject.SetActive(false);
        adsObject.SetActive(true);
        button.onClick.AddListener(() =>
        {
            AdsManager.ShowVideoReward((s) =>
            {
                if (s == AdEvent.Success)
                {
                    CoinManager.Add(DataManager.GameConfig.coinAdsReward);
                }
            }, "Select_Item", "select_item_coin_" + DataManager.GameConfig.coinAdsReward);
        });
        this.gameObject.SetActive(true);
    }
}