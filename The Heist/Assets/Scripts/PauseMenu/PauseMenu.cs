using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    public GameObject pauseMenu;
    [SerializeField] 
    private CinemachineBrain playerCamera;
    public static bool bIsPaused;

    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    
    private void OnEnable()
    {
        InputManager.Player.PauseMenu.performed += onPauseMenu;
    }
    private void OnDisable()
    {
        InputManager.Player.PauseMenu.performed -= onPauseMenu;
    }

    private void onPauseMenu(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (bIsPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1;
        bIsPaused = false;
        SceneManager.LoadScene("MenuScene");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        playerCamera.GetComponent<CinemachineBrain>().enabled = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        bIsPaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        playerCamera.GetComponent<CinemachineBrain>().enabled = false;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        bIsPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        playerCamera.GetComponent<CinemachineBrain>().enabled = true;
    }
}
