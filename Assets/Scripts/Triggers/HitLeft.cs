using UnityEngine;
using Bhaptics.SDK2;
public class HitLeft : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        BhapticsLibrary.Play(eventId: "car_hit_left");
    }
}