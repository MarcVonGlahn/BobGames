using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public string npcTag = "NPC";

    public AK.Wwise.Event hitEvent;
    public AK.Wwise.Event missEvent;

    private AkAmbient ambient;
    private AkGameObj akGO;

    private bool _hasHitSomething;

    private void Start()
    {
        akGO = gameObject.AddComponent<AkGameObj>();

        akGO.isEnvironmentAware = false;

        ambient = gameObject.AddComponent<AkAmbient>();
    }

    private void OnDisable()
    {
        if(_hasHitSomething) // Has hit something
        {
            ambient.data = hitEvent;
            hitEvent.Post(gameObject);
        }
        else // has not hit something
        {
            ambient.data = missEvent;
            missEvent.Post(gameObject);
        }

        _hasHitSomething = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        if(other.gameObject.tag == npcTag)
        {
            other.gameObject.GetComponent<AI_Car>().TakeHit();
            other.gameObject.GetComponent<WwiseAudio_PlaySecret>().PlaySecret();

            _hasHitSomething = true;
        }
    }
}
