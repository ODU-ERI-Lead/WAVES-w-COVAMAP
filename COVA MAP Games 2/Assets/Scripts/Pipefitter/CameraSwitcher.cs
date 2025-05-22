using UnityEngine;
using UnityEngine.UI;

public class CameraSwitcher : MonoBehaviour
{
    public Camera MainCamera;
    public Camera CutterCamera;
    public Camera AssemblyCamera;

    public Button CutCameraButton;
    public Button AssemblyCameraButton;
    public Button HomeButton;

    public void Start()
    {
        //ensuring only one camera starts active
        MainCamera.gameObject.SetActive(true);
        CutterCamera.gameObject.SetActive(false);
       // AssemblyCamera.gameObject.SetActive(false);    right now main and assembly are same I think.
    }


    public void OnClickCCB()
    {
       // if (CutCameraButton.)
       MainCamera.gameObject.SetActive(false);
        CutterCamera.gameObject.SetActive(true);
        AssemblyCamera.gameObject.SetActive(false);


    }

    public void OnClickACB()
    { 
        MainCamera.gameObject.SetActive(false);
        CutterCamera.gameObject.SetActive(false);
        AssemblyCamera.gameObject.SetActive(true);
    
    }



     public void OnClickHB()
    {
        MainCamera.gameObject.SetActive(true);
        CutterCamera.gameObject.SetActive(false);
       // AssemblyCamera.gameObject.SetActive(false);   unslash if different cams
    }
}
