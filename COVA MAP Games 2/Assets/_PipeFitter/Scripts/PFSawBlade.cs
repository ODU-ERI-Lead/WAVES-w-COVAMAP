using UnityEngine;

public class PFSawBlade : MonoBehaviour
{
    public PipeFitterMouseCutter CutterVisual;
    public Transform VisualBladeRoot;

    public void Update()
    {
        if (CutterVisual != null)
        {
            VisualBladeRoot.position = CutterVisual.MousePointPosition;
        }
    }
}
