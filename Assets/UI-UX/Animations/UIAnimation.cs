using DG.Tweening;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class UIAnimation : MonoBehaviour
{
    [SerializeField]
    protected NavigationType navigation;
    [SerializeField]
    public bool dontPlayWithParent = false;
    [SerializeField]
    private bool hideAtStart = true;
    [SerializeField]
    private bool hideAtEnd = true;
    [SerializeField]
    private bool animationAtStart = false;

    [SerializeField]
    private UIAnimStatus status;
    public UIAnimStatus Status
    {
        get
        {
            return status;
        }
        set
        {
            if (value != status)
            {
                status = value;
                OnStatusChanged?.Invoke(Status);
            }
        }
    }


    public bool IsAnimation => Status == UIAnimStatus.IsAnimationHide || Status == UIAnimStatus.IsAnimationShow;
    public delegate void OnStatusChangedDelegate(UIAnimStatus status);
    public OnStatusChangedDelegate OnStatusChanged;

    [SerializeField]
    protected Type animationIn = Type.FadeIn;
    [SerializeField]
    private MovePosition startPos = MovePosition.ParentPosition;

    [Range(0, 10)]
    [SerializeField]
    public float timeAnimationIn = 0.25f;
    [Range(0, 10)]
    [SerializeField]
    public float timeDelayIn = 0.0f;

    [Serializable]
    public class AnimEvent : UnityEvent { }

    [SerializeField]
    [Tooltip("Event on animation start")]
    private AnimEvent m_StartEvent = new AnimEvent();
    public AnimEvent OnStart { get { return m_StartEvent; } set { m_StartEvent = value; } }

    [SerializeField]
    [Tooltip("Event on animation show completed")]
    private AnimEvent m_ShowCompletedEvent = new AnimEvent();
    [SerializeField]
    public AnimEvent OnShowCompleted { get { return m_ShowCompletedEvent; } set { m_ShowCompletedEvent = value; } }

    [SerializeField]
    protected Type animationOut = Type.FadeOut;
    [SerializeField]
    private MovePosition endPos = MovePosition.ParentPosition;

    [Range(0, 10)]
    [SerializeField]
    public float timeAnimationOut = 0.175f;
    [Range(0, 10)]
    [SerializeField]
    private float timeDelayOut = 0.0f;


    [SerializeField]
    [Tooltip("Event on animation hide completed")]
    private AnimEvent m_HideCompletedEvent = new AnimEvent();
    [SerializeField]
    public AnimEvent OnHideCompleted { get { return m_HideCompletedEvent; } set { m_HideCompletedEvent = value; } }

    public TweenCallback onHideCompleted { get; set; }

    private RectTransform rectTransform { get; set; }
    private CanvasGroup canvasGroup { get; set; }

    private void Awake()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
    }

    private void Start()
    {
        try
        {
            gameObject.SetActive(!hideAtStart);
            if (animationAtStart)
                Show(false);
        }
        catch (Exception ex)
        {
            Debug.LogError("[UIAnimation] " + gameObject.name + "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!" + ex.Message);
        }
    }

    public void Show()
    {
        Show(false);
    }

    public void Show(bool playback)
    {
        Show(null, null, playback);
    }

    public virtual void Show(TweenCallback onStart, TweenCallback onCompleted = null, bool playback = true)
    {
        Show(onStart, onCompleted, null, playback);
    }

    public virtual void Show(TweenCallback onStart, TweenCallback onCompleted, TweenCallback onHide, bool playback = true)
    {
        onHideCompleted = onHide;
        UIManager.DoStartCoroutine(ShowAsync(playback, onStart, onCompleted));
    }

    private IEnumerator ShowAsync(bool playback, TweenCallback onStart = null, TweenCallback onCompleted = null)
    {
        if (UIManager.listScreen.Any(x => x.navigation == navigation && x.status == UIAnimStatus.IsAnimationShow))
            yield break;

        if (UIManager.listPopup.Any(x => x.navigation == navigation && x.status == UIAnimStatus.IsAnimationShow))
            yield break;

        while (IsAnimation)
            yield return null;

        if (Status != UIAnimStatus.IsShow)
        {
            Status = UIAnimStatus.IsAnimationShow;
            Play(animationIn, playback, onStart, onCompleted, null);
        }
    }

    public void Hide()
    {
        Hide(null);
    }

    public virtual void Hide(TweenCallback onCompleted = null)
    {
        UIManager.DoStartCoroutine(HideAsync(onCompleted));
    }

    private IEnumerator HideAsync(TweenCallback onCompleted = null)
    {
        while (IsAnimation)
            yield return null;

        if (Status != UIAnimStatus.IsHide)
        {
            Status = UIAnimStatus.IsAnimationHide;
            Play(animationOut, false, null, null, onCompleted);
        }
    }

    public void Play(Type type, bool playBack = false, TweenCallback onStart = null, TweenCallback onShowCompleted = null, TweenCallback onHideCompleted = null)
    {
        try
        {

            if (navigation == NavigationType.Screen)
            {
                if (!UIManager.listScreen.Contains(this))
                    UIManager.listScreen.Add(this);
                UIManager.CurScreen = this;
            }
            else if (navigation == NavigationType.Popup)
            {
                if (!UIManager.listPopup.Contains(this))
                    UIManager.listPopup.Add(this);
                UIManager.CurPopup = this;
            }

            switch (type)
            {
                case Type.SlideIn:
                    SlideIn(GetRectTransform, GetPosition(!playBack ? startPos : endPos), timeAnimationIn, timeDelayIn, onStart, onShowCompleted);
                    break;
                case Type.SlideOut:
                    SlideOut(GetRectTransform, GetPosition(!playBack ? endPos : startPos), timeAnimationOut, timeDelayOut, onHideCompleted);
                    break;
                case Type.FadeIn:
                    FaceIn(GetRectTransform, timeAnimationIn, timeDelayIn, 1f, onStart, onShowCompleted);
                    break;
                case Type.FadeOut:
                    FaceOut(GetRectTransform, timeAnimationOut, timeDelayOut, 0f, onHideCompleted);
                    break;
                default:
                    Debug.LogWarning("[UIAnimaion] " + startPos + " not implemented!");
                    break;
            }
        }
        catch (Exception ex)
        {
            Status = UIAnimStatus.IsHide;
            Debug.LogError("[UIAnimaion] SlideIn: " + (gameObject == null) + "  " + name + " | " + ex.StackTrace + " " + ex.Message);
        }
    }

    #region Slide Animation
    private void SlideIn(RectTransform rectTransform, Vector2 fromPosition, float timeAnimation, float timeDelay, TweenCallback actionOnStart = null, TweenCallback actionOnComplete = null, Ease ease = Ease.OutCubic)
    {
        if (canvasGroup == null)
            canvasGroup = rectTransform.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
            canvasGroup.alpha = 1;

        gameObject.SetActive(true);

        rectTransform.DOComplete();
        rectTransform.anchoredPosition = fromPosition;
        rectTransform
            .DOAnchorPos(UIManager.startAnchoredPosition2D, timeAnimation, false)
                .SetDelay(timeDelay)
                .SetEase(ease)
                .SetUpdate(UpdateType.Normal, true)
                .OnStart(() =>
                {
                    actionOnStart?.Invoke();
                    OnStart?.Invoke();
                })
                .OnComplete(() =>
                {
                    ShowElements(() =>
                    {
                        Status = UIAnimStatus.IsShow;
                        actionOnComplete?.Invoke();
                        OnShowCompleted?.Invoke();
                    });
                });
    }

    private void SlideOut(RectTransform rectTransform, Vector2 toPosition, float timeAnimation, float timeDelay, TweenCallback actionOnComplete = null, Ease ease = Ease.InCubic)
    {
        //Debug.Log("SlideOut start " + name);
        rectTransform.DOKill(true);
        rectTransform
            .DOAnchorPos(toPosition, timeAnimation, false)
            .SetDelay(timeDelay)
            .SetEase(ease)
            .SetUpdate(UpdateType.Normal, true)
            .OnComplete(() =>
            {
                //Debug.Log("SlideOut end " + name);
                Status = UIAnimStatus.IsHide;
                actionOnComplete?.Invoke();
                OnHideCompleted?.Invoke();
                if (onHideCompleted != null)
                {
                    onHideCompleted.Invoke();
                    onHideCompleted = null;
                }
                gameObject.SetActive(!hideAtEnd);
            });
        HideElements(null);
    }
    #endregion

    #region Face Animation
    private void FaceIn(RectTransform rectTransform, float timeAnimation, float timeDelay, float end = 1f, TweenCallback actionOnStart = null, TweenCallback actionOnComplete = null, Ease ease = Ease.OutCubic)
    {
        if (canvasGroup == null)
            canvasGroup = rectTransform.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = rectTransform.gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;

        gameObject.SetActive(true);
        canvasGroup.DOKill(true);
        rectTransform.DOKill(true);
        rectTransform
            .DOAnchorPos(UIManager.startAnchoredPosition2D, 0).OnComplete(() =>
            {
                canvasGroup
                    .DOFade(end, timeAnimation)
                    .SetDelay(timeDelay)
                    .SetEase(ease)
                    .SetUpdate(UpdateType.Normal, true)
                    .OnStart(() =>
                    {
                        actionOnStart?.Invoke();
                        OnStart?.Invoke();
                    })
                    .OnComplete(() =>
                    {
                        ShowElements(() =>
                        {
                            Status = UIAnimStatus.IsShow;
                            actionOnComplete?.Invoke();
                            OnShowCompleted?.Invoke();
                        });
                    });
            }).SetUpdate(UpdateType.Normal, true);
    }

    private void FaceOut(RectTransform rectTransform, float timeAnimation, float timeDelay, float end = 0f, TweenCallback actionOnComplete = null, Ease ease = Ease.InCubic)
    {
        if (canvasGroup == null)
            canvasGroup = rectTransform.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = rectTransform.gameObject.AddComponent<CanvasGroup>();
        canvasGroup.DOKill(true);
        canvasGroup
            .DOFade(end, timeAnimation)
            .SetDelay(timeDelay)
            .SetEase(ease)
            .SetUpdate(UpdateType.Normal, true)
            .OnComplete(() =>
            {
                Status = UIAnimStatus.IsHide;
                actionOnComplete?.Invoke();
                OnHideCompleted?.Invoke();
                if (onHideCompleted != null)
                {
                    onHideCompleted.Invoke();
                    onHideCompleted = null;
                }
                gameObject.SetActive(!hideAtEnd);
            });
        HideElements(null);
    }
    #endregion

    #region Helper
    public void ShowElements(TweenCallback actionOnCompleted)
    {
        var listAnimation = new List<UIAnimation>();
        for (int i = 0; i < transform.childCount; i++)
        {
            var a = transform.GetChild(i).gameObject.GetComponent<UIAnimation>();
            if (a != null && !a.dontPlayWithParent)
                listAnimation.Add(a);
        }

        if (listAnimation.Any())
        {
            for (int i = 0; i < listAnimation.Count; i++)
            {
                if (i == listAnimation.Count - 1)
                    listAnimation[i].Play(listAnimation[i].animationIn, false, null, actionOnCompleted, null);
                else
                    listAnimation[i].Play(listAnimation[i].animationIn);
            }
        }
        else
        {
            if (actionOnCompleted != null)
                actionOnCompleted.Invoke();
        }
    }

    public void HideElements(TweenCallback actionOnCompleted)
    {
        var listAnimation = new List<UIAnimation>();
        for (int i = 0; i < transform.childCount; i++)
        {
            var a = transform.GetChild(i).gameObject.GetComponent<UIAnimation>();
            if (a != null && !a.dontPlayWithParent)
                listAnimation.Add(a);
        }

        if (listAnimation.Any())
        {
            for (int i = 0; i < listAnimation.Count; i++)
            {
                if (i == listAnimation.Count - 1)
                    listAnimation[i].Play(listAnimation[i].animationOut, false, null, null, actionOnCompleted);
                else
                    listAnimation[i].Play(listAnimation[i].animationOut);
            }
        }
        else
        {
            if (actionOnCompleted != null)
                actionOnCompleted.Invoke();
        }
    }

    public Vector2 GetPosition(MovePosition movePosition)
    {
        try
        {
            RectTransform parent = rectTransform.parent.GetComponent<RectTransform>();  //We need to do this check because when we Instantiate a notification we need to use the uiContainer if the parent is null.
            if (parent == null)
            {
                parent = UIManager.UIContainer.GetComponent<RectTransform>();
            }

            Vector3 targetPosition = UIManager.startAnchoredPosition2D;

            Canvas tempCanvas = rectTransform.GetComponent<Canvas>();
            Canvas rootCanvas = null;

            if (tempCanvas == null) //this might be a button or an UIElement that does not have a Canvas component (this should not happen)
            {
                rootCanvas = rectTransform.root.GetComponentInChildren<Canvas>();
            }
            else
            {
                rootCanvas = tempCanvas.rootCanvas;
            }

            Rect rootCanvasRect = rootCanvas.GetComponent<RectTransform>().rect;
            float xOffset = rootCanvasRect.width / 2 + rectTransform.rect.width * rectTransform.pivot.x;
            float yOffset = rootCanvasRect.height / 2 + rectTransform.rect.height * rectTransform.pivot.y;

            var positionAdjustment = Vector3.zero;
            var positionFrom = Vector3.zero;

            switch (movePosition)
            {
                case MovePosition.ParentPosition:
                    if (parent == null)
                        return targetPosition;

                    targetPosition = new Vector2(parent.anchoredPosition.x + positionAdjustment.x,
                                                 parent.anchoredPosition.y + positionAdjustment.y);
                    break;

                case MovePosition.LocalPosition:
                    if (parent == null)
                        return targetPosition;

                    targetPosition = new Vector2(positionFrom.x + positionAdjustment.x,
                                                 positionFrom.y + positionAdjustment.y);
                    break;

                case MovePosition.TopScreenEdge:
                    targetPosition = new Vector2(positionAdjustment.x + UIManager.startAnchoredPosition2D.x,
                                                 positionAdjustment.y + yOffset);
                    break;

                case MovePosition.RightScreenEdge:
                    targetPosition = new Vector2(positionAdjustment.x + xOffset,
                                                 positionAdjustment.y + UIManager.startAnchoredPosition2D.y);
                    break;

                case MovePosition.BottomScreenEdge:
                    targetPosition = new Vector2(positionAdjustment.x + UIManager.startAnchoredPosition2D.x,
                                                 positionAdjustment.y - yOffset);
                    break;

                case MovePosition.LeftScreenEdge:
                    targetPosition = new Vector2(positionAdjustment.x - xOffset,
                                                 positionAdjustment.y + UIManager.startAnchoredPosition2D.y);
                    break;

                //case MovePosition.TopLeft:
                //    targetPosition = new Vector2(positionAdjustment.x - xOffset,
                //                                 positionAdjustment.y + yOffset);
                //    break;

                //case MovePosition.TopCenter:
                //    targetPosition = new Vector2(positionAdjustment.x,
                //                                 positionAdjustment.y + yOffset);
                //    break;

                //case MovePosition.TopRight:
                //    targetPosition = new Vector2(positionAdjustment.x + xOffset,
                //                                 positionAdjustment.y + yOffset);
                //    break;

                //case MovePosition.MiddleLeft:
                //    targetPosition = new Vector2(positionAdjustment.x - xOffset,
                //                                 positionAdjustment.y);
                //    break;

                //case MovePosition.MiddleCenter:
                //    targetPosition = new Vector2(positionAdjustment.x,
                //                                 positionAdjustment.y);
                //    break;

                //case MovePosition.MiddleRight:
                //    targetPosition = new Vector2(positionAdjustment.x + xOffset,
                //                                 positionAdjustment.y);
                //    break;

                //case MovePosition.BottomLeft:
                //    targetPosition = new Vector2(positionAdjustment.x - xOffset,
                //                                 positionAdjustment.y - yOffset);
                //    break;

                //case MovePosition.BottomCenter:
                //    targetPosition = new Vector2(positionAdjustment.x,
                //                                 positionAdjustment.y - yOffset);
                //    break;

                //case MovePosition.BottomRight:
                //    targetPosition = new Vector2(positionAdjustment.x + xOffset,
                //                                 positionAdjustment.y - yOffset);
                //break;

                default:
                    Debug.LogWarning("[UIAnimaion] This should not happen! DoMoveIn in UIAnimator went to the default setting!");
                    break;
            }

            //Debug.Log("[UIAnimaion] GetPosition: " + targetPosition);
            return targetPosition;
        }
        catch (Exception ex)
        {
            Debug.LogError("[UIAnimaion] GetPosition: " + gameObject.name + " " + ex.Message + "\n" + ex.StackTrace);
            return new Vector3();
        }
    }

    public RectTransform GetRectTransform
    {
        get
        {
            if (rectTransform == null)
                rectTransform = GetComponent<RectTransform>();
            return rectTransform;
        }
    }
    #endregion

    public static void DoShakeScreen(Transform tran, float timeAnimation, float strength, float timeDelay = 0.01f, TweenCallback onDone = null)
    {
        if (onDone != null)
            tran.DOShakePosition(timeAnimation, strength).SetDelay(timeDelay);
        else
            tran.DOShakePosition(timeAnimation, strength).SetDelay(timeDelay).OnComplete(onDone);
    }

    public static void DoScale(Transform tran, float scaleTo = 1.05f, float timeAnimation = 0.25f, float delayTime = 0.01f, bool autoRevese = true)
    {
        if (tran == null)
        {
            Debug.LogError("[UIAnimation] DoScale: " + tran.name + " NULL");
            return;
        }
        tran.DOKill();
        if (autoRevese)
        {
            tran.DOScale(scaleTo, timeAnimation * 0.35f).OnComplete(() =>
            {
                tran.DOScale(1, timeAnimation * 0.65f).SetEase(Ease.OutCubic);
            })
            .SetEase(Ease.InCubic)
            .SetDelay(delayTime);
        }
        else
        {
            tran.DOScale(scaleTo, timeAnimation * 0.35f).SetEase(Ease.InCubic);
        }
    }

    public static void DoMoveZ(Transform tran, float from = 0f, float to = -100f, float timeAnimation = 0.25f, float delayTime = 0.01f, bool autoRevese = false, TweenCallback onDone = null)
    {
        if (tran == null)
        {
            Debug.LogError("[UIAnimation] DoScale: " + tran.name + " NULL");
            return;
        }

        if (autoRevese)
        {
            tran.DOKill();
            tran.localPosition.Set(tran.localPosition.x, tran.localPosition.y, from);
            tran.DOLocalMoveZ(to, timeAnimation * 0.7f).OnComplete(() =>
            {
                if (onDone != null)
                    tran.DOLocalMoveZ(from, timeAnimation * 0.3f).SetEase(Ease.OutCubic).OnComplete(onDone);
                else
                    tran.DOLocalMoveZ(from, timeAnimation * 0.3f).SetEase(Ease.OutCubic);
            })
            .SetEase(Ease.InCubic)
            .SetDelay(delayTime);
        }
        else
        {
            tran.DOKill();
            tran.localPosition.Set(tran.localPosition.x, tran.localPosition.y, from);
            if (onDone != null)
                tran.DOLocalMoveZ(to, timeAnimation).SetEase(Ease.OutCubic).SetDelay(delayTime).OnComplete(onDone);
            else
                tran.DOLocalMoveZ(to, timeAnimation).SetEase(Ease.OutCubic).SetDelay(delayTime);
        }
    }

    public static void DoRotateZ(Transform tran, float to = 90f, float timeAnimation = 0.25f, float delayTime = 0.01f, TweenCallback onDone = null)
    {
        if (tran == null)
        {
            Debug.LogError("[UIAnimation] DoRotateZ: " + tran.name + " NULL");
            return;
        }

        tran.DOKill();
        tran.DORotate(new Vector3(0, 0, to), timeAnimation * 0.7f).OnComplete(() =>
          {
              if (onDone != null)
                  onDone.Invoke();
          })
        .SetEase(Ease.InOutCubic)
        .SetDelay(delayTime);
    }

    public static void DoAlpha(Image img, float from = 1f, float to = 0f, float timeAnimation = 0.25f, float delayTime = 0.01f, bool autoRevese = false, TweenCallback onDone = null)
    {
        if (img == null)
        {
            Debug.LogError("[UIAnimation] DoScale: " + img.name + " NULL");
            return;
        }

        if (autoRevese)
        {
            img.DOKill();
            img.DOFade(from, 0);
            img.DOFade(to, timeAnimation * 0.7f).OnComplete(() =>
            {
                if (onDone != null)
                    img.DOFade(from, timeAnimation * 0.3f).SetEase(Ease.OutCubic).OnComplete(onDone);
                else
                    img.DOFade(from, timeAnimation * 0.3f).SetEase(Ease.OutCubic);
            })
            .SetEase(Ease.InCubic)
            .SetDelay(delayTime);
        }
        else
        {
            img.DOKill();
            img.DOFade(from, 0);
            if (onDone != null)
                img.DOFade(to, timeAnimation).SetEase(Ease.OutCubic).OnComplete(onDone);
            else
                img.DOFade(to, timeAnimation).SetEase(Ease.OutCubic);
        }
    }

    public static void DoNumber(Text text, int startValue, int endValue, string fomat, TweenCallback onDone = null)
    {
        DoNumber(text, startValue, endValue, fomat, 0.01f, "sfx_score_loop", "sfx_score_stop", onDone);
    }

    public static void DoNumber(Text text, int startValue, int endValue, string fomat, float timeAnimationScale = 0.01f, string soundCount = "", string soundCompleted = "", TweenCallback onDone = null)
    {
        int nextValue = startValue;
        int tempValue = startValue;
        text.text = nextValue.ToString();
        DOVirtual.Float(startValue, endValue, Mathf.Clamp(endValue * timeAnimationScale, 0.25f, 1.5f), (e) =>
        {
            tempValue = Mathf.FloorToInt(e);
            if (tempValue != nextValue)
            {
                if (!string.IsNullOrEmpty(soundCount))
                    SoundManager.Play(soundCount);
                nextValue = tempValue;
                text.text = string.Format(fomat, nextValue);
            }
        })
        .OnComplete(() =>
        {
            if (!string.IsNullOrEmpty(soundCompleted))
                SoundManager.Play(soundCompleted);
            text.text = string.Format(fomat, endValue);
            if (onDone != null)
                onDone.Invoke();
        });
    }

    #region Enums - MovePosition, Type, Stage
    public enum MovePosition
    {
        ParentPosition,
        LocalPosition,
        TopScreenEdge,
        RightScreenEdge,
        BottomScreenEdge,
        LeftScreenEdge,
        //TopLeft,
        //TopCenter,
        //TopRight,
        //MiddleLeft,
        //MiddleCenter,
        //MiddleRight,
        //BottomLeft,
        //BottomCenter,
        //BottomRight
    }

    public enum Type
    {
        None,
        SlideIn,
        SlideOut,
        FadeIn,
        FadeOut,
    }

    public enum NavigationType
    {
        None,
        Screen,
        Menu,
        Popup,
        Toast,
    }
    #endregion
}

public enum UIAnimStatus
{
    IsHide,
    IsAnimationShow,
    IsAnimationHide,
    IsShow
}

public enum UIAnimType
{
    Scale,
    Zoom
}