using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Scoreboard  scoreboard;
   public static ScoreManager instance;

    public Text scoreText;

    public Text scoreFractionText;

    private int totalScore = 0; //GetScoreWithBonus ; remove zero
    private int totalPossibleScore = 500;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
