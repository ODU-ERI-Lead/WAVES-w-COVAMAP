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
    public bool EndPoint = false;
    [Range(0.01f, 0.5f)][Tooltip("Unity Meter to game inch scale")]
    public float MeterInchScale = 0.04f;
    public void PassData(PipeFitterPipeTargetDetails details)
    {
        if (details == null)
        {
            return;
        }
        PartDataReference = details.ConnectionData;
        Length = details.PipeLength / MeterInchScale;
        Debug.Log($"Length values: {Length} | {details.PipeLength} | {MeterInchScale}");
        //Width = details.PipeMesh.GetComponent<MeshFilter>().sharedMesh.bounds.size.x;
    }
}
