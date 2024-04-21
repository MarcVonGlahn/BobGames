using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlapTrigger : MonoBehaviour
{
    public float triggerTime = 0.1f;

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
        StopAllCoroutines();
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
            try
            {
                hitTarget.GetComponent<AI_Car>().TakeHit();
            }
            catch
            {

            }
            Destroy(hitTarget);
        }
    }
}
