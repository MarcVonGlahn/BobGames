using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_EngineSound : MonoBehaviour
{
    public bool isNPC = false;
    public AK.Wwise.RTPC enginePitchRTPC;
    public AK.Wwise.RTPC engineVolumeRTPC;

    CarController _carController;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        if(isNPC)
        {
            engineVolumeRTPC.SetValue(gameObject, 20f);
        }
    }

    private void Update()
    {
        enginePitchRTPC.SetValue(gameObject, _rb.velocity.magnitude);
    }
}
