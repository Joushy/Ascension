using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBehavior : MonoBehaviour
{
    Transform playerTransform;
    private NavMeshAgent aiAgent;

    [SerializeField] float visionRange, visionDistance, roamRadius, walkSpeed, chaseSpeed;
    private bool spotted, startFit, lostLOS;
    private enum eState { idle, roam, chase, ceaseChase, walkBack}
    private eState enemyState = eState.idle;
    private float distanceFromPlayer;
    Vector3 toPlayer, spawnSpot, lastSeenPos;
    

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        spawnSpot = this.transform.position;
        aiAgent = this.GetComponent<NavMeshAgent>();
        startFit = true;
    }

    // Update is called once per frame
    void Update()
    {
        ScoutingForPlayer();
        stateTree();
    }

    private void ScoutingForPlayer()
    {
        toPlayer = playerTransform.position - this.transform.position;
        float sightAngle = Vector3.Angle(this.transform.forward, toPlayer);
        distanceFromPlayer = Vector3.Distance(playerTransform.position, this.transform.position);

        if (distanceFromPlayer <= visionDistance)
        {
            if (sightAngle < visionRange && sightAngle > (visionRange * -1))
            {
                RaycastHit myRay;
                if (Physics.Raycast(transform.position, toPlayer, out myRay))
                {
                    if (myRay.transform.tag == "Player")
                    {
                        Debug.DrawRay(this.transform.position, toPlayer, Color.green);
                        spotted = true;
                        lostLOS = false;
                    }
                    else
                    {
                        Debug.DrawRay(this.transform.position, toPlayer, Color.yellow);
                        if (spotted) { lostLOS = true; lastSeenPos = playerTransform.position; }                       
                    }
                }
            }
            else
            {
                Debug.DrawRay(this.transform.position, toPlayer, Color.red);
                if (spotted) { lostLOS = true; lastSeenPos = playerTransform.position; }
            }
        }
    }

    private void stateTree()
    {
        float dis = Vector3.Distance(spawnSpot, this.transform.position);

        switch (enemyState)
        {
            case eState.idle:
                if (spotted) { enemyState = eState.chase; }
                break;
            case eState.roam:
                break;
            case eState.chase:
                if (dis < roamRadius)
                {
                    aiAgent.speed = chaseSpeed;
                    if (lostLOS) { aiAgent.SetDestination(lastSeenPos); }
                    else { aiAgent.SetDestination(playerTransform.position); }
                    
                }
                else { enemyState = eState.ceaseChase; }
                break;
            case eState.ceaseChase:
                aiAgent.SetDestination(this.transform.position);
                spotted = false;
                if (startFit)
                {
                    startFit = false;
                    StartCoroutine("beAngry");
                }
                break;
            case eState.walkBack:
                startFit = true;
                if (dis > .3)
                {
                    if (spotted) { enemyState = eState.chase; }
                    else { aiAgent.SetDestination(spawnSpot); }
                }
                else { enemyState = eState.idle; }
                break;

        }
    }

    private IEnumerator beAngry()
    {
        Debug.Log("Leave me alone ;.;");
        yield return new WaitForSeconds(2f);
        enemyState = eState.walkBack;
    }

}
