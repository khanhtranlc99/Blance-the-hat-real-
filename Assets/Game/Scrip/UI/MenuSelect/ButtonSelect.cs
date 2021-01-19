using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour
{
    [SerializeField] Image imageHome;
    [SerializeField] private Button button;
    [SerializeField] private GameObject playObject;
    [SerializeField] private GameObject priceObject;
    [SerializeField] private Image priceIcon;
    [SerializeField] private Text priceNumber;
    [Header("Price Icon")]
    [SerializeField] private Sprite iconAds;
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
            
            button.onClick.AddListener(() =>
            {
                DataManager.CurrentItem.isSelected = false;
                DataManager.CurrentItem = itemData;
                DataManager.CurrentItem.isSelected = true;
                //GameStateManager.LoadGame(null);   
                UIcontro.uIcontro.ChangeUI(UIcontro.MenuUI.Home);
                imageHome.sprite = DataManager.CurrentItem.thumbnail;
                imageHome.SetNativeSize();
            });
        }
        else
        {
            Debug.Log("Item " + itemData.name + " not unlocked");
            
            playObject.SetActive(false);
            priceObject.SetActive(true);
            priceNumber.text = itemData.unlockPrice.ToString();

            switch (itemData.unlockType)
            {
                case UnlockType.Ads:
                    priceIcon.sprite = iconAds;
                    break;
                case UnlockType.Gem:
                    priceIcon.sprite = iconGem;
                    break;
                case UnlockType.Gold:
                    priceIcon.sprite = iconGold;
                    break;
                case UnlockType.Star:
                    priceIcon.sprite = iconStar;
                    break;
            }
            
            button.onClick.AddListener(() =>
            {
                switch (itemData.unlockType)
                {
                    case UnlockType.Ads:
                        break;
                    case UnlockType.Gem:
                        break;
                    case UnlockType.Gold:
                        if (CoinManager.totalCoin >= itemData.unlockPrice)
                        { 
                            CoinManager.Add(- itemData.unlockPrice);
                            itemData.isUnlocked = true;

                            SetButton(itemData);
                        }
                        else
                        {
                            Debug.Log("Not enought coin");
                        }
                        break;
                    case UnlockType.Star:
                        break;
                }

            });
        }
    }
}
