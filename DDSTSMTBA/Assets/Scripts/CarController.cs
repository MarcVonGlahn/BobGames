using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CarController : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;

    private bool isAttacking = false;
    private bool isChargingAttack = false;
    private float attackChargeStartTime = -1.0f;

    // Settings
    [SerializeField] private float motorForce, breakForce, maxSteerAngle;

    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    // BaseCar
    [SerializeField] private BaseCar _baseCar;

    [Header("Attack")]
    [SerializeField] private float attackChargeTime = 0.24f;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject attackHitbox;
    [SerializeField] private string npcTag = "NPC";
       
    public bool EXPLODE;

    [Header("Sound")]
    public AK.Wwise.Event brakingEvent;
    public AkAmbient breakingSoundAmbient;

    private void Start()
    {
        attackHitbox.SetActive(false);

        breakingSoundAmbient = gameObject.AddComponent<AkAmbient>();
        breakingSoundAmbient.data = brakingEvent;
        breakingSoundAmbient.triggerList[0] = 0;
    }

    private void Update()
    {
        if (EXPLODE)
        {
            StartCoroutine(_baseCar.Explode());
            this.enabled = false;
        }

        GetInput();
    }

    private void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput()
    {
        // Steering Input
        horizontalInput = Input.GetAxis("Horizontal");

        // Acceleration Input
        verticalInput = Input.GetAxis("Vertical");

        // Breaking Input
        isBreaking = Input.GetKey(KeyCode.E);

        if (Input.GetKeyDown(KeyCode.E))
        {
            brakingEvent.Post(gameObject);
        }


        // Attack Input
        if (Input.GetKey(KeyCode.Space))
        {
            ChargeAttack();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            TryAttack();
        }
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }


    private void ChargeAttack()
    {
        if (!isChargingAttack)
        {
            attackChargeStartTime = Time.time;
            isChargingAttack = true;
            // TODO play charge sound

            animator.SetBool("isCharging", true);
        }
        else if (isAttacking == true)
        {
            isAttacking = false;
        }
    }

    private void TryAttack()
    {
        if (isAttacking)
            return;

        isAttacking = true;
        StartCoroutine(DoAttack());
    }

    private IEnumerator DoAttack()
    {
        float curAttackChargeTime = Time.time - attackChargeStartTime;

        while (curAttackChargeTime < attackChargeTime)
        {
            if (isAttacking == false)
            {
                yield break;
            }

            curAttackChargeTime += Time.deltaTime;

            yield return null;
        }

        if (isAttacking)
        {
            animator.SetBool("isAttacking", true);
            attackHitbox.SetActive(true);
            // TODO play attack sound

            yield return new WaitForSeconds(0.1f);

            isAttacking = false;
            isChargingAttack = false;

            attackHitbox.SetActive(false);
            animator.SetBool("isAttacking", false);
            animator.SetBool("isCharging", false);
            // TODO play idle anim 
        }
    }
}
