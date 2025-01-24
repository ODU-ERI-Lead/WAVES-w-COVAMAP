using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreHazards : MonoBehaviour
{
    public Text GetScoreText;

    public static class DontDestroyHazardScore
    {
        public static int Score = 0;
        public static int TotalScore = 0;
        public static int NumberCorrect = 0;
        public static string LevelChoice = string.Empty;
        public static int ScenariosCompleted = 0;   
    }
    public void GetScore()
    {
        print("GETTING HERE!!!!!!!!!");
        int maxScore = 100;
        int FinaltotalScore = 1100;
        //int totalScenarios = 11;
        if (DontDestroy.LevelChoice == "Easy")
        {
            DontDestroy.Score = DontDestroy.NumberCorrect * maxScore / 12;
            Debug.Log(DontDestroy.Score);

        }
        else if (DontDestroy.LevelChoice == "Medium")
        {
            DontDestroy.Score = DontDestroy.NumberCorrect * maxScore / 12;
            Debug.Log(DontDestroy.Score);

        }
        else if (DontDestroy.LevelChoice == "Hard")
        {
            DontDestroy.Score = DontDestroy.NumberCorrect * maxScore / 12;
            Debug.Log(DontDestroy.Score);

        }

        SceneManager.LoadScene("ScoreboardHazards");
    }

     void Start()
    {
        if (GetScoreText != null)
        {
            int maxScore = 100;
            GetScoreText.text = "Your Score: " + DontDestroy.Score + " / " + (maxScore * DontDestroy.NumberCorrect);
        }
        else
        {
            Debug.LogError("Score text reference not assigned!");
        }
    }

}