using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PlayButton()
    {
        SceneManager.LoadScene("Game");
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
