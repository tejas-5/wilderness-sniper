using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopUpController : MonoBehaviour
{
    public GameObject popUpPanel; // �p�l���i�|�b�v�A�b�v�j�̎Q��
    public Text[] rightSideNumbers; // �����_���Ȑ����i�E���j�̔z��
    public Text[] leftSideDigits;   // �\������鐔���i�����j�̔z��
    public int correctCodeIndex;    // �������ԍ��̃C���f�b�N�X

    private int currentDigitIndex = 0; // ���ݕҏW���̌�
    private int[] correctCode = new int[3]; // �v���C���[�����͂��ׂ��������R�[�h�i3���j
    private Coroutine activeDigitBlinkCoroutine; // �A�N�e�B�u�Ȍ��̓_�ł��Ǘ�����R���[�`��
    private Coroutine correctAnswerBlinkCoroutine; // �����������̓_�ł��Ǘ�����R���[�`��

    void Start()
    {
        if (rightSideNumbers.Length != 4 || leftSideDigits.Length != 3)
        {
            Debug.LogError("�z��͉E���̐�����4�A�����̐�����3�ł���K�v������܂��B");
            return;
        }

        GenerateRandomNumbers(); // �����_���Ȑ����𐶐�
        ShowPopUp(); // �|�b�v�A�b�v��\��
        StartBlinkingOnCurrentDigit(); // �ŏ��̃A�N�e�B�u���̓_�ł��J�n
    }

    void Update()
    {
        // ���͂��ꂽ�L�[�Ō���ύX����
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveToPreviousDigit(); // �O�̌��Ɉړ�
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            MoveToNextDigit(); // ���̌��Ɉړ�
        }

        // ���͂��ꂽ�L�[�Ō��̐����𑝌�������
        if (Input.GetKeyDown(KeyCode.W))
        {
            IncreaseDigit(currentDigitIndex); // ���݂̌��̐����𑝂₷
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            DecreaseDigit(currentDigitIndex); // ���݂̌��̐��������炷
        }
    }

    void GenerateRandomNumbers()
    {
        for (int i = 0; i < rightSideNumbers.Length; i++)
        {
            int randomNumber = Random.Range(100, 1000); // 100����999�̊ԂŃ����_���Ȑ����𐶐�
            rightSideNumbers[i].text = randomNumber.ToString(); // �e�L�X�g�ɐݒ�
        }

        correctCodeIndex = Random.Range(0, rightSideNumbers.Length); // �������R�[�h�̃C���f�b�N�X�������_���Ɍ���
        int correctNumber = int.Parse(rightSideNumbers[correctCodeIndex].text); // �������ԍ����擾
        correctCode[0] = correctNumber / 100; // �������R�[�h�̕S�̈�
        correctCode[1] = (correctNumber / 10) % 10; // �������R�[�h�̏\�̈�
        correctCode[2] = correctNumber % 10; // �������R�[�h�̈�̈�

        StartCoroutine(BlinkCorrectAnswer()); // ������������_�ł�����R���[�`�����J�n
    }

    IEnumerator BlinkCorrectAnswer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.4f); // 0.4�b���Ƃ�
            rightSideNumbers[correctCodeIndex].color = (rightSideNumbers[correctCodeIndex].color == Color.red) ? Color.white : Color.red; // �������ԍ��̐F��ԂƔ��œ_�ł�����
        }
    }

    void ShowPopUp()
    {
        popUpPanel.SetActive(true); // �|�b�v�A�b�v��\��
    }

    public void HidePopUp()
    {
        popUpPanel.SetActive(false); // �|�b�v�A�b�v���\���ɂ���
    }

    // �{�^���N���b�N�Ő����𑝂₷
    public void IncreaseDigit(int index)
    {
        string currentText = leftSideDigits[index].text; // ���݂̌��̃e�L�X�g���擾

        if (int.TryParse(currentText, out int currentValue))
        {
            // ���������āA9�𒴂�����0�ɖ߂�
            currentValue = (currentValue + 1) % 10;
            leftSideDigits[index].text = currentValue.ToString(); // �V�����l���e�L�X�g�ɐݒ�
        }
        else
        {
            Debug.LogWarning("�����Ȓl�ł�: " + index); // �����ȊO�̒l�������Ă���ꍇ
        }

        CheckCode(); // �R�[�h���`�F�b�N
    }

    // �{�^���N���b�N�Ő��������炷
    public void DecreaseDigit(int index)
    {
        string currentText = leftSideDigits[index].text; // ���݂̌��̃e�L�X�g���擾

        if (int.TryParse(currentText, out int currentValue))
        {
            // ���������āA0�����ɂȂ�����9�ɖ߂�
            currentValue = (currentValue - 1 + 10) % 10;
            leftSideDigits[index].text = currentValue.ToString(); // �V�����l���e�L�X�g�ɐݒ�
        }
        else
        {
            Debug.LogWarning("�����Ȓl�ł�: " + index); // �����ȊO�̒l�������Ă���ꍇ
        }

        CheckCode(); // �R�[�h���`�F�b�N
    }

    public void MoveToNextDigit()
    {
        StopActiveDigitBlink(); // �O�̃A�N�e�B�u���̓_�ł��~

        currentDigitIndex = (currentDigitIndex + 1) % leftSideDigits.Length; // ���̌��Ɉړ�

        StartBlinkingOnCurrentDigit(); // �V�������̓_�ł��J�n
    }

    public void MoveToPreviousDigit()
    {
        StopActiveDigitBlink(); // �O�̃A�N�e�B�u���̓_�ł��~

        currentDigitIndex = (currentDigitIndex - 1 + leftSideDigits.Length) % leftSideDigits.Length; // �O�̌��Ɉړ�

        StartBlinkingOnCurrentDigit(); // �V�������̓_�ł��J�n
    }

    // ���ݑI������Ă��錅�̓_�ł��J�n
    private void StartBlinkingOnCurrentDigit()
    {
        // �����_�ŃR���[�`��������Β�~
        if (activeDigitBlinkCoroutine != null)
        {
            StopCoroutine(activeDigitBlinkCoroutine);
        }

        // ���ݑI������Ă��錅�̓_�ŃR���[�`�����J�n
        activeDigitBlinkCoroutine = StartCoroutine(BlinkActiveDigit());
    }

    // ���ݑI������Ă��錅�̓_�ł��~
    private void StopActiveDigitBlink()
    {
        if (activeDigitBlinkCoroutine != null)
        {
            StopCoroutine(activeDigitBlinkCoroutine); // �_�ŃR���[�`�����~
            leftSideDigits[currentDigitIndex].color = Color.white; // �F�𔒂ɖ߂�
        }
    }

    // ���ݑI������Ă��錅�̓_�ł��s���R���[�`��
    IEnumerator BlinkActiveDigit()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.4f); // 0.4�b���Ƃɓ_��
            // �A�N�e�B�u���̐F�����Ɣ��Ō��݂ɓ_�ł�����
            leftSideDigits[currentDigitIndex].color = (leftSideDigits[currentDigitIndex].color == Color.black) ? Color.white : Color.black;
        }
    }

    // ���͂��ꂽ�R�[�h���`�F�b�N
    public void CheckCode()
    {
        bool codeMatches = true; // �R�[�h����v���邩�ǂ����̃t���O

        string enteredCode = "";
        for (int i = 0; i < leftSideDigits.Length; i++)
        {
            enteredCode += leftSideDigits[i].text + " "; // ���͂��ꂽ�R�[�h�𕶎���Ƃ��Ă܂Ƃ߂�
        }
        Debug.Log("���͂��ꂽ�R�[�h: " + enteredCode); // �f�o�b�O�p�ɕ\��

        // ���͂��ꂽ�R�[�h�����������ǂ������`�F�b�N
        for (int i = 0; i < leftSideDigits.Length; i++)
        {
            int playerInputValue = 0;
            if (int.TryParse(leftSideDigits[i].text, out playerInputValue)) // ���������������͂���Ă��邩�`�F�b�N
            {
                Debug.Log($"��r: �v���C���[����: {playerInputValue}, �������R�[�h: {correctCode[i]}");

                if (playerInputValue != correctCode[i]) // �������R�[�h�ƈ�v���Ȃ��ꍇ
                {
                    codeMatches = false;
                    break;
                }
            }
            else
            {
                Debug.LogWarning("�����ȓ��͂ł�: " + i); // �����ȊO�����͂��ꂽ�ꍇ
                codeMatches = false;
                break;
            }
        }

        // �R�[�h����v�����ꍇ�A�|�b�v�A�b�v�����
        if (codeMatches)
        {
            Debug.Log("�R�[�h����v���܂����I�|�b�v�A�b�v����܂�...");
            HidePopUp();
        }
        else
        {
            Debug.Log("�R�[�h����v���܂���B������x�����Ă��������B");
        }
    }

    void ResetInput()
    {
        for (int i = 0; i < leftSideDigits.Length; i++)
        {
            leftSideDigits[i].text = "0"; // ���ׂĂ̌���0�Ƀ��Z�b�g
        }
    }
}
