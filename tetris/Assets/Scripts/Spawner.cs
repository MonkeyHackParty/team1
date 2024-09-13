using UnityEngine;

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