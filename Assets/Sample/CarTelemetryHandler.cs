using System;
using System.Collections;
using _2DOF;
using LogitechG29.Sample.Input;
using UnityEngine;

public class CarTelemetryHandler : MonoBehaviour
    {
        private const float WAIT_TIME = SendingData.WAIT_TIME / 1000f;
        [Header("G29 Input")]
        [SerializeField] private InputControllerReader _inputControllerReader;

        [SerializeField] private Transform vehicleTransform;
        [SerializeField] private Rigidbody carRigidbody;

        [Header("Platform Tilt Settings")]
        public float accelerationTilt = 8f;
        public float brakeTilt = 10f;
        public float turnTilt = 12f;
        public float maxTiltAngle = 15f;
        public float tiltResponse = 4f;

        [Header("Advanced Tilt Settings")]
        public bool enableAdvancedPhysics = true;
        public float speedBasedTiltMultiplier = 0.5f;
        public float suspensionSoftness = 2f;

        private ObjectTelemetryData _telemetryData;
        private SendingData _sendingData;
        private Vector3 currentTilt;
        private Vector3 targetTilt;
        private float lastSpeed;
        private float currentAcceleration;

        private float smoothSteerInput;
        private float smoothAccelInput;
        private float smoothBrakeInput;

        private void Awake()
        {
            _sendingData = new SendingData();
            _telemetryData = _sendingData.ObjectTelemetryData;

            if (carRigidbody == null)
                carRigidbody = GetComponent<Rigidbody>();
        }

        public void OnEnable()
        {
            StartCoroutine(TelemetryHandler());
            _sendingData.SendingStart();
        }

        public void OnDisable()
        {
            StopCoroutine(TelemetryHandler());
            _sendingData.SendingStop();
        }

        private IEnumerator TelemetryHandler()
        {
            while (true)
            {
                if (_telemetryData == null)
                {
                    yield return new WaitForSeconds(WAIT_TIME * 10f);
                    continue;
                }

                CalculateMovementTilt();
                UpdateTelemetryData();


                yield return new WaitForSeconds(WAIT_TIME);
            }
        }

        private void UpdateTelemetryData()
        {
            currentTilt = Vector3.Lerp(currentTilt, targetTilt, tiltResponse * Time.deltaTime);

            var vehicleEuler = vehicleTransform.eulerAngles;
            vehicleEuler.x = NormalizeAngle(vehicleEuler.x);
            vehicleEuler.y = NormalizeAngle(vehicleEuler.y);
            vehicleEuler.z = NormalizeAngle(vehicleEuler.z);

            Vector3 finalAngles = vehicleEuler + currentTilt;

            finalAngles.x = Mathf.Clamp(finalAngles.x, -maxTiltAngle, maxTiltAngle);
            finalAngles.z = Mathf.Clamp(finalAngles.z, -maxTiltAngle, maxTiltAngle);

            _telemetryData.Angles = finalAngles;
            _telemetryData.Velocity = carRigidbody.linearVelocity;
        }

        private void CalculateMovementTilt()
        {
            targetTilt = Vector3.zero;

            float steerInput = 0f;
            float accelInput = 0f;
            float brakeInput = 0f;

            if (_inputControllerReader != null)
            {
                steerInput = _inputControllerReader.Steering;
                accelInput = _inputControllerReader.Throttle;
                brakeInput = _inputControllerReader.Brake;
            }
            else
            {
                steerInput = Input.GetAxis("Horizontal");
                accelInput = Input.GetKey(KeyCode.W) ? 1f : 0f;
                brakeInput = (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.Space)) ? 1f : 0f;
            }

            smoothSteerInput = Mathf.Lerp(smoothSteerInput, steerInput, Time.deltaTime * 5f);
            smoothAccelInput = Mathf.Lerp(smoothAccelInput, accelInput, Time.deltaTime * 4f);
            smoothBrakeInput = Mathf.Lerp(smoothBrakeInput, brakeInput, Time.deltaTime * 6f);

            float currentSpeed = carRigidbody.linearVelocity.magnitude;
            currentAcceleration = (currentSpeed - lastSpeed) / Time.deltaTime;
            lastSpeed = currentSpeed;

            if (enableAdvancedPhysics)
            {
                CalculatePhysicsBasedTilt(currentSpeed, currentAcceleration);
            }
            else
            {
                CalculateSimpleTilt(currentSpeed);
            }
        }

        private void CalculateSimpleTilt(float currentSpeed)
        {
            if (smoothAccelInput > 0.1f)
            {
                float accelFactor = smoothAccelInput * Mathf.Clamp01(currentSpeed / 5f);
                targetTilt.x += accelerationTilt * accelFactor;
            }

            if (smoothBrakeInput > 0.1f)
            {
                targetTilt.x -= brakeTilt * smoothBrakeInput;
            }

            if (Mathf.Abs(smoothSteerInput) > 0.1f)
            {
                float speedFactor = Mathf.Clamp01(currentSpeed / 8f);
                targetTilt.z = -smoothSteerInput * turnTilt * speedFactor;
            }
        }

        private void CalculatePhysicsBasedTilt(float currentSpeed, float acceleration)
        {
            CarControllerSample carController = GetComponent<CarControllerSample>();

            if (Mathf.Abs(acceleration) > 0.1f)
            {
                float accelerationTiltAmount = Mathf.Clamp(acceleration / 10f, -1f, 1f);

                targetTilt.x = accelerationTiltAmount * (acceleration > 0 ? accelerationTilt : brakeTilt);
                targetTilt.x = -targetTilt.x; // Коррекция знака
            }

            if (Mathf.Abs(smoothSteerInput) > 0.1f && currentSpeed > 1f)
            {
                float turnIntensity = Mathf.Abs(smoothSteerInput) * currentSpeed * speedBasedTiltMultiplier;
                float rollAmount = -Mathf.Sign(smoothSteerInput) * Mathf.Clamp(turnIntensity, 0, turnTilt);

                targetTilt.z = Mathf.Lerp(targetTilt.z, rollAmount, Time.deltaTime * suspensionSoftness);
            }

            if (currentSpeed > 10f)
            {
                float vibration = Mathf.PerlinNoise(Time.time * 10f, 0) * 0.3f;
                targetTilt.x += vibration;
            }
        }

        private float NormalizeAngle(float angle)
        {
            angle %= 360f;
            if (angle > 180f) angle -= 360f;
            if (angle < -180f) angle += 360f;
            return angle;
        }
    }
