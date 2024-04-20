using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlapTrigger : MonoBehaviour
{
    public float triggerTime = 1f;

    private bool _inTriggerZone;

    private GameObject hitTarget;

    public AI_Car aiCar;

    private void OnTriggerEnter(Collider other)
    {
        if (!aiCar.inAction)
            return;

        if (!other.CompareTag("NPC") && !other.CompareTag("Player"))
            return;

        hitTarget = other.gameObject;
        StartCoroutine(ChargeSlap());
        _inTriggerZone = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _inTriggerZone = false;
    }

    private IEnumerator ChargeSlap()
    {
        yield return new WaitForSeconds(triggerTime);
        
        // trigger slap animation

        if (_inTriggerZone)
        {
            Debug.Log("SLAPPPP");
            Destroy(hitTarget);
        }
    }
}
