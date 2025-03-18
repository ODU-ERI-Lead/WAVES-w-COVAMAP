using FuzzPhyte.Dialogue;
using FuzzPhyte.Utility;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class correctHazardMono : UIDialogueContainer
{
    [Space]
    [Header("Hazard Related Needs")]
    public correctHazardData HazardData;
    [SerializeField]
    private float delayTime;
    [SerializeField]
    private int hazardPoints;
    
    public FP_Theme HazardTheme;
    public Image HeaderBackDropRef;
    public Image BodyBackDropRef;
    protected HazardhotspotGameManager gameManager;
    [Space]
    [Header("Unity Events")]
    public UnityEvent OnSetupFinished;
    public UnityEvent OnPopUp;
    [Tooltip("This event is invoked after a period ")]
    public UnityEvent DisplayOver;
    public void Awake()
    {
        //Debug.Log($"Bye");
    }
    public void Start()
    {
        Setup();
    }
    public void UpdateHazardExplainationDelay(float seconds)
    {
        delayTime = seconds;
    }
    public void UpdateHazardPointsOverride(int newPointValue)
    {
        hazardPoints = newPointValue;
    }
    protected void Setup()
    {
        //setup
        //0 = header
        // style guide
        this.UpdateHeaderTextFormat(HazardTheme.FontSettings[0]);
        //actual header data
        this.UpdateHeaderText(HazardData.HeaderText);
        //1 = paragraph
        //style guide
        this.UpdateReferenceTextFormat(HazardTheme.FontSettings[1]);
        //actual paragraph data
        this.UpdateReferenceText(HazardData.ParagraphText);
        //update icon data
        this.UpdateRefIconSprite(HazardData.IconSprite);
        HeaderBackDropRef.color = HazardData.HeaderBackgroundColor;
        BodyBackDropRef.color = HazardData.TextBackgroundColor;
        //cache data for possible override later
        delayTime = HazardData.DisplayTime;
        hazardPoints = HazardData.HazardCorrectPoints;
        //
        OnSetupFinished.Invoke();
        //check to see if there's a game manager out in the wild!
        if (HazardhotspotGameManager.Instance)
        {
            //HazardhotspotGameManager.Instance
            gameManager = (HazardhotspotGameManager)HazardhotspotGameManager.Instance;
            if (gameManager)
            {
                if (HazardData.CanvasLevel == gameManager.CurrentCanvasLevel)
                {
                    gameManager.AddHazardToList(this);
                }
            }
        }
    }
    public void NotifyPoints()
    {
        if (gameManager)
        {
            gameManager.UpdatePoints(hazardPoints);
        }
    }
    public void RunDisplayTimer()
    {
        FP_Timer.CCTimer.StartTimer(delayTime, DisplayTimerDelayFunction);
    }
    /// <summary>
    /// If I need to load this thing on prefab spawn, generate, pass data, and it magically handles itself
    /// </summary>
    /// <param name="hazardD"></param>
    /// <param name="hazardT"></param>
    public void SetupWithDat(correctHazardData hazardD, FP_Theme hazardT)
    {
        HazardData = hazardD;
        HazardTheme= hazardT;
        Setup();
    }
    public void DisplayTimerDelayFunction()
    {
        DisplayOver.Invoke();
    }
}
