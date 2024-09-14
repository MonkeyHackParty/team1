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
        else
        {
            Debug.LogError("SoundManager Instance が null です。SoundManager がシーンに存在するか確認してください。");
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene"); // タイトルからゲーム画面へ
    }
}
