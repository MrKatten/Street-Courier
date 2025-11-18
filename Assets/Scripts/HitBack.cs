using Bhaptics.SDK2;
using UnityEngine;

public class HitBack : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        BhapticsLibrary.Play(eventId: "car_hit_back");
    }
}
