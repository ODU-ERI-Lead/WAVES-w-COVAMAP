using FuzzPhyte.Tools.Connections;
using UnityEngine;

public class PipeFitterPipeTargetDetails : MonoBehaviour
{
    public ConnectionPart ConnectionData;
    public GameObject LeftEndPoint;
    public GameObject RightEndPoint;
    public GameObject ParentPivot;
    public GameObject PipeMesh;
    

    public void Start()
    {
        if (ConnectionData.ConnectionPointParent.childCount > 0)
        {
            LeftEndPoint = ConnectionData.ConnectionPointParent.GetChild(0).gameObject;
            RightEndPoint = ConnectionData.ConnectionPointParent.GetChild(1).gameObject;
        }
        
    }
}
