using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIAnimation))]
public abstract class PopupBase<T> : MonoBehaviour where T : PopupBase<T>
{
    [Tooltip("Parent transform to add content inside")]
    public RectTransform contentRect;

    [Header("Animation")]
    [SerializeField]
    private UIAnimation anim = null;

    [SerializeField]
    public GameObject notice = null;

    public UIAnimStatus Status { get => instance.anim != null ? instance.anim.Status : UIAnimStatus.IsHide; }
    public bool IsAnimation => Status == UIAnimStatus.IsAnimationHide || Status == UIAnimStatus.IsAnimationShow;

    [SerializeField]
    protected Button backButton = null;

    protected static T instance;

    protected virtual void Awake()
    {
        if (anim == null)
            anim = GetComponent<UIAnimation>();
        if (notice != null)
            notice?.SetActive(false);

        instance = (T)this;
    }

    protected virtual void Start()
    {

    }

    public abstract void InitData();

    public abstract void FillData();

    public abstract void ResetData();

    public static void ResetAndFill()
    {
        instance?.ResetData();
        instance?.InitData();
        instance?.FillData();
    }

    protected virtual void AnimShow(TweenCallback onStart, TweenCallback onCompleted, TweenCallback onHide)
    {
        InitData();
        FillData();
        instance?.anim?.Show(onStart, onCompleted, onHide);
    }

    protected virtual void AnimHide(TweenCallback onHide)
    {
        instance?.anim?.Hide(onHide);
    }
}
