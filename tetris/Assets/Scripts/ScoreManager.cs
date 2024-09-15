using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private int score = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // シーンを跨いでもスコアを保持
            Debug.Log("ScoreManager が初期化されました。");
        }
        else
        {
            Debug.LogError("複数の ScoreManager が存在します。");
            Destroy(gameObject);  // 複数存在する場合は新しいものを削除
        }
    }

    // スコアを加算するメソッド
    public void AddScore(int points)
    {
        score += points;
        Debug.Log("スコアが加算されました。現在のスコア: " + score);
    }

    // 現在のスコアを取得するメソッド
    public int GetScore()
    {
        return score;
    }
}
