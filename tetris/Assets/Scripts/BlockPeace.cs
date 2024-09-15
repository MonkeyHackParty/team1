using UnityEngine;

public class BlockPeace : MonoBehaviour
{
    [SerializeField] private int number;  // ブロックの数値（インスペクタから設定）
    [SerializeField] private GameObject[] prefabs;  // プレハブを格納する配列

    private GameObject currentBlockInstance;  // 現在のPrefabインスタンスを保持

    void Start()
    {
        // 数値に応じたPrefabを生成
        UpdatePrefab();
    }

    // 数値に対応したPrefabを生成する処理
    private void UpdatePrefab()
{
    // 配列が正しく設定されているか確認
    if (prefabs == null || prefabs.Length == 0)
    {
        return;
    }

    // 数値が範囲内か確認
    if (number < 0 || number >= prefabs.Length)
    {
        return;
    }

    // Prefabがnullでないか確認
    if (prefabs[number / 2] == null)
    {
        return;
    }

    // 既に表示されているPrefabがあれば削除
    if (currentBlockInstance != null)
    {
        Destroy(currentBlockInstance);  // 既存のPrefabを削除
    }

    // 数値に対応するPrefabを生成
    currentBlockInstance = Instantiate(prefabs[number / 2], transform.position, Quaternion.identity, transform);
}


    // 数値を設定するメソッド
    public void SetNumber(int newNumber)
    {
        number = newNumber;  // 新しい数値を設定
        UpdatePrefab();  // 新しい数値に応じてPrefabを更新
    }

    // 数値を取得するメソッド
    public int GetNumber()
    {
        return number;
    }
}


