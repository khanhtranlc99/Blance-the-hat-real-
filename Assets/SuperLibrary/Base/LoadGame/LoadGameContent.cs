using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class LoadGameContent : MonoBehaviour
{
    public static StageColorData currStageColorData;
    
    private static LoadGameContent instance { get; set; }

    private string[] tips = new string[]
    {
        "[TIP] Tip 1 Descriptions!!!" ,
        "[TIP] Tip 2 Descriptions!!!",
        "[TIP] Tip 3 Descriptions!!!"
    };

    private string currentMode = "";
    private string[] modes = { "beginner", "advance", "expert", "master" }; 
    private void Awake()
    {
        instance = this;

        
        //0.0 -> 1.0
        FileExtend.OnProcessChanged += (process) =>
        {
            if (process > 0 && process < 1)
            {
                if (loadAudioStatus == FileStatus.Download)
                {
                    UILoadGame.Process(0.1f, 0.7f, process);
                }
                else if (loadMidiStatus == FileStatus.Download)
                {
                    UILoadGame.Process(0.7f, 0.8f, process);
                }
            }
        };
    }


    protected string randomTip => tips[UnityEngine.Random.Range(0, tips.Length)];

    public static void PrepairDataToPlay(StageData stage, Action<bool> actionOnDone = null)
    {
        instance.StartCoroutine(instance.DoPrepairDataToPlay(stage, actionOnDone));
    }
    public static void ChangeMode(int index)
    {
        instance.currentMode = instance.modes[index];
    }

    protected float elapsedTime = 0f;
    protected FileStatus loadAudioStatus = FileStatus.Download;
    protected FileStatus loadMidiStatus = FileStatus.Download;
    private IEnumerator DoPrepairDataToPlay(StageData stage, Action<bool> actionOnDone = null)
    {

        UIToast.ShowLoading(randomTip, 5f, UIToast.IconTip);
        UILoadGame.Init(true, null);

        while (UILoadGame.currentProcess < 0.1f)
        {
            UILoadGame.Process(0, 1, -1, LocalizedManager.Key("base_Loading") + LocalizedManager.Key("base_PleaseWait"));
            yield return null;
        }

        if (UIToast.Status != UIAnimStatus.IsShow)
            UIToast.ShowLoading(randomTip, 5f, UIToast.IconTip);

        MusicManager.Stop(null, false, 0.25f);

        if(string.IsNullOrEmpty(currentMode))
        {
            currentMode = modes[0];
        }

        //Init game object
        elapsedTime = 0f;
        loadMidiStatus = FileStatus.Download;
        loadMidiStatus = FileStatus.Success;

        GameStateManager.Init(null);

        while (GameStateManager.CurrentState == GameState.Init || UILoadGame.currentProcess < 1)
        {
            if (GameStateManager.CurrentState != GameState.Idle && elapsedTime < 5)
            {
                UILoadGame.Process();
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            else
            {
                yield return UILoadGame.DoRollBack(() =>
                {
                    UIToast.Hide();
                    actionOnDone(false);
                });
                yield break;
            }
        }

        UIToast.Hide();
        actionOnDone(true);
    }

    public void ShowError(FileStatus status)
    {
        string note = "";

        if (status == FileStatus.TimeOut || status == FileStatus.NoInternet)
            note = LocalizedManager.Key("base_DownloadFirstTime") + "\n" + "\n";
        if (status == FileStatus.TimeOut)
        {
            note += LocalizedManager.Key("base_DownloadTimeOut");
        }
        else if (status == FileStatus.NoInternet)
        {
            note += LocalizedManager.Key("base_PleaseCheckYourInternetConnection");
        }
        else
        {
            note += LocalizedManager.Key("base_SomethingWrongs") + "\n ERROR #" + status;
        }
        PopupMes.Show("Oops...!", note, "Ok");
    }
}

public class StageColorData
{
    public int numStack = 0;
    public List<int> bubbleTypes = new List<int>();
}