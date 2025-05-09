using UnityEngine;

public class HoverTextscript : MonoBehaviour
{
    public GameObject ThreeDText;
    public GameObject TwoDText;
    public GameObject Weldtext;
    public GameObject GrindText;
    public GameObject CutText;
    public GameObject pipeText;
    public GameObject ElbowPipeText;
    public GameObject BlueprintText;
    public GameObject Madaptertext;
    public GameObject FadapterText;
    public GameObject ValveText;
    public GameObject TrashText;
    public GameObject DragText;
    public GameObject Remove3DText;
    public GameObject Remove2DText;
    public GameObject SliderText;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ThreeDText.SetActive(false);
        TwoDText.SetActive(false); 
        Weldtext.SetActive(false);
        GrindText.SetActive(false);
        CutText.SetActive(false);
        pipeText.SetActive(false);
        ElbowPipeText.SetActive(false);
        BlueprintText.SetActive(false);
        Madaptertext.SetActive(false);
        FadapterText.SetActive(false);
        ValveText.SetActive(false);
        TrashText.SetActive(false);
        DragText.SetActive(false);
        Remove3DText.SetActive(false);
        Remove2DText.SetActive(false);
        SliderText.SetActive(false);

          

    }

    public void OnMouseOver()
    {
        ThreeDText.SetActive(false);

    }
}
