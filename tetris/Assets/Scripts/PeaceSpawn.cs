using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaceSpawn : MonoBehaviour
{
    [SerializeField] BlockPeace[] BlockPeaces;
    BlockPeace GetRandomBlockPeace()
    {
        int i = Random.Range(0, BlockPeaces.Length);
        if (BlockPeaces[i])
        {
            return BlockPeaces[i];
        }
        else
        {
            return null;
        }
    }
    //生成
    public BlockPeace SpawnBlockPeace()
    {
        BlockPeace blockpeace = Instantiate(GetRandomBlockPeace(), transform.position, Quaternion.identity);
        if (blockpeace)
        {
            return blockpeace;
        }
        else
        {
            return null;
        }
    }
}
