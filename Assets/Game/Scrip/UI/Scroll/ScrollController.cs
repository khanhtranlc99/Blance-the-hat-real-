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

    private int currentIndexItem = 100;

    private void Awake()
    {
        DataManager.OnLoaded += DataManager_OnLoaded;
        scroller.cellViewVisibilityChanged += OnCellViewVisibilityChanged;
    }
    
    private void OnEnable()
    {
        InitScroll();
    }

    private void OnDisable()
    {
        scroller.ClearAll();
        currentIndexItem = 100;
        btnSelect.gameObject.SetActive(false);
    }

    private void DataManager_OnLoaded(GameData gameData)
    {
        InitScroll();
    }
    
    private void OnCellViewVisibilityChanged(EnhancedScrollerCellView cellview)
    {
        if (cellview.dataIndex == currentIndexItem)
        {
            this.PostEvent((int) EventID.ItemScrollSelect, cellview as ItemScroll);
        }
    }

    private void InitScroll()
    {
        if (items == null || items.list.Count < 0) return;
        
        scroller.Delegate = this;
        itemsData = new List<ItermScrollData>();
        CreateItems();
        scroller.ReloadData();

        btnSelect.SetRandomItem();
        OnItemSelected(1, () =>
        {
            var item = scroller.GetCellViewAtDataIndex(1);
            this.PostEvent((int) EventID.ItemScrollSelect, item as ItemScroll);
            btnSelect.SetRandomItem();
        });
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

        for (int i = 1; i < items.list.Count; i++)
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
        currentIndexItem = index;
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
                OnItemSelected(dataIndex);
                btnSelect.SetCoinButton();
                this.PostEvent((int) EventID.ItemScrollSelect, item);
            });
        }
        else if (itemsData[dataIndex].name.Equals(Constant.RANDOM_ITEM))
        {
            item.gameObject.AddComponent<RandomItemScroll>().ItemImage = item.ItemImage;
            item.SetAction(() =>
            {
                
                OnItemSelected(dataIndex);
                btnSelect.SetRandomItem();
                this.PostEvent((int) EventID.ItemScrollSelect, item);
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