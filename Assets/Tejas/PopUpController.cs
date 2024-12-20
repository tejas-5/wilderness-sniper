using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class PopUpController : MonoBehaviour
{
    [Header("UI Panel")]
    public GameObject popUpPanel; // ポップアップのパネル

    [Header("Digits Elements")]
    [SerializeField] private TextMeshProUGUI[] rightSideNumbers; // 嘔�箸侶�催
    [SerializeField] private TextMeshProUGUI[] leftSideDigits; // 恣�箸諒�忖
    public int correctCodeIndex; // 屎しいコ�`ドのインデックス

    private int currentDigitIndex = 0; // �F壓の方忖のインデックス
    private int[] correctCode = new int[3]; // 屎しいコ�`ド��3蓐��
    private Coroutine activeDigitBlinkCoroutine; // �F壓の方忖の泣�腑灰覃`チン
    private Coroutine correctAnswerBlinkCoroutine; // 屎しい基えの泣�腑灰覃`チン
    private Coroutine randomPopUpCoroutine; // ランダムなポップアップのコル�`チン
    private bool[] digitAltered; // 方忖が�筝�されたかどうか

    private bool isPopUpActive = false; // ポップアップがアクティブかどうか

    // Start is called before the first frame update
    void Start()
    {
        if (rightSideNumbers.Length != 4 || leftSideDigits.Length != 3)
        {
            return; // 塘双の�Lさが�g�`っている��栽、�I尖を�K阻
        }

        digitAltered = new bool[leftSideDigits.Length]; // 恣�箸諒�忖の�筝�フラグを兜豚晒
        for (int i = 0; i < digitAltered.Length; i++)
        {
            digitAltered[i] = false; // �筝�されていないと�O協
        }

        StartRandomPopUpCoroutine(); // ランダムポップアップを�_兵
    }

    // Update is called once per frame
    void Update()
    {
        // Aキ�`を兀したとき
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveToPreviousDigit(); // 念の方忖に卞��
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            MoveToNextDigit(); // 肝の方忖に卞��
        }

        // Wキ�`を兀したとき
        if (Input.GetKeyDown(KeyCode.W))
        {
            IncreaseDigit(currentDigitIndex); // �F壓の方忖を��やす
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            DecreaseDigit(currentDigitIndex); // �F壓の方忖を�pらす
        }
    }

    // ランダムなポップアップを�_兵する
    public void StartRandomPopUpCoroutine(bool skipWaitTime = false)
    {
        if (randomPopUpCoroutine != null)
        {
            StopCoroutine(randomPopUpCoroutine); // すでに�g佩嶄のポップアップを峭める
        }
        randomPopUpCoroutine = StartCoroutine(RandomPopUpCoroutine(skipWaitTime)); // 仟しいランダムポップアップを�_兵
    }

    // ランダムなポップアップを燕幣するコル�`チン
    IEnumerator RandomPopUpCoroutine(bool skipWaitTime)
    {
        while (true) // �o�泪覃`プ
        {
            if (isPopUpActive)
            {
                yield return null; // ポップアップがアクティブな��栽、採もしない
                continue;
            }

            if (skipWaitTime)
            {
                GenerateRandomNumbers(); // ランダムな桑催を伏撹
                ShowPopUp(); // ポップアップを燕幣
                yield return new WaitUntil(() => CheckCode()); // コ�`ドが屎しいか�_�J
                HidePopUp(); // 屎しいコ�`ドが秘薦されたらポップアップを�Lす
                yield break; // コル�`チンを�K阻
            }
            else
            {
                float popUpTime = 30f;
                yield return new WaitForSeconds(popUpTime); // 30昼棋つ

                GenerateRandomNumbers(); // ランダムな桑催を伏撹
                ShowPopUp(); // ポップアップを燕幣
                yield return new WaitUntil(() => CheckCode()); // コ�`ドが屎しいか�_�J
                HidePopUp(); // 屎しいコ�`ドが秘薦されたらポップアップを�Lす
                yield break; // コル�`チンを�K阻
            }
        }
    }

    // ランダムな桑催を伏撹
    void GenerateRandomNumbers()
    {
        for (int i = 0; i < rightSideNumbers.Length; i++)
        {
            rightSideNumbers[i].color = Color.white; // 嘔�箸諒�忖の弼を易に�O協
            int randomNumber = Random.Range(100, 1000); // 100から999までのランダムな方忖
            rightSideNumbers[i].text = randomNumber.ToString(); // 方忖を燕幣
        }

        correctCodeIndex = Random.Range(0, rightSideNumbers.Length); // 屎しいコ�`ドのインデックスをランダムに�x�k
        int correctNumber = int.Parse(rightSideNumbers[correctCodeIndex].text);
        correctCode[0] = correctNumber / 100; // 為の了
        correctCode[1] = (correctNumber / 10) % 10; // 噴の了
        correctCode[2] = correctNumber % 10; // 匯の了

        rightSideNumbers[correctCodeIndex].color = Color.red; // 屎しい桑催の弼を橿に�O協
        StartBlinkingCorrectAnswer(); // 屎しい桑催を泣�腓気擦�
        currentDigitIndex = 0; // �F壓の方忖のインデックスをリセット
        StartBlinkingOnCurrentDigit(); // �F壓の方忖を泣�腓気擦�
    }

    // 屎しい基えの泣�腓鱸_兵
    private void StartBlinkingCorrectAnswer()
    {
        if (correctAnswerBlinkCoroutine != null)
        {
            StopCoroutine(correctAnswerBlinkCoroutine); // すでに�g佩嶄の泣�腓鰆垢瓩�
        }

        correctAnswerBlinkCoroutine = StartCoroutine(BlinkCorrectAnswer()); // 屎しい基えの泣�腓鱸_兵
    }

    // 屎しい基えの泣�腓��g佩するコル�`チン
    IEnumerator BlinkCorrectAnswer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.4f); // 0.4昼棋つ
            rightSideNumbers[correctCodeIndex].color = (rightSideNumbers[correctCodeIndex].color == Color.red) ? Color.white : Color.red; // 橿と易を住札に泣��
        }
    }

    // ポップアップを燕幣
    public void ShowPopUp()
    {
        if (!isPopUpActive)
        {
            isPopUpActive = true; // ポップアップをアクティブに�O協
            popUpPanel.SetActive(true); // ポップアップパネルを燕幣
        }
    }

    // ポップアップを�Lす
    public void HidePopUp()
    {
        if (isPopUpActive)
        {
            isPopUpActive = false; // ポップアップを掲アクティブに�O協
            popUpPanel.SetActive(false); // ポップアップパネルを掲燕幣
        }
    }

    // コ�`ドが屎しいかチェック
    public bool CheckCode()
    {
        bool codeMatches = true; // コ�`ドが匯崑しているかどうか

        for (int i = 0; i < leftSideDigits.Length; i++)
        {
            int playerInputValue = 0;
            if (int.TryParse(leftSideDigits[i].text, out playerInputValue)) // プレイヤ�`の秘薦を屁方に���Q
            {
                if (playerInputValue != correctCode[i]) // 屎しいコ�`ドと匯崑しない��栽
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
            HidePopUp(); // コ�`ドが匯崑したらポップアップを�Lす
            return true; // コ�`ドが匯崑
        }

        return false; // コ�`ドが匯崑しない
    }

    // �F壓の方忖を��やす
    public void IncreaseDigit(int index)
    {
        string currentText = leftSideDigits[index].text;

        if (int.TryParse(currentText, out int currentValue))
        {
            currentValue = (currentValue + 1) % 10; // 方忖を1��やす��0-9の���譯�
            leftSideDigits[index].text = currentValue.ToString(); // 方忖を燕幣
            digitAltered[index] = true; // 方忖が�筝�されたと�O協
        }

        CheckCode(); // コ�`ドが屎しいか�_�J
        UpdateDigitColors(); // 方忖の弼を厚仟
    }

    // �F壓の方忖を�pらす
    public void DecreaseDigit(int index)
    {
        string currentText = leftSideDigits[index].text;

        if (int.TryParse(currentText, out int currentValue))
        {
            currentValue = (currentValue - 1 + 10) % 10; // 方忖を1�pらす��0-9の���譯�
            leftSideDigits[index].text = currentValue.ToString(); // 方忖を燕幣
            digitAltered[index] = true; // 方忖が�筝�されたと�O協
        }

        CheckCode(); // コ�`ドが屎しいか�_�J
        UpdateDigitColors(); // 方忖の弼を厚仟
    }

    // 肝の方忖に卞��
    public void MoveToNextDigit()
    {
        StopActiveDigitBlink(); // �F壓の方忖の泣�腓鰺Ｖ�
        currentDigitIndex = (currentDigitIndex + 1) % leftSideDigits.Length; // 肝の方忖に卞��
        StartBlinkingOnCurrentDigit(); // 仟しい方忖の泣�腓鱸_兵
    }

    // 念の方忖に卞��
    public void MoveToPreviousDigit()
    {
        StopActiveDigitBlink(); // �F壓の方忖の泣�腓鰺Ｖ�
        currentDigitIndex = (currentDigitIndex - 1 + leftSideDigits.Length) % leftSideDigits.Length; // 念の方忖に卞��
        StartBlinkingOnCurrentDigit(); // 仟しい方忖の泣�腓鱸_兵
    }

    // �F壓の方忖の泣�腓鱸_兵
    private void StartBlinkingOnCurrentDigit()
    {
        if (activeDigitBlinkCoroutine != null)
        {
            StopCoroutine(activeDigitBlinkCoroutine); // すでに�g佩嶄の泣�腓鰆垢瓩�
        }

        activeDigitBlinkCoroutine = StartCoroutine(BlinkActiveDigit()); // �F壓の方忖の泣�腓鱸_兵
    }

    // �F壓の方忖の泣�腓鰺Ｖ�
    private void StopActiveDigitBlink()
    {
        if (activeDigitBlinkCoroutine != null)
        {
            StopCoroutine(activeDigitBlinkCoroutine); // 泣�腓鰺Ｖ�
            leftSideDigits[currentDigitIndex].color = Color.white; // 弼を易に�O協
        }
    }

    // �F壓の方忖の泣�腓��g佩するコル�`チン
    IEnumerator BlinkActiveDigit()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.4f); // 0.4昼棋つ
            leftSideDigits[currentDigitIndex].color = (leftSideDigits[currentDigitIndex].color == Color.black) ? Color.white : Color.black; // �\と易を住札に泣��
        }
    }

    // 方忖の弼を厚仟
    private void UpdateDigitColors()
    {
        for (int i = 0; i < leftSideDigits.Length; i++)
        {
            leftSideDigits[i].color = digitAltered[i] ? Color.white : Color.black; // �筝�された方忖の弼を易、�筝�されていない方忖の弼を�\に�O協
        }
    }
}