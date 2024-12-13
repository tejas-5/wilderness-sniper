using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSysem : MonoBehaviour

{
    // 移動先のシーン(スタート画面)
    public string StartScene;

    // スタートボタンを押したら実行する
    public void StartGame()
    {
        SceneManager.LoadScene("Scene_ot");
    }

    // Optionボタンを押したら実行する
    public void Option()
    {
        SceneManager.LoadScene("OptionScene");
    }

    // ゲーム終了ボタンを押したら実行する
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
