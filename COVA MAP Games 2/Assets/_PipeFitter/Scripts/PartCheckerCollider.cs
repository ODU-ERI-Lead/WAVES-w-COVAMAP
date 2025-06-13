using FuzzPhyte.Tools.Connections;
using UnityEngine;



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
    [Range(0,1f)][Tooltip("Percentage of error allowed in length check, 0.1 = 10%")]
    public float ErrorRange = 0.1f;
    public bool IsPartRightLength = false;
    [Header("Auto Adjust Box Collider")]
    public bool DynamicAdjust = false;
    public bool UseX = false;
    public bool UseY = false;
    public bool UseZ = false;
    
    public void SetupColliderByScale()
    {
        var box = this.GetComponent<BoxCollider>();
        if(box == null)
        {
            return;
        }
        if (UseX)
        {
            box.size = new Vector3(this.Length * MeterInchScale, MeterInchScale * 3, MeterInchScale * 3);
            return;
        }
        if (UseY)
        {
            box.size = new Vector3(MeterInchScale * 3, this.Length * MeterInchScale, MeterInchScale * 3);
            return;
        }
        if (UseZ)
        {
            box.size = new Vector3(MeterInchScale * 3, MeterInchScale * 3, this.Length * MeterInchScale);
            return;
        }
    }
   
    public void Start()
    {
        if (DynamicAdjust)
        {
            SetupColliderByScale();
        }
    }

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
                        int connectionsWelded = 0;
                        //hows our left look - are we somewhat welded?
                        if (PartDataReference.ConnectionPairs.ContainsKey(leftEdge))
                        {
                            if (PartDataReference.ConnectionPairs[leftEdge].IsPartiallyConnected)
                            {
                                connectionsWelded += 1;
                            }
                        }
                        //hows our right look are we somewhat welded?
                        if (PartDataReference.ConnectionPairs.ContainsKey(rightEdge))
                        {
                            if (PartDataReference.ConnectionPairs[rightEdge].IsPartiallyConnected)
                            {
                                connectionsWelded += 1;
                            }
                        }
                        //is the part we have on us somewhat welded?
                        if (PartDataReference.IsPartiallyConnected)
                        {
                            connectionsWelded += 1;
                        }
                        if (connectionsWelded >= 3)
                        {
                            ///basically we are welded except for one extreme case

                            CorrectlyWelded?.Invoke(this);
                        }
                    }
                }
            }
        }
    }
    /*
    public void OnCollisionEnter(Collision collision)
    {
        Debug.LogWarning($"Collision information  {collision.collider.name}");
        if (this.PartDataReference != null)
        {
            return;
        }
        if (collision.gameObject.GetComponent<PartChecker>())
        {
            OtherDetails = collision.gameObject.GetComponent<PartChecker>();
            this.PartDataReference = collision.gameObject.GetComponent<PartChecker>().PartDataReference;
        }
        Debug.LogWarning("Part has entered an answer collider.");
        UserEvaluatePart();
    }
    */
    public void OnTriggerEnter(Collider other)
    {
        
        if (this.PartDataReference != null)
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
            Debug.LogWarning($"Part has entered an answer collider, {OtherDetails.gameObject.name} with {other.gameObject.name} ");
            UserEvaluatePart();
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
                Debug.LogWarning("Part has left an answer collider.");
            }
        }
        
    }
/*
    public void OnCollisionExit(Collision collision)
    {
        if (this.PartDataReference != null && collision.gameObject.GetComponent<PartChecker>())
        {
            if (collision.gameObject.GetComponent<PartChecker>().PartDataReference == this.PartDataReference)
            {
                OtherDetails = null;
                this.PartDataReference = null;
            }
        }
        Debug.LogWarning("Part has left an answer collider.");
    }
*/

    public void OnCollisionStay(Collision collision)
    {






       // Debug.LogWarning("Part is within a answer collider.");
    }


}
