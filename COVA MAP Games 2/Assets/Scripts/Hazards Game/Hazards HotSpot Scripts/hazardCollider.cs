using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class hazardCollider : MonoBehaviour
{
    public RectTransform ParentContainer;
    public float MinWidthNeeded = 100;
    protected RectTransform MyRect;
    public correctHazardMono RightJustified;
    public correctHazardMono LeftJustified;

    public float testPos;
    public bool BeenClicked = false;

    public void Awake()
    {
        MyRect = GetComponent<RectTransform>();
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.LogWarning($"Something hit me");
    }
    /*
    public void Update()
    {
        if (Input.GetKey(KeyCode.Space)) 
        {
            OnClicked();
        }
    }
    */
    /// <summary>
    /// Called via our Mouse/Raycast unit/system
    /// </summary>
    public void OnClicked()
     {
        //where am I
        if (BeenClicked)
        {
            return;
        }
        var halfWidthParent = ParentContainer.sizeDelta.x * 0.5f;
        //right or left of center
        bool RightEdgeFree=false;
        bool LeftEdgeFree=false;
        if (MyRect.localPosition.x > 0)
        {
            //right of center
            testPos = (halfWidthParent - MyRect.localPosition.x) - MinWidthNeeded;
            if (testPos > 0)
            {
                RightEdgeFree = true;
            }
            LeftEdgeFree = true;
        }
        else
        {
            //left of center
            testPos = (halfWidthParent + MyRect.localPosition.x) - MinWidthNeeded;
            if (testPos > 0)
            {
                LeftEdgeFree=true;
            }
            RightEdgeFree = true;
        }
        //priority is always right
        correctHazardMono theDisplayPanel=RightJustified;
        if (RightEdgeFree)
        {
            if (RightJustified)
            {
                theDisplayPanel = RightJustified;
               
            }
        }
        else
        {
            if (LeftJustified)
            {
                theDisplayPanel = LeftJustified;
                
            }
        }
        // any additional actions and/or settings can done here... like staying on, play a sound
        theDisplayPanel.gameObject.SetActive(true);
        theDisplayPanel.OnPopUp.Invoke();
        theDisplayPanel.RunDisplayTimer();
        BeenClicked = true;
    }
}
