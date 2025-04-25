using UnityEngine;
using FuzzPhyte.Utility;
using System;




[CreateAssetMenu(fileName ="CorrectPartData", menuName = "Pipefitter/CorrectPartData")]
public class PipefitterpartData : FP_Data
{
    [Header("Part_Name")]
    public Array[] Part_type;

    [Header("Part_Specs")]
    public int part_Length;
    public int Roughness;


    [Header("Logic")]


    [Header("Checklist parameters")]
    public bool beenWelded;
    public bool HasConnection;
    public bool Open;
    public bool lockedIn;
    public bool isConnected;
    public bool matchesBlueprint;
    public bool NeedsSanding;
    public bool Sanded;
}
