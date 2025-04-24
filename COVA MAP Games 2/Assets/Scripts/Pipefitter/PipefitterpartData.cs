using UnityEngine;
using FuzzPhyte.Utility;
using System;




[CreateAssetMenu(fileName ="CorrectPartData", menuName = "Pipefitter/CorrectPartData")]
public class PipefitterpartData : FP_Data
{
    [Header("Part_Name")]
    public Array[] Part_type; 

    [Header("Part_Specs")]


    [Header("Logic")]


    [Header("Checklist parameters")]
    public bool beenWelded;
}
