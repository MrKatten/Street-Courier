using LogitechG29.Sample.Input;
using Unity.VisualScripting;
using UnityEngine;

public class CarMenu : MonoBehaviour
{
    [SerializeField] private InputControllerReader inputControllerReader;
    [SerializeField] private GameObject Car;
    [SerializeField] private AudioSource audioSource;
    private void Update()
    {
        if (inputControllerReader.LeftStickButton)
        {
            CrushButton();
        }
    }
    public void CrushButton()
    {
        audioSource.Play();
        Car.transform.position = new Vector3(Car.transform.position.x, Car.transform.position.y + 1/10, Car.transform.position.z);
        Car.transform.rotation = new Quaternion(0, Car.transform.rotation.y, 0, Car.transform.rotation.w);
    }
}
