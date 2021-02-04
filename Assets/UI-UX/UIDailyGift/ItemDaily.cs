using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemDaily : MonoBehaviour
{
    [SerializeField] private Text dayText;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite currentDayBackgroundSprite;
    [SerializeField] private Sprite otherDayBackgroundSprite;
    [SerializeField] private Image currentDayLighting;
    [SerializeField] private Image itemReward;
    [SerializeField] private Text itemText;
    [SerializeField] private GameObject tickClaimed;
    [SerializeField] private Button button;

    public void FillData(DailyGift data)
    {
        itemReward.sprite = data.itemSprire;
        dayText.text = $"Day {data.index + 1}";
        // switch (data.rewardType)
        // {
        //     case RewardType.Coin:
        //         CoinSelection(data.amount);
        //         break;
        //     case RewardType.Item:
        //         ItemSelection(data.rewardID);
        //         break;
        //     case RewardType.Skin:
        //         SkinSelection(data.rewardID);
        //         break;
        // }
        
        button.onClick.AddListener(() =>
        {
            Debug.Log("Click");
        });
    }
}
