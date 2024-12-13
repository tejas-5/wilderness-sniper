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
    private Coroutine randomPopUpCoroutine; // Store the coroutine for showing the popup at random intervals
    private bool[] digitAltered;

    private bool isRKeyPressed = false;

    void Start()
    {
        if (rightSideNumbers.Length != 4 || leftSideDigits.Length != 3)
        {
            Debug.LogError("�z��͉E���̐�����4�A�����̐�����3�ł���K�v������܂��B");
            return;
        }

        digitAltered = new bool[leftSideDigits.Length]; // Initialize the altered status array
        for (int i = 0; i < digitAltered.Length; i++)
        {
            digitAltered[i] = false; // Initially, no digit has been altered
        }

        StartRandomPopUpCoroutine(); // Start showing popups at random intervals
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            isRKeyPressed = true;
            if (!popUpPanel.activeSelf) // Only start showing the popup if it's not already active
            {
                StartCoroutine(RandomPopUpCoroutine());
            }
        }
    }

    // Start the coroutine to show popups at random intervals
    void StartRandomPopUpCoroutine()
    {
        if (randomPopUpCoroutine != null)
        {
            StopCoroutine(randomPopUpCoroutine); // Stop any existing coroutine to avoid multiple coroutines running
        }

        randomPopUpCoroutine = StartCoroutine(RandomPopUpCoroutine()); // Start the coroutine that shows the popup at random intervals
    }

    IEnumerator RandomPopUpCoroutine()
    {
        float popUpTime = 3f; // Declare popUpTime once at the start of the coroutine

        while (true) // Keep showing the popup randomly while the game is running
        {
            if (isRKeyPressed)
            {
                isRKeyPressed = false; // Reset the flag

                // Generate new random numbers for the popup
                GenerateRandomNumbers();

                // Show the popup with the generated numbers
                ShowPopUp();

                // Wait until the user successfully enters the correct code before proceeding to the next popup
                yield return new WaitUntil(() => CheckCode());

                // After the correct code, hide the pop-up
                HidePopUp();

                // Wait for a random time before showing the next pop-up
                yield return new WaitForSeconds(popUpTime); // Use the existing popUpTime
                continue; // Proceed to the next iteration
            }

            // Wait for a random time before showing the next pop-up
            yield return new WaitForSeconds(popUpTime); // Use the existing popUpTime

            // Generate new random numbers for the popup
            GenerateRandomNumbers();

            // Show the popup with the generated numbers
            ShowPopUp();

            // Wait until the user successfully enters the correct code before proceeding to the next popup
            yield return new WaitUntil(() => CheckCode());

            // After the correct code, hide the pop-up
            HidePopUp();

            // Wait for the next pop-up to appear
            yield return new WaitForSeconds(popUpTime); // Use the existing popUpTime
        }
    }

    // Generate random numbers for the right-side display and correct code
    void GenerateRandomNumbers()
    {
        for (int i = 0; i < rightSideNumbers.Length; i++)
        {
            rightSideNumbers[i].color = Color.white;
            int randomNumber = Random.Range(100, 1000); // 100����999�̊ԂŃ����_���Ȑ����𐶐�
            rightSideNumbers[i].text = randomNumber.ToString(); // �e�L�X�g�ɐݒ�
        }

        // Randomly select the correct code from the generated numbers
        correctCodeIndex = Random.Range(0, rightSideNumbers.Length);
        int correctNumber = int.Parse(rightSideNumbers[correctCodeIndex].text);
        correctCode[0] = correctNumber / 100; // �S�̈�
        correctCode[1] = (correctNumber / 10) % 10; // �\�̈�
        correctCode[2] = correctNumber % 10; // ��̈�

        rightSideNumbers[correctCodeIndex].color = Color.red;
        // Start blinking on the correct answer
        StartBlinkingCorrectAnswer();
        currentDigitIndex = 0; // Set the first digit as the active digit
        StartBlinkingOnCurrentDigit();
    }

    // Start blinking on the correct answer (the number that is the correct code)
    private void StartBlinkingCorrectAnswer()
    {
        if (correctAnswerBlinkCoroutine != null)
        {
            StopCoroutine(correctAnswerBlinkCoroutine); // Stop any existing blinking coroutine
        }

        // Start the blinking coroutine for the correct answer number
        correctAnswerBlinkCoroutine = StartCoroutine(BlinkCorrectAnswer());
    }

    // Coroutine to blink the correct answer number
    IEnumerator BlinkCorrectAnswer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.4f); // 0.4 seconds blink interval
            // Toggle the color of the correct answer number between red and white
            rightSideNumbers[correctCodeIndex].color = (rightSideNumbers[correctCodeIndex].color == Color.red) ? Color.white : Color.red;
        }
    }

    // Show the popup panel
    void ShowPopUp()
    {
        if (!popUpPanel.activeSelf) // Only show the popup if it's not already active
        {
            popUpPanel.SetActive(true); // �|�b�v�A�b�v��\��
        }
    }

    // Hide the popup panel
    public void HidePopUp()
    {
        popUpPanel.SetActive(false); // �|�b�v�A�b�v���\���ɂ���
    }

    // Check if the entered code is correct
    public bool CheckCode()
    {
        bool codeMatches = true; // Code match flag

        // Check each digit entered by the player
        for (int i = 0; i < leftSideDigits.Length; i++)
        {
            int playerInputValue = 0;
            if (int.TryParse(leftSideDigits[i].text, out playerInputValue))
            {
                // Compare the player's input with the correct code
                if (playerInputValue != correctCode[i])
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

        // If the code matches, hide the popup and return true
        if (codeMatches)
        {
            HidePopUp();
            return true; // Correct code entered
        }

        // If the code doesn't match, return false
        return false;
    }

    // Button clicks to modify the current digit
    public void IncreaseDigit(int index)
    {
        string currentText = leftSideDigits[index].text;

        if (int.TryParse(currentText, out int currentValue))
        {
            currentValue = (currentValue + 1) % 10;
            leftSideDigits[index].text = currentValue.ToString();
            digitAltered[index] = true;
        }

        CheckCode();
        UpdateDigitColors();
    }

    public void DecreaseDigit(int index)
    {
        string currentText = leftSideDigits[index].text;

        if (int.TryParse(currentText, out int currentValue))
        {
            currentValue = (currentValue - 1 + 10) % 10;
            leftSideDigits[index].text = currentValue.ToString();
            digitAltered[index] = true;
        }

        CheckCode();
        UpdateDigitColors();
    }

    public void MoveToNextDigit()
    {
        StopActiveDigitBlink(); // Stop the blinking on the previous digit
        currentDigitIndex = (currentDigitIndex + 1) % leftSideDigits.Length; // Move to the next digit
        StartBlinkingOnCurrentDigit(); // Start blinking on the new digit
    }

    public void MoveToPreviousDigit()
    {
        StopActiveDigitBlink(); // Stop the blinking on the previous digit
        currentDigitIndex = (currentDigitIndex - 1 + leftSideDigits.Length) % leftSideDigits.Length; // Move to the previous digit
        StartBlinkingOnCurrentDigit(); // Start blinking on the new digit
    }

    // Start blinking on the current active digit
    private void StartBlinkingOnCurrentDigit()
    {
        if (activeDigitBlinkCoroutine != null)
        {
            StopCoroutine(activeDigitBlinkCoroutine); // Stop the existing blinking coroutine
        }

        // Start the new blinking coroutine for the current active digit
        activeDigitBlinkCoroutine = StartCoroutine(BlinkActiveDigit());
    }

    // Stop blinking on the current active digit
    private void StopActiveDigitBlink()
    {
        if (activeDigitBlinkCoroutine != null)
        {
            StopCoroutine(activeDigitBlinkCoroutine); // Stop the blinking coroutine
            leftSideDigits[currentDigitIndex].color = Color.white; // Reset the color to white
        }
    }

    // Blinking coroutine for the active digit
    IEnumerator BlinkActiveDigit()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.4f); // 0.4 seconds blink interval
            // Toggle the color of the current active digit between black and white
            leftSideDigits[currentDigitIndex].color = (leftSideDigits[currentDigitIndex].color == Color.black) ? Color.white : Color.black;
        }
    }
    private void UpdateDigitColors()
    {
        for (int i = 0; i < leftSideDigits.Length; i++)
        {
            leftSideDigits[i].color = digitAltered[i] ? Color.white : Color.black;
        }
    }

    void ResetInput()
    {
        for (int i = 0; i < leftSideDigits.Length; i++)
        {
            leftSideDigits[i].text = "0"; // Reset all digits to 0
            digitAltered[i] = false;
        }
    }
}