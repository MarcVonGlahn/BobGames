using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement_AudioControl : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] bool muteOnStart = true;
    [SerializeField] AK.Wwise.RTPC speedPitchRTPC;

    private bool _soundIsPlaying = false;
    private bool _isGrounded = false;

    private AkAmbient _ambientObject;
    Rigidbody _rb;


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _ambientObject = GetComponent<AkAmbient>();

        if (muteOnStart)
        {
            _soundIsPlaying = false;
            _ambientObject.data.Stop(gameObject);
        }
    }


    private void OnCollisionStay(Collision other)
    {
        _isGrounded = (other.collider.transform.tag == "Ground") ? true : false;
    }


    void Update()
    {
        if (!_isGrounded)
        {
            if (_soundIsPlaying)
            {
                _soundIsPlaying = false;
                _ambientObject.data.Stop(gameObject);
            }

            return;
        }

        if (Mathf.Abs(_rb.velocity.y) > 0.01)
            return;

        if (_rb.velocity.magnitude > 0.5f)
        {
            if (!_soundIsPlaying)
            {
                _soundIsPlaying = true;
                _ambientObject.data.Post(gameObject);
            }

            speedPitchRTPC.SetValue(gameObject, _rb.velocity.magnitude);
        }
        else
        {
            if (_soundIsPlaying)
            {
                _soundIsPlaying = false;
                _ambientObject.data.Stop(gameObject);
            }
        }
    }
}
