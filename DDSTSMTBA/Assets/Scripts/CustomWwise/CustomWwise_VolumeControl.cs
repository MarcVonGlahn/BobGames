using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomWwise_VolumeControl : MonoBehaviour
{
    [SerializeField] AK.Wwise.RTPC backgroundMusicVolume_RTPC;
    [SerializeField][Range(0, 100)] float bgMusicVolume = 100;

    public AK.Wwise.Event stopBackgroundMusicEvent;
    public AK.Wwise.Event playWinningSoundEvent;
    public AK.Wwise.Event playLosingSoundEvent;

    private AkAmbient _akAmbient;
    private AkAmbient _tempGameEndAkAmbient;

    private void Start()
    {
        _akAmbient = GetComponent<AkAmbient>();
    }

    public void PlayGameEndSound(bool hasWon = true)
    {
        stopBackgroundMusicEvent.Post(gameObject);

        _tempGameEndAkAmbient = gameObject.AddComponent<AkAmbient>();

        if(hasWon)
        {
            _tempGameEndAkAmbient.data = playWinningSoundEvent;
            playWinningSoundEvent.Post(gameObject);
        }
        else
        {
            _tempGameEndAkAmbient.data = playLosingSoundEvent;
            playLosingSoundEvent.Post(gameObject);
        }
    }

}
