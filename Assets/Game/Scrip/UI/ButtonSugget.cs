using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSugget : MonoBehaviour
{
    [SerializeField] Button button;
    private void Start()
    {
        SetCoinButton();
    }
    public void SetCoinButton()
    {
       
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
    }
}
