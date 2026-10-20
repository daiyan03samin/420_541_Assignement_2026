using UnityEngine;

// Keeps track of the player's score.
// It's a singleton: there is only ever one, reachable from any script
// through GameManager.Instance.
public class GameManager : MonoBehaviour
{
    // The single shared instance that any script can reach.
    // "static" means it belongs to the class, not to one object.
    public static GameManager Instance { get; private set; }

    // Anyone can read the score, only GameManager can change it
    public int Score { get; private set; }

    public float ScoreMultiplier { get; set; } = 1f;

    // "static" means this value belongs to the ScoreManager CLASS,
    // not to one ScoreManager object. Objects are destroyed when a
    // new scene loads, but static values stay in memory.
    public static int FinalScore { get; private set; }


    // Awake runs before any Start(), so Instance is ready before coins need it
    private void Awake()
    {
        // If a GameManager already exists, remove this extra component
        // (only the script, not the whole Player)
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        // Otherwise, this is the one and only GameManager  
        // Do not destroy after load a new scene
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Called by coins when they're collected
    public void AddScore(int amount)
    {
        int pointsToAdd = Mathf.RoundToInt(amount * ScoreMultiplier);
        Score += pointsToAdd;

        Debug.Log("Score: " + Score);
    }

    // Called just before we leave the level
    public void SaveFinalScore()
    {
        FinalScore = Score;
    }

}
