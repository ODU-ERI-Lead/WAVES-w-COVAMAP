using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Scoreboard;


public class ScoreManager : MonoBehaviour
{
    public Scoreboard  scoreboard;
   public static ScoreManager instance;
    public ScoringPPE ScoringPPE;


    public Text scoreText;

    public Text scoreFractionText;

    int totalScore = 0 ; //StartCoroutine(IEnumerator(GetScoreWithBonus)); may remove may need to implement
    //int Roundscore = StartCoroutine(GetScoreWithBonus());//(GetScoreWithBonus); //GetScoreWithBonus ; remove zero
    private int totalPossibleScore = 500;
    public static int sceneCount = 0; // Keep track of how many scenes have been loaded
    //public static Score DontDestroyOnLoad();
   // public static int GetScoreWithBonus { get; private set; }

   // public IEnumerator GetScoreWithBonus(); may remove
    void Awake()
    {
        //ensuring only one scoremanager instance
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       // LoadScore();
        UpdateScoreUI();
        //StartCoroutine IEnumerator GetScoreWithBonus();
    }

    public void AddScore(int score, int possibleScore)
    {
        totalScore += score;  // Add score to the total score
        totalPossibleScore += possibleScore;  // Add possible score to the total possible score
                                              // Increment the scene count
        sceneCount++;

        // Update UI if it's the 5th round or beyond
        if (sceneCount >= 5)
        {
            scoreFractionText.text = "Score: " + totalScore + " / " + totalPossibleScore;
        }
        else
        {
            scoreFractionText.text = "Score: " + totalScore;  // Before 5th round, just show score
        }
        StartCoroutine(scoreboard.GetScoreWithBonus());
        //SaveScore(); //scoreboard.GetScoreWithBonus()
        //ScoreManager.instance.AddScore(DontDestroy.Score + GetScoreWithBonus, 500);  // Adding to the global score
       // PlayerPrefs.Getint();
        UpdateScoreUI();

    }

    public void UpdateScoreUI()
    {
        // Log the values to ensure they are being correctly set
        Debug.Log("Total Score: " + totalScore);
        Debug.Log("Total Possible Score: " + totalPossibleScore);
        // Update the UI text with both the total score and the fraction
        scoreText.text = "Total Score: " + totalScore.ToString();
        scoreFractionText.text = "Score / Possible: " + totalScore.ToString() + " / " + totalPossibleScore.ToString();
        Canvas.ForceUpdateCanvases();
    }

    public int GetTotalScore(int score)
    {
        return totalScore; //+ score; - maybe this too (personal note)
    }

    public int GetPossibleScore() 
    { 
    return totalPossibleScore; 
    }

    public void SaveScore()
    {
        PlayerPrefs.SetInt("TotalScore", totalScore);
        PlayerPrefs.SetInt("TotalPossibleScore", totalPossibleScore);
        PlayerPrefs.Save();
       // Debug.Log("Score saved: " + totalScore);
    }

    public void LoadScore()
    {
        totalScore = PlayerPrefs.GetInt("TotalScore", 100);
        totalPossibleScore = PlayerPrefs.GetInt("TotalPossibleScore", 500);
        Debug.Log("Loaded Score: " + totalScore);
        Debug.Log("Loaded Possible Score: " + totalPossibleScore);
        UpdateScoreUI();
    }
    public void SetTotalScore(int score)
    { 
        totalScore = score; 
        UpdateScoreUI() ;  
    }

}
