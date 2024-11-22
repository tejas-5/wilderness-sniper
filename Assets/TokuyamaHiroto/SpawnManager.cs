using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject objectToMove;
    public Transform spawnPoint;
    public float spawnDelay = 3.0f;

    void Start()
    {
        StartCoroutine(SpawnAfterDelay());
    }

    IEnumerator SpawnAfterDelay()
    {
        // 遅延時間を待機
        yield return new WaitForSeconds(spawnDelay);

        // オブジェクトをスポーン地点に移動
        if (objectToMove != null && spawnPoint != null)
        {
            objectToMove.transform.position = spawnPoint.position;
            objectToMove.transform.rotation = spawnPoint.rotation;
            objectToMove.SetActive(true);
        }
        else
        {
            Debug.LogWarning("objectToMoveまたはspawnPointが設定されていません。");
        }
    }
}
