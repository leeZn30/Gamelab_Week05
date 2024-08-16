using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections; // Added to use IEnumerator
public class KeypadManager : MonoBehaviour
{
    public GameObject keypadUI; // Keypad UI Canvas
    public TextMeshProUGUI inputText; // Text to show entered numbers
    public TextMeshProUGUI successText; // Text to show success message
    public TextMeshProUGUI failText; // Text to show fail message
    public string correctCode = "123456"; // The correct code to unlock
    private string enteredCode = ""; // The code entered by the player
    public bool KeypadUnlocked { get; private set; } = false; // Whether the keypad is unlocked
    private float failDisplayTime = 2f; // Time to display the fail message
    private float successDisplayTime = 1f; // Time to display the success message
    public GameObject door; // Reference to the door object to be destroyed
    public AudioClip doorSound;  // 재생할 소리 파일
    private AudioSource audioSource;  // 오디오 소스 컴포넌트
    void Start()
    {
        // Initially hide the success and fail messages
        successText.gameObject.SetActive(false);
        failText.gameObject.SetActive(false);
        // 오디오 소스 컴포넌트를 가져오거나 추가
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    public void OnNumberButtonClick(string number)
    {
        // Add the number to the entered code if less than 6 digits
        if (enteredCode.Length < 6)
        {
            enteredCode += number;
            inputText.text = enteredCode; // Update the input text UI
            Debug.Log("Entered Code: " + enteredCode); // Log for debugging
        }
    }
    public void OnClearButtonClick()
    {
        // Clear the entered code
        enteredCode = "";
        inputText.text = enteredCode;
    }
    public void OnConfirmButtonClick()
    {
        // Check the code if it has 6 digits
        if (enteredCode.Length == 6)
        {
            CheckCode();
        }
    }
    private void CheckCode()
    {
        // Compare the entered code with the correct code
        if (enteredCode == correctCode)
        {
            StartCoroutine(ShowSuccessMessage());
        }
        else
        {
            StartCoroutine(ShowFailMessage());
        }
    }
    private IEnumerator ShowSuccessMessage()
    {
        // Display the success message and hide it after a delay
        inputText.text = "";
        successText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(successDisplayTime); // Use WaitForSecondsRealtime
        successText.gameObject.SetActive(false);
        KeypadUnlocked = true;
        HideKeypad();
        // Destroy the door object
        if (door != null)
        {
            Destroy(door);
            Debug.Log("Door unlocked and destroyed.");
            audioSource.PlayOneShot(doorSound);  // 소리 재생
        }
    }
    private IEnumerator ShowFailMessage()
    {
        // Display the fail message and hide it after a delay
        inputText.text = "";
        failText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(failDisplayTime); // Use WaitForSecondsRealtime
        failText.gameObject.SetActive(false);
        OnClearButtonClick();
    }
    public void ShowKeypad()
    {
        // Show the keypad UI and unlock the cursor
        keypadUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }
    public void HideKeypad()
    {
        // Hide the keypad UI and lock the cursor
        keypadUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        OnClearButtonClick();
        successText.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}