using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PlayButton()
    {
        audioSource.Play();
        SceneManager.LoadScene("Game");
    }

    public void ExitButton()
    {
        audioSource.Play();
        Application.Quit();
    }
}
