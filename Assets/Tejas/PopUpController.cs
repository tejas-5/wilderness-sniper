using UnityEngine;
using UnityEngine.UI;  // For UI Text and Button
using System.Collections;

public class PopUpController : MonoBehaviour
{
    public GameObject popUpPanel; // Reference to the Panel (pop-up) GameObject
    public Text[] rightSideNumbers; // Array for the random numbers (right side) - now 4 numbers
    public Text[] leftSideDigits;   // Array for the displayed digits (left side) - still 3 digits
    public int correctCodeIndex;    // The index of the correct number

    private int currentDigitIndex = 0; // Tracks which digit the player is currently editing
    private int[] correctCode = new int[3]; // The correct code that the player must enter (3 digits)

    void Start()
    {
        // Ensure arrays are initialized with the correct number of elements
        if (rightSideNumbers.Length != 4 || leftSideDigits.Length != 3)
        {
            Debug.LogError("Arrays must have 4 elements for rightSideNumbers and 3 elements for leftSideDigits.");
            return;  // Exit early if arrays don't match expected sizes
        }

        // Generate random numbers and show the pop-up
        GenerateRandomNumbers();
        ShowPopUp();
    }

    // Update is called every frame to check for keyboard input
    void Update()
    {
        // Move between digits using 'A' (left) and 'D' (right)
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveToPreviousDigit();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            MoveToNextDigit();
        }

        // Increase or decrease the current digit using 'W' (increase) and 'S' (decrease)
        if (Input.GetKeyDown(KeyCode.W))
        {
            IncreaseDigit(currentDigitIndex);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            DecreaseDigit(currentDigitIndex);
        }

        // Press Enter to check if the entered code matches the correct code
        if (Input.GetKeyDown(KeyCode.Return))  // Enter key
        {
            CheckCode();
        }
    }

    // Generate random 3-digit numbers and assign them to the Text fields
    void GenerateRandomNumbers()
    {
        for (int i = 0; i < rightSideNumbers.Length; i++)  // Loop through 4 digits (for right side)
        {
            int randomNumber = Random.Range(100, 1000);  // Random 3-digit number
            rightSideNumbers[i].text = randomNumber.ToString();  // Set the number
        }

        // Randomly select one number to blink (correct answer)
        correctCodeIndex = Random.Range(0, rightSideNumbers.Length);
        correctCode[0] = int.Parse(rightSideNumbers[correctCodeIndex].text); // Store the correct number

        StartCoroutine(BlinkCorrectAnswer());
    }

    // Coroutine to blink the correct number on the right side
    IEnumerator BlinkCorrectAnswer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            rightSideNumbers[correctCodeIndex].color = (rightSideNumbers[correctCodeIndex].color == Color.red) ? Color.white : Color.red;
        }
    }

    // Show the pop-up panel when triggered
    void ShowPopUp()
    {
        popUpPanel.SetActive(true);
    }

    // Hide the pop-up panel when done
    public void HidePopUp()
    {
        popUpPanel.SetActive(false);
    }

    // Function to increase the value of a specific digit
    public void IncreaseDigit(int index)
    {
        string currentText = leftSideDigits[index].text;

        // Try parsing the current digit and update
        if (int.TryParse(currentText, out int currentValue))
        {
            if (currentValue < 9)  // Stop at 9, don't wrap around
            {
                currentValue++;
                leftSideDigits[index].text = currentValue.ToString();
            }
        }
        else
        {
            Debug.LogWarning("Invalid value for digit " + index);
        }
    }

    // Function to decrease the value of a specific digit
    public void DecreaseDigit(int index)
    {
        string currentText = leftSideDigits[index].text;

        // Try parsing the current digit and update
        if (int.TryParse(currentText, out int currentValue))
        {
            if (currentValue > 0)  // Stop at 0, don't wrap around
            {
                currentValue--;
                leftSideDigits[index].text = currentValue.ToString();
            }
        }
        else
        {
            Debug.LogWarning("Invalid value for digit " + index);
        }
    }

    // Function to move to the next digit (change the currentDigitIndex)
    public void MoveToNextDigit()
    {
        currentDigitIndex = (currentDigitIndex + 1) % leftSideDigits.Length; // Loop through digits
    }

    // Function to move to the previous digit (change the currentDigitIndex)
    public void MoveToPreviousDigit()
    {
        currentDigitIndex = (currentDigitIndex - 1 + leftSideDigits.Length) % leftSideDigits.Length; // Loop through digits
    }

    // Function to check if the player's entered code matches the correct code
    public void CheckCode()
    {
        // Compare entered code with correct code
        bool codeMatches = true;
        for (int i = 0; i < leftSideDigits.Length; i++)
        {
            if (int.Parse(leftSideDigits[i].text) != correctCode[i])
            {
                codeMatches = false;
                break;
            }
        }

        if (codeMatches)
        {
            // If the code matches, close the pop-up
            Debug.Log("Code matches! Closing pop-up...");
            HidePopUp();  // Hide the pop-up panel
        }
        else
        {
            // If the code doesn't match, notify the player (or reset the input)
            Debug.Log("Code does not match. Try again.");
            ResetInput();  // Optional: Reset the digits if needed
        }
    }

    // Optional: Reset the input fields if the code doesn't match
    void ResetInput()
    {
        for (int i = 0; i < leftSideDigits.Length; i++)
        {
            leftSideDigits[i].text = "0";  // Reset each digit to 0 (or you could leave it as is)
        }
    }
}
