using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseAudio_PlaySecret : MonoBehaviour
{
    public AK.Wwise.Event playSecretEvent;

    private AkAmbient _ambient;

    private void Start()
    {
        _ambient = GetComponent<AkAmbient>();

        _ambient.data = playSecretEvent;
    }

    public void PlaySecret()
    {
        Debug.Log(gameObject.name);

        _ambient.data.Post(gameObject);
    }
}
