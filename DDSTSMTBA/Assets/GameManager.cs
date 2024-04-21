using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<AI_Car> cars;

    public UIManager uiManager;

    public carController_v2 player;

    private bool lockGameEnd = false;

    // Update is called once per frame
    void Update()
    {
        bool carsLeft = false;

        foreach(AI_Car car in cars)
        {
            if (!car.isDead)
            {
                carsLeft = true;
            }
        }

        if (!carsLeft)
        {
            if (!lockGameEnd)
            {
                uiManager.DoWinAnimation();
                lockGameEnd = true;
            }
        }

        if (player.isDead)
        {
            if (!lockGameEnd)
            {
                uiManager.DoLoseAnimation();
                lockGameEnd = true;
            }
        }
    }
}
