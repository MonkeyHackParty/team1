using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaceSpawn : MonoBehaviour
{
    // 各数値に対応する BlockPeace プレハブ (2, 4, 8, 16)
    [SerializeField] BlockPeace[] BlockPeaces;

    // 2, 4, 8, 16 のいずれかのプレハブをランダムに取得
    BlockPeace GetRandomBlockPeace()
    {
        // BlockPeaces 配列のインデックスが 0 ～ 3 までの範囲内であることを確認
        if (BlockPeaces.Length < 4)
        {
            Debug.LogError("BlockPeaces配列に正しくプレハブが設定されていません。2, 4, 8, 16 の4つのプレハブを設定してください。");
            return null;
        }

        // ランダムに 2, 4, 8, 16 に対応するプレハブを取得
        int randomIndex = Random.Range(0, 4);  // 0 ～ 3 の範囲でランダムに選択

        return BlockPeaces[randomIndex];
    }

    // ランダムな数値を持つブロックを生成
    public void SpawnBlockPeace()
    {
        BlockPeace blockPeaceInstance = Instantiate(GetRandomBlockPeace(), transform.position, Quaternion.identity, transform.parent);

        if (blockPeaceInstance != null)
        {
            Debug.Log($"生成されたブロック: {blockPeaceInstance.name}");
        }

        Destroy(gameObject);  // スパナーを削除
    }
}

