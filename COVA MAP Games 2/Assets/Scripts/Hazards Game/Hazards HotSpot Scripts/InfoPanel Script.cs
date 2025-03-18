using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPanelScript : MonoBehaviour
{
    public GameObject Infopanel;
    public HazardhotspotData gamedata;
    void Start()
    {
        Infopanel.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && Infopanel.activeSelf == true)
        {
            Infopanel.SetActive(false);
            HazardhotspotGameManager.Instance.Initialize(false, gamedata);
        }
    }
}
