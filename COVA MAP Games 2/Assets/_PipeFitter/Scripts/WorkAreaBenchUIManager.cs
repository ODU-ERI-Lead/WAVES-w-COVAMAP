using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum StateOfUITool
{
    NA=0,
    Cutting=1,
    PipeNotCut=2,
    EveryOtherPart=3
}
public class WorkAreaBenchUIManager : MonoBehaviour
{
    public StateOfUITool CurrentUIState=StateOfUITool.NA;
    public Button CutButton;
    public Button SendToWorkButton;
    public Button RotationButton;
    public Button BluePrintButton;
    
    [Space]
    public Button PipeButton;
    public Button ElbowButton;
    public Button MAdapterButton;
    public Button FAdapterButton;
    public Button ValveButton;
    
    private WaitForEndOfFrame waitforEndFrame = new WaitForEndOfFrame();
    [Space]
    public MoveCutParts MoveCutPartsRef;
    public PipeFitterMouseCutter MouseCutterRef;
    protected List<Button> allPartsButtons = new List<Button>();
    
    public void OnEnable()
    {
        MouseCutterRef.OnFininshedCutting += MouseCutterRef_OnFininshedCutting;
        StartCoroutine(YieldASecond());
        
    }

    private void MouseCutterRef_OnFininshedCutting()
    {
        MouseCutterRef.gameObject.SetActive(false);
    }

    IEnumerator YieldASecond()
    {
        yield return waitforEndFrame;
        allPartsButtons.Clear();
        //assumes all buttons are referenced or you will see an error

        allPartsButtons.Add(PipeButton);
        allPartsButtons.Add(ElbowButton);
        allPartsButtons.Add(MAdapterButton);
        allPartsButtons.Add(FAdapterButton);
        allPartsButtons.Add(ValveButton);
        UpdateAllToolButtons(true);
        CutButton.interactable = false;
        SendToWorkButton.interactable = false;
        RotationButton.interactable = false;
    }
    public void OnDisable()
    {
        allPartsButtons.Clear();
        MouseCutterRef.OnFininshedCutting -= MouseCutterRef_OnFininshedCutting;
    }
    public void UIPipeButtonPushed()
    {
        UpdateAllToolButtons();
        CutButton.interactable = true;
        SendToWorkButton.interactable = true;
        RotationButton.interactable = false;
        CurrentUIState = StateOfUITool.PipeNotCut;
    }
    /// <summary>
    /// Not responsible for anything to with cutting
    /// just managing state for all of our other dumb UI buttons
    /// </summary>
    public void UICutButtonPushed()
    {
        UpdateAllToolButtons();
        CutButton.interactable = false;
        RotationButton.interactable = true;
        SendToWorkButton.interactable = true;
        CurrentUIState = StateOfUITool.Cutting;
    }
    public void UIElbowButtonPushed()
    {
        UpdateAllToolButtons();
        CutButton.interactable = false;
        RotationButton.interactable = true;
        SendToWorkButton.interactable = true;
        CurrentUIState = StateOfUITool.EveryOtherPart;
    }
    public void UIMAdapterButtonPushed()
    {
        UpdateAllToolButtons();
        CutButton.interactable = false;
        RotationButton.interactable = true;
        SendToWorkButton.interactable = true;
        CurrentUIState = StateOfUITool.EveryOtherPart;
    }
    public void UIFAdapterButtonPushed()
    {
        UpdateAllToolButtons();
        CutButton.interactable = false;
        RotationButton.interactable = true;
        SendToWorkButton.interactable = true;
        CurrentUIState = StateOfUITool.EveryOtherPart;
    }
    public void UIValveButtonPushed()
    {
        UpdateAllToolButtons();
        CutButton.interactable = false;
        RotationButton.interactable = true;
        SendToWorkButton.interactable = true;
        CurrentUIState = StateOfUITool.EveryOtherPart;
    }
    /// <summary>
    /// Reference for rotation
    /// </summary>
    public void UIRotationButtonPushed()
    {
        if (MoveCutPartsRef != null)
        {
            //who are we moving?
            var listOfItems = MoveCutPartsRef.ReturnPartsToRotate();
            Debug.LogError($"List of Items Count? {listOfItems.Count}");
            if (listOfItems != null)
            {
                for(int i=0; i < listOfItems.Count; i++)
                {
                    var item = listOfItems[i];
                    Debug.LogError($"Who am I? {item.name}");
                    if (item.GetComponent<OrbitalRotationscript>())
                    {

                        //TheTypeOfOrbital
                        var ourItem = item.GetComponent<OrbitalRotationscript>();

                        //do the deal
                        ourItem.Rotate90Degrees();
                        /*
                        if(ourItem.TheTypeOfOrbital == OrbitalType.Shifted)
                        {
                            //move someamount based on the data
                            var theDetails = ourItem.gameObject.GetComponent<PipeFitterPipeTargetDetails>();
                            if (theDetails != null)
                            {
                                (bool validReturn, Vector3 midPointWorld) = theDetails.ReturnWorldMidPoint();
                                if (validReturn)
                                {
                                    //spawn at midpoint
                                    var fakemidPt = GameObject.Instantiate(new GameObject(), midPointWorld, Quaternion.identity);
                                    ourItem.transform.SetParent(fakemidPt.transform);
                                    ourItem.Rotate90DegreesPassedObject(fakemidPt.transform);
                                    ourItem.transform.SetParent(null);
                                    Destroy(fakemidPt);
                                    
                                }
                            }
                        }
                        else
                        {
                           
                        }
                        */

                    }
                }
            }
        }
    }
    public void UIBluePrintButtonPushed()
    {
        UpdateAllToolButtons();
        CutButton.interactable = false;
        SendToWorkButton.interactable = false;
        RotationButton.interactable = false;
        
    }
    /// <summary>
    /// called from the home button on the work bench area (NOT THE BLUE PRINT HOME)
    /// </summary>
    public void UIBackToAssembly()
    {
        UpdateAllToolButtons();
        CutButton.interactable = false;
        SendToWorkButton.interactable = false;
        RotationButton.interactable = false;
        CurrentUIState = StateOfUITool.NA;
    }
    /// <summary>
    /// Must reference this from the coming out of blue print button
    /// </summary>
    public void UIComingBackFromBluePrint()
    {
        switch (CurrentUIState){
            case StateOfUITool.Cutting:
                UpdateAllToolButtons();
                CutButton.interactable = false;
                RotationButton.interactable = true;
                SendToWorkButton.interactable = true;
                break;
            case StateOfUITool.EveryOtherPart:
                UpdateAllToolButtons();
                CutButton.interactable = false;
                RotationButton.interactable = true;
                SendToWorkButton.interactable = true;
                break;
            case StateOfUITool.PipeNotCut:
                UpdateAllToolButtons();
                CutButton.interactable = true;
                SendToWorkButton.interactable = true;
                RotationButton.interactable = false;
                break;
            case StateOfUITool.NA:
                UpdateAllToolButtons(true);
                CutButton.interactable = false;
                SendToWorkButton.interactable = false;
                RotationButton.interactable = false;
                break;
        }
    }
    public void UISendToWorkButtonPushed()
    {
        //RESET
        UpdateAllToolButtons(true);
        CutButton.interactable = false;
        SendToWorkButton.interactable = false;
        RotationButton.interactable = false;
        CurrentUIState = StateOfUITool.NA;
    }
    protected void UpdateAllToolButtons(bool allButtonState = false,Button notMe=null,bool notMeState=false)
    {
        foreach (Button button in allPartsButtons) 
        {
            button.interactable = allButtonState;
        }
        if (notMe != null)
        {
            notMe.interactable = notMeState;
        }   
    }
}
