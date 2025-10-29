using UnityEngine;
using System.Collections.Generic;

public class DeliveryStartManager : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] public GameObject _deliveryPointStart;
    [SerializeField] public GameObject _deliveryPointEnd;
    [SerializeField] public GameObject _boxes;
    [SerializeField] public GameObject[] _people;
    [SerializeField] public ArrowToTarget _arrow;
    [SerializeField] public DeliveryEndManager _endManager;

    [Header("Transform")]
    [SerializeField] public Transform _deliveryPointStartTransform;
    [SerializeField] public Transform _deliveryPointEndTransform;
    Vector3[] points = new Vector3[]
        {
            new Vector3(110,(float)-0.08,(float)-4.24),
            new Vector3(128,(float)-0.08,(float)29.2000008),
            new Vector3((float) 87.75,(float)-0.08,(float)14.1000004),
            new Vector3((float) 73.2099991,(float)-0.08,(float)-1.52999997),
            new Vector3((float) 52.0999985,(float)-0.08,(float)-37),
            new Vector3((float) 25.0100002,(float)-0.08,(float)48.0499992),
            new Vector3((float) 1.19000006,(float)-0.08,(float)29.3299999),
            new Vector3((float) 110.75,(float)-0.08,(float)48.0099983),
            new Vector3((float) 127.690002,(float)-0.08,(float)5.4000001),
            new Vector3((float) 65.3000031,(float)-0.08,(float)47.9129982),
            new Vector3((float) 41.2799988,(float)-0.08,(float)-24.7999992)
        };
    Vector3[] _peoplePositions = new Vector3[]
        {
            new Vector3((float)110.071999,(float)0.200000003,(float)-6.61999989), //0
            new Vector3((float)129.990005,(float)0.225999996,(float)29.2299995), //-90
            new Vector3((float)85.4150009,(float)0.204999998,(float)14.1000004), //90
            new Vector3((float)73.2099991,(float)0.206,(float)0.889999986), //180
            new Vector3((float)52.0099983,(float)0.25,(float)-34.8100014), //180
            new Vector3((float)25.0100002,(float)0.319999993,(float)50.3800011), //180
            new Vector3((float)-1.03999996,(float)0.310000002,(float)29.3299999), //90
            new Vector3((float)110.739998,(float)0.230000004,(float)50.25), //180
            new Vector3((float)129.970001,(float)0.200000003,(float)5.46000004), //-90
            new Vector3((float)65.2900009,(float)0.310000002,(float)50.8199997), //180
            new Vector3((float)43.5470009,(float)0.303000003,(float)-24.8700008) //-90
        };
    void Start()
    {
        _boxes.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            _deliveryPointStartTransform.position = new Vector3(_deliveryPointStartTransform.position.x, -3, _deliveryPointStartTransform.position.z);
            int randomRangeZone = Random.Range(0, points.Length);
            _deliveryPointEndTransform.position = points[randomRangeZone];
            int randomRangePerson = Random.Range(0, _people.Length);
            _people[randomRangePerson].transform.position = _peoplePositions[randomRangeZone];
            Debug.Log(randomRangePerson);
            if (randomRangeZone == 0)
            {
                _people[randomRangeZone].transform.eulerAngles = new Vector3(0, 0, 0);
                Debug.Log(0);
            }
            else if (randomRangeZone == 1 || randomRangeZone == 8 || randomRangeZone == 10)
            {
                _people[randomRangePerson].transform.eulerAngles = new Vector3(0, -90, 0);
                Debug.Log(-90);
            }
            else if (randomRangeZone == 2 || randomRangeZone == 6)
            {
                _people[randomRangePerson].transform.eulerAngles = new Vector3(0, 90, 0); ;
                Debug.Log(90);
            }
            else if (randomRangeZone == 3 || randomRangeZone == 4 || randomRangeZone == 5 || randomRangeZone == 7 || randomRangeZone == 9)
            {
                _people[randomRangePerson].transform.eulerAngles = new Vector3(0, 180, 0);
            }
            _boxes.SetActive(true);
            _arrow.target = _deliveryPointEndTransform;
            _endManager.Person(_people[randomRangePerson]);
        }
    }
}
