using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Animations;

public class Spawner : MonoBehaviour
{
    //次のブロックを受け取って生成してそれを返す
    public Block SpawnBlock(Block block)
    {
        block.transform.position = transform.position;
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
