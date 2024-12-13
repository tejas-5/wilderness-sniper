using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSysem : MonoBehaviour

{
    // �ړ���̃V�[��(�X�^�[�g���)
    public string StartScene;

    // �X�^�[�g�{�^��������������s����
    public void StartGame()
    {
        SceneManager.LoadScene("Scene_ot");
    }

    // Option�{�^��������������s����
    public void Option()
    {
        SceneManager.LoadScene("OptionScene");
    }

    // �Q�[���I���{�^��������������s����
    public void EndGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
		    Application.Quit();
        #endif
    }

    public void Exit()
    {
        SceneManager.LoadScene("StartScene");

        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    SceneManager.LoadScene("StartScene");
        //}
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
