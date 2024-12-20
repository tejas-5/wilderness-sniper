using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class PopUpController : MonoBehaviour
{
    [Header("UI Panel")]
    public GameObject popUpPanel; // ポップアップのパネル

    [Header("Digits Elements")]
    [SerializeField] private TextMeshProUGUI[] rightSideNumbers; // 右側の番号
    [SerializeField] private TextMeshProUGUI[] leftSideDigits; // 左側の数字
    public int correctCodeIndex; // 正しいコードのインデックス

    private int currentDigitIndex = 0; // 現在の数字のインデックス
    private int[] correctCode = new int[3]; // 正しいコード（3桁）
    private Coroutine activeDigitBlinkCoroutine; // 現在の数字の点滅コルーチン
    private Coroutine correctAnswerBlinkCoroutine; // 正しい答えの点滅コルーチン
    private Coroutine randomPopUpCoroutine; // ランダムなポップアップのコルーチン
    private bool[] digitAltered; // 数字が変更されたかどうか

    private bool isPopUpActive = false; // ポップアップがアクティブかどうか

    // Start is called before the first frame update
    void Start()
    {
        if (rightSideNumbers.Length != 4 || leftSideDigits.Length != 3)
        {
            return; // 配列の長さが間違っている場合、処理を終了
        }

        digitAltered = new bool[leftSideDigits.Length]; // 左側の数字の変更フラグを初期化
        for (int i = 0; i < digitAltered.Length; i++)
        {
            digitAltered[i] = false; // 変更されていないと設定
        }

        StartRandomPopUpCoroutine(); // ランダムポップアップを開始
    }

    // Update is called once per frame
    void Update()
    {
        // Aキーを押したとき
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveToPreviousDigit(); // 前の数字に移動
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            MoveToNextDigit(); // 次の数字に移動
        }

        // Wキーを押したとき
        if (Input.GetKeyDown(KeyCode.W))
        {
            IncreaseDigit(currentDigitIndex); // 現在の数字を増やす
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            DecreaseDigit(currentDigitIndex); // 現在の数字を減らす
        }
    }

    // ランダムなポップアップを開始する
    public void StartRandomPopUpCoroutine(bool skipWaitTime = false)
    {
        if (randomPopUpCoroutine != null)
        {
            StopCoroutine(randomPopUpCoroutine); // すでに実行中のポップアップを止める
        }
        randomPopUpCoroutine = StartCoroutine(RandomPopUpCoroutine(skipWaitTime)); // 新しいランダムポップアップを開始
    }

    // ランダムなポップアップを表示するコルーチン
    IEnumerator RandomPopUpCoroutine(bool skipWaitTime)
    {
        while (true) // 無限ループ
        {
            if (isPopUpActive)
            {
                yield return null; // ポップアップがアクティブな場合、何もしない
                continue;
            }

            if (skipWaitTime)
            {
                GenerateRandomNumbers(); // ランダムな番号を生成
                ShowPopUp(); // ポップアップを表示
                yield return new WaitUntil(() => CheckCode()); // コードが正しいか確認
                HidePopUp(); // 正しいコードが入力されたらポップアップを隠す
                yield break; // コルーチンを終了
            }
            else
            {
                float popUpTime = 30f;
                yield return new WaitForSeconds(popUpTime); // 30秒待つ

                GenerateRandomNumbers(); // ランダムな番号を生成
                ShowPopUp(); // ポップアップを表示
                yield return new WaitUntil(() => CheckCode()); // コードが正しいか確認
                HidePopUp(); // 正しいコードが入力されたらポップアップを隠す
                yield break; // コルーチンを終了
            }
        }
    }

    // ランダムな番号を生成
    void GenerateRandomNumbers()
    {
        for (int i = 0; i < rightSideNumbers.Length; i++)
        {
            rightSideNumbers[i].color = Color.white; // 右側の数字の色を白に設定
            int randomNumber = Random.Range(100, 1000); // 100から999までのランダムな数字
            rightSideNumbers[i].text = randomNumber.ToString(); // 数字を表示
        }

        correctCodeIndex = Random.Range(0, rightSideNumbers.Length); // 正しいコードのインデックスをランダムに選択
        int correctNumber = int.Parse(rightSideNumbers[correctCodeIndex].text);
        correctCode[0] = correctNumber / 100; // 百の位
        correctCode[1] = (correctNumber / 10) % 10; // 十の位
        correctCode[2] = correctNumber % 10; // 一の位

        rightSideNumbers[correctCodeIndex].color = Color.red; // 正しい番号の色を赤に設定
        StartBlinkingCorrectAnswer(); // 正しい番号を点滅させる
        currentDigitIndex = 0; // 現在の数字のインデックスをリセット
        StartBlinkingOnCurrentDigit(); // 現在の数字を点滅させる
    }

    // 正しい答えの点滅を開始
    private void StartBlinkingCorrectAnswer()
    {
        if (correctAnswerBlinkCoroutine != null)
        {
            StopCoroutine(correctAnswerBlinkCoroutine); // すでに実行中の点滅を止める
        }

        correctAnswerBlinkCoroutine = StartCoroutine(BlinkCorrectAnswer()); // 正しい答えの点滅を開始
    }

    // 正しい答えの点滅を実行するコルーチン
    IEnumerator BlinkCorrectAnswer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.4f); // 0.4秒待つ
            rightSideNumbers[correctCodeIndex].color = (rightSideNumbers[correctCodeIndex].color == Color.red) ? Color.white : Color.red; // 赤と白を交互に点滅
        }
    }

    // ポップアップを表示
    public void ShowPopUp()
    {
        if (!isPopUpActive)
        {
            isPopUpActive = true; // ポップアップをアクティブに設定
            popUpPanel.SetActive(true); // ポップアップパネルを表示
        }
    }

    // ポップアップを隠す
    public void HidePopUp()
    {
        if (isPopUpActive)
        {
            isPopUpActive = false; // ポップアップを非アクティブに設定
            popUpPanel.SetActive(false); // ポップアップパネルを非表示
        }
    }

    // コードが正しいかチェック
    public bool CheckCode()
    {
        bool codeMatches = true; // コードが一致しているかどうか

        for (int i = 0; i < leftSideDigits.Length; i++)
        {
            int playerInputValue = 0;
            if (int.TryParse(leftSideDigits[i].text, out playerInputValue)) // プレイヤーの入力を整数に変換
            {
                if (playerInputValue != correctCode[i]) // 正しいコードと一致しない場合
                {
                    codeMatches = false;
                    break;
                }
            }
            else
            {
                codeMatches = false;
                break;
            }
        }

        if (codeMatches)
        {
            HidePopUp(); // コードが一致したらポップアップを隠す
            return true; // コードが一致
        }

        return false; // コードが一致しない
    }

    // 現在の数字を増やす
    public void IncreaseDigit(int index)
    {
        string currentText = leftSideDigits[index].text;

        if (int.TryParse(currentText, out int currentValue))
        {
            currentValue = (currentValue + 1) % 10; // 数字を1増やす（0-9の範囲）
            leftSideDigits[index].text = currentValue.ToString(); // 数字を表示
            digitAltered[index] = true; // 数字が変更されたと設定
        }

        CheckCode(); // コードが正しいか確認
        UpdateDigitColors(); // 数字の色を更新
    }

    // 現在の数字を減らす
    public void DecreaseDigit(int index)
    {
        string currentText = leftSideDigits[index].text;

        if (int.TryParse(currentText, out int currentValue))
        {
            currentValue = (currentValue - 1 + 10) % 10; // 数字を1減らす（0-9の範囲）
            leftSideDigits[index].text = currentValue.ToString(); // 数字を表示
            digitAltered[index] = true; // 数字が変更されたと設定
        }

        CheckCode(); // コードが正しいか確認
        UpdateDigitColors(); // 数字の色を更新
    }

    // 次の数字に移動
    public void MoveToNextDigit()
    {
        StopActiveDigitBlink(); // 現在の数字の点滅を停止
        currentDigitIndex = (currentDigitIndex + 1) % leftSideDigits.Length; // 次の数字に移動
        StartBlinkingOnCurrentDigit(); // 新しい数字の点滅を開始
    }

    // 前の数字に移動
    public void MoveToPreviousDigit()
    {
        StopActiveDigitBlink(); // 現在の数字の点滅を停止
        currentDigitIndex = (currentDigitIndex - 1 + leftSideDigits.Length) % leftSideDigits.Length; // 前の数字に移動
        StartBlinkingOnCurrentDigit(); // 新しい数字の点滅を開始
    }

    // 現在の数字の点滅を開始
    private void StartBlinkingOnCurrentDigit()
    {
        if (activeDigitBlinkCoroutine != null)
        {
            StopCoroutine(activeDigitBlinkCoroutine); // すでに実行中の点滅を止める
        }

        activeDigitBlinkCoroutine = StartCoroutine(BlinkActiveDigit()); // 現在の数字の点滅を開始
    }

    // 現在の数字の点滅を停止
    private void StopActiveDigitBlink()
    {
        if (activeDigitBlinkCoroutine != null)
        {
            StopCoroutine(activeDigitBlinkCoroutine); // 点滅を停止
            leftSideDigits[currentDigitIndex].color = Color.white; // 色を白に設定
        }
    }

    // 現在の数字の点滅を実行するコルーチン
    IEnumerator BlinkActiveDigit()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.4f); // 0.4秒待つ
            leftSideDigits[currentDigitIndex].color = (leftSideDigits[currentDigitIndex].color == Color.black) ? Color.white : Color.black; // 黒と白を交互に点滅
        }
    }

    // 数字の色を更新
    private void UpdateDigitColors()
    {
        for (int i = 0; i < leftSideDigits.Length; i++)
        {
            leftSideDigits[i].color = digitAltered[i] ? Color.white : Color.black; // 変更された数字の色を白、変更されていない数字の色を黒に設定
        }
    }
}