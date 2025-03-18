using UnityEngine;
using FuzzPhyte.Utility.FPSystem;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class HazardhotspotGameManager :FPSystemBase<HazardhotspotData>
{
    [Space]
    [Header("Hazard Parameters For Setup")]
    public bool OverrideInfoPanelTime=true;
    public bool OverrideHazardPoints = false;
    [SerializeField]
    protected int totalPoints=0;
    public FractionText ScoreDisplaySystem;
    public List<correctHazardMono> AllHotSpotExplainations = new List<correctHazardMono>();
    public Canvas CanvasLevelZeroParent;
    //level zero data is coming in from InfoPanelScript!
    public UnityEngine.UI.Button LevelZeroEndButton;
    public Canvas CanvasLevelOneParent;
    public HazardhotspotData LevelOneData;
    public UnityEngine.UI.Button LevelOneEndButton;
    public Canvas CanvasLevelTwoParent;
    public HazardhotspotData LevelTwoData;
    public UnityEngine.UI.Button LevelTwoEndButton;
    public int CurrentCanvasLevel = 0;
    /// <summary>
    /// This needs to be called at the beginning of each 'Canvas Level'
    /// It automatically gets called our first time (Canvas Level zero) with the InfoPanelScript
    /// </summary>
    /// <param name="runAfterLateUpdateLoop"></param>
    /// <param name="data"></param>
    public override void Initialize(bool runAfterLateUpdateLoop, HazardhotspotData data = null)
    {
        //base.Initialize(runAfterLateUpdateLoop, data);
        systemData = data;
        //wtf are we doing

        foreach (var item in AllHotSpotExplainations)
        {
            if (OverrideInfoPanelTime)
            {
                item.UpdateHazardExplainationDelay(systemData.theDisplayPanelDelay);
            }
            if (OverrideHazardPoints) 
            {
                item.UpdateHazardPointsOverride(systemData.thePointsOverrideValue);
            }
        }
        //double check our max hazards from data to what we have in the scene
        // divide correctHazardMono by 2
        if (systemData.TotalHazardsToClick != (AllHotSpotExplainations.Count / 2f))
        {
            Debug.LogError($"Something is off with how many we have in our data file vs how many Hazard Colliders are in the scene... we think...");
        }
        else
        {
            //update FractionText Display for Max Number ends values
        }
        UpdatePoints(0);
           
    }

    public void UpdatePoints(int ptValue)
    {
        totalPoints += ptValue;
        if (ScoreDisplaySystem != null) 
        {
            ScoreDisplaySystem.DisplayUpdated(totalPoints,systemData.TotalHazardsToClick);
        }
        if (totalPoints >= systemData.TotalHazardsToClick)
        {
            //we have in theory hit max/ found them all
            if (CurrentCanvasLevel == 0)
            {
                LevelZeroEndButton.gameObject.SetActive(true);
            }
            if(CurrentCanvasLevel == 1)
            {
                LevelOneEndButton.gameObject.SetActive(true);
            }
            if(CurrentCanvasLevel == 2)
            {
                //FUCKING DONE
                LevelTwoEndButton.gameObject.SetActive(true);
            }
            totalPoints = 0;
        }
    }
    // You will need to call this when you end your Hazard Game
    // you
    public void CanvasLevelChange()
    {
        AllHotSpotExplainations.Clear();
        //update your new level value
        CurrentCanvasLevel++;
        if (CurrentCanvasLevel == 1)
        {
            CanvasLevelZeroParent.gameObject.SetActive(false);
            CanvasLevelTwoParent.gameObject.SetActive(false);
            CanvasLevelOneParent.gameObject.SetActive(true);
            StartCoroutine(DelayCanvasDataSetup(LevelOneData));
        }
        
        if (CurrentCanvasLevel == 2) 
        {
            CanvasLevelTwoParent.gameObject.SetActive(true);
            CanvasLevelZeroParent.gameObject.SetActive(false);
            CanvasLevelOneParent.gameObject.SetActive(false);
            StartCoroutine(DelayCanvasDataSetup(LevelTwoData));
        }
    }
    IEnumerator  DelayCanvasDataSetup(HazardhotspotData dataToReset)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        this.Initialize(false, dataToReset);

    }
    #region Updating Lists
    public void AddHazardToList(correctHazardMono aHazard)
    {
        if (!AllHotSpotExplainations.Contains(aHazard))
        {
            AllHotSpotExplainations.Add(aHazard);
        }
    }
    public void RemoveHazardFromList(correctHazardMono aHazard)
    {
        if (AllHotSpotExplainations.Contains(aHazard))
        {
            AllHotSpotExplainations.Remove(aHazard);
        }
    }
    #endregion

}
