using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_EngineSound : MonoBehaviour
{
    public bool isNPC = false;
    public AK.Wwise.RTPC enginePitchRTPC;

    CarController _carController;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        enginePitchRTPC.SetValue(gameObject, _rb.velocity.magnitude);
    }
}
