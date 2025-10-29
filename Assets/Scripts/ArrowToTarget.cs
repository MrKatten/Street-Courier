using UnityEngine;

public class ArrowToTarget : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] public Transform target; // Цель, на которую указывает стрелка

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 5f; // Скорость вращения

    private void Update()
    {
        if (target != null)
        {
            PointToTarget();
        }
    }

    // Основной метод для поворота стрелки к цели
    public void PointToTarget()
    {
        Vector3 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, angle, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
