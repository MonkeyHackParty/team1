using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // シーン遷移用

public class GameManager : MonoBehaviour
{
    Spawner spawner;
    Block activeBlock;
    NextSpawner nextSpawner;
    Block setBlock;

    [SerializeField] private float dropInterval = 0.25f;
    float nextdropTimer;
    Board board;

    [SerializeField] private GameObject gameOverUI;  // ゲームオーバー画面のUI
    private bool isGameOver = false;  // ゲームオーバー状態を管理するフラグ

    float nextKeyDowntimer, nextKeyLeftRighttimer, nextKeyRotatetimer;
    [SerializeField] private float nextKeyDownInterval, nextKeyLeftRightInterval, nextKeyRotateInterval;

    private void Start()
    {
        spawner = GameObject.FindObjectOfType<Spawner>();
        board = GameObject.FindObjectOfType<Board>();
        nextSpawner = GameObject.FindObjectOfType<NextSpawner>();

        // スポナーの位置をきれいにする
        spawner.transform.position = Rounding.Round(spawner.transform.position);

        nextKeyDowntimer = Time.time + nextKeyDownInterval;
        nextKeyLeftRighttimer = Time.time + nextKeyLeftRightInterval;
        nextKeyRotatetimer = Time.time + nextKeyRotateInterval;

        if (!activeBlock)
        {
            // 初回のブロック生成
            setBlock = nextSpawner.NextBlock();
            setBlock.MakeRandomPeace();
            activeBlock = spawner.SpawnBlock(setBlock);

            // 次のブロック生成
            setBlock = nextSpawner.NextBlock();
            setBlock.MakeRandomPeace();
        }
    }

    private void Update()
    {
        if (isGameOver)
        {
            return;  // ゲームオーバー時は操作を無効化
        }

        PlayerInput();
    }

    void PlayerInput()
    {
        if (Input.GetKey(KeyCode.D) && (Time.time > nextKeyLeftRighttimer) || Input.GetKeyDown(KeyCode.D))
        {
            activeBlock.MoveRight();
            nextKeyLeftRighttimer = Time.time + nextKeyLeftRightInterval;
            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.MoveLeft();
            }
        }
        else if (Input.GetKey(KeyCode.A) && (Time.time > nextKeyLeftRighttimer) || Input.GetKeyDown(KeyCode.A))
        {
            activeBlock.MoveLeft();
            nextKeyLeftRighttimer = Time.time + nextKeyLeftRightInterval;
            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.MoveRight();
            }
        }
        else if (Input.GetKey(KeyCode.E) && (Time.time > nextKeyRotatetimer) || Input.GetKeyDown(KeyCode.E))
        {
            activeBlock.RotateRight();
            nextKeyRotatetimer = Time.time + nextKeyRotateInterval;
            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.RotateLeft();
            }
        }
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

    // 底についたときの処理
    void BottomBoard()
    {
        activeBlock.MoveUp();  // 一個上げる
        board.SaveBlockInGrid(activeBlock);  // 座標を保存

        // ゲームオーバー判定
        if (board.OverLimit(activeBlock))
        {
            GameOver();
            return;  // ゲームオーバーなら次の処理をしない
        }

        // 合体処理を呼び出し
        board.CheckForMerge(activeBlock);



        // 次のブロックをスポーン
        activeBlock = spawner.SpawnBlock(setBlock);
        setBlock = nextSpawner.NextBlock();  // 新しい次のブロックを生成してブロックの中身をランダムにする
        setBlock.MakeRandomPeace();

        nextKeyDowntimer = Time.time;
        nextKeyLeftRighttimer = Time.time;
        nextKeyRotatetimer = Time.time;

        //board.ClearAllRows();  // 行を削除する
    }

    // リスタート処理
    public void RestartGame()
    {
        gameOverUI.SetActive(false);  // ゲームオーバーUIを非表示
        isGameOver = false;  // ゲームオーバーフラグをリセット

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // 現在のシーンを再読み込みしてリスタート
    }

    // ゲームオーバー処理
    private void GameOver()
    {
        isGameOver = true;  // ゲームオーバーフラグを立てる
        SceneManager.LoadScene("GameOverScene");  // 新しいゲームオーバーシーンに遷移
        Debug.Log("Game Over!");
    }
}
