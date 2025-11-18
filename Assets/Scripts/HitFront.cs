using UnityEngine;
using Bhaptics.SDK2;
using UnityEngine.LightTransport;
using Unity.VisualScripting;
public class HitFront : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        BhapticsLibrary.Play(eventId: "car_front_hit");
    }
}
