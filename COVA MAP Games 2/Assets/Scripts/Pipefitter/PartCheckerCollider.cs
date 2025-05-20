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
    
    public void UserEvaluatePart()
    {
        if (OtherDetails.PipeType == this.PipeType)
        {
            CorrectPart?.Invoke(this);
            switch (this.PipeType)
            {
                case PartType.StraightPipe:
                    if (OtherDetails.Length == this.Length)
                    {
                        CorrectLength?.Invoke(this);
                    }
                    break;
            }
        }
        else
        {
            //dont care about length
        }

        //weld checks here
        if (PartDataReference != null)
        {
            //check Part Parent thing
            if (PartDataReference.ConnectionPtParent != null)
            {
                //JOHN PartDataReference.WeldsByPoint = is what we really need to look at but its protected
                
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
                            connectionsWelded+=1;
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
                        ///
                        CorrectlyWelded?.Invoke(this);
                    }
                }
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (this.PartDataReference != null)
        {
            return;
        }
        if (collision.gameObject.GetComponent<PartChecker>())
        {
            OtherDetails = collision.gameObject.GetComponent<PartChecker>();
            this.PartDataReference = collision.gameObject.GetComponent<PartChecker>().PartDataReference;
        }
    }

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
    }


    public void OnCollisionStay(Collision collision)
    {
        
    }


}
