using FuzzPhyte.Tools.Connections;
using UnityEngine;
using DynamicMeshCutter;
public class PipeFitterPipeTargetDetails : MonoBehaviour
{
    public ConnectionPart ConnectionData;
    public MeshTarget ReferenceToMeshTarget;
    public GameObject LeftEndPoint;
    public GameObject RightEndPoint;
    public GameObject ParentPivot;
    public GameObject PipeMesh;
    [SerializeField]protected float pipeLength;
    public float PipeLength
    {
        get { return pipeLength; }
    }

    public void Start()
    {
        if (ConnectionData.ConnectionPointParent.childCount > 0)
        {
            LeftEndPoint = ConnectionData.ConnectionPointParent.GetChild(0).gameObject;
            RightEndPoint = ConnectionData.ConnectionPointParent.GetChild(1).gameObject;
        }
        
    }

    public void UpdateLength(float newLength)
    {
        pipeLength = newLength;
    }
    public (bool,Vector3) ReturnWorldMidPoint()
    {
        if(RightEndPoint !=null && LeftEndPoint != null)
        {
            return (true,(RightEndPoint.transform.position + LeftEndPoint.transform.position) * 0.5f);
        }
        return (false,Vector3.zero);
    }
}
