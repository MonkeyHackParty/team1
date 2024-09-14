using UnityEngine;
using TMPro;  // TextMeshPro用の名前空間

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;  // TextMeshProのTextコンポーネント

    private void Update()
    {
        // スコアをTextMeshProのTextに表示
        scoreText.text = "Score: " + ScoreManager.Instance.GetScore().ToString();
    }
}
