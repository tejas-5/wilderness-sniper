using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.EventSystems;

public class PopUpController : MonoBehaviour
{
    [Header("UI Panel")]
    public GameObject popUpPanel; // The pop-up panel

    [Header("Digits Elements")]
    [SerializeField] private TextMeshProUGUI[] rightSideNumbers; // Right-side numbers
    [SerializeField] private TextMeshProUGUI[] leftSideDigits; // Left-side digits
    public int correctCodeIndex; // Index of the correct code

    private int currentDigitIndex = 0; // Index of the current digit
    private int[] correctCode = new int[3]; // Correct code (3 digits)
    private Coroutine activeDigitBlinkCoroutine; // Coroutine for blinking the current digit
    private Coroutine correctAnswerBlinkCoroutine; // Coroutine for blinking the correct answer
    private Coroutine randomPopUpCoroutine; // Coroutine for random pop-up
    private bool[] digitAltered; // Whether the digits have been altered

    private bool isPopUpActive = false; // Whether the pop-up is active

    public GameObject errorCodePanel;

    // Start is called before the first frame update
    void Start()
    {
        if (rightSideNumbers.Length != 4 || leftSideDigits.Length != 3)
        {
            return; // If the array lengths are incorrect, exit the function
        }

        digitAltered = new bool[leftSideDigits.Length]; // Initialize the digit altered array
        for (int i = 0; i < digitAltered.Length; i++)
        {
            digitAltered[i] = false; // Set all digits as not altered by default
        }

        StartRandomPopUpCoroutine(); // Start the random pop-up coroutine
    }

    // Update is called once per frame
    void Update()
    {
        // When the 'A' key is pressed
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveToPreviousDigit(); // Move to the previous digit
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            MoveToNextDigit(); // Move to the next digit
        }

        // When the 'W' key is pressed
        if (Input.GetKeyDown(KeyCode.W))
        {
            IncreaseDigit(currentDigitIndex); // Increase the current digit
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            DecreaseDigit(currentDigitIndex); // Decrease the current digit
        }
        //if (!EventSystem.current.currentSelectedGameObject)
        //    {
        //    if (EventSystem.current.IsPointerOverGameObject())
        //    {
        //        Debug.Log(EventSystem.current.currentSelectedGameObject.gameObject.name);
        //        return;
        //    }
        //}

    }

    // Start the random pop-up coroutine
    public void StartRandomPopUpCoroutine(bool skipWaitTime = false)
    {
        if (randomPopUpCoroutine != null)
        {
            StopCoroutine(randomPopUpCoroutine); // Stop the currently running pop-up coroutine
        }
        Debug.Log("courtine started" + skipWaitTime);
        randomPopUpCoroutine = StartCoroutine(RandomPopUpCoroutine(skipWaitTime)); // Start a new random pop-up coroutine
    }

    // Coroutine to display random pop-ups
    IEnumerator RandomPopUpCoroutine(bool skipWaitTime)
    {
        while (true) // Infinite loop
        {
            if (isPopUpActive)
            {
                yield return null; // If the pop-up is already active, wait and continue
                continue;
            }

            if (skipWaitTime)
            {
                GenerateRandomNumbers(); // Generate random numbers
                ShowPopUp(); // Show the pop-up

                yield return new WaitUntil(() => CheckCode()); // Wait until the correct code is entered
                HidePopUp(); // Hide the pop-up once the correct code is entered
                yield break; // Exit the coroutine
            }
            else
            {
                float popUpTime = 30f;
                yield return new WaitForSeconds(popUpTime); // Wait for 30 seconds

                GenerateRandomNumbers(); // Generate random numbers
                ShowPopUp(); // Show the pop-up
                yield return new WaitUntil(() => CheckCode()); // Wait until the correct code is entered
                HidePopUp(); // Hide the pop-up once the correct code is entered
                yield break; // Exit the coroutine
            }
        }
    }

    // Generate random numbers
    void GenerateRandomNumbers()
    {
        for (int i = 0; i < rightSideNumbers.Length; i++)
        {
            rightSideNumbers[i].color = Color.white; // Set the color of the right-side numbers to white
            int randomNumber = Random.Range(100, 1000); // Generate a random number between 100 and 999
            rightSideNumbers[i].text = randomNumber.ToString(); // Display the number
        }

        correctCodeIndex = Random.Range(0, rightSideNumbers.Length); // Randomly select the index of the correct code
        int correctNumber = int.Parse(rightSideNumbers[correctCodeIndex].text);
        correctCode[0] = correctNumber / 100; // Hundreds place
        correctCode[1] = (correctNumber / 10) % 10; // Tens place
        correctCode[2] = correctNumber % 10; // Ones place

        rightSideNumbers[correctCodeIndex].color = Color.red; // Set the color of the correct number to red
        StartBlinkingCorrectAnswer(); // Start blinking the correct answer
        currentDigitIndex = 0; // Reset the current digit index
        StartBlinkingOnCurrentDigit(); // Start blinking the current digit
    }

    // Start blinking the correct answer
    private void StartBlinkingCorrectAnswer()
    {
        if (correctAnswerBlinkCoroutine != null)
        {
            StopCoroutine(correctAnswerBlinkCoroutine); // Stop the currently running blink coroutine
        }

        correctAnswerBlinkCoroutine = StartCoroutine(BlinkCorrectAnswer()); // Start blinking the correct answer
    }

    // Coroutine to blink the correct answer
    IEnumerator BlinkCorrectAnswer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.4f); // Wait for 0.4 seconds
            rightSideNumbers[correctCodeIndex].color = (rightSideNumbers[correctCodeIndex].color == Color.red) ? Color.white : Color.red; // Toggle between red and white
        }
    }

    // Show the pop-up
    public void ShowPopUp()
    {
        if (!isPopUpActive)
        {
            isPopUpActive = true; // Set the pop-up as active
            popUpPanel.SetActive(true); // Show the pop-up panel
        }
    }

    // Hide the pop-up
    public void HidePopUp()
    {
        if (isPopUpActive)
        {
            isPopUpActive = false; // Set the pop-up as inactive
            popUpPanel.SetActive(false); // Hide the pop-up panel
        }

        // Notify the PlayerController that the pop-up is resolved
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.SetPopUpWaiting(false);
        }
    }


    // Check if the entered code is correct
    public bool CheckCode()
    {
        bool codeMatches = true; // Whether the code matches

        for (int i = 0; i < leftSideDigits.Length; i++)
        {
            int playerInputValue = 0;
            if (int.TryParse(leftSideDigits[i].text, out playerInputValue)) // Parse the player's input as an integer
            {
                if (playerInputValue != correctCode[i]) // If the input does not match the correct code
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
            HidePopUp(); // Hide the pop-up if the code is correct
            return true; // The code is correct
        }

        return false; // The code is incorrect
    }

    // Increase the current digit
    public void IncreaseDigit(int index)
    {
        string currentText = leftSideDigits[index].text;

        if (int.TryParse(currentText, out int currentValue))
        {
            currentValue = (currentValue + 1) % 10; // Increase the digit (0-9 loop)
            leftSideDigits[index].text = currentValue.ToString(); // Display the digit
            digitAltered[index] = true; // Mark the digit as altered
        }

        CheckCode(); // Check if the code is correct
        UpdateDigitColors(); // Update the digit colors
    }

    // Decrease the current digit
    public void DecreaseDigit(int index)
    {
        string currentText = leftSideDigits[index].text;

        if (int.TryParse(currentText, out int currentValue))
        {
            currentValue = (currentValue - 1 + 10) % 10; // Decrease the digit (0-9 loop)
            leftSideDigits[index].text = currentValue.ToString(); // Display the digit
            digitAltered[index] = true; // Mark the digit as altered
        }

        CheckCode(); // Check if the code is correct
        UpdateDigitColors(); // Update the digit colors
    }

    // Move to the next digit
    public void MoveToNextDigit()
    {
        StopActiveDigitBlink(); // Stop blinking the current digit
        currentDigitIndex = (currentDigitIndex + 1) % leftSideDigits.Length; // Move to the next digit
        StartBlinkingOnCurrentDigit(); // Start blinking the new digit
    }

    // Move to the previous digit
    public void MoveToPreviousDigit()
    {
        StopActiveDigitBlink(); // Stop blinking the current digit
        currentDigitIndex = (currentDigitIndex - 1 + leftSideDigits.Length) % leftSideDigits.Length; // Move to the previous digit
        StartBlinkingOnCurrentDigit(); // Start blinking the new digit
    }

    // Start blinking the current digit
    private void StartBlinkingOnCurrentDigit()
    {
        if (activeDigitBlinkCoroutine != null)
        {
            StopCoroutine(activeDigitBlinkCoroutine); // Stop the currently running blink coroutine
        }

        activeDigitBlinkCoroutine = StartCoroutine(BlinkActiveDigit()); // Start blinking the current digit
    }

    // Stop blinking the current digit
    private void StopActiveDigitBlink()
    {
        if (activeDigitBlinkCoroutine != null)
        {
            StopCoroutine(activeDigitBlinkCoroutine); // Stop the blinking
            leftSideDigits[currentDigitIndex].color = Color.white; // Set the color to white
        }
    }

    // Coroutine to blink the current digit
    IEnumerator BlinkActiveDigit()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.4f); // Wait for 0.4 seconds
            leftSideDigits[currentDigitIndex].color = (leftSideDigits[currentDigitIndex].color == Color.black) ? Color.white : Color.black; // Toggle between black and white
        }
    }

    // Update the digit colors
    private void UpdateDigitColors()
    {
        for (int i = 0; i < leftSideDigits.Length; i++)
        {
            leftSideDigits[i].color = digitAltered[i] ? Color.white : Color.black; // Set altered digits to white, unaltered digits to black
        }
    }

    public void ClosePopUp()
    {
        if (errorCodePanel != null)
        {
            errorCodePanel.SetActive(false);  // Hides the ErrorCode panel
        }
        else
        {
            Debug.LogWarning("ErrorCode panel is not assigned in the PopUpController!");
        }
    }
}