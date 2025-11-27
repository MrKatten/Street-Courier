using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Bhaptics.SDK2;

public class DeliveryEndManager : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] public GameObject _deliveryPointStart;
    [SerializeField] public GameObject _deliveryPointEnd;
    [SerializeField] public GameObject _boxes;
    [SerializeField] public GameObject _person;
    [SerializeField] public ArrowToTarget _arrow;
    [SerializeField] public TMP_Text _text;
    [SerializeField] public int _count = 0;
    
    [Header("Transform")]
    [SerializeField] public Transform _deliveryPointStartTransform;
    [SerializeField] public Transform _deliveryPointEndTransform;
    
    public void Person(GameObject person)
    {
        _person = person;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            _count++;
            _text.text = "Доставлено: ";
            _text.text += _count.ToString() + "/ 5";
            BhapticsLibrary.Play(eventId: "delivery");
            if (_count >= 5)
            {
                SceneManager.LoadScene("MainMenu");
            }
            else
            {
                _deliveryPointStartTransform.position = new Vector3(_deliveryPointStartTransform.position.x, (float)-0.08, _deliveryPointStartTransform.position.z);
                _deliveryPointEndTransform.position = new Vector3(0, -5, 0);
                _boxes.SetActive(false);
                _arrow.target = _deliveryPointStartTransform;
                _person.transform.position = new Vector3(0, -10, 0);
            }
        }
    }
}
