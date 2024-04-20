using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public string npcTag = "NPC";
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        if(other.gameObject.tag == npcTag)
        {
            other.gameObject.GetComponent<AI_Car>().TakeHit();
            other.gameObject.GetComponent<WwiseAudio_PlaySecret>().PlaySecret();
        }
    }
}
