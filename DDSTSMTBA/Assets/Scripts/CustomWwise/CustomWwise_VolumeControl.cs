using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomWwise_VolumeControl : MonoBehaviour
{
    [SerializeField] AK.Wwise.RTPC backgroundMusicVolume_RTPC;
    [SerializeField][Range(0, 100)] float bgMusicVolume = 100;

    private void Update()
    {
        backgroundMusicVolume_RTPC.SetValue(gameObject, bgMusicVolume);
    }
}
