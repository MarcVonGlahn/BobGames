using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.Splines;
using UnityEngine.AI;

public class AI_Car : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent _agent;
    private SplineAnimate splineAnimation;

    public ChasingState chasingState = ChasingState.None;

    private Vector3 _startPos;

    public Transform target;

    public float speed;

    public float initalPeace = 2.0f;

    public bool inAction = true;
    public bool goingToStart;
    private bool _samplePosition = true;

    void Awake()
    {
        _agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        _agent.speed = speed;
        splineAnimation = GetComponent<SplineAnimate>();

        StartCoroutine(InitialPeaceWait());
    }

    private IEnumerator InitialPeaceWait()
    {
        yield return new WaitForSeconds(initalPeace);

        inAction = false;
    }

    // trigger spline animate somewhere

    // Update is called once per frame
    void Update()
    {
        if (chasingState == ChasingState.Chaser)
        {
            if (target == null)
            {
                chasingState = ChasingState.None;
                inAction = false;
                goingToStart = true;
                _agent.SetDestination(_startPos);
                return;
            }
            else
            {
                _agent.SetDestination(target.position);
            }
        }
        
        if (goingToStart && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            goingToStart = false;
            splineAnimation.Restart(true);
        }

        // vary speed in curves and shit
        // set agent destination
    }

    private void LateUpdate()
    {
        if (_samplePosition)
        {
            _startPos = transform.position;
            _samplePosition = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            if (other.GetComponent<AI_Car>().inAction)
                return;

            if (inAction)
                return;

            other.GetComponent<AI_Car>().chasingState = ChasingState.BeingChase;
            chasingState = ChasingState.Chaser;
            target = other.transform;
            inAction = true;
            GetComponent<SplineAnimate>().Pause();

            // let them fight
            // or one run away from the other?
            // how?
        }
        else if (other.CompareTag("Player"))
        {
            chasingState = ChasingState.Chaser;
            target = other.transform;
            inAction = true;
        }
    }
}

public enum ChasingState
{
    Chaser,
    BeingChase,
    None
}