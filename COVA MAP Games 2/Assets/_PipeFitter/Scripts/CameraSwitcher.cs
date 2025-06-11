using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    /// <summary>
    /// Austin you had three cameras in here, one repeated itself :), you then were telling the same camera by two diff references
    /// to do opposite things... meaning by luck you got it right as in you turned it off then on ;)
    /// I fixed this and renamed everything so it made sense and removed uneeded references to random buttons
    /// </summary>
    public Camera AssemblyZoneMainCamera;
    public Camera WorkZoneMainCamera;

    public void Start()
    {
        //ensuring only one camera starts active
        AssemblyZoneMainCamera.gameObject.SetActive(true);
        WorkZoneMainCamera.gameObject.SetActive(false);
    }


    public void OnClickCCB()
    {
        AssemblyZoneMainCamera.gameObject.SetActive(false);
        WorkZoneMainCamera.gameObject.SetActive(true);
    }

    public void OnClickACB()
    { 
        AssemblyZoneMainCamera.gameObject.SetActive(false);
        WorkZoneMainCamera.gameObject.SetActive(false);
    }

    public void OnClickHB()
    {
        AssemblyZoneMainCamera.gameObject.SetActive(true);
        WorkZoneMainCamera.gameObject.SetActive(false);
    }
}
