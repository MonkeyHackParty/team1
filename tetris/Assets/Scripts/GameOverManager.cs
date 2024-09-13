using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    // リスタートボタンを押した時の処理
    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");  // ゲームシーンに遷移
    }

    // メニューに戻るボタンを押した時の処理
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("StartScene");  // スタート画面シーンに遷移
    }
}

