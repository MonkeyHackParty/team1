using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Board : MonoBehaviour
{
    private Transform[,] grid;

    [SerializeField] private Transform tetrisFlame;
    [SerializeField] private int height = 30, width = 10, header = 8;//ボードの大きさ
    private void Awake()
    {
        grid = new Transform[width, height];
    }
    private void Start()
    {
        CreateBoard();
    }
    //フィールドの作成
    void CreateBoard()
    {
        if (tetrisFlame)
        {
            for (int y = 0; y < height - header; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Transform clone = Instantiate(tetrisFlame, new Vector3(x, y, 0), Quaternion.identity);
                    clone.transform.parent = transform;
                }
            }
        }
    }
    //はみ出てないかのチェック
    public bool CheckPosition(Block block)
    {
        foreach (Transform item in block.transform)
        {
            Vector2 pos = Rounding.Round(item.position);

            if (!BoardOutCheck((int)pos.x, (int)pos.y))
            {
                return false;
            }
            if (BlockCheck((int)pos.x, (int)pos.y, block))
            {
                return false;
            }
        }
        return true;
    }
    //枠内判定
    bool BoardOutCheck(int x, int y)
    {
        return (x >= 0 && x < width && y >= 0);
    }
    //他のブロックがないか判定
    bool BlockCheck(int x, int y, Block block)
    {
        return (grid[x, y] != null && grid[x, y].parent != block.transform);
    }
    //ブロックの座標を記録
    public void SaveBlockInGrid(Block block)
    {
        foreach (Transform item in block.transform)
        {
            Vector2 pos = Rounding.Round(item.position);

            grid[(int)pos.x, (int)pos.y] = item;
        }
    }

    // ブロックが着地した時に呼び出される
    public void CheckForMerge(Block block)
    {
        List<Transform> mergedBlocks = new List<Transform>();  // 合体したブロックの追跡

        // ブロックの子であるBlockPeaceを確認
        foreach (Transform child in block.transform)
        {
            BlockPeace blockPeace = child.GetComponent<BlockPeace>();
            if (blockPeace != null && !mergedBlocks.Contains(child))
            {
                Vector2 pos = Rounding.Round(child.position);

                // 左右上下の隣接ブロックをチェック
                CheckAndMergeAdjacentBlocks(blockPeace, (int)pos.x - 1, (int)pos.y, ref mergedBlocks);  // 左
                CheckAndMergeAdjacentBlocks(blockPeace, (int)pos.x + 1, (int)pos.y, ref mergedBlocks);  // 右
                CheckAndMergeAdjacentBlocks(blockPeace, (int)pos.x, (int)pos.y - 1, ref mergedBlocks);  // 下
                CheckAndMergeAdjacentBlocks(blockPeace, (int)pos.x, (int)pos.y + 1, ref mergedBlocks);  // 上
            }
        }
    }

    // 隣接するブロックを確認して合体させる
    private void CheckAndMergeAdjacentBlocks(BlockPeace blockPeace, int x, int y, ref List<Transform> mergedBlocks)
    {
        if (x >= 0 && x < width && y >= 0 && y < height && grid[x, y] != null)
        {
            BlockPeace adjacentBlockPeace = grid[x, y].GetComponent<BlockPeace>();
            if (adjacentBlockPeace != null && blockPeace.GetNumber() == adjacentBlockPeace.GetNumber())
            {
                // 合体処理
                MergeBlocks(blockPeace, adjacentBlockPeace, ref mergedBlocks);
            }
        }
    }

    // 合体処理
   private void MergeBlocks(BlockPeace blockPeace, BlockPeace adjacentBlockPeace, ref List<Transform> mergedBlocks)
{
    // 数値を倍にして更新
    int newNumber = blockPeace.GetNumber() * 2;
    blockPeace.SetNumber(newNumber);  // 合体後にスプライトと数値を更新

    // 合体したブロックを削除
    Destroy(adjacentBlockPeace.gameObject);

    // デバッグログで確認
    Debug.Log($"合体されたブロック: {blockPeace.name} (元の数値: {blockPeace.GetNumber() / 2}, 新しい数値: {newNumber})");

    // グリッドから削除処理も行う
    Vector2 adjacentPos = Rounding.Round(adjacentBlockPeace.transform.position);
    grid[(int)adjacentPos.x, (int)adjacentPos.y] = null;

    // 合体済みリストに追加
    mergedBlocks.Add(blockPeace.transform);
}

    //消す処理
    //public void ClearAllRows()
    //{
    //    for (int y = 0; y < height; y++)
    //   {
    //        if (IsComplate(y))
    //        {
    //            ClearRow(y);

    //            ShiftRowsDown(y + 1);
    //            y--;
    //        }
    //    }
    //}
    //全部埋まってるかの確認
    //bool IsComplate(int y)
    //{
    //    for (int x = 0; x < width; x++)
    //    {
    //        if (grid[x, y] == null)
    //        {
    //            return false;
    //        }
    //    }
    //    return true;
    //}
    //一列消す
    //void ClearRow(int y)
    //{
    //    for (int x = 0; x < width; x++)
    //    {
    //        if (grid[x, y] != null)
    //        {
    //            Destroy(grid[x, y].gameObject);
    //        }
    //        grid[x, y] = null;
    //    }
    //}
    //上にある奴らを一個落とす
    //void ShiftRowsDown(int startY)
    //{
    //    for (int y = startY; y < height; y++)
    //    {
    //        for (int x = 0; x < width; x++)
    //        {
    //            if (grid[x, y] != null)
    //            {
    //                grid[x, y - 1] = grid[x, y];
    //               grid[x, y] = null;
    //                grid[x, y - 1].position += new Vector3(0, -1, 0);
    //            }
    //        }
    //    }
    //}

    //ブロックが上についてしまったか
    public bool OverLimit(Block block)
    {
        foreach (Transform item in block.transform)
        {
            if (item.transform.position.y >= height - header)
            {
                return true;
            }
        }
        return false;
    }
}