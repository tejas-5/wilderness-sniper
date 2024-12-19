using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class PopUpController : MonoBehaviour
{

    [Header(" UI Panel ")]
    public GameObject popUpPanel; // ポップアップのパネル

    [Header(" Digits Elements ")]
    [SerializeField] private TextMeshProUGUI[] rightSideNumbers; // 右側に表示する数字の配列
    [SerializeField] private TextMeshProUGUI[] leftSideDigits; // 左側に表示する数字の配列
    public int correctCodeIndex; // 正しいコードのインデックス

    private int currentDigitIndex = 0; // 現在選択されている桁
    private int[] correctCode = new int[3]; // 正しいコード（3桁）
    private Coroutine activeDigitBlinkCoroutine; // アクティブ桁の点滅コルーチン
    private Coroutine correctAnswerBlinkCoroutine; // 正しい答えの点滅コルーチン
    private Coroutine randomPopUpCoroutine; // ランダムなタイミングでポップアップを表示するコルーチン
    private bool[] digitAltered; // 入力された桁が変更されたかどうか

    private bool isPopUpActive = false; // ポップアップがアクティブかどうか

    // Start is called before the first frame update
    void Start()
    {
        if (rightSideNumbers.Length != 4 || leftSideDigits.Length != 3)
        {
            return; // 配列のサイズが正しくない場合、処理を終了
        }

        digitAltered = new bool[leftSideDigits.Length]; // 変更された桁の状態を管理
        for (int i = 0; i < digitAltered.Length; i++)
        {
            digitAltered[i] = false; // 最初は変更なし
        }

        StartRandomPopUpCoroutine(); // ランダムタイミングでポップアップを表示するコルーチンを開始
    }

    // Update is called once per frame
    void Update()
    {
        // 入力されたキーで桁を変更する
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveToPreviousDigit(); // 前の桁へ移動
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            MoveToNextDigit(); // 次の桁へ移動
        }

        // 入力されたキーで桁の数字を増減させる
        if (Input.GetKeyDown(KeyCode.W))
        {
            IncreaseDigit(currentDigitIndex); // 現在の桁を増やす
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            DecreaseDigit(currentDigitIndex); // 現在の桁を減らす
        }
    }

    // ランダムなタイミングでポップアップを表示するコルーチンを開始
    public void StartRandomPopUpCoroutine(bool skipWaitTime = false)
    {
        if (randomPopUpCoroutine != null)
        {
            StopCoroutine(randomPopUpCoroutine); // 既存のコルーチンを停止
        }
        randomPopUpCoroutine = StartCoroutine(RandomPopUpCoroutine(skipWaitTime)); // 新しいコルーチンを開始
    }

    // ランダムなタイミングでポップアップを表示するコルーチン
    IEnumerator RandomPopUpCoroutine(bool skipWaitTime)
    {
        while (true) // ゲームが進行している間、ポップアップを表示し続ける
        {
            if (isPopUpActive)
            {
                yield return null; // ポップアップが表示中なら待機
                continue;
            }

            if (skipWaitTime)
            {
                GenerateRandomNumbers(); // ランダムな数字を生成
                ShowPopUp(); // ポップアップを表示
                yield return new WaitUntil(() => CheckCode()); // 正しいコードが入力されるまで待機
                HidePopUp(); // Hide the popup after correct input
                yield break; // Exit the coroutine once the code is entered correctly
            }
            else
            {
                float popUpTime = 30f;
                yield return new WaitForSeconds(popUpTime); // 30秒待機

                GenerateRandomNumbers(); // ランダムな数字を生成
                ShowPopUp(); // ポップアップを表示
                yield return new WaitUntil(() => CheckCode()); // 正しいコードが入力されるまで待機
                HidePopUp(); // Hide the popup after correct input
                yield break; // Exit the coroutine once the code is entered correctly
            }
        }
    }

    // ランダムな数字を生成し、ポップアップを表示する準備をする
    void GenerateRandomNumbers()
    {
        for (int i = 0; i < rightSideNumbers.Length; i++)
        {
            rightSideNumbers[i].color = Color.white; // 数字の色をリセット
            int randomNumber = Random.Range(100, 1000); // 100から999の間でランダムな数字を生成
            rightSideNumbers[i].text = randomNumber.ToString(); // テキストに設定
        }

        correctCodeIndex = Random.Range(0, rightSideNumbers.Length); // 正しいコードをランダムに選択
        int correctNumber = int.Parse(rightSideNumbers[correctCodeIndex].text);
        correctCode[0] = correctNumber / 100; // 百の位
        correctCode[1] = (correctNumber / 10) % 10; // 十の位
        correctCode[2] = correctNumber % 10; // 一の位

        rightSideNumbers[correctCodeIndex].color = Color.red; // 正しい数字を赤色に設定
        StartBlinkingCorrectAnswer(); // 正しい答えを点滅させる
        currentDigitIndex = 0; // 最初の桁を選択
        StartBlinkingOnCurrentDigit(); // 現在の桁を点滅させる
    }

    // 正しい答えを点滅させる
    private void StartBlinkingCorrectAnswer()
    {
        if (correctAnswerBlinkCoroutine != null)
        {
            StopCoroutine(correctAnswerBlinkCoroutine); // 既存の点滅コルーチンを停止
        }

        correctAnswerBlinkCoroutine = StartCoroutine(BlinkCorrectAnswer()); // 新しい点滅コルーチンを開始
    }

    // 正しい答えが点滅するコルーチン
    IEnumerator BlinkCorrectAnswer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.4f); // 0.4秒ごとに点滅
            rightSideNumbers[correctCodeIndex].color = (rightSideNumbers[correctCodeIndex].color == Color.red) ? Color.white : Color.red; // 赤と白を切り替え
        }
    }

    // ポップアップを表示する
    public void ShowPopUp()
    {
        if (!isPopUpActive)
        {
            isPopUpActive = true; // ポップアップを表示状態にする
            popUpPanel.SetActive(true); // パネルを表示
        }
    }

    // ポップアップを非表示にする
    public void HidePopUp()
    {
        if (isPopUpActive)
        {
            isPopUpActive = false; // ポップアップを非表示状態にする
            popUpPanel.SetActive(false); // パネルを非表示
        }
    }

    // 入力されたコードが正しいかチェックする
    public bool CheckCode()
    {
        bool codeMatches = true; // コードが一致するかどうか

        for (int i = 0; i < leftSideDigits.Length; i++)
        {
            int playerInputValue = 0;
            if (int.TryParse(leftSideDigits[i].text, out playerInputValue)) // 入力された値を整数に変換
            {
                if (playerInputValue != correctCode[i]) // 正しいコードと比較
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
            HidePopUp(); // コードが一致したらポップアップを非表示にする
            return true; // 正しいコード
        }

        return false; // コードが一致しない
    }

    // 現在の桁の数字を増やす
    public void IncreaseDigit(int index)
    {
        string currentText = leftSideDigits[index].text;

        if (int.TryParse(currentText, out int currentValue))
        {
            currentValue = (currentValue + 1) % 10; // 1増やす（10になったら0に戻る）
            leftSideDigits[index].text = currentValue.ToString(); // テキストを更新
            digitAltered[index] = true; // 変更された桁としてマーク
        }

        CheckCode(); // コードが一致するか確認
        UpdateDigitColors(); // 桁の色を更新
    }

    // 現在の桁の数字を減らす
    public void DecreaseDigit(int index)
    {
        string currentText = leftSideDigits[index].text;

        if (int.TryParse(currentText, out int currentValue))
        {
            currentValue = (currentValue - 1 + 10) % 10; // 1減らす（0になったら9に戻る）
            leftSideDigits[index].text = currentValue.ToString(); // テキストを更新
            digitAltered[index] = true; // 変更された桁としてマーク
        }

        CheckCode(); // コードが一致するか確認
        UpdateDigitColors(); // 桁の色を更新
    }

    // 次の桁に移動する
    public void MoveToNextDigit()
    {
        StopActiveDigitBlink(); // 現在の桁の点滅を止める
        currentDigitIndex = (currentDigitIndex + 1) % leftSideDigits.Length; // 次の桁に移動
        StartBlinkingOnCurrentDigit(); // 新しい桁を点滅させる
    }

    // 前の桁に移動する
    public void MoveToPreviousDigit()
    {
        StopActiveDigitBlink(); // 現在の桁の点滅を止める
        currentDigitIndex = (currentDigitIndex - 1 + leftSideDigits.Length) % leftSideDigits.Length; // 前の桁に移動
        StartBlinkingOnCurrentDigit(); // 新しい桁を点滅させる
    }

    // 現在の桁を点滅させる
    private void StartBlinkingOnCurrentDigit()
    {
        if (activeDigitBlinkCoroutine != null)
        {
            StopCoroutine(activeDigitBlinkCoroutine); // 既存の点滅コルーチンを停止
        }

        activeDigitBlinkCoroutine = StartCoroutine(BlinkActiveDigit()); // 新しい点滅コルーチンを開始
    }

    // 現在の桁を点滅させるコルーチン
    private void StopActiveDigitBlink()
    {
        if (activeDigitBlinkCoroutine != null)
        {
            StopCoroutine(activeDigitBlinkCoroutine); // 点滅コルーチンを停止
            leftSideDigits[currentDigitIndex].color = Color.white; // 色を元に戻す
        }
    }

    // 現在の桁を点滅させるコルーチン
    IEnumerator BlinkActiveDigit()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.4f); // 0.4秒ごとに点滅
            leftSideDigits[currentDigitIndex].color = (leftSideDigits[currentDigitIndex].color == Color.black) ? Color.white : Color.black; // 黒と白を切り替え
        }
    }

    // 桁の色を更新する
    private void UpdateDigitColors()
    {
        for (int i = 0; i < leftSideDigits.Length; i++)
        {
            leftSideDigits[i].color = digitAltered[i] ? Color.white : Color.black; // 変更された桁は白、変更されていない桁は黒
        }
    }
}