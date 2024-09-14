#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class PrefabLoaderEditor : MonoBehaviour
{
    [SerializeField] private int number;
    private GameObject currentBlockInstance;

    // Prefabのロード処理
    public void LoadPrefab()
    {
        // プレハブのパスを作成
        string prefabPath = $"Assets/Prefabs/{number}.prefab";  // "Assets/Prefabs/2.prefab" など
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

        if (prefab == null)
        {
            Debug.LogError($"指定された数値 {number} に対応するPrefabが {prefabPath} に存在しません");
            return;
        }

        // 既存のPrefabがあれば削除
        if (currentBlockInstance != null)
        {
            DestroyImmediate(currentBlockInstance);
        }

        // プレハブを生成
        currentBlockInstance = Instantiate(prefab, transform.position, Quaternion.identity);
        Debug.Log($"Prefab {prefab.name} が生成されました。数値: {number}");
    }
}
#endif
