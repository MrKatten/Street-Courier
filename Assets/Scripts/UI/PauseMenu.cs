using LogitechG29.Sample.Input;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Menu UI")]
    [SerializeField] private InputControllerReader inputControllerReader;
    public GameObject pauseMenu;

    [Header("Buttons")]
    public Button resumeButton;
    public Button restartButton;
    public Button quitButton;

    private bool isPaused = false;
    private bool _buttonPressed = false;
    private bool _actionPressed = false;

    void Start()
    {
        pauseMenu.SetActive(false);

        // Настраиваем кнопки
        if (resumeButton != null)
            resumeButton.onClick.AddListener(ResumeGame);
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
    }

    void Update()
    {
        if (inputControllerReader == null) return;

        // Обработка кнопки паузы
        HandlePauseButton();

        // Обработка действий в меню паузы
        if (isPaused)
        {
            HandleMenuActions();
        }
    }

    private void HandlePauseButton()
    {
        if (inputControllerReader.NorthButton && !_buttonPressed)
        {
            _buttonPressed = true;
            TogglePause();
        }
        else if (!inputControllerReader.NorthButton)
        {
            _buttonPressed = false;
        }
    }

    private void HandleMenuActions()
    {
        // Кнопка возобновления (East Button)
        if (inputControllerReader.EastButton && !_actionPressed)
        {
            _actionPressed = true;
            ResumeGame();
        }
        // Кнопка перезапуска (South Button)
        else if (inputControllerReader.SouthButton && !_actionPressed)
        {
            _actionPressed = true;
            RestartGame();
        }
        // Кнопка выхода (West Button)
        else if (inputControllerReader.WestButton && !_actionPressed)
        {
            _actionPressed = true;
            QuitGame();
        }
        else if (!inputControllerReader.EastButton &&
                 !inputControllerReader.SouthButton &&
                 !inputControllerReader.WestButton)
        {
            _actionPressed = false;
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        _actionPressed = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
        
    private void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }

    private void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}