using UnityEngine;
using FuzzPhyte.Utility;
using System.Collections.Generic;
using FuzzPhyte.Tools.Connections;
using FuzzPhyte.UI;
using Unity.VisualScripting;





public class FinalInspectionScript : MonoBehaviour //PipefitterpartData may need to switch??
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
    public bool SetBPA_Active = false;
    public Transform[] Blueprint_Answer_Transforms;
    public GameObject[] Spawned_parts;
    public Collider[] correct_parts_Colliders;
    public List<PipefitterpartData> Answer_Parts = new List<PipefitterpartData>();
    public List<PipefitterpartData> Correct_parts_data;
    public AnswerSlots[] answerSlots;
    private PipefitterpartData PipefitterpartData;
    public GameObject Congrats_Display;
    public bool beenMatched = false;

    //filling in list for spawned parts
  //  public void OnCheck()
   // {
    //    GameObject[blueprinted_answer_parts] activate
    //  all blueprinted_answer_parts.SetActive
  //  }
  public void DisplayBPA_Parts()
    {
        foreach (GameObject gameObjects in blueprinted_answer_parts)
        {
            gameObjects.SetActive(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (Collider target in correct_parts_Colliders)
        {
            if (collision.collider == target)
            {
                // Try to get the PipefitterpartData from the collided GameObject
                PipefitterpartData partData = collision.gameObject.GetComponent<PipefitterpartData>();
                if (partData != null && !Answer_Parts.Contains(partData))
                {
                    Answer_Parts.Add(partData);
                    Debug.Log($"Correct collision: {collision.gameObject.name} added to Answer_Parts");
                }
            }
        }
    }
    public void UpdateListBAT()
    {
        // if correct_parts_colliders been hit add to list
        // OnCollisionEnter any Correct_Parts_Colliders[i]
        int correctMatches = 0;

        foreach (AnswerSlots slot in answerSlots)
        {
            bool matchFound = false;

            foreach (PipefitterpartData part in Answer_Parts)
            {
                // Match either by ID or Name — adjust as needed
                if (part.CorrectPart_ID == slot.expectedPartID || part.PartName == slot.expectedPartName)
                {
                    matchFound = true;
                    break;
                }
            }

            if (matchFound)
            {
                correctMatches++;
            }
        }

        Debug.Log($"{correctMatches} out of {answerSlots.Length} parts matched.");

        if (correctMatches == answerSlots.Length)
        {
            Debug.Log("All parts correctly matched!");
            ShowCongrats();
        }
    }


    public void Update()
    {
       // if Lastselectedpart
       
    }
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
           if (PipefitterpartData.Matches(PipefitterpartData))
            {
                Debug.Log($"Correct part at {slot.rayOrigin.name}");
            }
           else
           {
              Debug.LogWarning($"Incorrect part at {slot.rayOrigin.name}");
           }

            // start if loops like is bool welded checked
            //is bool connected proper check
            //is whatever is next correct


            // then once finished display end game menu/coingrats 

            //also display work on board or display empty answer key on back once properly completed!
           
        }

    }

    public void ShowCongrats()
    {
        if (beenMatched == true)
        {
            Congrats_Display.SetActive(true);
        }
    }

    ///checks for part data in plce welded etc.
}
