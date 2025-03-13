using FuzzPhyte.Utility;
using UnityEngine;

[CreateAssetMenu(fileName = "correctHazardData", menuName = "HazardsGame/correctHazardData")]
public class correctHazardData : FP_Data
{
    [Header("Header")]
    public string HeaderText;
    public Color HeaderBackgroundColor;
    [Header("Body Text")]
    [TextArea(2,4)]
    public string ParagraphText;
    public Color TextBackgroundColor;
    public Sprite IconSprite;
    [Header("Logic")]
    public float DisplayTime;
}
