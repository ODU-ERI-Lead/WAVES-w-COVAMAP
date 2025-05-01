using UnityEngine;
using FuzzPhyte.Utility;
using System;
using FuzzPhyte.Tools.Connections;



    [System.Serializable]
[CreateAssetMenu(fileName ="CorrectPartData", menuName = "Pipefitter/CorrectPartData")]
public class PipefitterpartData : FP_Data
{
    [Header("Part_Name")]
    public Array[] Part_type;
    public string PartName;
    public int CorrectPart_ID;

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


    [Header("Debug")]
    public bool debugOutput = false;

    public bool Matches(PipefitterpartData other)
    {
        if (other == null) return false;

        bool match =
            PartName == other.PartName &&
            CorrectPart_ID == other.CorrectPart_ID &&
            part_Length == other.part_Length &&
            Roughness == other.Roughness &&
            beenWelded == other.beenWelded &&
            HasConnection == other.HasConnection &&
            Open == other.Open &&
            lockedIn == other.lockedIn &&
            isConnected == other.isConnected &&
            matchesBlueprint == other.matchesBlueprint &&
            NeedsSanding == other.NeedsSanding &&
            Sanded == other.Sanded;

        if (debugOutput)
            Debug.Log($"Match result for {name}: {match}");

        return match;
    }


}
