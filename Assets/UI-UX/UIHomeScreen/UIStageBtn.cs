using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStageBtn : MonoBehaviour
{
    [SerializeField]
    private Text txtStage = null;
    [SerializeField]
    private Text txtTime = null;
    [SerializeField]
    private Image imgLock = null;
    [SerializeField]
    private Image imgPlay = null;
    [SerializeField]
    private GameObject starGroup = null;
    [SerializeField]
    private GameObject[] starChildren = null;

    private StageData currStageData;
    /// <summary>
    /// /
    public int id;




    public void FillData(StageData stageData)
    {
        currStageData = stageData;
        if(!currStageData.isUnlocked && currStageData.unlockType == UnlockType.Star 
            && currStageData.unlockPrice <= DataManager.UserData.totalStar)
        {
            currStageData.isUnlocked = true;
        }


        txtStage.text = stageData.name;
        txtTime.text = stageData.time.ToString("00.00");

        txtTime.gameObject.SetActive(stageData.isUnlocked && stageData.time > 0);
        imgLock.gameObject.SetActive(!stageData.isUnlocked);
        imgPlay.gameObject.SetActive(stageData.isUnlocked && stageData.time <= 0);

        starGroup.SetActive(stageData.totalComplete > 0);
        for(int i = 0; i < starChildren.Length; i++)
        {
            starChildren[i].SetActive(i < stageData.star);
        }
    }

    public void Ins_OnSelected()
    {

        if (currStageData.isUnlocked)
        {
            DataManager.CurrentStage = currStageData;
            GameStateManager.LoadGame(null);
        }
        else
        {
            UIToast.ShowNotice($"{currStageData.name} is locked!");
        }
  
    }
}
