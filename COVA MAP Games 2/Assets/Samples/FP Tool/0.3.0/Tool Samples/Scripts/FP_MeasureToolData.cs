namespace FuzzPhyte.Tools.Samples
{
    using FuzzPhyte.Utility;
    using UnityEngine;

    [CreateAssetMenu(fileName = "FP_MeasureToolData", menuName = "FuzzPhyte/Tools/FPMeasureData")]
    public class FP_MeasureToolData : FP_Data
    {
        public Color lineColor = Color.green;
        public Material LineMat;
        public float lineWidth = 0.02f;
        [Tooltip("Not needed on 2D, only 3D - for depth")]
        public float lineDepth = 1f;
        public UnitOfMeasure measurementUnits=UnitOfMeasure.Meter;
        public string measurementPrefix = "Dis:";
        [Tooltip("Pixel amount of offset from whatever placed value you are working with")]
        public Vector3 measurementLabelOffsetPixels = Vector3.zero;
        public float measurementPrecision = 2f; // Decimal places
        public FontSetting MeasurementFontSetting;
        public GameObject MeasurementPointPrefab;
        public float RaycastMax = 15;
    }
}
