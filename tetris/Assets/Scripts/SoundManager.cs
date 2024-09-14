using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource bgmAudioSource;  // BGM用AudioSource
    [SerializeField] AudioSource seAudioSource;   // SE用AudioSource

    [SerializeField] List<BGMSoundData> bgmSoundDatas;  // BGMデータのリスト
    [SerializeField] List<SESoundData> seSoundDatas;    // SEデータのリスト

    [SerializeField] private AudioSource oneTimeAudioSource; // 一度だけ再生する音源用のAudioSource
    [SerializeField] private AudioClip startSoundClip; // StartScene用の音声ファイル
    
    public static float masterVolume = 1;
    public static float bgmMasterVolume = 1;
    public static float seMasterVolume = 1;

    private float datavolume = 0f;

    public static SoundManager Instance { get; private set; }

    private void Awake()
{
    if (Instance == null)
    {
        Instance = this;
        DontDestroyOnLoad(gameObject); // シーンを跨いでもオブジェクトを保持
        Debug.Log("SoundManager Instance が初期化されました");
    }
    else
    {
        Destroy(gameObject);
    }
}


    void Update()
    {
        // 音量調整
        bgmAudioSource.volume = datavolume * bgmMasterVolume * masterVolume;
    }

    public void PlayStartSound()
    {
        if (startSoundClip != null)
        {
            oneTimeAudioSource.clip = startSoundClip;
            oneTimeAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("start.wavが見つかりません");
        }
    }

    // BGMを再生する（ループ再生対応）
    public void PlayBGM(BGMSoundData.BGM bgm, bool loop = true)
    {
        BGMSoundData data = bgmSoundDatas.Find(data => data.bgm == bgm);
        if (data != null)
        {
            bgmAudioSource.clip = data.audioClip;
            datavolume = data.volume;
            bgmAudioSource.loop = loop;  // ループ設定
            bgmAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("指定されたBGMが見つかりません: " + bgm);
        }
    }

    // BGMを停止する
    public void StopBGM()
    {
        bgmAudioSource.Stop();
    }

    // SEを再生する（同時再生対応）
    public void PlaySE(SESoundData.SE se)
    {
        SESoundData data = seSoundDatas.Find(data => data.se == se);
        if (data != null)
        {
            // SE用のAudioSourceを動的に追加
            AudioSource newSeAudioSource = gameObject.AddComponent<AudioSource>();
            newSeAudioSource.volume = data.volume * seMasterVolume * masterVolume;
            newSeAudioSource.PlayOneShot(data.audioClip);

            // 再生終了後にAudioSourceを削除
            Destroy(newSeAudioSource, data.audioClip.length);
        }
        else
        {
            Debug.LogWarning("指定されたSEが見つかりません: " + se);
        }
    }

    // SEを停止する
    public void StopSE()
    {
        seAudioSource.Stop();
    }

    // 音量を設定するための関数群
    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
    }

    public void SetBGMVolume(float volume)
    {
        bgmMasterVolume = volume;
    }

    public void SetSEVolume(float volume)
    {
        seMasterVolume = volume;
    }
}

[System.Serializable]
public class BGMSoundData
{
    public enum BGM
    {
        All, // ラベル
    }

    public BGM bgm;
    public AudioClip audioClip;
    [Range(0, 1)] public float volume = 1;
}

[System.Serializable]
public class SESoundData
{
    public enum SE
    {
        Correct,
        InCorrect, // これがラベルになる
        Clear,
        GameOver,
    }

    public SE se;
    public AudioClip audioClip;
    [Range(0, 1)] public float volume = 1;
}
