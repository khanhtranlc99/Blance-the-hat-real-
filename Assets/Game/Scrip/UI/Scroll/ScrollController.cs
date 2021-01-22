using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EnhancedUI.EnhancedScroller;
using UnityEngine;

public class ScrollController : MonoBehaviour, IEnhancedScrollerDelegate
{
    [SerializeField] private EnhancedScroller scroller;
    [SerializeField] private EnhancedScrollerCellView cellViewPrefab;
    [SerializeField] private EnhancedScroller.TweenType scrollerTweenType;
    [SerializeField] private float scrollerTweenTime = 0.2f;
    [SerializeField] private ButtonSelect btnSelect;

    [SerializeField] private Sprite itemAdsCoinSprite;

    private ItemsAsset items => DataManager.ItemsAsset;
    private List<ItermScrollData> itemsData;


    private void Start()
    {
        scroller.Delegate = this;
        itemsData = new List<ItermScrollData>();
        CreateItems();
        scroller.ReloadData();

        if (PlayerPrefs.GetInt(Constant.IS_RANDOM_ITEM_PREFS, 0) == 1)
        {
            OnItemSelected(2, () =>
            {
                var item = scroller.GetCellViewAtDataIndex(2);
                item.transform.localScale = new Vector2(1.5f, 1.5f);
            });
        }
        else
        {
            var currentItem = DataManager.CurrentItem;
            if (currentItem != null && !currentItem.name.Equals(Constant.COIN_ADS))
            {
                var itemData = itemsData.FirstOrDefault(_ => _.name.Equals(currentItem.name));
                var currentItemIndex = itemsData.IndexOf(itemData);
                OnItemSelected(currentItemIndex, () =>
                {
                    var item = scroller.GetCellViewAtDataIndex(currentItemIndex);
                    item.transform.localScale = new Vector2(1.5f, 1.5f);
                });
            }
        }
    }

    private void CreateItems()
    {
        itemsData.Add(new ItermScrollData()
        {
            name = Constant.COIN_ADS,
            sprite = itemAdsCoinSprite,
        });

        var randomeItem = new ItermScrollData()
        {
            name = Constant.RANDOM_ITEM,
            sprite = itemAdsCoinSprite,
        };

        itemsData.Add(randomeItem);

        for (int i = 0; i < items.list.Count; i++)
        {
            itemsData.Add(new ItermScrollData()
            {
                name = items.list[i].name,
                sprite = items.list[i].thumbnail
            });
        }
    }

    private void OnItemSelected(int index, Action actionComplete = null)
    {
        scroller.JumpToDataIndex(index, 0.5f, 0.5f, true, scrollerTweenType, scrollerTweenTime, actionComplete);
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return itemsData.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return 100;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        ItemScroll item = scroller.GetCellView(cellViewPrefab) as ItemScroll;
        item.name = itemsData[dataIndex].name;
        item.SetData(itemsData[dataIndex]);

        var randomItem = item.GetComponent<RandomItemScroll>();
        if (randomItem)
        {
            Destroy(randomItem);
        }

        if (itemsData[dataIndex].name.Equals(Constant.COIN_ADS))
        {
            item.SetText($"+{DataManager.GameConfig.coinAdsReward}");
            item.ActiveText(true);
            item.SetAction(() =>
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
        else if (itemsData[dataIndex].name.Equals(Constant.RANDOM_ITEM))
        {
            item.gameObject.AddComponent<RandomItemScroll>().ItemImage = item.ItemImage;
            item.SetAction(() =>
            {
                PlayerPrefs.SetInt(Constant.IS_RANDOM_ITEM_PREFS, 1);
                btnSelect.SetRandomItem();
                this.PostEvent((int) EventID.ItemScrollSelect, item);
            });
        }
        else
        {
            item.ActiveText(false);
            item.SetAction(() =>
            {
                PlayerPrefs.SetInt(Constant.IS_RANDOM_ITEM_PREFS, 0);
                OnItemSelected(dataIndex);
                btnSelect.SetButton(items.list[dataIndex - 2]);
                this.PostEvent((int) EventID.ItemScrollSelect, item);
            });
        }

        return item;
    }
}