using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Spawner spawner;
    Block activeBlock;
    private Block nextBlock;

    [SerializeField] private float dropInterval = 0.25f;
    float nextdropTimer;
    Board board;

    float nextKeyDowntimer, nextKeyLeftRighttimer, nextKeyRotatetimer;

    [SerializeField] private float nextKeyDownInterval, nextKeyLeftRightInterval, nextKeyRotateInterval;
    private void Start()
    {
        //初回のブロック
        spawner = GameObject.FindObjectOfType<Spawner>();

        board = GameObject.FindObjectOfType<Board>();

        //スポナーの位置を綺麗に
        spawner.transform.position = Rounding.Round(spawner.transform.position);

        nextKeyDowntimer = Time.time + nextKeyDownInterval;

        nextKeyLeftRighttimer = Time.time + nextKeyLeftRightInterval;

        nextKeyRotatetimer = Time.time + nextKeyRotateInterval;

        if (!activeBlock)
        {
            activeBlock = spawner.SpawnBlock();
            nextBlock = spawner.SpawnBlock(); // 次のブロックを生成
        }
    }
    //落ちてく処理
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
        activeBlock = nextBlock;
        nextBlock = spawner.SpawnBlock(); // 新しい次のブロックを生成

        nextKeyDowntimer = Time.time;
        nextKeyLeftRighttimer = Time.time;
        nextKeyRotatetimer = Time.time;

        //削除
        board.ClearAllRows();
    }
}
