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

    [SerializeField] private float attackChargeTime = 0.24f;

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
    public float maxAttackCharge = 3f;
    public float currentAttackCharge = 0f;
    public GameObject attackHitbox;
    public string npcTag = "NPC";

    public bool EXPLODE;

    private void Start()
    {
        attackHitbox.SetActive(false);
    }

    private void Update()
    {
        if (EXPLODE)
        {
            StartCoroutine(_baseCar.Explode());
            this.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        HandleAttack();
    }

    private void GetInput()
    {
        // Steering Input
        horizontalInput = Input.GetAxis("Horizontal");

        // Acceleration Input
        verticalInput = Input.GetAxis("Vertical");

        // Breaking Input
        isBreaking = Input.GetKey(KeyCode.E);

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
        if (isAttacking || isChargingAttack)
            return;

        attackChargeStartTime = Time.time;
        isChargingAttack = true;
        Debug.Log("charging");
        // TODO start charge anim
        //      play charge sound
    }

    private void TryAttack()
    {
        float curAttackChargeTime = Time.time - attackChargeStartTime;

        if (curAttackChargeTime < attackChargeTime)
            return;

        isChargingAttack = false;
        isAttacking = true;
        Debug.Log("attacking");
        // TODO activate hit collider
        //      play attack anim
        //      play attack sound

        StartCoroutine(EndAttack());
    }

    private IEnumerator EndAttack()
    {
        yield return new WaitForSeconds(0.1f);

        isAttacking = false;
        Debug.Log("end attack");
        // TODO deactivate hit collider
        //      play idle anim
    }

    private void HandleAttack()
    {
        if (!isAttacking)
        {
            if(currentAttackCharge > 0f)
            {
                DoAttack();
                return;
            }
            return;
        }

        if(currentAttackCharge < maxAttackCharge)
        {
            currentAttackCharge += Time.deltaTime;
        }

        isAttacking = false;
    }

    private void DoAttack()
    {
        // Do Attack with current charge
        StartCoroutine(DoAttack_Routine());

        // Reset Attack Charge
        currentAttackCharge = 0f;
    }


    private IEnumerator DoAttack_Routine()
    {
        attackHitbox.SetActive(true);
        yield return new WaitForEndOfFrame();

        attackHitbox.SetActive(false);
    }
}
