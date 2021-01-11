using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class UIMainScreen : MonoBehaviour
{
    [SerializeField] RectTransform stagesRect = null;
    [SerializeField] UIStageBtn uiStageBtnPrefab = null;

    public UIAnimStatus Status => anim.Status;

    private UIAnimation anim;
    private StagesAsset stages => DataManager.StagesAsset;
    private List<UIStageBtn> allStages = new List<UIStageBtn>();

    private void Awake()
    {
        anim = GetComponent<UIAnimation>();
        uiStageBtnPrefab.CreatePool(100);
    }


    public IEnumerator FillData()
    {

        if (stages)
        {
            if (allStages.Count == stages.list.Count)
            {
                for (int i = 0; i < allStages.Count; i++)
                {
                    var stageData = stages.list[i];
                    var obj = allStages[i];
                    obj.FillData(stageData);

                    yield return null;
                }
            }
            else
            {
                allStages.Clear();
                uiStageBtnPrefab.RecycleAll();
                for (int i = 0; i < stages.list.Count; i++)
                {
                    var stageData = stages.list[i];
                    var obj = uiStageBtnPrefab.Spawn(stagesRect);
                    obj.FillData(stageData);
                    allStages.Add(obj);

                    yield return null;
                }
            }
        }

        yield return null;
    }


    public void Show(TweenCallback onStart = null, TweenCallback onCompleted = null)
    {
        anim.Show(onStart, ()=> {
        });
    }

    public void Hide()
    {
        anim.Hide();
    }
}
