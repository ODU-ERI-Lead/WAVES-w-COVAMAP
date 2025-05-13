using UnityEngine;
using DynamicMeshCutter;
using Unity.VisualScripting;
using NUnit.Framework.Constraints;


[System.Serializable]
public class CutterTool_AC : CutterBehaviour

{
    // PlaneBehaviour  maybe unslash
    public GameObject LeftRootTransform;
    public GameObject RightRootTransform;
    public GameObject CutObjectParent;
    public LineRenderer LR => GetComponent<LineRenderer>();

       
    





}







public class CutterTool_Data: PipefitterpartData
{


}