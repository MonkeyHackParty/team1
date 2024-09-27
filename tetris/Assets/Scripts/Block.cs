using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using TreeEditor;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private bool canRotate = true;

    [SerializeField] PeaceSpawn[] peaceSpawns;
    //動き方
    void Move(Vector3 moveDirection)
    {
        transform.position += moveDirection;
    }
    //各種動き
    public void MoveLeft()
    {
        Move(new Vector3(-1, 0, 0));
    }
    public void MoveRight()
    {
        Move(new Vector3(1, 0, 0));
    }
    public void MoveUp()
    {
        Move(new Vector3(0, 1, 0));
    }
    public void MoveDown()
    {
        Move(new Vector3(0, -1, 0));
    }
    //回転
    public void RotateRight()
    {
        if (canRotate)
        {
            transform.Rotate(0, 0, -90);
        }
    }
    public void RotateLeft()
    {
        if (canRotate)
        {
            transform.Rotate(0, 0, 90);
        }
    }
    //ランダムなブロックの中身作成
    public void MakeRandomPeace()
    {
        if (peaceSpawns != null && peaceSpawns.Length > 0)
        {
            // 全ての PeaceSpawn に対して SpawnBlockPeace を呼び出す
            foreach (PeaceSpawn spawn in peaceSpawns)
            {
                spawn.SpawnBlockPeace();
            }
        }
    }
}
