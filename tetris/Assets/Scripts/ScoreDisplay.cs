using TMPro;  // TextMeshPro用
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;  // スコア表示用のTextMeshProのテキスト

    private void Update()
    {
        // 現在のスコアを取得してTextに反映
        if (scoreText != null)
        {
            scoreText.text = "Score: " + ScoreManager.Instance.GetScore().ToString();
        }
        else
        {
            Debug.LogError("ScoreText が設定されていません");
        }
    }
}
