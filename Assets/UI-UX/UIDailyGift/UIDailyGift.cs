using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIDailyGift : UIAnimation
{
    public DailyGiftData data;
    public Transform dailyContainer;
    public ItemDaily item;

    private List<GameObject> listDailyGO = new List<GameObject>();
    

    public override void Show(TweenCallback onStart, TweenCallback onCompleted = null, bool playback = true)
    {
        base.Show(onStart, onCompleted, playback);
        if (data.dailysGift.Count <= 0) return;
        foreach (var daily in data.dailysGift)
        {
            var item = Instantiate(this.item, dailyContainer);
            item.FillData(daily);
            listDailyGO.Add(item.gameObject);
        }
    }

    private void OnDisable()
    {
        foreach (var ob in listDailyGO)
        {
            Destroy(ob);
        }
        listDailyGO.Clear();
    }
}
