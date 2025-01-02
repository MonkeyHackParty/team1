using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Linq;
using System;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    Spawner spawner;
    Block activeBlock;
    Block ghostBlock;
    NextSpawner nextSpawner;

    Block holdBlock;
    Block saveBlock;

    [SerializeField]
    private float dropInterval = 0.25f;
    float nextdropTimer = 0.25f;
    Board board;
    HoldSpawner holdSpawner;

    private bool holdcheck = true;

    float nextKeyDowntimer,
        nextKeyLeftRighttimer,
        nextKeyRotatetimer;

    [SerializeField]
    private float nextKeyDownInterval,
        nextKeyLeftRightInterval,
        nextKeyRotateInterval;

    [SerializeField]
    private GameObject gameOverPanel;

    bool gameOver;
    float beforerotationZ;
    float afterrotationZ;
    float setuplimit = 0f;
    bool setup = false;

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
        // 次のブロックを取得
        Block nextBlock = nextSpawner.GetAndShiftNextBlock();

        return spawner.SpawnBlock(nextBlock);
    }

    //動く処理
    private void Update()
    {
        if (gameOver)
        {
            return;
        }
        PlayerInput();
        UpdateGhostBlock();
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
            while (board.IsWithinPosition(activeBlock))
            {
                activeBlock.MoveDown();
            }
            if (board.OverLimit(activeBlock))
            {
                GameOver();
            }
            else
            {
                //一個上げる
                activeBlock.MoveUp();
                BottomBoard();
            }
        }
        //右
        else if (
            Input.GetKey(KeyCode.RightArrow) && (Time.time > nextKeyLeftRighttimer)
            || Input.GetKeyDown(KeyCode.RightArrow)
        )
        {
            activeBlock.MoveRight();

            nextKeyLeftRighttimer = Time.time + nextKeyLeftRightInterval;
            if (!board.IsWithinPosition(activeBlock))
            {
                activeBlock.MoveLeft();
            }
        }
        //左
        else if (
            Input.GetKey(KeyCode.LeftArrow) && (Time.time > nextKeyLeftRighttimer)
            || Input.GetKeyDown(KeyCode.LeftArrow)
        )
        {
            activeBlock.MoveLeft();

            nextKeyLeftRighttimer = Time.time + nextKeyLeftRightInterval;
            if (!board.IsWithinPosition(activeBlock))
            {
                activeBlock.MoveRight();
            }
        }
        //右回転
        else if (
            Input.GetKey(KeyCode.UpArrow) && (Time.time > nextKeyRotatetimer)
            || Input.GetKeyDown(KeyCode.UpArrow)
        )
        {
            beforerotationZ = activeBlock.transform.eulerAngles.z;
            activeBlock.RotateRight();
            nextKeyRotatetimer = Time.time + nextKeyRotateInterval;
            if (!board.IsWithinPosition(activeBlock))
            {
                TryRotateLeftRight(activeBlock, 1);
            }
        }
        //左回転
        else if (
            Input.GetKey(KeyCode.Z) && (Time.time > nextKeyRotatetimer)
            || Input.GetKeyDown(KeyCode.Z)
        )
        {
            beforerotationZ = activeBlock.transform.eulerAngles.z;
            activeBlock.RotateLeft();
            nextKeyRotatetimer = Time.time + nextKeyRotateInterval;
            if (!board.IsWithinPosition(activeBlock))
            {
                TryRotateLeftRight(activeBlock, 2);
            }
        }
        //下加速
        else
        {
            activeBlock.MoveDown();
            if (!board.IsWithinPosition(activeBlock))
            {
                setuplimit += Time.deltaTime;
                if (setuplimit >= 1f)
                {
                    setup = true;
                }
            }
            activeBlock.MoveUp();
            if (
                Input.GetKey(KeyCode.DownArrow) && (Time.time > nextKeyDowntimer)
                || Time.time > nextdropTimer
            )
            {
                activeBlock.MoveDown();

                nextKeyDowntimer = Time.time + nextKeyDownInterval;
                nextdropTimer = Time.time + dropInterval;
                if (!board.IsWithinPosition(activeBlock))
                {
                    if (board.OverLimit(activeBlock))
                    {
                        GameOver();
                    }
                    else
                    {
                        //一個上げる
                        activeBlock.MoveUp();
                        if (setup)
                        {
                            BottomBoard();
                        }
                    }
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

            if (!board.IsWithinPosition(block))
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
                if (!board.IsWithinPosition(block))
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

                    if (!board.IsWithinPosition(block))
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
                        if (!board.IsWithinPosition(block))
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
            if (!board.IsWithinPosition(block))
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
                if (!board.IsWithinPosition(block))
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
                    if (!board.IsWithinPosition(block))
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
                        if (!board.IsWithinPosition(block))
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
        CancelInvoke();
        while (board.IsWithinPosition(activeBlock))
        {
            activeBlock.MoveDown();
        }
        //一個上げる
        activeBlock.MoveUp();

        //座標を保存
        board.SaveBlockInGrid(activeBlock);
        CheckAdjacentBlockNumbers();
        // 次のブロックをスポーン
        activeBlock = GetNextBlock();
        while (!board.IsWithinPosition(activeBlock))
        {
            activeBlock.MoveUp();
        }
        //ゴーストブロックの変更
        Destroy(ghostBlock.gameObject);
        CreateGhostBlock();

        holdcheck = true;

        setup = false;
        setuplimit = 0f;
        nextKeyDowntimer = Time.time;
        nextKeyLeftRighttimer = Time.time;
        nextKeyRotatetimer = Time.time;
        nextdropTimer = Time.time + dropInterval;
    }

    private void CheckAdjacentBlockNumbers()
    {
        if (activeBlock != null)
        {
            List<BlockPeace> blockPeaces = activeBlock.GetComponentsInChildren<BlockPeace>().ToList();
            MargeBlock(blockPeaces);
        }
    }

    void MargeBlock(List<BlockPeace> blockPeaces)
    {
        blockPeaces = blockPeaces.OrderBy(bp => bp.Number).ToList();
        HashSet<BlockPeace> movedBlocks = new HashSet<BlockPeace>();
        List<Vector3Int> positions = new List<Vector3Int>();
        for (int i = 0; i < blockPeaces.Count; i++)
        {
            BlockPeace blockPeace = blockPeaces[i];
            HashSet<BlockPeace> visited = new HashSet<BlockPeace>();
            if (!visited.Contains(blockPeace))
            {
                ExploreBlock(blockPeace, visited);
                int count = visited.Count;
                if (count >= 2)
                {
                    Vector3 newPos = visited.OrderBy(bp => bp.transform.position.y).First().transform.position;
                    // コルーチンを格納するリスト
                    List<IEnumerator> moveCoroutines = new List<IEnumerator>();
                    foreach (BlockPeace bp in visited)
                    {
                        Vector3Int pos = Vector3Int.RoundToInt(bp.transform.position);
                        board.RemoveBlock(pos);

                        if (pos != Rounding.Round(newPos))
                        {
                            positions.Add(pos);
                        }
                        BlockMover blockMover = bp.gameObject.AddComponent<BlockMover>();
                        // コルーチンをリストに追加
                        moveCoroutines.Add(blockMover.MoveToPosition(newPos, 0.5f));
                    }

                    // すべてのコルーチンが終了するまで待機
                    StartCoroutine(WaitForAllCoroutines(moveCoroutines, () =>
                    {
                        // すべての移動が終わった後に実行する処理
                        int n = (int)Mathf.Pow(2, count - 1);
                        int newNumber = n * blockPeace.Number;
                        Debug.Log(newNumber);
                        BlockPeace newBlockPeace = board.CreateNewBlock(newPos, newNumber);
                        blockPeaces.Add(newBlockPeace);
                        blockPeaces = blockPeaces.OrderBy(bp => bp.Number).ToList();

                    }));

                }
            }
        }
        positions = positions.OrderByDescending(pos => pos.y).ToList();
        foreach (Vector3Int pos in positions)
        {
            movedBlocks.AddRange(board.ShiftRowsDownColumn(pos.x, pos.y + 1));
        }
        blockPeaces = movedBlocks.ToList();
        if (blockPeaces.Count > 0)
        {
            MargeBlock(blockPeaces);
        }
    }

    private IEnumerator WaitForAllCoroutines(List<IEnumerator> coroutines, System.Action onComplete)
    {
        foreach (var coroutine in coroutines)
        {
            yield return StartCoroutine(coroutine);
        }

        onComplete?.Invoke();
    }

    private void ExploreBlock(BlockPeace blockPeace, HashSet<BlockPeace> visited)
    {
        Stack<BlockPeace> stack = new Stack<BlockPeace>();
        stack.Push(blockPeace);
        int count = 0;

        Vector2Int[] directions = new Vector2Int[]
        {
            Vector2Int.left,
            Vector2Int.right,
            Vector2Int.up,
            Vector2Int.down
        };

        while (stack.Count > 0)
        {
            BlockPeace current = stack.Pop();
            if (visited.Contains(current))
            {
                continue;
            }

            visited.Add(current);
            count++;

            Vector2Int blockPosition = Rounding.RoundToInt(current.transform.position);
            foreach (Vector2Int direction in directions)
            {
                Vector2Int checkPosition = blockPosition + direction;
                Transform neighborTransform = board.GetBlockAtPosition(checkPosition);
                if (neighborTransform != null)
                {
                    BlockPeace adjacentBlockPeace = neighborTransform.GetComponent<BlockPeace>();
                    if (adjacentBlockPeace != null && adjacentBlockPeace.Number == current.Number && !visited.Contains(adjacentBlockPeace))
                    {
                        stack.Push(adjacentBlockPeace);
                    }
                }
            }
        }
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
                //ゴーストブロックの変更
                Destroy(ghostBlock.gameObject);
                CreateGhostBlock();
            }
            else
            {
                activeBlock = GetNextBlock();
                //ゴーストブロックの変更
                Destroy(ghostBlock.gameObject);
                CreateGhostBlock();
            }
            holdBlock = holdSpawner.HoldBlock(saveBlock);
            holdcheck = false;
        }
    }

    //ゲームオーバー
    void GameOver()
    {
        activeBlock.MoveUp();
        if (!gameOverPanel.activeInHierarchy)
        {
            gameOverPanel.SetActive(true);
        }
        gameOver = true;
    }

    //リトライ
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    // ゴーストブロックを作成
    void CreateGhostBlock()
    {
        if (activeBlock != null)
        {
            ghostBlock = Instantiate(
                activeBlock,
                activeBlock.transform.position,
                activeBlock.transform.rotation
            );
            // ゴーストブロックの色や透明度を変更
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
                // ゴーストブロックの透明度を設定
                Color color = renderer.color;
                color.a = 0f; // 透明度を設定
                renderer.color = color;

                // ゴーストブロックの描画順を後ろに設定
                renderer.sortingOrder = -1; // アクティブブロックより低い値にする
                // ゴーストブロックのすべての子オブジェクトからCanvasを持つものを取得して処理
                foreach (Canvas childCanvas in ghostBlock.GetComponentsInChildren<Canvas>())
                {
                    childCanvas.sortingOrder = -1;
                }
            }
        }
    }

    // ゴーストブロックをアップデート
    void UpdateGhostBlock()
    {
        if (ghostBlock != null)
        {
            // ゴーストブロックをアクティブブロックと同じ位置に配置
            ghostBlock.transform.position = activeBlock.transform.position;
            ghostBlock.transform.rotation = activeBlock.transform.rotation;

            // ゴーストブロックを落下させる
            while (board.IsWithinPosition(ghostBlock))
            {
                ghostBlock.MoveDown();
            }

            // 1つ上に戻す（衝突する直前の位置にする）
            ghostBlock.MoveUp();
        }
    }
}
