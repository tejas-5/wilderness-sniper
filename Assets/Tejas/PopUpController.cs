using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class PopUpController : MonoBehaviour
{
    [Header("UI Panel")]
    public GameObject popUpPanel; // �ݥåץ��åפΥѥͥ�

    [Header("Digits Elements")]
    [SerializeField] private TextMeshProUGUI[] rightSideNumbers; // �҂Ȥη���
    [SerializeField] private TextMeshProUGUI[] leftSideDigits; // ��Ȥ�����
    public int correctCodeIndex; // ���������`�ɤΥ���ǥå���

    private int currentDigitIndex = 0; // �F�ڤ����֤Υ���ǥå���
    private int[] correctCode = new int[3]; // ���������`�ɣ�3�죩
    private Coroutine activeDigitBlinkCoroutine; // �F�ڤ����֤ε�祳��`����
    private Coroutine correctAnswerBlinkCoroutine; // �������𤨤ε�祳��`����
    private Coroutine randomPopUpCoroutine; // ������ʥݥåץ��åפΥ���`����
    private bool[] digitAltered; // ���֤�������줿���ɤ���

    private bool isPopUpActive = false; // �ݥåץ��åפ������ƥ��֤��ɤ���

    // Start is called before the first frame update
    void Start()
    {
        if (rightSideNumbers.Length != 4 || leftSideDigits.Length != 3)
        {
            return; // ���Ф��L�����g�`�äƤ�����ϡ��I���K��
        }

        digitAltered = new bool[leftSideDigits.Length]; // ��Ȥ����֤Ή���ե饰����ڻ�
        for (int i = 0; i < digitAltered.Length; i++)
        {
            digitAltered[i] = false; // �������Ƥ��ʤ����O��
        }

        StartRandomPopUpCoroutine(); // ������ݥåץ��åפ��_ʼ
    }

    // Update is called once per frame
    void Update()
    {
        // A���`��Ѻ�����Ȥ�
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveToPreviousDigit(); // ǰ�����֤��Ƅ�
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            MoveToNextDigit(); // �Τ����֤��Ƅ�
        }

        // W���`��Ѻ�����Ȥ�
        if (Input.GetKeyDown(KeyCode.W))
        {
            IncreaseDigit(currentDigitIndex); // �F�ڤ����֤򉈤䤹
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            DecreaseDigit(currentDigitIndex); // �F�ڤ����֤�p�餹
        }
    }

    // ������ʥݥåץ��åפ��_ʼ����
    public void StartRandomPopUpCoroutine(bool skipWaitTime = false)
    {
        if (randomPopUpCoroutine != null)
        {
            StopCoroutine(randomPopUpCoroutine); // ���Ǥˌg���ФΥݥåץ��åפ�ֹ���
        }
        randomPopUpCoroutine = StartCoroutine(RandomPopUpCoroutine(skipWaitTime)); // �¤���������ݥåץ��åפ��_ʼ
    }

    // ������ʥݥåץ��åפ��ʾ���륳��`����
    IEnumerator RandomPopUpCoroutine(bool skipWaitTime)
    {
        while (true) // �o�ޥ�`��
        {
            if (isPopUpActive)
            {
                yield return null; // �ݥåץ��åפ������ƥ��֤ʈ��ϡ��Τ⤷�ʤ�
                continue;
            }

            if (skipWaitTime)
            {
                GenerateRandomNumbers(); // ������ʷ��Ť�����
                ShowPopUp(); // �ݥåץ��åפ��ʾ
                yield return new WaitUntil(() => CheckCode()); // ���`�ɤ����������_�J
                HidePopUp(); // ���������`�ɤ��������줿��ݥåץ��åפ��L��
                yield break; // ����`�����K��
            }
            else
            {
                float popUpTime = 30f;
                yield return new WaitForSeconds(popUpTime); // 30�����

                GenerateRandomNumbers(); // ������ʷ��Ť�����
                ShowPopUp(); // �ݥåץ��åפ��ʾ
                yield return new WaitUntil(() => CheckCode()); // ���`�ɤ����������_�J
                HidePopUp(); // ���������`�ɤ��������줿��ݥåץ��åפ��L��
                yield break; // ����`�����K��
            }
        }
    }

    // ������ʷ��Ť�����
    void GenerateRandomNumbers()
    {
        for (int i = 0; i < rightSideNumbers.Length; i++)
        {
            rightSideNumbers[i].color = Color.white; // �҂Ȥ����֤�ɫ��פ��O��
            int randomNumber = Random.Range(100, 1000); // 100����999�ޤǤΥ����������
            rightSideNumbers[i].text = randomNumber.ToString(); // ���֤��ʾ
        }

        correctCodeIndex = Random.Range(0, rightSideNumbers.Length); // ���������`�ɤΥ���ǥå������������x�k
        int correctNumber = int.Parse(rightSideNumbers[correctCodeIndex].text);
        correctCode[0] = correctNumber / 100; // �٤�λ
        correctCode[1] = (correctNumber / 10) % 10; // ʮ��λ
        correctCode[2] = correctNumber % 10; // һ��λ

        rightSideNumbers[correctCodeIndex].color = Color.red; // ���������Ť�ɫ�����O��
        StartBlinkingCorrectAnswer(); // ���������Ť��礵����
        currentDigitIndex = 0; // �F�ڤ����֤Υ���ǥå�����ꥻ�å�
        StartBlinkingOnCurrentDigit(); // �F�ڤ����֤��礵����
    }

    // �������𤨤ε����_ʼ
    private void StartBlinkingCorrectAnswer()
    {
        if (correctAnswerBlinkCoroutine != null)
        {
            StopCoroutine(correctAnswerBlinkCoroutine); // ���Ǥˌg���Фε���ֹ���
        }

        correctAnswerBlinkCoroutine = StartCoroutine(BlinkCorrectAnswer()); // �������𤨤ε����_ʼ
    }

    // �������𤨤ε���g�Ф��륳��`����
    IEnumerator BlinkCorrectAnswer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.4f); // 0.4�����
            rightSideNumbers[correctCodeIndex].color = (rightSideNumbers[correctCodeIndex].color == Color.red) ? Color.white : Color.red; // ��Ȱפ򽻻��˵��
        }
    }

    // �ݥåץ��åפ��ʾ
    public void ShowPopUp()
    {
        if (!isPopUpActive)
        {
            isPopUpActive = true; // �ݥåץ��åפ򥢥��ƥ��֤��O��
            popUpPanel.SetActive(true); // �ݥåץ��åץѥͥ���ʾ
        }
    }

    // �ݥåץ��åפ��L��
    public void HidePopUp()
    {
        if (isPopUpActive)
        {
            isPopUpActive = false; // �ݥåץ��åפ�ǥ����ƥ��֤��O��
            popUpPanel.SetActive(false); // �ݥåץ��åץѥͥ��Ǳ�ʾ
        }
    }

    // ���`�ɤ��������������å�
    public bool CheckCode()
    {
        bool codeMatches = true; // ���`�ɤ�һ�¤��Ƥ��뤫�ɤ���

        for (int i = 0; i < leftSideDigits.Length; i++)
        {
            int playerInputValue = 0;
            if (int.TryParse(leftSideDigits[i].text, out playerInputValue)) // �ץ쥤��`�������������ˉ�Q
            {
                if (playerInputValue != correctCode[i]) // ���������`�ɤ�һ�¤��ʤ�����
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
            HidePopUp(); // ���`�ɤ�һ�¤�����ݥåץ��åפ��L��
            return true; // ���`�ɤ�һ��
        }

        return false; // ���`�ɤ�һ�¤��ʤ�
    }

    // �F�ڤ����֤򉈤䤹
    public void IncreaseDigit(int index)
    {
        string currentText = leftSideDigits[index].text;

        if (int.TryParse(currentText, out int currentValue))
        {
            currentValue = (currentValue + 1) % 10; // ���֤�1���䤹��0-9�ι��죩
            leftSideDigits[index].text = currentValue.ToString(); // ���֤��ʾ
            digitAltered[index] = true; // ���֤�������줿���O��
        }

        CheckCode(); // ���`�ɤ����������_�J
        UpdateDigitColors(); // ���֤�ɫ�����
    }

    // �F�ڤ����֤�p�餹
    public void DecreaseDigit(int index)
    {
        string currentText = leftSideDigits[index].text;

        if (int.TryParse(currentText, out int currentValue))
        {
            currentValue = (currentValue - 1 + 10) % 10; // ���֤�1�p�餹��0-9�ι��죩
            leftSideDigits[index].text = currentValue.ToString(); // ���֤��ʾ
            digitAltered[index] = true; // ���֤�������줿���O��
        }

        CheckCode(); // ���`�ɤ����������_�J
        UpdateDigitColors(); // ���֤�ɫ�����
    }

    // �Τ����֤��Ƅ�
    public void MoveToNextDigit()
    {
        StopActiveDigitBlink(); // �F�ڤ����֤ε���ֹͣ
        currentDigitIndex = (currentDigitIndex + 1) % leftSideDigits.Length; // �Τ����֤��Ƅ�
        StartBlinkingOnCurrentDigit(); // �¤������֤ε����_ʼ
    }

    // ǰ�����֤��Ƅ�
    public void MoveToPreviousDigit()
    {
        StopActiveDigitBlink(); // �F�ڤ����֤ε���ֹͣ
        currentDigitIndex = (currentDigitIndex - 1 + leftSideDigits.Length) % leftSideDigits.Length; // ǰ�����֤��Ƅ�
        StartBlinkingOnCurrentDigit(); // �¤������֤ε����_ʼ
    }

    // �F�ڤ����֤ε����_ʼ
    private void StartBlinkingOnCurrentDigit()
    {
        if (activeDigitBlinkCoroutine != null)
        {
            StopCoroutine(activeDigitBlinkCoroutine); // ���Ǥˌg���Фε���ֹ���
        }

        activeDigitBlinkCoroutine = StartCoroutine(BlinkActiveDigit()); // �F�ڤ����֤ε����_ʼ
    }

    // �F�ڤ����֤ε���ֹͣ
    private void StopActiveDigitBlink()
    {
        if (activeDigitBlinkCoroutine != null)
        {
            StopCoroutine(activeDigitBlinkCoroutine); // ����ֹͣ
            leftSideDigits[currentDigitIndex].color = Color.white; // ɫ��פ��O��
        }
    }

    // �F�ڤ����֤ε���g�Ф��륳��`����
    IEnumerator BlinkActiveDigit()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.4f); // 0.4�����
            leftSideDigits[currentDigitIndex].color = (leftSideDigits[currentDigitIndex].color == Color.black) ? Color.white : Color.black; // �\�Ȱפ򽻻��˵��
        }
    }

    // ���֤�ɫ�����
    private void UpdateDigitColors()
    {
        for (int i = 0; i < leftSideDigits.Length; i++)
        {
            leftSideDigits[i].color = digitAltered[i] ? Color.white : Color.black; // ������줿���֤�ɫ��ס��������Ƥ��ʤ����֤�ɫ���\���O��
        }
    }
}