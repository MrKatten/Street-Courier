//#region

//using System;
//using System.Collections.Generic;
//using LogitechG29.Sample.Input;
//using UnityEngine;

//#endregion

//public class CarControllerSample : MonoBehaviour
//{
//    [SerializeField] private InputControllerReader inputControllerReader;
//    [SerializeField] private List<AxleInfo> axleInfos; // информация о каждой отдельной оси

//    [SerializeField]
//    private float maxMotorTorque; // максимальный крутящий момент, который двигатель может приложить к колесу

//    [SerializeField] private float maxSteeringAngle; // максимальный угол поворота, который может иметь колесо

//    public void FixedUpdate()
//    {
//        var speed = 0f;
//        if (inputControllerReader.Throttle != 0)
//        {
//            speed = inputControllerReader.Throttle;
//        }
//        else if (inputControllerReader.Brake != 0)
//        {
//            speed = -inputControllerReader.Brake;
//        }

//        var motor = maxMotorTorque * speed;
//        var steering = maxSteeringAngle * inputControllerReader.Steering;

//        foreach (var axleInfo in axleInfos)
//        {
//            if (axleInfo.steering)
//            {
//                axleInfo.leftWheel.steerAngle = steering;
//                axleInfo.rightWheel.steerAngle = steering;
//            }

//            if (axleInfo.motor)
//            {
//                axleInfo.leftWheel.motorTorque = motor;
//                axleInfo.rightWheel.motorTorque = motor;
//            }
//        }
//    }

//    [Serializable]
//    public class AxleInfo
//    {
//        public WheelCollider leftWheel;
//        public WheelCollider rightWheel;
//        public bool motor; // это колесо прикреплено к мотору?
//        public bool steering; // применяет ли это колесо угол поворота?
//    }
//}
#region

using System;
using System.Collections.Generic;
using LogitechG29.Sample.Input;
using UnityEngine;
using TMPro;
using System.Media;

#endregion

public class CarControllerSample : MonoBehaviour
{
    [SerializeField] private InputControllerReader inputControllerReader;
    [SerializeField] private List<AxleInfo> axleInfos;
    [SerializeField] private float maxMotorTorque = 300f;
    [SerializeField] private float maxSteeringAngle = 30f;
    [SerializeField] private float maxBrakeTorque = 500f;
    [SerializeField] private TMP_Text _speedText;
    [SerializeField] private TMP_Text _gearText;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Boolean sound;

    [Header("Gear Settings")]
    [SerializeField] private float[] gearMaxSpeeds = { 20f, 40f, 60f, 80f, 100f, 120f, 15f }; // Максимальные скорости для каждой передачи (последняя - задняя)

    private int currentGear = 0; // 0-5 - передние, 6 - задняя
    private Rigidbody carRigidbody;

    private void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        HandleGearShift();
        ApplyForcesToWheels();
    }

    private void HandleGearShift()
    {
        // Проверяем команды переключения передач
        for (int i = 0; i < 7; i++)
        {
            if (GetGearInput(i))
            {
                currentGear = i;
                break;
            }
        }
    }

    private bool GetGearInput(int gear)
    {
        switch (gear)
        {
            case 0: return inputControllerReader.Shifter1;
            case 1: return inputControllerReader.Shifter2;
            case 2: return inputControllerReader.Shifter3;
            case 3: return inputControllerReader.Shifter4;
            case 4: return inputControllerReader.Shifter5;
            case 5: return inputControllerReader.Shifter6;
            case 6: return inputControllerReader.Shifter7; // Задняя передача
            default: return false;
        }
    }

    private void ApplyForcesToWheels()
    {
        float brakeInput = 0f;
        float motorInput = 0f;
        float currentMaxSpeed = gearMaxSpeeds[currentGear];

        // Получаем текущую скорость в км/ч
        float currentSpeed = carRigidbody.linearVelocity.magnitude * 3.6f;

        // Ограничиваем скорость в зависимости от передачи
        if (currentSpeed < currentMaxSpeed)
        {
            if (currentGear == 6) // Задняя передача
            {
                motorInput = -inputControllerReader.Throttle; // Газ для движения назад
            }
            else // Передние передачи
            {
                motorInput = inputControllerReader.Throttle;
            }
        }
        if ((int)currentSpeed > 0)
        {
            if (sound)
            {
                audioSource.Play();
                sound = false;
            }
        }
        else if ((int)currentSpeed <= 0)
        {
            sound = true;
            audioSource.Stop();
        }
        _speedText.text = Convert.ToString((int)currentSpeed) + " КМ/Ч";
        int gear = GetCurrentGear();
        if (gear < 6)
        {
            _gearText.text = Convert.ToString(gear + 1);
        }
        else if (gear == 6)
        {
            _gearText.text = "R";
        }
        brakeInput = inputControllerReader.Brake;

        var brake = maxBrakeTorque * brakeInput;

        var motor = maxMotorTorque * motorInput;
        var steering = maxSteeringAngle * inputControllerReader.Steering;

        foreach (var axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }

            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
            axleInfo.leftWheel.brakeTorque = brake;
            axleInfo.rightWheel.brakeTorque = brake;
        }
    }

    public int GetCurrentGear()
    {
        return currentGear;
    }

    public float GetCurrentMaxSpeed()
    {
        return gearMaxSpeeds[currentGear];
    }

    [Serializable]
    public class AxleInfo
    {
        public WheelCollider leftWheel;
        public WheelCollider rightWheel;
        public bool motor;
        public bool steering;
    }
}