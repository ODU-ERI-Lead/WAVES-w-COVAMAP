using FuzzPhyte.Utility;
using UnityEngine;

[CreateAssetMenu(fileName = "Hanger Data", menuName = "Pipefitter/Tools/HangerData")]
public class PFHangerData : FP_Data
{
    public float RaycastMax=5;
    public GameObject HangerDecalPrefab;
}
