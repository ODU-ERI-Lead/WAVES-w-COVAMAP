using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using FuzzPhyte.Game.Samples;

public class FinalInspectionScript : MonoBehaviour //PipefitterpartData may need to switch??
{
 
    // public PipefitterpartData PipefitterpartData;
    public PartCheckerCollider[] CorrectParts;
    public UnityEvent FinishedEvent;
    public FPGameManager_ToolExample GameManager;
    public float MaxScore = 100;
    public float ScoreToFinish = 80;
    public float PerCorrectPipeValue = 2f;
    public float PerCorrectPipeLength = 2f;
    public float PerPartWeldCorrect = 2f;
    /// <summary>
    /// Called via inspection button UI
    /// </summary>
    [ContextMenu("Check Score!")]
    public void CheckAnswers()
    {
        int totalPartCorrect = 0;
        int totalPipeLengthCorrect = 0;
        int totalWeldPartsCorrect = 0;
        for(int i=0; i< CorrectParts.Length; i++)
        {
            var part = CorrectParts[i];

            if (part.IsPartCorrect)
            {
                totalPartCorrect++;
                switch (part.PipeType)
                {
                    case PartType.StraightPipe:
                        if (part.IsPartRightLength)
                        {
                            totalPipeLengthCorrect++;
                        }
                        break;
                    case PartType.Elbow:
                    case PartType.MaleAdapter:
                    case PartType.FemaleAdapter:
                    case PartType.Valve:
                        break;
                }
            }
            //weld checks
            if (part.WeldCheck())
            {
                totalWeldPartsCorrect++;
            }
        }
        //calculate score
        var total = totalPartCorrect*PerCorrectPipeValue + (totalPipeLengthCorrect* PerCorrectPipeLength) + (totalWeldPartsCorrect* PerPartWeldCorrect);
        Debug.LogWarning($"CURRENT SCORE = {total}, with {totalPartCorrect} parts placed correctly, with {totalPipeLengthCorrect} correct cut pipes, and with {totalWeldPartsCorrect} correct welds!");
        if (total > ScoreToFinish)
        {
            FinishedEvent.Invoke();
            GameManager.UpdatePipeFitterState(PipeFitterGameState.Finished);
            //send the score
            if (total > 100)
            {
                total = 100;
            }
            GameManager.OnScoreUpdated(total);
            GameManager.OnScoreRenderUpdate();
            GameManager.OnClockEnd();
        }

    }
    public void ShowCongrats()
    {
        
    }

}
