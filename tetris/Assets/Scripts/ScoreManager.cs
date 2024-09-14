using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    
    private int score = 0;

    private void Awake()
    {
        // シングルトンのインスタンスを設定
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // シーンを切り替えても保持する
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // スコアを加算する
    public void AddScore(int points)
    {
        score += points;
        Debug.Log("Score: " + score);
    }

    // 現在のスコアを取得する
    public int GetScore()
    {
        return score;
    }
}

