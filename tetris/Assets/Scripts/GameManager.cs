using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
    float beforerotationZ;
    float afterrotationZ;

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
            beforerotationZ = activeBlock.transform.eulerAngles.z;
            activeBlock.RotateRight();
            nextKeyRotatetimer = Time.time + nextKeyRotateInterval;
            if (!board.CheckPosition(activeBlock))
            {
                TryRotateLeftRight(activeBlock, 1);
            }
        }
        //左回転
        else if (Input.GetKey(KeyCode.Z) && (Time.time > nextKeyRotatetimer) || Input.GetKeyDown(KeyCode.Z))
        {
            beforerotationZ = activeBlock.transform.eulerAngles.z;
            activeBlock.RotateLeft();
            nextKeyRotatetimer = Time.time + nextKeyRotateInterval;
            if (!board.CheckPosition(activeBlock))
            {
                TryRotateLeftRight(activeBlock, 2);
            }
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
    //回転の処理
    void TryRotateLeftRight(Block block, int rotate)
    {

        afterrotationZ = block.transform.eulerAngles.z;
        Vector3 savePosition = block.transform.position;
        Vector3 savePosition1 = block.transform.position;
        Vector3 savePosition2 = block.transform.position;
        if (block.CompareTag("I"))
        {
            switch (afterrotationZ)
            {
                //B
                case 270:
                    switch (beforerotationZ)
                    {
                        //A
                        case 0:
                            for (int i = 0; i < 2; ++i)
                            {
                                block.MoveLeft();

                            }
                            savePosition1 = block.transform.position;
                            break;
                        //C
                        case 180:
                            block.MoveRight();
                            savePosition1 = block.transform.position;
                            break;
                    }
                    break;
                //D
                case 90:
                    switch (beforerotationZ)
                    {
                        //A
                        case 0:
                            block.MoveLeft();
                            savePosition1 = block.transform.position;
                            break;
                        //C
                        case 180:
                            for (int i = 0; i < 2; i++)
                            {
                                block.MoveRight();
                            }
                            savePosition1 = block.transform.position;
                            break;
                    }
                    break;
                //A
                case 0:
                    switch (beforerotationZ)
                    {
                        //D
                        case 90:
                            for (int i = 0; i < 2; i++)
                            {
                                block.MoveLeft();
                            }

                            savePosition1 = block.transform.position;
                            break;
                        //B
                        case 270:
                            for (int i = 0; i < 2; i++)
                            {
                                block.MoveRight();
                            }
                            savePosition1 = block.transform.position;
                            break;
                    }
                    break;
                //C
                case 180:
                    switch (beforerotationZ)
                    {
                        //B
                        case 270:
                            block.MoveLeft();
                            savePosition1 = block.transform.position;
                            break;
                        //D
                        case 90:
                            block.MoveRight();
                            savePosition1 = block.transform.position;
                            break;
                    }
                    break;
            }


            if (!board.CheckPosition(block))
            {
                switch (afterrotationZ)
                {
                    //D
                    case 90:
                    //B
                    case 270:
                        switch (beforerotationZ)
                        {
                            //A
                            case 0:
                                for (int i = 0; i < 3; i++)
                                {
                                    block.MoveRight();
                                }
                                savePosition2 = block.transform.position;
                                break;
                            //C
                            case 180:
                                for (int i = 0; i < 3; i++)
                                {
                                    block.MoveLeft();
                                }
                                savePosition2 = block.transform.position;
                                break;
                        }
                        break;
                    //A
                    case 0:
                        switch (beforerotationZ)
                        {
                            //D
                            case 90:
                                for (int i = 0; i < 3; i++)
                                {
                                    block.MoveRight();
                                }
                                savePosition2 = block.transform.position;
                                break;
                            //B
                            case 270:
                                for (int i = 0; i < 3; i++)
                                {
                                    block.MoveLeft();
                                }
                                savePosition2 = block.transform.position;
                                break;

                        }
                        break;
                    //C
                    case 180:
                        switch (beforerotationZ)
                        {
                            //D
                            case 90:
                                for (int i = 0; i < 3; i++)
                                {
                                    block.MoveLeft();
                                }
                                savePosition2 = block.transform.position;
                                break;
                            //B
                            case 270:
                                for (int i = 0; i < 3; i++)
                                {
                                    block.MoveRight();
                                }
                                savePosition2 = block.transform.position;
                                break;

                        }
                        break;

                }
                if (!board.CheckPosition(block))
                {
                    switch (afterrotationZ)
                    {
                        //B
                        case 270:
                            block.transform.position = savePosition1;
                            for (int i = 0; i < rotate; i++)
                            {
                                block.MoveDown();
                            }
                            break;
                        //D
                        case 90:
                            block.transform.position = savePosition1;
                            for (int i = 0; i < rotate; i++)
                            {
                                block.MoveUp();
                            }
                            break;
                        //A
                        case 0:
                        //C
                        case 180:
                            switch (beforerotationZ)
                            {
                                //B
                                case 270:
                                    block.transform.position = savePosition1;
                                    for (int i = 2; i > 0; i = i - rotate)
                                    {
                                        block.MoveUp();
                                    }
                                    break;
                                //D
                                case 90:
                                    block.transform.position = savePosition2;
                                    for (int i = 2; i > 0; i = i - rotate)
                                    {
                                        block.MoveDown();
                                    }
                                    break;
                            }
                            break;
                    }


                    if (!board.CheckPosition(block))
                    {
                        switch (afterrotationZ)
                        {
                            //B
                            case 270:
                                block.transform.position = savePosition2;
                                for (int i = 2; i > 0; i = i - rotate)
                                {
                                    block.MoveUp();
                                }
                                break;
                            //D
                            case 90:
                                block.transform.position = savePosition2;
                                for (int i = 2; i > 0; i = i - rotate)
                                {
                                    block.MoveDown();
                                }
                                break;
                            //A
                            case 0:
                            //C
                            case 180:
                                switch (beforerotationZ)
                                {
                                    //D
                                    case 90:
                                        block.transform.position = savePosition1;
                                        for (int i = 0; i < rotate; i++)
                                        {
                                            block.MoveUp();
                                        }
                                        break;
                                    //B
                                    case 270:
                                        block.transform.position = savePosition2;
                                        for (int i = 0; i < rotate; i++)
                                        {
                                            block.MoveDown();
                                        }
                                        break;
                                }
                                break;
                        }
                        if (!board.CheckPosition(block))
                        {
                            block.transform.position = savePosition;
                            switch (rotate)
                            {
                                case 1:
                                    block.RotateLeft();
                                    break;
                                case 2:
                                    block.RotateRight();
                                    break;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            switch (afterrotationZ)
            {
                //B
                case 270:
                    block.MoveLeft();
                    break;
                //D
                case 90:
                    block.MoveRight();
                    break;
                //A
                case 0:
                //C
                case 180:
                    switch (beforerotationZ)
                    {
                        //D
                        case 90:
                            block.MoveLeft();
                            break;
                        //B
                        case 270:
                            block.MoveRight();
                            break;
                    }
                    break;
            }
            if (!board.CheckPosition(block))
            {
                switch (afterrotationZ)
                {
                    //D
                    case 90:
                    //B
                    case 270:
                        block.MoveUp();
                        break;
                    //A
                    case 0:
                    //C
                    case 180:
                        block.MoveDown();
                        break;
                }
                if (!board.CheckPosition(block))
                {
                    block.transform.position = savePosition;
                    switch (afterrotationZ)
                    {
                        //D
                        case 90:
                        //B
                        case 270:
                            for (int i = 0; i < 2; ++i)
                            {
                                block.MoveDown();
                            }
                            break;
                        //A
                        case 0:
                        //C
                        case 180:
                            for (int i = 0; i < 2; ++i)
                            {
                                block.MoveUp();
                            }
                            break;
                    }
                    if (!board.CheckPosition(block))
                    {
                        switch (afterrotationZ)
                        {
                            //B
                            case 270:
                                block.MoveLeft();
                                break;
                            //D
                            case 90:
                                block.MoveRight();
                                break;
                            //A
                            case 0:
                            //C
                            case 180:
                                switch (beforerotationZ)
                                {
                                    //B
                                    case 270:
                                        block.MoveRight();
                                        break;
                                    //D
                                    case 90:
                                        block.MoveLeft();
                                        break;
                                }
                                break;
                        }
                        if (!board.CheckPosition(block))
                        {
                            block.transform.position = savePosition;
                            switch (rotate)
                            {
                                case 1:
                                    block.RotateLeft();
                                    break;
                                case 2:
                                    block.RotateRight();
                                    break;
                            }
                        }
                    }
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
