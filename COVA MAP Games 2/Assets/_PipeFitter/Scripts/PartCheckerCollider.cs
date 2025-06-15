using FuzzPhyte.Tools.Connections;
using UnityEngine;
using UnityEngine.Events;

public class PartCheckerCollider : PartChecker
{
    [Space]
    [Header("Collision Checker information")]
    //public BoxCollider PartAnswerCollider;
    public PartChecker OtherDetails;
    public delegate void PartCheckerSystem(PartChecker item);
    public PartCheckerSystem CorrectPart;
    public PartCheckerSystem CorrectLength;
    public PartCheckerSystem CorrectlyWelded;
    public UnityEvent OnFirstCorrectPartPlacedEvent;
    /// <summary>
    /// this is a way for us to temp cache if we have the right part, if we do we kick out other trigger enters if we don't we accept other trigger enters
    /// </summary>
    [SerializeField] protected bool tempCorrectPartType = false;
    private bool unityFiredEvent = false;
    [Range(0,1f)][Tooltip("Percentage of error allowed in length check, 0.1 = 10%")]
    public float ErrorRange = 0.1f;
    public bool IsPartRightLength = false;
    public bool IsPartCorrect = false;
    [Header("Auto Adjust Box Collider")]
    public bool DynamicAdjust = false;
    [Tooltip("3 Works for most of our pipe settings-change to 1 for elbow/adapters")]
    public float WidthScaleValue = 3f;
    public bool UseX = false;
    public bool UseY = false;
    public bool UseZ = false;
    [SerializeField] protected int connectionsWelded = 0;



    public void Start()
    {
        if (DynamicAdjust)
        {
            SetupColliderByScale();
        }
    }
    public void SetupColliderByScale()
    {
        var box = this.GetComponent<BoxCollider>();
        if (box == null)
        {
            return;
        }
        if(UseX && UseY)
        {
            box.size = new Vector3(this.Length * MeterInchScale, this.Length * MeterInchScale, MeterInchScale * WidthScaleValue);
        }
        else
        {
            if (UseX)
            {
                box.size = new Vector3(this.Length * MeterInchScale, MeterInchScale * WidthScaleValue, MeterInchScale * WidthScaleValue);
                return;
            }
            if (UseY)
            {
                box.size = new Vector3(MeterInchScale * WidthScaleValue, this.Length * MeterInchScale, MeterInchScale * WidthScaleValue);
                return;
            }
            if (UseZ)
            {
                box.size = new Vector3(MeterInchScale * WidthScaleValue, MeterInchScale * WidthScaleValue, this.Length * MeterInchScale);
                return;
            }
        }
        
    }
    /// <summary>
    /// Called for Part Confirmation as well as Weld checks
    /// </summary>
    public void UserEvaluatePart()
    {
        if (OtherDetails == null)
        {
            return;
        }
        //error check percentage

        if (OtherDetails.PipeType == this.PipeType)
        {
            CorrectPart?.Invoke(this);
            IsPartCorrect = true;
            if (!unityFiredEvent)
            {
                OnFirstCorrectPartPlacedEvent.Invoke();
                unityFiredEvent = true;

            }
            switch (this.PipeType)
            {
                case PartType.StraightPipe:
                    if (OtherDetails.Length < this.Length * (1 - ErrorRange) || OtherDetails.Length > this.Length * (1 + ErrorRange))
                    {
                        Debug.LogWarning($"Part {this.gameObject.name} is not the correct length for {OtherDetails.gameObject.name}");
                        IsPartRightLength = false;
                    }
                    else
                    {
                        Debug.LogWarning($"Part {this.gameObject.name} is the correct length for {OtherDetails.gameObject.name}");
                        CorrectLength?.Invoke(this);
                        IsPartRightLength = true;
                    }
                    break;
                case PartType.Elbow:
                    Debug.LogWarning($"Elbow pipe is correct for {this.gameObject.name}");
                    break;
                case PartType.Valve:
                    Debug.LogWarning($"Valve pipe is correct for {this.gameObject.name}");
                    break;
                case PartType.MaleAdapter:
                    Debug.LogWarning($"MaleAdapter is correct for {this.gameObject.name}");
                    break;
                case PartType.FemaleAdapter:
                    Debug.LogWarning($"FemaleAdapter is correct for {this.gameObject.name}");
                    break;
            }
        }
        else
        {
            IsPartCorrect = false;
        }

        //WeldCheck();
       
    }
    /// <summary>
    /// Called on Inspection for Weld Check by Part Collider
    /// </summary>
    public bool WeldCheck()
    {
        //weld checks here
        if (PartDataReference != null)
        {
            //check Part Parent thing
            if (PartDataReference.ConnectionPtParent != null)
            {
                //JOHN PartDataReference.WeldsByPoint = is what we really need to look at but its protected
                if (PartDataReference.ConnectionPtParent.childCount > 1)
                {
                    var leftEdge = PartDataReference.ConnectionPtParent.GetChild(0).gameObject.GetComponent<ConnectionPointUnity>();
                    var rightEdge = PartDataReference.ConnectionPtParent.GetChild(1).gameObject.GetComponent<ConnectionPointUnity>();
                    if (leftEdge != null && rightEdge != null)
                    {
                        //IsPartiallyConnected
                        //ConnectionPairs
                        connectionsWelded = 0;
                        //hows our left look - are we somewhat welded?
                        if (PartDataReference.MyConnectionPoints.Contains(leftEdge))
                        {
                            //get left edge out of my lsit
                            //get edge state of parent ConnectionPart
                            var alignedPoint = PartDataReference.MyConnectionPoints[PartDataReference.MyConnectionPoints.IndexOf(leftEdge)].OtherAlignedPoint;
                            if(alignedPoint != null)
                            {
                                if (alignedPoint.TheConnectionPart.CurrentState == FuzzPhyte.Utility.FPToolState.Locked)
                                {
                                    connectionsWelded += 1;
                                }
                            }
                        }
                        //hows our right look are we somewhat welded?
                        if (PartDataReference.MyConnectionPoints.Contains(rightEdge))
                        {
                            var alignedPoint = PartDataReference.MyConnectionPoints[PartDataReference.MyConnectionPoints.IndexOf(rightEdge)].OtherAlignedPoint;
                            if (alignedPoint != null)
                            {
                                if (alignedPoint.TheConnectionPart.CurrentState == FuzzPhyte.Utility.FPToolState.Locked)
                                {
                                    connectionsWelded += 1;
                                }
                            }
                        }
                        //is the part we have on us somewhat welded?
                        if (PartDataReference.CurrentState == FuzzPhyte.Utility.FPToolState.Locked)
                        {
                            connectionsWelded += 1;
                        }
                        if (connectionsWelded >= 3)
                        {
                            ///basically we are welded except for one extreme case

                            CorrectlyWelded?.Invoke(this);
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
    
    public void OnTriggerEnter(Collider other)
    {
        
        if (this.PartDataReference != null&&tempCorrectPartType)
        {
            return;
        }
        if (other.gameObject.GetComponent<PartChecker>())
        {
            if (other.gameObject.GetComponent<PartChecker>().EndPoint)
            {
                return;
            }
            //Debug.LogError($"Trigger information  {other.gameObject.name}");
            OtherDetails = other.gameObject.GetComponent<PartChecker>();
            this.PartDataReference = other.gameObject.GetComponent<PartChecker>().PartDataReference;
            //Debug.LogWarning($"Part has entered an answer collider, {OtherDetails.gameObject.name} with {other.gameObject.name} ");
            if (OtherDetails.PipeType == this.PipeType)
            {
                UserEvaluatePart();
                tempCorrectPartType = true;
            }
            else
            {
                tempCorrectPartType = false;
            }
            
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (this.PartDataReference != null && other.gameObject.GetComponent<PartChecker>())
        {
            if (other.gameObject.GetComponent<PartChecker>().PartDataReference == this.PartDataReference)
            {
                OtherDetails = null;
                this.PartDataReference = null;
                //Debug.LogWarning("Part has left an answer collider.");
            }
        }
    }
}
