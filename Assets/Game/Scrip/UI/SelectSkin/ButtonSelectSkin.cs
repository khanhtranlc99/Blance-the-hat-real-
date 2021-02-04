using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelectSkin : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject playObject;
    [SerializeField] private GameObject priceObject;
    [SerializeField] private Image priceIcon;
    [SerializeField] private Text priceNumber;

    [Header("Price Icon")] [SerializeField]
    private Sprite iconAds;

    [SerializeField] private Sprite iconGold;
    [SerializeField] private Sprite iconGem;
    [SerializeField] private Sprite iconStar;

    public void SetButton(SkinData skinData)
    {
        button.onClick.RemoveAllListeners();

        if (skinData.isUnlocked)
        {
            playObject.SetActive(true);
            priceObject.SetActive(false);

            button.onClick.AddListener(() =>
            {
                DataManager.CurrentSkin.isSelected = false;
                DataManager.CurrentSkin = skinData;
                DataManager.CurrentSkin.isSelected = true;
                GameUIManager.SelectSkinScreen.Hide();
            });
        }
        else
        {
            Debug.Log("Skin " + skinData.name + " not unlocked");
            playObject.SetActive(false);
            priceObject.SetActive(true);

            int unlockRequire = 0;

            switch (skinData.unlockType)
            {
                case UnlockType.Ads:
                    priceIcon.sprite = iconAds;
                    unlockRequire = (skinData.unlockPrice - skinData.unlockPay);
                    break;
                case UnlockType.Gem:
                    priceIcon.sprite = iconGem;
                    break;
                case UnlockType.Gold:
                    priceIcon.sprite = iconGold;
                    unlockRequire = skinData.unlockPrice;
                    break;
                case UnlockType.Star:
                    priceIcon.sprite = iconStar;
                    break;
            }

            priceNumber.text = unlockRequire.ToString();

            button.onClick.AddListener(() =>
            {
                switch (skinData.unlockType)
                {
                    case UnlockType.Ads:

                        AdsManager.ShowVideoReward((s) =>
                        {
                            if (s == AdEvent.Success)
                            {
                                var unlockPlayed = skinData.unlockPay++;
                                if (unlockPlayed >= skinData.unlockPrice - 1)
                                {
                                    skinData.isUnlocked = true;
                                }

                                SetButton(skinData);
                            }
                        }, "Select_Item", "select_skin_" + skinData.id);

                        break;
                    case UnlockType.Gem:
                        break;
                    case UnlockType.Gold:
                        if (CoinManager.totalCoin >= skinData.unlockPrice)
                        {
                            CoinManager.Add(-skinData.unlockPrice);
                            skinData.isUnlocked = true;
                        }
                        else
                        {
                            Debug.Log("Not enought coin");
                        }

                        SetButton(skinData);

                        break;
                    case UnlockType.Star:
                        break;
                }
            });
        }
    }
}