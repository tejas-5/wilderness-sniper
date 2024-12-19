using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class PopUpController : MonoBehaviour
{

    [Header(" UI Panel ")]
    public GameObject popUpPanel; // �|�b�v�A�b�v�̃p�l��

    [Header(" Digits Elements ")]
    [SerializeField] private TextMeshProUGUI[] rightSideNumbers; // �E���ɕ\�����鐔���̔z��
    [SerializeField] private TextMeshProUGUI[] leftSideDigits; // �����ɕ\�����鐔���̔z��
    public int correctCodeIndex; // �������R�[�h�̃C���f�b�N�X

    private int currentDigitIndex = 0; // ���ݑI������Ă��錅
    private int[] correctCode = new int[3]; // �������R�[�h�i3���j
    private Coroutine activeDigitBlinkCoroutine; // �A�N�e�B�u���̓_�ŃR���[�`��
    private Coroutine correctAnswerBlinkCoroutine; // �����������̓_�ŃR���[�`��
    private Coroutine randomPopUpCoroutine; // �����_���ȃ^�C�~���O�Ń|�b�v�A�b�v��\������R���[�`��
    private bool[] digitAltered; // ���͂��ꂽ�����ύX���ꂽ���ǂ���

    private bool isPopUpActive = false; // �|�b�v�A�b�v���A�N�e�B�u���ǂ���

    // Start is called before the first frame update
    void Start()
    {
        if (rightSideNumbers.Length != 4 || leftSideDigits.Length != 3)
        {
            return; // �z��̃T�C�Y���������Ȃ��ꍇ�A�������I��
        }

        digitAltered = new bool[leftSideDigits.Length]; // �ύX���ꂽ���̏�Ԃ��Ǘ�
        for (int i = 0; i < digitAltered.Length; i++)
        {
            digitAltered[i] = false; // �ŏ��͕ύX�Ȃ�
        }

        StartRandomPopUpCoroutine(); // �����_���^�C�~���O�Ń|�b�v�A�b�v��\������R���[�`�����J�n
    }

    // Update is called once per frame
    void Update()
    {
        // ���͂��ꂽ�L�[�Ō���ύX����
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveToPreviousDigit(); // �O�̌��ֈړ�
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            MoveToNextDigit(); // ���̌��ֈړ�
        }

        // ���͂��ꂽ�L�[�Ō��̐����𑝌�������
        if (Input.GetKeyDown(KeyCode.W))
        {
            IncreaseDigit(currentDigitIndex); // ���݂̌��𑝂₷
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            DecreaseDigit(currentDigitIndex); // ���݂̌������炷
        }
    }

    // �����_���ȃ^�C�~���O�Ń|�b�v�A�b�v��\������R���[�`�����J�n
    public void StartRandomPopUpCoroutine(bool skipWaitTime = false)
    {
        if (randomPopUpCoroutine != null)
        {
            StopCoroutine(randomPopUpCoroutine); // �����̃R���[�`�����~
        }
        randomPopUpCoroutine = StartCoroutine(RandomPopUpCoroutine(skipWaitTime)); // �V�����R���[�`�����J�n
    }

    // �����_���ȃ^�C�~���O�Ń|�b�v�A�b�v��\������R���[�`��
    IEnumerator RandomPopUpCoroutine(bool skipWaitTime)
    {
        while (true) // �Q�[�����i�s���Ă���ԁA�|�b�v�A�b�v��\����������
        {
            if (isPopUpActive)
            {
                yield return null; // �|�b�v�A�b�v���\�����Ȃ�ҋ@
                continue;
            }

            if (skipWaitTime)
            {
                GenerateRandomNumbers(); // �����_���Ȑ����𐶐�
                ShowPopUp(); // �|�b�v�A�b�v��\��
                yield return new WaitUntil(() => CheckCode()); // �������R�[�h�����͂����܂őҋ@
                HidePopUp(); // Hide the popup after correct input
                yield break; // Exit the coroutine once the code is entered correctly
            }
            else
            {
                float popUpTime = 30f;
                yield return new WaitForSeconds(popUpTime); // 30�b�ҋ@

                GenerateRandomNumbers(); // �����_���Ȑ����𐶐�
                ShowPopUp(); // �|�b�v�A�b�v��\��
                yield return new WaitUntil(() => CheckCode()); // �������R�[�h�����͂����܂őҋ@
                HidePopUp(); // Hide the popup after correct input
                yield break; // Exit the coroutine once the code is entered correctly
            }
        }
    }

    // �����_���Ȑ����𐶐����A�|�b�v�A�b�v��\�����鏀��������
    void GenerateRandomNumbers()
    {
        for (int i = 0; i < rightSideNumbers.Length; i++)
        {
            rightSideNumbers[i].color = Color.white; // �����̐F�����Z�b�g
            int randomNumber = Random.Range(100, 1000); // 100����999�̊ԂŃ����_���Ȑ����𐶐�
            rightSideNumbers[i].text = randomNumber.ToString(); // �e�L�X�g�ɐݒ�
        }

        correctCodeIndex = Random.Range(0, rightSideNumbers.Length); // �������R�[�h�������_���ɑI��
        int correctNumber = int.Parse(rightSideNumbers[correctCodeIndex].text);
        correctCode[0] = correctNumber / 100; // �S�̈�
        correctCode[1] = (correctNumber / 10) % 10; // �\�̈�
        correctCode[2] = correctNumber % 10; // ��̈�

        rightSideNumbers[correctCodeIndex].color = Color.red; // ������������ԐF�ɐݒ�
        StartBlinkingCorrectAnswer(); // ������������_�ł�����
        currentDigitIndex = 0; // �ŏ��̌���I��
        StartBlinkingOnCurrentDigit(); // ���݂̌���_�ł�����
    }

    // ������������_�ł�����
    private void StartBlinkingCorrectAnswer()
    {
        if (correctAnswerBlinkCoroutine != null)
        {
            StopCoroutine(correctAnswerBlinkCoroutine); // �����̓_�ŃR���[�`�����~
        }

        correctAnswerBlinkCoroutine = StartCoroutine(BlinkCorrectAnswer()); // �V�����_�ŃR���[�`�����J�n
    }

    // �������������_�ł���R���[�`��
    IEnumerator BlinkCorrectAnswer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.4f); // 0.4�b���Ƃɓ_��
            rightSideNumbers[correctCodeIndex].color = (rightSideNumbers[correctCodeIndex].color == Color.red) ? Color.white : Color.red; // �ԂƔ���؂�ւ�
        }
    }

    // �|�b�v�A�b�v��\������
    public void ShowPopUp()
    {
        if (!isPopUpActive)
        {
            isPopUpActive = true; // �|�b�v�A�b�v��\����Ԃɂ���
            popUpPanel.SetActive(true); // �p�l����\��
        }
    }

    // �|�b�v�A�b�v���\���ɂ���
    public void HidePopUp()
    {
        if (isPopUpActive)
        {
            isPopUpActive = false; // �|�b�v�A�b�v���\����Ԃɂ���
            popUpPanel.SetActive(false); // �p�l�����\��
        }
    }

    // ���͂��ꂽ�R�[�h�����������`�F�b�N����
    public bool CheckCode()
    {
        bool codeMatches = true; // �R�[�h����v���邩�ǂ���

        for (int i = 0; i < leftSideDigits.Length; i++)
        {
            int playerInputValue = 0;
            if (int.TryParse(leftSideDigits[i].text, out playerInputValue)) // ���͂��ꂽ�l�𐮐��ɕϊ�
            {
                if (playerInputValue != correctCode[i]) // �������R�[�h�Ɣ�r
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
            HidePopUp(); // �R�[�h����v������|�b�v�A�b�v���\���ɂ���
            return true; // �������R�[�h
        }

        return false; // �R�[�h����v���Ȃ�
    }

    // ���݂̌��̐����𑝂₷
    public void IncreaseDigit(int index)
    {
        string currentText = leftSideDigits[index].text;

        if (int.TryParse(currentText, out int currentValue))
        {
            currentValue = (currentValue + 1) % 10; // 1���₷�i10�ɂȂ�����0�ɖ߂�j
            leftSideDigits[index].text = currentValue.ToString(); // �e�L�X�g���X�V
            digitAltered[index] = true; // �ύX���ꂽ���Ƃ��ă}�[�N
        }

        CheckCode(); // �R�[�h����v���邩�m�F
        UpdateDigitColors(); // ���̐F���X�V
    }

    // ���݂̌��̐��������炷
    public void DecreaseDigit(int index)
    {
        string currentText = leftSideDigits[index].text;

        if (int.TryParse(currentText, out int currentValue))
        {
            currentValue = (currentValue - 1 + 10) % 10; // 1���炷�i0�ɂȂ�����9�ɖ߂�j
            leftSideDigits[index].text = currentValue.ToString(); // �e�L�X�g���X�V
            digitAltered[index] = true; // �ύX���ꂽ���Ƃ��ă}�[�N
        }

        CheckCode(); // �R�[�h����v���邩�m�F
        UpdateDigitColors(); // ���̐F���X�V
    }

    // ���̌��Ɉړ�����
    public void MoveToNextDigit()
    {
        StopActiveDigitBlink(); // ���݂̌��̓_�ł��~�߂�
        currentDigitIndex = (currentDigitIndex + 1) % leftSideDigits.Length; // ���̌��Ɉړ�
        StartBlinkingOnCurrentDigit(); // �V��������_�ł�����
    }

    // �O�̌��Ɉړ�����
    public void MoveToPreviousDigit()
    {
        StopActiveDigitBlink(); // ���݂̌��̓_�ł��~�߂�
        currentDigitIndex = (currentDigitIndex - 1 + leftSideDigits.Length) % leftSideDigits.Length; // �O�̌��Ɉړ�
        StartBlinkingOnCurrentDigit(); // �V��������_�ł�����
    }

    // ���݂̌���_�ł�����
    private void StartBlinkingOnCurrentDigit()
    {
        if (activeDigitBlinkCoroutine != null)
        {
            StopCoroutine(activeDigitBlinkCoroutine); // �����̓_�ŃR���[�`�����~
        }

        activeDigitBlinkCoroutine = StartCoroutine(BlinkActiveDigit()); // �V�����_�ŃR���[�`�����J�n
    }

    // ���݂̌���_�ł�����R���[�`��
    private void StopActiveDigitBlink()
    {
        if (activeDigitBlinkCoroutine != null)
        {
            StopCoroutine(activeDigitBlinkCoroutine); // �_�ŃR���[�`�����~
            leftSideDigits[currentDigitIndex].color = Color.white; // �F�����ɖ߂�
        }
    }

    // ���݂̌���_�ł�����R���[�`��
    IEnumerator BlinkActiveDigit()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.4f); // 0.4�b���Ƃɓ_��
            leftSideDigits[currentDigitIndex].color = (leftSideDigits[currentDigitIndex].color == Color.black) ? Color.white : Color.black; // ���Ɣ���؂�ւ�
        }
    }

    // ���̐F���X�V����
    private void UpdateDigitColors()
    {
        for (int i = 0; i < leftSideDigits.Length; i++)
        {
            leftSideDigits[i].color = digitAltered[i] ? Color.white : Color.black; // �ύX���ꂽ���͔��A�ύX����Ă��Ȃ����͍�
        }
    }
}