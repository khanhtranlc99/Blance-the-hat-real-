using System;
using System.Linq;
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class DataManager : MonoBehaviour
{
    #region STATIC
    public static GameConfig GameConfig => instance.configAsset.gameConfig;
    public static UserData UserData
    {
        get { return gameData?.user; }
    }
    public static StageData CurrentStage
    {
        get => StagesAsset?.Current;
        set => StagesAsset.Current = value;
    }
    public static ItemData CurrentItem
    {
        get => ItemsAsset?.Current;
        set => ItemsAsset.Current = value;
    }
    public static StagesAsset StagesAsset { get; private set; }
    public static ItemsAsset ItemsAsset { get; private set; }
    public static GameData gameData { get; private set; }
    private static DataManager instance { get; set; }

    #endregion

    [Space(10)]
    [Header("Default Data")]
    [SerializeField]
    protected ConfigAsset configAsset = null;
    [SerializeField]
    protected StagesAsset stagesAsset = null;
    [SerializeField]
    protected ItemsAsset itemsAsset = null;

    public static bool IsFirstTime = false;

    [Header("GameData auto SAVE LOAD")]
    [SerializeField]
    protected bool loadOnStart = true;
    [SerializeField]
    protected bool saveOnPause = true;
    [SerializeField]
    protected bool saveOnQuit = true;

    public delegate void LoadedDelegate(GameData gameData);
    public static event LoadedDelegate OnLoaded;

    #region BASE
    private void Awake()
    {
        instance = this;
        ItemsAsset = itemsAsset;
    }


    private void Start()
    {
        if (loadOnStart)
            Load();
    }

    public static void Save(bool saveCloud = true)
    {
        if (instance && gameData != null && gameData.user != null)
        {
            var time = DateTime.Now;
            gameData.user.LastTimeUpdate = DateTime.Now;
            gameData.stages = StagesAsset.stageSaveList;
            gameData.items = StagesAsset.itemSaveList;

            Debug.Log("ConvertData in " + (DateTime.Now - time).TotalMilliseconds + "ms");
            FileExtend.SaveData<GameData>("GameData", gameData);
            Debug.Log("SaveData in " + (DateTime.Now - time).TotalMilliseconds + "ms");

            if (saveCloud)
            {
                //Save cloud in here;
                Debug.Log("Save cloud is not implement!");
            }
        }
    }

    public static IEnumerator DoLoad()
    {
        if (instance)
        {
            var elapsedTime = 0f;
            if (gameData == null)
                Load();
            else
                Debug.LogWarning("GameData not NULL");

            while (gameData == null || StagesAsset == null)
            {
                if (elapsedTime < 5)
                {
                    Debug.LogWarning("GameData load " + elapsedTime.ToString("0.0"));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
            }
        }
    }

    public static void Load()
    {
        var time = DateTime.Now;
        if (instance)
        {
            //Create default
            var tempData = new GameData();

            if (StagesAsset == null)
            {
                StagesAsset = ScriptableObject.CreateInstance("StagesAsset") as StagesAsset;
                foreach (var i in instance.stagesAsset.list)
                    StagesAsset.list.Add(i);
            }
            else
                Debug.Log("stageDatas is not NULL");

            

            //Load gamedata
            GameData loadData = FileExtend.LoadData<GameData>("GameData") as GameData;
            if (loadData != null)
            {

                if (loadData.stages != null && loadData.stages.Any())
                    StagesAsset.ConvertToData(loadData.stages);

                if (loadData.user != null)
                {
                    tempData.user = loadData.user;
                    if ((DateTime.Now - tempData.user.LastTimeUpdate).TotalSeconds >= 15 * 60)
                        tempData.user.Session++;
                    tempData.user.totalStar = StagesAsset.list.Sum(s => s.star);

                    if (tempData.user.VersionInstall == 0)
                        tempData.user.VersionInstall = UIManager.BundleVersion;
                    tempData.user.VersionCurrent = UIManager.BundleVersion;
                }

                Debug.Log("LoadData in " + (DateTime.Now - time).TotalMilliseconds + "ms");
            }
            else
            {
                tempData.user.Session++;
                Debug.Log("CreateData in " + (DateTime.Now - time).TotalMilliseconds + "ms");
            }
            gameData = tempData;

            if (gameData.user.TotalTimePlay == 0)
            {
                IsFirstTime = true;
                gameData.user.TotalTimePlay++;
            }
        }
        else
        {
            throw new Exception("Data Manager instance is NULL. Maybe it hasn't been created.");
        }
        OnLoaded?.Invoke(gameData);
    }

    public static StageData Next()
    {
        return StagesAsset.Next();
    }
    public static void Reset()
    {
        var path = FileExtend.FileNameToPath("GameData.gd");
        FileExtend.Delete(path);
        PlayerPrefs.DeleteAll();
        Debug.Log("Reset game data");
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause && !GameStateManager.isBusy && saveOnPause)
            Save(false);
    }

    private void OnApplicationQuit()
    {
        if (saveOnQuit)
            Save(true);
    }

    public void ResetAndUpdateData()
    {
        try
        {
            stagesAsset.ResetData();
            stagesAsset.UpdateCost();
            itemsAsset.ResetData();
            Reset();
            Debug.Log("Reset and Update data to BUILD!!!");
        }
        catch (Exception ex)
        {
            Debug.LogError("Please update and save DATA before build!!!");
            Debug.LogException(ex);
        }
    }
    #endregion

}