using UnityEngine;
using FuzzPhyte.Utility;
using System.Collections.Generic;
using FuzzPhyte.Tools.Connections;
using FuzzPhyte.UI;



public class FinalInspectionScript : MonoBehaviour
{
    [System.Serializable]
    public class AnswerSlots
        {
        public Transform rayOrigin;
        public string expectedPartName;
        public int expectedPartID;
    }
   // public PipefitterpartData PipefitterpartData;
    public GameObject[] blueprinted_answer_parts;
    public Transform[] Blueprint_Answer_Transforms;
    public GameObject[] Spawned_parts;
    public Collider[] correct_parts_Colliders;
    public List<PipefitterpartData> Answer_Parts = new List<PipefitterpartData>();
    public List<PipefitterpartData> Correct_parts_data;
    public AnswerSlots[] answerSlots;


    //filling in list for spawned parts



    ///raycast portion/function loop use if drag or on drag of moving part scrpt

    public void CheckAnswers()
    {
        foreach (var slot in answerSlots)
        {
            RaycastHit hit;
            if (Physics.Raycast(slot.rayOrigin.position, slot.rayOrigin.forward, out hit, 5f))
            {
                PipefitterpartData part = hit.collider.GetComponent<PipefitterpartData>();
                if (part != null)
                {
                    if (part.PartName == slot.expectedPartName && part.CorrectPart_ID == slot.expectedPartID)
                    {
                        Debug.Log($"Correct part at slot {slot.rayOrigin.name}");
                    }
                    else
                    {
                        Debug.LogWarning($"Wrong part at slot {slot.rayOrigin.name}");
                    }
                }
                else
                {
                    Debug.LogWarning($"No valid part at slot {slot.rayOrigin.name}");
                }
            }
            else
            {
                Debug.LogWarning($"No hit from ray at {slot.rayOrigin.name}");
            }
           // if (PipefitterpartData.Matches())
           // {
           //     Debug.Log($"Correct part at {slot.rayOrigin.name}");
          //  }
          //  else
          //  {
           //     Debug.LogWarning($"Incorrect part at {slot.rayOrigin.name}");
           // }

            // start if loops like is bool welded checked
            //is bool connected proper check
            //is whatever is next correct


            // then once finished display end game menu/coingrats 

            //also display work on board or display empty answer key on back once properly completed!

        }

    }



    ///checks for part data in plce welded etc.
}
