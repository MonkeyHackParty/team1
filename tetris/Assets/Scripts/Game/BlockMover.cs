using System.Collections;
using System;
using UnityEngine;

public class BlockMover : MonoBehaviour
{
    public IEnumerator MoveToPosition(Vector3 targetPosition, float duration)
    {
        Debug.Log("MoveToPosition");
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            Debug.Log("MoveToPosition while");
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        Debug.Log("MoveToPosition end");
        Destroy(gameObject); // 移動完了後にオブジェクトを削除
    }
}