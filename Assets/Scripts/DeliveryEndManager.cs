using UnityEngine;

public class DeliveryEndManager : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] public GameObject _deliveryPointStart;
    [SerializeField] public GameObject _deliveryPointEnd;
    [SerializeField] public GameObject _boxes;
    [SerializeField] public GameObject _person;
    [SerializeField] public ArrowToTarget _arrow;
    
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
            _deliveryPointStartTransform.position = new Vector3(_deliveryPointStartTransform.position.x, (float)-0.08, _deliveryPointStartTransform.position.z);
            _deliveryPointEndTransform.position = new Vector3(0, -5, 0);
            _boxes.SetActive(false);
            _arrow.target = _deliveryPointStartTransform;
            _person.transform.position = new Vector3(0, -10, 0);
        }
    }
}
