using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopUpController : MonoBehaviour
{
    public GameObject popUpPanel; // パネル（ポップアップ）の参照
    public Text[] rightSideNumbers; // ランダムな数字（右側）の配列
    public Text[] leftSideDigits;   // 表示される数字（左側）の配列
    public int correctCodeIndex;    // 正しい番号のインデックス

    private int currentDigitIndex = 0; // 現在編集中の桁
    private int[] correctCode = new int[3]; // プレイヤーが入力すべき正しいコード（3桁）
    private Coroutine activeDigitBlinkCoroutine; // アクティブな桁の点滅を管理するコルーチン
    private Coroutine correctAnswerBlinkCoroutine; // 正しい答えの点滅を管理するコルーチン

    void Start()
    {
        if (rightSideNumbers.Length != 4 || leftSideDigits.Length != 3)
        {
            Debug.LogError("配列は右側の数字が4つ、左側の数字が3つである必要があります。");
            return;
        }

        GenerateRandomNumbers(); // ランダムな数字を生成
        ShowPopUp(); // ポップアップを表示
        StartBlinkingOnCurrentDigit(); // 最初のアクティブ桁の点滅を開始
    }

    void Update()
    {
        // 入力されたキーで桁を変更する
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveToPreviousDigit(); // 前の桁に移動
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            MoveToNextDigit(); // 次の桁に移動
        }

        // 入力されたキーで桁の数字を増減させる
        if (Input.GetKeyDown(KeyCode.W))
        {
            IncreaseDigit(currentDigitIndex); // 現在の桁の数字を増やす
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            DecreaseDigit(currentDigitIndex); // 現在の桁の数字を減らす
        }
    }

    void GenerateRandomNumbers()
    {
        for (int i = 0; i < rightSideNumbers.Length; i++)
        {
            int randomNumber = Random.Range(100, 1000); // 100から999の間でランダムな数字を生成
            rightSideNumbers[i].text = randomNumber.ToString(); // テキストに設定
        }

        correctCodeIndex = Random.Range(0, rightSideNumbers.Length); // 正しいコードのインデックスをランダムに決定
        int correctNumber = int.Parse(rightSideNumbers[correctCodeIndex].text); // 正しい番号を取得
        correctCode[0] = correctNumber / 100; // 正しいコードの百の位
        correctCode[1] = (correctNumber / 10) % 10; // 正しいコードの十の位
        correctCode[2] = correctNumber % 10; // 正しいコードの一の位

        StartCoroutine(BlinkCorrectAnswer()); // 正しい答えを点滅させるコルーチンを開始
    }

    IEnumerator BlinkCorrectAnswer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.4f); // 0.4秒ごとに
            rightSideNumbers[correctCodeIndex].color = (rightSideNumbers[correctCodeIndex].color == Color.red) ? Color.white : Color.red; // 正しい番号の色を赤と白で点滅させる
        }
    }

    void ShowPopUp()
    {
        popUpPanel.SetActive(true); // ポップアップを表示
    }

    public void HidePopUp()
    {
        popUpPanel.SetActive(false); // ポップアップを非表示にする
    }

    // ボタンクリックで数字を増やす
    public void IncreaseDigit(int index)
    {
        string currentText = leftSideDigits[index].text; // 現在の桁のテキストを取得

        if (int.TryParse(currentText, out int currentValue))
        {
            // 増加させて、9を超えたら0に戻る
            currentValue = (currentValue + 1) % 10;
            leftSideDigits[index].text = currentValue.ToString(); // 新しい値をテキストに設定
        }
        else
        {
            Debug.LogWarning("無効な値です: " + index); // 数字以外の値が入っている場合
        }

        CheckCode(); // コードをチェック
    }

    // ボタンクリックで数字を減らす
    public void DecreaseDigit(int index)
    {
        string currentText = leftSideDigits[index].text; // 現在の桁のテキストを取得

        if (int.TryParse(currentText, out int currentValue))
        {
            // 減少させて、0未満になったら9に戻る
            currentValue = (currentValue - 1 + 10) % 10;
            leftSideDigits[index].text = currentValue.ToString(); // 新しい値をテキストに設定
        }
        else
        {
            Debug.LogWarning("無効な値です: " + index); // 数字以外の値が入っている場合
        }

        CheckCode(); // コードをチェック
    }

    public void MoveToNextDigit()
    {
        StopActiveDigitBlink(); // 前のアクティブ桁の点滅を停止

        currentDigitIndex = (currentDigitIndex + 1) % leftSideDigits.Length; // 次の桁に移動

        StartBlinkingOnCurrentDigit(); // 新しい桁の点滅を開始
    }

    public void MoveToPreviousDigit()
    {
        StopActiveDigitBlink(); // 前のアクティブ桁の点滅を停止

        currentDigitIndex = (currentDigitIndex - 1 + leftSideDigits.Length) % leftSideDigits.Length; // 前の桁に移動

        StartBlinkingOnCurrentDigit(); // 新しい桁の点滅を開始
    }

    // 現在選択されている桁の点滅を開始
    private void StartBlinkingOnCurrentDigit()
    {
        // もし点滅コルーチンがあれば停止
        if (activeDigitBlinkCoroutine != null)
        {
            StopCoroutine(activeDigitBlinkCoroutine);
        }

        // 現在選択されている桁の点滅コルーチンを開始
        activeDigitBlinkCoroutine = StartCoroutine(BlinkActiveDigit());
    }

    // 現在選択されている桁の点滅を停止
    private void StopActiveDigitBlink()
    {
        if (activeDigitBlinkCoroutine != null)
        {
            StopCoroutine(activeDigitBlinkCoroutine); // 点滅コルーチンを停止
            leftSideDigits[currentDigitIndex].color = Color.white; // 色を白に戻す
        }
    }

    // 現在選択されている桁の点滅を行うコルーチン
    IEnumerator BlinkActiveDigit()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.4f); // 0.4秒ごとに点滅
            // アクティブ桁の色を黒と白で交互に点滅させる
            leftSideDigits[currentDigitIndex].color = (leftSideDigits[currentDigitIndex].color == Color.black) ? Color.white : Color.black;
        }
    }

    // 入力されたコードをチェック
    public void CheckCode()
    {
        bool codeMatches = true; // コードが一致するかどうかのフラグ

        string enteredCode = "";
        for (int i = 0; i < leftSideDigits.Length; i++)
        {
            enteredCode += leftSideDigits[i].text + " "; // 入力されたコードを文字列としてまとめる
        }
        Debug.Log("入力されたコード: " + enteredCode); // デバッグ用に表示

        // 入力されたコードが正しいかどうかをチェック
        for (int i = 0; i < leftSideDigits.Length; i++)
        {
            int playerInputValue = 0;
            if (int.TryParse(leftSideDigits[i].text, out playerInputValue)) // 数字が正しく入力されているかチェック
            {
                Debug.Log($"比較: プレイヤー入力: {playerInputValue}, 正しいコード: {correctCode[i]}");

                if (playerInputValue != correctCode[i]) // 正しいコードと一致しない場合
                {
                    codeMatches = false;
                    break;
                }
            }
            else
            {
                Debug.LogWarning("無効な入力です: " + i); // 数字以外が入力された場合
                codeMatches = false;
                break;
            }
        }

        // コードが一致した場合、ポップアップを閉じる
        if (codeMatches)
        {
            Debug.Log("コードが一致しました！ポップアップを閉じます...");
            HidePopUp();
        }
        else
        {
            Debug.Log("コードが一致しません。もう一度試してください。");
        }
    }

    void ResetInput()
    {
        for (int i = 0; i < leftSideDigits.Length; i++)
        {
            leftSideDigits[i].text = "0"; // すべての桁を0にリセット
        }
    }
}
