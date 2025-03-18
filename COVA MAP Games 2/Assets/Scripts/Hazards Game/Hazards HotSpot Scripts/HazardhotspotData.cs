using UnityEngine;
using FuzzPhyte.Utility;

[CreateAssetMenu(fileName = "hazard hotspot game data", menuName = "HazardsGame/Hazard Game Data", order = 100)]
public class HazardhotspotData : FP_Data 
{
    [Tooltip("Time delay in secfonds for hazards explaination panel")]
    public float theDisplayPanelDelay;
    [Tooltip("Points to override for Hazard Correct")]
    public int thePointsOverrideValue = 2;
    [Tooltip("Update this to reflect the number of hazards in the canvas/scene etc.")]
    public int TotalHazardsToClick = 18;
}
