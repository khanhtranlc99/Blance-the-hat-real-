using System;
using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEngine;

public class ScrollController : MonoBehaviour, IEnhancedScrollerDelegate
{
    [SerializeField] private EnhancedScroller scroller;
    [SerializeField] private EnhancedScrollerCellView cellViewPrefab;
    [SerializeField] private EnhancedScroller.TweenType scrollerTweenType;
    [SerializeField] private float scrollerTweenTime = 2f;
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

    private void OnItemSelected(int index)
    {
        scroller.JumpToDataIndex(index, 0.5f, 0.5f, true, scrollerTweenType, scrollerTweenTime);
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
            
        }
        else
        {
            item.SetAction(() =>
            {
                OnItemSelected(dataIndex);
                btnSelect.SetButton(items.list[dataIndex]);
                this.PostEvent((int) EventID.ItemScrollSelect, item);
            });
        }
        return item;
    }
}
