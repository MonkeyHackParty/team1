using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Spawner spawner;
    Block activeBlock;
    Block ghostBlock;
    NextSpawner nextSpawner;

    Block holdBlock;
    Block saveBlock;

    [SerializeField] private float dropInterval = 0.25f;
    float nextdropTimer;
    Board board;
    HoldSpawner holdSpawner;

    private bool holdcheck = true;

    [SerializeField] private GameObject gameOverUI;  // ゲームオーバー画面のUI
    private bool isGameOver = false;  // ゲームオーバー状態を管理するフラグ

    float nextKeyDowntimer, nextKeyLeftRighttimer, nextKeyRotatetimer;
    [SerializeField] private float nextKeyDownInterval, nextKeyLeftRightInterval, nextKeyRotateInterval;

    [SerializeField] private GameObject gameOverPanel;

    private void Start()
    {
        spawner = FindObjectOfType<Spawner>();
        board = FindObjectOfType<Board>();
        nextSpawner = FindObjectOfType<NextSpawner>();
        holdSpawner = FindObjectOfType<HoldSpawner>();

        // スポナーの位置をきれいにする
        spawner.transform.position = Rounding.Round(spawner.transform.position);

        nextKeyDowntimer = Time.time + nextKeyDownInterval;
        nextKeyLeftRighttimer = Time.time + nextKeyLeftRightInterval;
        nextKeyRotatetimer = Time.time + nextKeyRotateInterval;

        if (!activeBlock)
        {
            activeBlock = GetNextBlock();
            CreateGhostBlock();
        }

        if (gameOverPanel.activeInHierarchy)
        {
            gameOverPanel.SetActive(false);
        }
    }

    private Block GetNextBlock()
    {
        // 次のブロックを取得してスポーン
        Block nextBlock = nextSpawner.GetAndShiftNextBlock();
        return spawner.SpawnBlock(nextBlock);
    }

    private void Update()
    {
        if (isGameOver) return;

        PlayerInput();
        UpdateGhostBlock();
    }

    void PlayerInput()
    {
        // ホールド機能
        if (Input.GetKeyDown(KeyCode.C))
        {
            Hold();
        }
        // ハードドロップ
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            while (board.CheckPosition(activeBlock))
            {
                activeBlock.MoveDown();
            }
            if (board.OverLimit(activeBlock))
            {
                GameOver();
            }
            else
            {
                activeBlock.MoveUp();
                BottomBoard();
            }
        }
        // 右移動
        else if (Input.GetKey(KeyCode.RightArrow) && Time.time > nextKeyLeftRighttimer || Input.GetKeyDown(KeyCode.RightArrow))
        {
            activeBlock.MoveRight();
            nextKeyLeftRighttimer = Time.time + nextKeyLeftRightInterval;
            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.MoveLeft();
            }
        }
        // 左移動
        else if (Input.GetKey(KeyCode.LeftArrow) && Time.time > nextKeyLeftRighttimer || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            activeBlock.MoveLeft();
            nextKeyLeftRighttimer = Time.time + nextKeyLeftRightInterval;
            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.MoveRight();
            }
        }
        // 回転
        else if (Input.GetKey(KeyCode.UpArrow) && Time.time > nextKeyRotatetimer || Input.GetKeyDown(KeyCode.UpArrow))
        {
            activeBlock.RotateRight();
            nextKeyRotatetimer = Time.time + nextKeyRotateInterval;
            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.RotateLeft();
            }
        }
        // 下加速
        else if (Input.GetKey(KeyCode.DownArrow) && Time.time > nextKeyDowntimer || Time.time > nextdropTimer)
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
                    activeBlock.MoveUp();
                    BottomBoard();
                }
            }
        }
    }

    // ブロックが着地したときの処理
    void BottomBoard()
    {
        board.SaveBlockInGrid(activeBlock);

        if (board.OverLimit(activeBlock))
        {
            GameOver();
            return;
        }

        board.CheckForMerge(activeBlock);

        activeBlock = GetNextBlock();
        CreateGhostBlock();

        nextKeyDowntimer = Time.time;
        nextKeyLeftRighttimer = Time.time;
        nextKeyRotatetimer = Time.time;
    }

    // ホールド機能
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
                activeBlock = GetNextBlock();
            }
            holdBlock = holdSpawner.HoldBlock(saveBlock);
            holdcheck = false;

            Destroy(ghostBlock.gameObject);
            CreateGhostBlock();
        }
    }

    // ゴーストブロックを作成
    void CreateGhostBlock()
    {
        if (activeBlock != null)
        {
            ghostBlock = Instantiate(activeBlock, activeBlock.transform.position, activeBlock.transform.rotation);
            ChangeGhostAppearance();
        }
    }

    // ゴーストブロックの外観を変更
    void ChangeGhostAppearance()
    {
        foreach (Transform child in ghostBlock.transform)
        {
            SpriteRenderer renderer = child.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                Color color = renderer.color;
                color.a = 0.2f;  // ゴーストブロックを半透明にする
                renderer.color = color;
                renderer.sortingOrder = -1;  // ゴーストブロックを背景に表示
            }
        }
    }

    // ゴーストブロックを更新
    void UpdateGhostBlock()
    {
        if (ghostBlock != null)
        {
            ghostBlock.transform.position = activeBlock.transform.position;
            ghostBlock.transform.rotation = activeBlock.transform.rotation;

            while (board.CheckPosition(ghostBlock))
            {
                ghostBlock.MoveDown();
            }

            ghostBlock.MoveUp();
        }
    }

    // ゲームオーバー処理
    private void GameOver()
    {
        isGameOver = true;  // ゲームオーバーフラグを立てる
        SceneManager.LoadScene("GameOverScene");  // 新しいゲームオーバーシーンに遷移
        Debug.Log("Game Over!");
    }
}
