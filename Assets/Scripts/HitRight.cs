using Bhaptics.SDK2;
using UnityEngine;

public class HitRight : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        BhapticsLibrary.Play(eventId: "car_hit_right");
    }
}
