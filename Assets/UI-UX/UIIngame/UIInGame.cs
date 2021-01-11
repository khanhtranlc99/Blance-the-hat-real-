using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIAnimation))]
public class UIInGame : MonoBehaviour
{
    [Header("Base")]
    [SerializeField]
    private UIAnimation anim = null;
    public UIAnimStatus Status => anim.Status;

    [SerializeField]
    private Button pauseButton = null;
    [SerializeField]
    private Button resumeButton = null;
    [SerializeField]
    private Button backButton = null;
    [SerializeField]
    private Button playButton = null;
    [SerializeField]
    private Button addButton = null;
    [SerializeField]
    private Button stepButton = null;
    [SerializeField]
    private Text stageName = null;


    private void Awake()
    {
        if (anim == null)
            anim = GetComponent<UIAnimation>();
        GameStateManager.OnStateChanged += GameStateManager_OnStateChanged;
    }

    private void Start()
    {
        pauseButton?.onClick.AddListener(() =>
        {
            GameStateManager.Pause(null);
        });

        backButton?.onClick.AddListener(backButtonOnClick);

        resumeButton?.onClick.AddListener(() =>
        {
        });

        addButton?.onClick.AddListener(() => {
        });
        stepButton?.onClick.AddListener(() => {
        });
    }

    private void backButtonOnClick()
    {
        if (GameStateManager.CurrentState == GameState.Pause)
        {
            PopupMes.Show("BACK!?", "Do you really want back to main!?",
                "No", null,
                "Ok", () => GameStateManager.Idle(null));
        }
    }

    private void GameStateManager_OnStateChanged(GameState current, GameState last, object data)
    {
        switch (current)
        {
            case GameState.Init:
                if (last != GameState.RebornContinue && last != GameState.Restart)
                    playButton?.gameObject.SetActive(true);
                pauseButton?.gameObject.SetActive(false);
                resumeButton?.gameObject.SetActive(false);
                backButton?.gameObject.SetActive(false);
                addButton?.gameObject.SetActive(false);
                stepButton?.gameObject.SetActive(false);
                if (stageName)
                    stageName.text = DataManager.CurrentStage.name;
                break;
            case GameState.Play:
                playButton?.gameObject.SetActive(false);
                pauseButton?.gameObject.SetActive(true);
                resumeButton?.gameObject.SetActive(false);
                backButton?.gameObject.SetActive(false);
                addButton?.gameObject.SetActive(true);
                stepButton?.gameObject.SetActive(true);
                break;
            case GameState.Pause:
                pauseButton?.gameObject.SetActive(false);
                resumeButton?.gameObject.SetActive(true);
                backButton?.gameObject.SetActive(true);
                addButton?.gameObject.SetActive(false);
                stepButton?.gameObject.SetActive(false);

                break;
            case GameState.GameOver:
                resumeButton?.gameObject.SetActive(false);
                pauseButton?.gameObject.SetActive(false);
                backButton?.gameObject.SetActive(false);
                addButton?.gameObject.SetActive(false);
                stepButton?.gameObject.SetActive(false);
                break;
            case GameState.Complete:
                break;
        }
    }

    public void Show()
    {
        anim.Show();
    }

    public void Hide()
    {
        anim.Hide();
    }

    public void Ins_BtnGameOver()
    {
        GameStatisticsManager.Score = Random.Range(1000, 10000);
        GameStatisticsManager.Stars = Random.Range(1, 3);
        GameStatisticsManager.Combo = Random.Range(50, 100);
        GameStatisticsManager.Perfect = 99;
        GameStateManager.WaitGameOver(null);
    }
    public void Ins_BtnGameComplete()
    {
        GameStatisticsManager.Score = Random.Range(1000, 10000);
        GameStatisticsManager.Stars = 3;
        GameStatisticsManager.Combo = Random.Range(50, 100);
        GameStatisticsManager.Perfect = 99;
        GameStateManager.WaitComplete(null);
    }
    public void Ins_BtnBack()
    {
        GameStateManager.Idle(null);
    }
    public void Ins_Toast()
    {
        UIToast.ShowNotice("This is toast!!");
    }
    public void Ins_Popup()
    {
        PopupMes.Show("This is Popup", "This is popup description", 
            "OK", ()=> {
                Debug.Log("Popup -> OK");
            },
            "Cancel", ()=> {
                Debug.Log("Popup -> Cancel");
            });
    }
}
