using UnityEngine;
using FuzzPhyte.Tools.Connections;



public enum PartType
{
    None= 0, 
    Elbow = 1,
    Valve = 2,  
    MaleAdapter = 3,
    FemaleAdapter = 4,
    StraightPipe =10
}

[RequireComponent(typeof(Collider))]
public class PartChecker : MonoBehaviour
{

    public PartType PipeType;
    public float Length;
    public float Width;
    public ConnectionPart PartDataReference;
    



   






}
