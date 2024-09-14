using UnityEngine;

public class BlockPeace : MonoBehaviour
{
    [SerializeField] private int number;  // ブロックの数値（インスペクターから設定）
    [SerializeField] private GameObject[] prefabs;  // Prefabsを格納する配列

    private GameObject currentBlockInstance;  // 現在のPrefabインスタンスを保持

    void Start()
    {
        // 数値に応じたPrefabを生成
        UpdatePrefab();
    }

    // 数値に対応したPrefabを生成する処理
    private void UpdatePrefab()
    {
        // 数値が範囲内か確認
        if (number < 0 || number >= prefabs.Length)
        {
            Debug.LogError($"指定された数値 {number} に対応するPrefabが存在しません。Prefab配列のサイズ: {prefabs.Length}");
            return;
        }

        // 既に表示されているPrefabがあれば削除
        if (currentBlockInstance != null)
        {
            Destroy(currentBlockInstance);  // 既存のPrefabを削除
        }

        // 数値に対応するPrefabを生成
        currentBlockInstance = Instantiate(prefabs[number], transform.position, Quaternion.identity, transform);
        Debug.Log($"Prefab {prefabs[number].name} が生成されました。数値: {number}");
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
