using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Transform tetrisFlame;
    [SerializeField] private int height = 30, width = 10, header = 8;//ボードの大きさ
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
                    clone.transform.parent=transform;
                }
            }
        }
    }
    //はみ出てないかのチェック
    public bool CheckPosition(Block block)
    {
        foreach(Transform item in block.transform)
        {
            Vector2 pos =new Vector2(Mathf.Round(item.position.x),Mathf.Round(item.position.y));

            if(!BoardOutCheck((int)pos.x,(int)pos.y))
            {
                return false;
            }
        }
        return true;
    }
    //枠内判定
    bool BoardOutCheck(int x,int y)
    {
        return(x>=0&&x<width&&y>=0);
    }
}
