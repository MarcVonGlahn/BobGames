using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.Splines;

public class AI_Car : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent _agent;
    private SplineAnimate splineAnimation;

    public ChasingState chasingState = ChasingState.None;

    [SerializeField] private GameObject explosionParticles;

    private Vector3 _startPos;

    public Transform target;

    public float initalPeace = 2.0f;

    public bool inAction = true;
    public bool goingToStart;
    private bool _samplePosition = true;

    public bool EXPLODE;
    public bool isChasingPlayer;

    public BaseCar baseCar;

    public bool isDead;

    void Awake()
    {
        _agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        splineAnimation = GetComponent<SplineAnimate>();

        StartCoroutine(InitialPeaceWait());
    }

    private void FixedUpdate()
    {
        if (EXPLODE)
        {
            StartCoroutine(baseCar.Explode());
            this.enabled = false;
        }
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
        if (isDead)
            return;

        if (chasingState == ChasingState.Chaser)
        {
            splineAnimation.Pause();
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
            // 80% chance they dont fight
            if (true)
            //if (Random.Range(0.0f,1.0f) < 0.8f)
            {
                return;
            }

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
            try
            {
                if (other.transform.GetComponentInParent<carController_v2>().isChased)
                    return;
            }
            catch
            {
                return;
            }


            other.transform.GetComponentInParent<carController_v2>().isChased = true;

            isChasingPlayer = true;
            chasingState = ChasingState.Chaser;
            target = other.transform;
            inAction = true;
            splineAnimation.Pause();
        }
    }

    public void TakeHit()
    {
        GameObject explosionOfDoom = Instantiate(explosionParticles, transform.position, transform.rotation);
        explosionOfDoom.transform.localScale *= 2.0f;
        explosionOfDoom.transform.parent = transform;

        if (isChasingPlayer)
        {
            target.GetComponentInParent<carController_v2>().isChased = false;
        }

        isDead = true;

        baseCar.Explode();
        GetComponent<SplineAnimate>().Pause();

        // GetComponent<WwiseAudio_PlaySecret>().PlaySecret();
        // explode here
    }
}

public enum ChasingState
{
    Chaser,
    BeingChase,
    None
}