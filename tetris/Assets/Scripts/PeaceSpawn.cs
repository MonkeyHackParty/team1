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
    public void SpawnBlockPeace()
    {
        Transform parentTransform = transform.parent;
        Instantiate(GetRandomBlockPeace(), transform.position, Quaternion.identity, parentTransform);
        Destroy(gameObject);
        
    }
}