using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SaveBed : MonoBehaviour
{
    private bool isPlayerInRange;
    private bool isTimeStopped = false;
    public GameObject interactionUI;
    public GameObject saveUI;
    public SaveManager saveManager;

    void Start()
    {
        saveUI.SetActive(false);
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !isTimeStopped)
        {
            saveUI.SetActive(true);
            isTimeStopped = true;
            Time.timeScale = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("들어옴");
            isPlayerInRange = true;
            if (interactionUI != null)
            {
                interactionUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (interactionUI != null)
            {
                interactionUI.SetActive(false);
            }
        }
    }

    public void SaveClicked()
    {
        saveManager.Save();
        saveUI.SetActive(false);
        Time.timeScale = 1f; // 시간 재개
        isTimeStopped = false;
    }

    public void notSaveClicked()
    {
        saveUI.SetActive(false);
        Time.timeScale = 1f; // 시간 재개
        isTimeStopped = false;
    }
}
