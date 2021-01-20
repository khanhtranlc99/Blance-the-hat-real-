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

    [SerializeField] private string itemAdsCoinName;
    [SerializeField] private Sprite itemAdsCoinSprite;
    
    private ItemsAsset items => DataManager.ItemsAsset;
    private List<ItermScrollData> itemsData;
    

    private void Start()
    {
        scroller.Delegate = this;
        itemsData = new List<ItermScrollData>();
        CreateItems();
        scroller.ReloadData();

        var currentItem = DataManager.CurrentItem;
        if (currentItem != null && !currentItem.name.Equals(itemAdsCoinName))
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

    private void CreateItems()
    {
        itemsData.Add(new ItermScrollData()
        {
            name = itemAdsCoinName,
            sprite = itemAdsCoinSprite,
        });
        
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
        
        if (itemsData[dataIndex].name.Equals(itemAdsCoinName))
        {
            item.SetText($"+{DataManager.GameConfig.coinAdsReward}");
            item.ActiveText(true);
            item.SetAction(() =>
            {
                CoinManager.Add(DataManager.GameConfig.coinAdsReward);
            });
        }
        else
        {
            item.ActiveText(false);
            item.SetAction(() =>
            {
                OnItemSelected(dataIndex);
                btnSelect.SetButton(items.list[dataIndex - 1]);
                this.PostEvent((int) EventID.ItemScrollSelect, item);
            });
        }
        return item;
    }
}
