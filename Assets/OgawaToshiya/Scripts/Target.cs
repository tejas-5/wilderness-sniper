using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{    
    Vector3 mousePos, worldPos;     // ���W�p�̕ϐ�
    public int maxBullets = 15;     // �e��15 
    private int currentBullets;     // �c���Ă���e

    // Start is called before the first frame update
    void Start()
    {
        currentBullets = maxBullets;
        UpdateClickDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        //�}�E�X���W�̎擾
        mousePos = Input.mousePosition;
        //�X�N���[�����W�����[���h���W�ɕϊ�
        worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
        //���[���h���W�����g�̍��W�ɐݒ�
        transform.position = worldPos;

        // �c�e��������ꍇ�I�u�W�F�N�g��������
        if (Input.GetMouseButtonDown(0) && currentBullets > 0)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            currentBullets--;

            if (hit.collider != null && hit.collider.gameObject.CompareTag("DestroyableObject"))
            {
                Destroy(hit.collider.gameObject);
            }
            UpdateClickDisplay();
        }
    }

    void UpdateClickDisplay()
    {
        if (currentBullets > 0)
        {
            Debug.Log("�c��N���b�N��: " + currentBullets);
        }
        else
        {
            Debug.Log("�N���b�N�񐔂�0�ɂȂ�܂����B����ȏ�N���b�N�ł��܂���B");
        }
    }
}
