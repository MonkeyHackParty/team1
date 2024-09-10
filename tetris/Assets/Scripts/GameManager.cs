using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Spawner spawner;
    Block activeBlock;
    [SerializeField] private float dropInterval = 0.25f;
    float nextdropTimer;
    Board board;
    private void Start()
    {
        //初回のブロック
        spawner = GameObject.FindObjectOfType<Spawner>();

board=GameObject

        if (!activeBlock)
        {
            activeBlock = spawner.SpawnBlock();
        }
    }
    //落ちてく処理
    private void Update()
    {
        //クールタイム
        if (Time.time > nextdropTimer)
        {
            nextdropTimer = Time.time + dropInterval;
            if (activeBlock)
            {
                activeBlock.MoveDown();
            }
        }

    }
}
