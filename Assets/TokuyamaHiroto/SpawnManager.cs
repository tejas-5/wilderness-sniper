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
        // �x�����Ԃ�ҋ@
        yield return new WaitForSeconds(spawnDelay);

        // �I�u�W�F�N�g���X�|�[���n�_�Ɉړ�
        if (objectToMove != null && spawnPoint != null)
        {
            objectToMove.transform.position = spawnPoint.position;
            objectToMove.transform.rotation = spawnPoint.rotation;
            objectToMove.SetActive(true);
        }
        else
        {
            Debug.LogWarning("objectToMove�܂���spawnPoint���ݒ肳��Ă��܂���B");
        }
    }
}
