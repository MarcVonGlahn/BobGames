using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Animator animator;

    public void DoWinAnimation()
    {
        animator.SetTrigger("Win_Trigger");
    }


    public void DoLoseAnimation()
    {
        animator.SetTrigger("Lose_Trigger");
    }
}
