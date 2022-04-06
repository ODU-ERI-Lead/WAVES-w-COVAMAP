using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickWhiteWire : MonoBehaviour
{
    public GameObject Bundle2B;
    public GameObject Bundle3B;
    public GameObject Bundle4B;
    public AllWiresInvisableToStart AllWiresInvisableToStartScript;

    private void OnMouseDown()
    {
        DontDestroy.CableType = "F4";
        DontDestroy.WireColor = "white";
        Bundle2B.SetActive(false);
        Bundle3B.SetActive(false);
        Bundle4B.SetActive(false);
        print(DontDestroy.WireColor);
        AllWiresInvisableToStartScript.EnableConnectors();

    }

}
