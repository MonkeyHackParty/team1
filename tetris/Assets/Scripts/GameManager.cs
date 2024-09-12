using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    Spawner spawner;
    Block activeBlock;
    NextSpawner nextSpawner;
    Block setBlock;
    Block holdBlock;
    Block saveBlock;

    [SerializeField] private float dropInterval = 0.25f;
    float nextdropTimer;
    Board board;
    HoldSpawner holdSpawner;

    private bool holdcheck = true;

    float nextKeyDowntimer, nextKeyLeftRighttimer, nextKeyRotatetimer;

    [SerializeField] private float nextKeyDownInterval, nextKeyLeftRightInterval, nextKeyRotateInterval;

    [SerializeField] private GameObject gameOverPanel;

    bool gameOver;

    private void Start()
    {

        spawner = GameObject.FindObjectOfType<Spawner>();

        board = GameObject.FindObjectOfType<Board>();

        nextSpawner = GameObject.FindObjectOfType<NextSpawner>();

        holdSpawner = GameObject.FindObjectOfType<HoldSpawner>();

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
        if (gameOverPanel.activeInHierarchy)
        {
            gameOverPanel.SetActive(false);
        }
    }
    //右回転の処理
    void TryRotateRight(Block block)
    {
        // 通常の回転をまず行う
        block.RotateRight();

        // 回転後に位置が適切かどうか確認
        if (!board.CheckPosition(block))
        {
            // 回転後の位置が適切でない場合に移動を試みる
            bool success = false;

            // 右に移動して確認
            block.MoveRight();
            if (board.CheckPosition(block))
            {
                success = true;
            }
            else
            {
                block.MoveLeft(); // 失敗したら元に戻す
            }

            // 左に移動して確認
            if (!success)
            {
                block.MoveLeft();
                if (board.CheckPosition(block))
                {
                    success = true;
                }
                else
                {
                    block.MoveRight(); // 失敗したら元に戻す
                }
            }

            // 上に移動して確認
            if (!success)
            {
                block.MoveUp();
                if (board.CheckPosition(block))
                {
                    success = true;
                }
                else
                {
                    block.MoveDown(); // 失敗したら元に戻す
                }
            }

            // 下に移動して確認
            if (!success)
            {
                block.MoveDown();
                if (board.CheckPosition(block))
                {
                    success = true;
                }
                else
                {
                    block.MoveUp(); // 失敗したら元に戻す
                }
            }

            // どの移動でも成功しなかった場合、回転を元に戻す
            if (!success)
            {
                block.RotateLeft();
            }
        }
    }
    //左回転の処理
    void TryRotateleft(Block block)
    {
        // 通常の回転をまず行う
        block.RotateLeft();

        // 回転後に位置が適切かどうか確認
        if (!board.CheckPosition(block))
        {
            // 回転後の位置が適切でない場合に移動を試みる
            bool success = false;

            // 右に移動して確認
            block.MoveRight();
            if (board.CheckPosition(block))
            {
                success = true;
            }
            else
            {
                block.MoveLeft(); // 失敗したら元に戻す
            }

            // 左に移動して確認
            if (!success)
            {
                block.MoveLeft();
                if (board.CheckPosition(block))
                {
                    success = true;
                }
                else
                {
                    block.MoveRight(); // 失敗したら元に戻す
                }
            }

            // 上に移動して確認
            if (!success)
            {
                block.MoveUp();
                if (board.CheckPosition(block))
                {
                    success = true;
                }
                else
                {
                    block.MoveDown(); // 失敗したら元に戻す
                }
            }

            // 下に移動して確認
            if (!success)
            {
                block.MoveDown();
                if (board.CheckPosition(block))
                {
                    success = true;
                }
                else
                {
                    block.MoveUp(); // 失敗したら元に戻す
                }
            }

            // どの移動でも成功しなかった場合、回転を元に戻す
            if (!success)
            {
                block.RotateRight();
            }
        }
    }
    //動く処理
    private void Update()
    {
        if (gameOver)
        {
            return;
        }
        PlayerInput();

    }
    void PlayerInput()
    {
        //ホールド
        if (Input.GetKeyDown(KeyCode.C))
        {
            Hold();
        }
        //ハードドロップ
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            while (board.CheckPosition(activeBlock))
            {
                activeBlock.MoveDown();
            }
            BottomBoard();
        }
        //右
        else if (Input.GetKey(KeyCode.RightArrow) && (Time.time > nextKeyLeftRighttimer) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            activeBlock.MoveRight();

            nextKeyLeftRighttimer = Time.time + nextKeyLeftRightInterval;
            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.MoveLeft();
            }
        }
        //左
        else if (Input.GetKey(KeyCode.LeftArrow) && (Time.time > nextKeyLeftRighttimer) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            activeBlock.MoveLeft();

            nextKeyLeftRighttimer = Time.time + nextKeyLeftRightInterval;
            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.MoveRight();
            }
        }
        //右回転
        else if (Input.GetKey(KeyCode.UpArrow) && (Time.time > nextKeyRotatetimer) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            TryRotateRight(activeBlock);
            nextKeyRotatetimer = Time.time + nextKeyRotateInterval;
        }
        //左回転
        else if (Input.GetKey(KeyCode.Z) && (Time.time > nextKeyRotatetimer) || Input.GetKeyDown(KeyCode.Z))
        {
            TryRotateleft(activeBlock);
            nextKeyRotatetimer = Time.time + nextKeyRotateInterval;
        }
        //下加速
        else if (Input.GetKey(KeyCode.DownArrow) && (Time.time > nextKeyDowntimer) || Time.time > nextdropTimer)
        {
            activeBlock.MoveDown();

            nextKeyDowntimer = Time.time + nextKeyDownInterval;
            nextdropTimer = Time.time + dropInterval;
            if (!board.CheckPosition(activeBlock))
            {
                if (board.OverLimit(activeBlock))
                {
                    GameOver();
                }
                else
                {
                    BottomBoard();
                }

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
        setBlock.MakeRandomPeace();//中身をランダムに

        holdcheck = true;

        nextKeyDowntimer = Time.time;
        nextKeyLeftRighttimer = Time.time;
        nextKeyRotatetimer = Time.time;

        //削除
        board.ClearAllRows();
    }
    //ホールド機能
    void Hold()
    {
        if (holdcheck)
        {
            saveBlock = activeBlock;
            if (holdBlock != null)
            {
                activeBlock = spawner.SpawnBlock(holdBlock);
            }
            else
            {
                activeBlock = spawner.SpawnBlock(setBlock);
                setBlock = nextSpawner.NextBlock();
                setBlock.MakeRandomPeace();
            }
            holdBlock = holdSpawner.HoldBlock(saveBlock);
            holdcheck = false;
        }
    }
    void GameOver()
    {
        activeBlock.MoveUp();
        if (!gameOverPanel.activeInHierarchy)
        {
            gameOverPanel.SetActive(true);
        }
        gameOver = true;
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
