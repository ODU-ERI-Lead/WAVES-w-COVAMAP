using UnityEngine;
using FuzzPhyte.Tools;
using FuzzPhyte.Utility.FPSystem;
using FuzzPhyte.Tools.Connections;
public class PFMouseHangerListener : FP_UIInputListener<FP_Tool<PFHangerData>>
{
    /// <summary>
    /// Called from a UI Item like a Button
    /// </summary>
    /// <param name="theTool"></param>
    public void UI_ToolActivated(FP_Tool<PFHangerData> theTool)
    {
        SetCurrentEngagedData(theTool);
        SetCurrentEngagedGameObject(theTool.gameObject);
    }
}
