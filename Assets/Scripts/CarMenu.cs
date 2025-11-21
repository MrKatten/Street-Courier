using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarMenu : MonoBehaviour
{
    [SerializeField] private GameObject Car;
    [SerializeField] private AudioSource audioSource;
    public void CrushButton()
    {
        Car.transform.position = new Vector3(Car.transform.position.x, Car.transform.position.y + 1, Car.transform.position.z);
        Car.transform.rotation = new Quaternion(0, 0, 0, 0);
        audioSource.Play();

    }
    public void ExitToMainMenu()
    {
        audioSource.Play();
        SceneManager.UnloadSceneAsync("Game");
        SceneManager.LoadScene("MainMenu");
    }
}
