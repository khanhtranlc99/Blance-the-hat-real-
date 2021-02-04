using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EnhancedUI.EnhancedScroller;
using UnityEngine;

public class SkinScrollController : MonoBehaviour, IEnhancedScrollerDelegate
{
    [SerializeField] private EnhancedScroller scroller;
    [SerializeField] private EnhancedScrollerCellView cellViewPrefab;
    [SerializeField] private EnhancedScroller.TweenType scrollerTweenType;
    [SerializeField] private float scrollerTweenTime = 0.2f;
    [SerializeField] private ButtonSelectSkin btnSelect;
    private SkinAsset skins => DataManager.SkinsAsset;
    private List<ItemSkinScrollData> itemsData;
    
    private int currentIndexItem = 100;
    
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
        ItemSkinScroll item = scroller.GetCellView(cellViewPrefab) as ItemSkinScroll;
        item.name = itemsData[dataIndex].name;
        item.SetData(itemsData[dataIndex]);
        
        item.SetAction(() =>
        {
            OnItemSelected(dataIndex);
            btnSelect.SetButton(skins.list[dataIndex]);
            this.PostEvent((int) EventID.ItemScrollSelect, item);
        });
        
        return item;
    }
    
    private void OnItemSelected(int index, Action actionComplete = null)
    {
        scroller.JumpToDataIndex(index, 0.5f, 0.5f, true, scrollerTweenType, scrollerTweenTime, actionComplete);
        currentIndexItem = index;
    }
    
    private void OnEnable()
    {
        scroller.cellViewVisibilityChanged += OnCellViewVisibilityChanged;
        InitScroll();
    }

    private void OnDisable()
    {
        btnSelect.gameObject.SetActive(false);
        currentIndexItem = 100;
    }

    private void OnCellViewVisibilityChanged(EnhancedScrollerCellView cellview)
    {
        if (cellview.dataIndex == currentIndexItem)
        {
            this.PostEvent((int) EventID.ItemScrollSelect, cellview as ItemSkinScroll);
        }
    }


    private void InitScroll()
    {
        if (skins == null || skins.list.Count < 0) return;
        
        scroller.Delegate = this;
        itemsData = new List<ItemSkinScrollData>();
        CreateItems();
        scroller.ReloadData();

        var skinIndex = skins.list.IndexOf(skins.list.FirstOrDefault(_=>_.Equals(DataManager.CurrentSkin)));
        OnItemSelected(skinIndex, () =>
        {
            var item = scroller.GetCellViewAtDataIndex(skinIndex);
            item.transform.localScale = new Vector2(1.5f, 1.5f);
            btnSelect.SetButton(skins.list[skinIndex]);
        });
    }
    
    private void CreateItems()
    {
        for (int i = 0; i < skins.list.Count; i++)
        {
            itemsData.Add(new ItemSkinScrollData()
            {
                name = skins.list[i].name,
                sprite = skins.list[i].thumbnail
            });
        }
    }
    
}
