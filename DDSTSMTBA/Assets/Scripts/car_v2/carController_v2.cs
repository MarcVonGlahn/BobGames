// https://docs.unity3d.com/Manual/WheelColliderTutorial.html

using System.Collections;
using UnityEngine;

public class carController_v2 : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float motorTorque = 2000;
    [SerializeField] private float maxSpeed = 20;
    [SerializeField] private float steeringRange = 30;
    [SerializeField] private float steeringRangeAtMaxSpeed = 10;

    [Header("Attack")]
    [SerializeField] private float attackChargeTime = 0.45f;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject attackHitbox;

    private float brakeTorque = 2000;
    private wheelControl_v2[] wheels;
    private Rigidbody rigidBody;

    private bool isAttacking = false;
    private bool isChargingAttack = false;
    private float attackChargeStartTime = -1.0f;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        wheels = GetComponentsInChildren<wheelControl_v2>();
    }

    private void Update()
    {
        // Movement
        float vInput = Input.GetAxis("Vertical");
        float hInput = Input.GetAxis("Horizontal");
        Move(vInput, hInput);

        // Attacking
        if (Input.GetKey(KeyCode.Space))
        {
            ChargeAttack();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            TryAttack();
        }
    }

    private void Move(float vInput, float hInput)
    {
        float forwardSpeed = Vector3.Dot(transform.forward, rigidBody.velocity);
        float speedFactor = Mathf.InverseLerp(0, maxSpeed, forwardSpeed);
        float currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor);
        float currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);
        bool isAccelerating = Mathf.Sign(vInput) == Mathf.Sign(forwardSpeed);

        foreach (var wheel in wheels)
        {
            if (wheel.steerable)
            {
                wheel.WheelCollider.steerAngle = hInput * currentSteerRange;
            }

            if (isAccelerating)
            {
                if (wheel.motorized)
                {
                    wheel.WheelCollider.motorTorque = vInput * currentMotorTorque;
                }
                wheel.WheelCollider.brakeTorque = 0;
            }
            else
            {
                wheel.WheelCollider.brakeTorque = Mathf.Abs(vInput) * brakeTorque;
                wheel.WheelCollider.motorTorque = 0;
            }
        }
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