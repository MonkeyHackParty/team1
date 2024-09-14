using UnityEngine;

public class BlockPeace : MonoBehaviour
{
    [SerializeField] private int number;  // ブロックの数値
    [SerializeField] private GameObject[] blockPrefabs;  // 数値に対応するPrefabを格納する配列

    private GameObject currentBlockInstance;  // 現在のPrefabインスタンスを保持

    void Start()
    {
        // 最初に対応するPrefabを表示
        UpdatePrefab();
    }

    // 数値に対応したPrefabを生成する処理
    private void UpdatePrefab()
    {
        // 既に表示されているPrefabがあれば、それを削除
        if (currentBlockInstance != null)
        {
            Destroy(currentBlockInstance);
        }

        // 数値が範囲内ならPrefabを生成
        if (number >= 0 && number < blockPrefabs.Length)
        {
            currentBlockInstance = Instantiate(blockPrefabs[number], transform.position, Quaternion.identity, transform);
            Debug.Log($"Prefab {blockPrefabs[number].name} を生成しました。");
        }
        else
        {
            Debug.LogError($"指定された数値 {number} に対応するPrefabが存在しません。");
        }
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
