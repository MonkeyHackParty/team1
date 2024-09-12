using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Spawner spawner;
    Block activeBlock;
    NextSpawner nextSpawner;
    Block setBlock;

    [SerializeField] private float dropInterval = 0.25f;
    float nextdropTimer;
    Board board;

    float nextKeyDowntimer, nextKeyLeftRighttimer, nextKeyRotatetimer;

    [SerializeField] private float nextKeyDownInterval, nextKeyLeftRightInterval, nextKeyRotateInterval;
    private void Start()
    {

        spawner = GameObject.FindObjectOfType<Spawner>();

        board = GameObject.FindObjectOfType<Board>();

        nextSpawner = GameObject.FindObjectOfType<NextSpawner>();

        //スポナーの位置を綺麗に
        spawner.transform.position = Rounding.Round(spawner.transform.position);

        nextKeyDowntimer = Time.time + nextKeyDownInterval;

        nextKeyLeftRighttimer = Time.time + nextKeyLeftRightInterval;

        nextKeyRotatetimer = Time.time + nextKeyRotateInterval;

        if (!activeBlock)
        {
            //初回のブロックを生成してブロックの中身をランダムにする
            setBlock = nextSpawner.NextBlock();
            setBlock.MakeRandomPeace();
            activeBlock = spawner.SpawnBlock(setBlock);
            //次のブロックを生成してブロックの中身をランダムにする
            setBlock = nextSpawner.NextBlock();
            setBlock.MakeRandomPeace();
        }
    }
    //動く処理
    private void Update()
    {
        PlayerInput();

    }
    void PlayerInput()
    {
        //右
        if (Input.GetKey(KeyCode.D) && (Time.time > nextKeyLeftRighttimer) || Input.GetKeyDown(KeyCode.D))
        {
            activeBlock.MoveRight();

            nextKeyLeftRighttimer = Time.time + nextKeyLeftRightInterval;
            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.MoveLeft();
            }
        }
        //左
        else if (Input.GetKey(KeyCode.A) && (Time.time > nextKeyLeftRighttimer) || Input.GetKeyDown(KeyCode.A))
        {
            activeBlock.MoveLeft();

            nextKeyLeftRighttimer = Time.time + nextKeyLeftRightInterval;
            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.MoveRight();
            }
        }
        //右回転
        else if (Input.GetKey(KeyCode.E) && (Time.time > nextKeyRotatetimer) || Input.GetKeyDown(KeyCode.E))
        {
            activeBlock.RotateRight();
            nextKeyRotatetimer = Time.time + nextKeyRotateInterval;
            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.RotateLeft();
            }
        }
        //下加速
        else if (Input.GetKey(KeyCode.S) && (Time.time > nextKeyDowntimer) || Time.time > nextdropTimer)
        {
            activeBlock.MoveDown();

            nextKeyDowntimer = Time.time + nextKeyDownInterval;
            nextdropTimer = Time.time + dropInterval;
            if (!board.CheckPosition(activeBlock))
            {
                BottomBoard();
            }
        }
    }
    //底についたときの処理
    void BottomBoard()
    {
        //一個上げる
        activeBlock.MoveUp();
        //座標を保存
        board.SaveBlockInGrid(activeBlock);
        // 次のブロックをスポーン
        activeBlock = spawner.SpawnBlock(setBlock);
        setBlock = nextSpawner.NextBlock(); // 新しい次のブロックを生成してブロックの中身をランダムにする
        setBlock.MakeRandomPeace();

        nextKeyDowntimer = Time.time;
        nextKeyLeftRighttimer = Time.time;
        nextKeyRotatetimer = Time.time;

        //削除
        board.ClearAllRows();
    }
}
