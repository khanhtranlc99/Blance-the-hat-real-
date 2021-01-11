using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UITabView : MonoBehaviour
{
    [SerializeField] Button[] tabButtonHeaders = null;
    [SerializeField] Transform imgSlider = null;
    [SerializeField] float animTime = 0.5f;

    private int tabSelectedIndex = 0;

    public System.Action<int> OnTabSelected;

    private void Awake()
    {
        for(int i = 0; i < tabButtonHeaders.Length; i++)
        {
            var index = i;
            tabButtonHeaders[i].onClick.AddListener(() => {
                TabSelected(index, true);
            });
        }
    }

    public void FillData()
    {
        TabSelected(tabSelectedIndex);
    }

    private void TabSelected(int index, bool anim = false)
    {
        if(tabSelectedIndex != index)
        {
            OnTabSelected?.Invoke(index);
        }
        tabSelectedIndex = index;

        var x = tabButtonHeaders[tabSelectedIndex].transform.GetLocalPosX();
        imgSlider.DOKill();
        if (anim)
            imgSlider.DOLocalMoveX(x, animTime);
        else
            imgSlider.SetLocalX(x);
        for (int i = 0; i < tabButtonHeaders.Length; i++)
        {
            tabButtonHeaders[i].GetComponent<CanvasGroup>().alpha = i == tabSelectedIndex ? 1 : 0.5f;
        }
    }
}
