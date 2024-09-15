using UnityEngine;
using UnityEngine.SceneManagement;


public class StartManager : MonoBehaviour
{
    private void Start()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayStartSound(); // StartSoundを再生
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene"); // タイトルからゲーム画面へ
    }
}
