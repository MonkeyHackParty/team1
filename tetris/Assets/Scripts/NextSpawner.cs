using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextSpawner : MonoBehaviour
{
    //配列
    [SerializeField] Block[] Blocks;
    private List<Block> availableBlocks;

    void Awake()
    {
        ResetAvailableBlocks();
    }

    private void ResetAvailableBlocks()
    {
        availableBlocks = new List<Block>(Blocks);
    }

    public Block GetRandomBlock()
    {
        if (availableBlocks.Count == 0)
        {
            ResetAvailableBlocks();
        }

        int index = Random.Range(0, availableBlocks.Count);
        Block selectedBlock = availableBlocks[index];
        availableBlocks.RemoveAt(index);

        return selectedBlock;
    }
    //生成
    public Block NextBlock()
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
