using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Animations;

public class Spawner : MonoBehaviour
{
    //配列
    [SerializeField] Block[] Blocks;

    //ランダムなブロックを返す
    Block GetRandomBlock()
    {
        int i = Random.Range(0, Blocks.Length);
        if (Blocks[i])
        {
            return Blocks[i];
        }
        else
        {
            return null;
        }
    }
    //生成
    public Block SpawnBlock()
    {
        Block block = Instantiate(GetRandomBlock(), transform.position, Quaternion.identity);
        if (block)
        {
            return block;
        }
        else
        {
            return null;
        }
    }
}
