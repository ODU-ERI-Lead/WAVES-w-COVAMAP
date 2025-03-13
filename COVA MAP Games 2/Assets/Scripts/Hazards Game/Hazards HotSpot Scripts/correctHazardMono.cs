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
    public FP_Theme HazardTheme;
    public Image HeaderBackDropRef;
    public Image BodyBackDropRef;
    [Space]
    [Header("Unity Events")]
    public UnityEvent OnSetupFinished;
    public UnityEvent OnPopUp;
    [Tooltip("This event is invoked after a period ")]
    public UnityEvent DisplayOver;
    public void Start()
    {
        Setup();
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
        OnSetupFinished.Invoke();
    }
    public void RunDisplayTimer()
    {
        FP_Timer.CCTimer.StartTimer(HazardData.DisplayTime, DisplayTimerDelayFunction);
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
