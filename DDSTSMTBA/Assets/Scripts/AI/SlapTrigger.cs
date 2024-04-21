using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlapTrigger : MonoBehaviour
{
    public float triggerTime = 0.5f;

    private bool _inTriggerZone;

    private GameObject hitTarget;

    public AI_Car aiCar;

    public Animator slapAnimator;

    private void OnTriggerEnter(Collider other)
    {
        if (!aiCar.inAction)
            return;

        if (!other.CompareTag("NPC") && !other.CompareTag("Player"))
            return;

        hitTarget = other.gameObject;
        StopAllCoroutines();

        slapAnimator.SetBool("isCharging", true);

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
        slapAnimator.SetBool("isAttacking", true);

        yield return new WaitForSeconds(0.1f);

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

        slapAnimator.SetBool("isAttacking", false);
        slapAnimator.SetBool("isCharging", false);
    }
}
