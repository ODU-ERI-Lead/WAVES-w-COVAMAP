using UnityEngine;
using FuzzPhyte.Utility;

[CreateAssetMenu(fileName = "FP_CutToolData", menuName = "FuzzPhyte/Tools/FPCutData")]

public class FP_CutToolData : FP_Data
{
    // Created by AC
    public Color CutIndicator = Color.magenta;
    public Material IndicatorMat;
    public float CutIndicWidth = .1f;
    public UnitOfMeasure measurementUnits = UnitOfMeasure.Inch;
    public string measurementPrefix = "Dis:";
    [Tooltip("pixel amount of offset from whatever placed value you are working with")]
    public Vector3 measurementLabelOffsetPixels = Vector3.zero;
    public float measurementPrecision = 2f; //decimal places
    public GameObject CutPointPrefab;
    public FontSetting CutMeasurementFontSetting;
}
