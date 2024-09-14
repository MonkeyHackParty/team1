using UnityEngine;

public class BlockPeace : MonoBehaviour
{
    private int number;  // 数値を保持
    [SerializeField] private SpriteRenderer spriteRenderer;  // スプライトレンダラー

    void Start()
    {
        // ここでスプライト名から数値を設定する処理などが入る
        if (spriteRenderer != null && spriteRenderer.sprite != null)
        {
            string spriteName = spriteRenderer.sprite.name;
            number = int.Parse(spriteName);  // スプライト名から数値を取得
        }
    }

    // 数値をセットし、スプライトを変更するメソッド
    public void SetNumber(int newNumber)
    {
        number = newNumber;  // 数値を更新

        // スプライトをロードして設定する場合
        string newSpriteName = number.ToString();  // 数字の名前がスプライト名であると仮定

        // スプライトを取得して更新（Resourcesフォルダ内のスプライトを使用）
        Sprite newSprite = Resources.Load<Sprite>(newSpriteName);
        if (newSprite != null)
        {
            spriteRenderer.sprite = newSprite;  // スプライトを更新
            Debug.Log($"スプライトを {newSpriteName} に更新しました");
        }
        else
        {
            // スプライトが見つからなかった場合のエラーログ
            Debug.LogError($"指定されたスプライトが見つかりません: {newSpriteName}");
        }
    }

    // 現在の数値を取得するメソッド
    public int GetNumber()
    {
        return number;
    }
}
