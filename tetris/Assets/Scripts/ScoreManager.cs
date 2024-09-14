using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }  // シングルトン

    private int score = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // スコアを保持
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // スコアを加算するメソッド
    public void AddScore(int points)
    {
        score += points;
        Debug.Log("Score: " + score);
    }

    // スコアを取得するメソッド
    public int GetScore()
    {
        return score;
    }
}
