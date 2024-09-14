using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    private void Update()
    {
        // スコアを表示
        scoreText.text = "Score: " + ScoreManager.Instance.GetScore().ToString();
    }
}
