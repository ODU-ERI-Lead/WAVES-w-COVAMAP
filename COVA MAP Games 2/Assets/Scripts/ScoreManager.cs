using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private bool bonusApplied = false;
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
        //delete below if needed
        if (PlayerPrefs.GetInt("SceneLoadCount", 0) >= 5)
        {
            // Clear saved scores and reset scene count
            ClearSavedScores();
            PlayerPrefs.SetInt("SceneLoadCount", 0);
            Debug.Log("Scene count exceeded 5, scores cleared.");
        }
        else
        {
            int sceneLoadCount = PlayerPrefs.GetInt("SceneLoadCount", 0);
            sceneLoadCount++;
            PlayerPrefs.SetInt("SceneLoadCount", sceneLoadCount);
            Debug.Log("Incrementing scene count: " + sceneLoadCount);
        }
        // If this is the first scene (sceneCount == 1), reset the score
        if (sceneCount == 1)
        {
            totalScore = 0;  // Reset score to 0
            Debug.Log("Resetting totalScore to 0 as this is the first scene.");
        }
        string currentSceneName = SceneManager.GetActiveScene().name;
        Debug.Log("Current Scene: " + currentSceneName);

        // If the current scene is the PPE scene, increment the scene count
        if (currentSceneName == "PPE")  // Replace "PPE" with the actual name of your PPE scene
        {
            sceneCount++;

            // Save the updated scene count
            PlayerPrefs.SetInt("SceneLoadCount", sceneCount);
            PlayerPrefs.Save();

            // Debugging log to verify the scene count is being incremented correctly
            Debug.Log("Scene count incremented: " + sceneCount);
        }
        //sceneCount++;
        PlayerPrefs.SetInt("SceneLoadCount", sceneCount);
        PlayerPrefs.Save();

        // Debugging logs to track the count
        Debug.Log("Current Scene Count: " + sceneCount);
        //dont delete below
        LoadScore();
        UpdateScoreUI();
        //StartCoroutine IEnumerator GetScoreWithBonus();
        // clear below if errors
        if (sceneCount >= 6) // change back to 5 if need be
            {
            ClearSavedScores();
            sceneCount = 0;
            totalScore = 0;  // Reset the total score and delelte below if messes up.
            UpdateScoreUI();
        }
        if (sceneCount < 5)
        {
            //sceneCount++;
            Debug.Log("scenecount" + sceneCount);
        }
        //may need to add all this "//" below may not:  // Only clear saved scores and reset scene count when it's 5 or more, not immediately at 5
        if (sceneCount >= 5 && !PlayerPrefs.HasKey("ResetCompleted"))
        {
             //Mark that the reset has been completed
           PlayerPrefs.SetInt("ResetCompleted", 1);
           PlayerPrefs.Save();

           ClearSavedScores();  // Clear the saved scores
           sceneCount = 0;  // Reset scene count
           PlayerPrefs.SetInt("SceneLoadCount", sceneCount);  // Save the reset scene count
           PlayerPrefs.Save();

            // Reset the score
            totalScore = 0;
            UpdateScoreUI();
            Debug.Log("Scene count reached 5, reset completed.");
        }

    }

    public void AddScore(int score, int possibleScore)
    {
        if (!bonusApplied)
        {
            totalScore += score;  // Add score to the total score
            totalPossibleScore += possibleScore;  // Add possible score to the total possible score
            if (totalPossibleScore != 500)
            { 
                totalPossibleScore = possibleScore; //sets possible score only once
            }// Increment the scene count
            if (totalScore > totalPossibleScore)
            {
                totalScore = totalPossibleScore;  // Cap the score at the total possible score
            }

            //sceneCount++; remove // if needed after test.
            scoreFractionText.text = $"Score: {totalScore} / {totalPossibleScore}";
            bonusApplied |= true; // allegedly ensures bonus apllies once
        }

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
    // This method will be triggered when the Quit button is pressed on scene count 5
    private void OnQuitButtonPressed()
    {
        ClearSavedScores();  // Reset the score and scene count when quit is pressed on scene 5
        Debug.Log("Quit button pressed, scores reset.");
    }


    //clear below if not functioning correctly
    public void ClearSavedScores()
    {
        Debug.Log("Clearing saved scores...");
        PlayerPrefs.DeleteKey("TotalScore");
        PlayerPrefs.DeleteKey("TotalPossibleScore");
        PlayerPrefs.Save(); // Save the changes after deleting
        Debug.Log("Saved scores cleared after 5 scene loads.");
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
        totalScore = PlayerPrefs.GetInt("TotalScore", 0); // change back to 100 if still not right.
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
