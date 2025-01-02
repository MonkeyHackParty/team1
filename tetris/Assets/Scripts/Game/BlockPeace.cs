using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlockPeace : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI NumberText;
    public int Number { get; private set; }
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetNumber(int number)
    {
        Number = number;
        if (NumberText != null)
        {
            NumberText.text = Number.ToString();
        }
        UpdateColor();
    }

    private void UpdateColor()
    {
        if (spriteRenderer != null)
        {
            switch (Number)
            {
                case 2:
                    spriteRenderer.color = new Color(1f,150/255f,0f);
                    return;
                case 4:
                    spriteRenderer.color = new Color(0f, 0f, 1f);
                    return;
                case 8:
                    spriteRenderer.color = new Color(1f, 0f, 0f);
                    return;
                case 16:
                    spriteRenderer.color = new Color(1f, 0f, 1f);
                    return;
                // 必要に応じて他のケースを追加
                default:
                    spriteRenderer.color = Color.gray;
                    return;
            }
        }
    }
}
