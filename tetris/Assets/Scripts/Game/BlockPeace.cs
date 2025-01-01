using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlockPeace : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI NumberText;
    public int Number { get; private set; }
    public void SetNumber(int number)
    {
        Number = number;
        if (NumberText != null)
        {
            NumberText.text = Number.ToString();
        }
    }
}
