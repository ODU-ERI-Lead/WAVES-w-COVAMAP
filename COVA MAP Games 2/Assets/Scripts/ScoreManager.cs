using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Scoreboard  scoreboard;
   public static ScoreManager instance;
    public ScoringPPE ScoringPPE;


    public Text scoreText;

    public Text scoreFractionText;

    private int totalScore = 0; //(GetScoreWithBonus); //GetScoreWithBonus ; remove zero
    private int totalPossibleScore = 500;
    public static int sceneCount = 0; // Keep track of how many scenes have been loaded

    public static int GetScoreWithBonus { get; private set; }

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
        UpdateScoreUI();
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
        ScoreManager.instance.AddScore(DontDestroy.Score + GetScoreWithBonus, 500);  // Adding to the global score
        UpdateScoreUI();
    }

    public void UpdateScoreUI()
    {
        // Log the values to ensure they are being correctly set
        Debug.Log("Total Score: " + totalScore);
        Debug.Log("Total Possible Score: " + totalPossibleScore);
        // Update the UI text with both the total score and the fraction
        scoreText.text = "Total Score: " + totalScore.ToString();
        scoreFractionText.text = "Score / Possible: " + totalScore.ToString("0") + " / " + totalPossibleScore.ToString("0");
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
    }

    public void LoadScore()
    {
        totalScore = PlayerPrefs.GetInt("TotalScore", 100);
        totalPossibleScore = PlayerPrefs.GetInt("TotalPossibleScore", 500);
        UpdateScoreUI();
    }


}
