using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent),typeof(CapsuleCollider))]
public class EnemyAI : MonoBehaviour
{
    NavMeshAgent                    agent;

    [SerializeField] float          health = 100f;
    bool                            isDead = false;

    [SerializeField] Transform      player;
    [SerializeField] bool           playerSpotted = false;

    //attack vars
    float                           damage = 20f;
    float                           reloadTime = 0.2f;
    float                           reloadTimer = 0f;
    [SerializeField] GameObject     bulletPrefab;
    [SerializeField] Transform      bulletSpawnPoint;

    [SerializeField] GameObject     target = null;

    //Enemy line of sight
    [SerializeField] float          viewDist = 25f;
    [Range(0.0f, 360.0f)] 
    [SerializeField] float          fieldOfView = 90f;
    [Range(0.0f, 10.0f)] 
    [SerializeField] float          attackDistance = 20f;

    //idle variables
    float                           idleTime = 3f;
    float                           idleTimer = 0f;

    //patrol variables
    [SerializeField] Vector3        goalDestination;
    [SerializeField] GameObject     patrolPointsHandle = null;
    [SerializeField] List<Vector3>  patrolPoints = new List<Vector3>();
    int                             patrolPointIterator = 0;
    float                           stoppingDist = 2f;

    [SerializeField] State          state = State.DEFAULT;
    
    //Properties
    public bool                     IsDead { get { if (health > 0) return false; else return true; } }


    private void Awake()
    {
        if(patrolPointsHandle)
        {
            Transform[] transfroms = patrolPointsHandle.GetComponentsInChildren<Transform>();

            foreach (var t in transfroms)
            {
                if (t == patrolPointsHandle.transform)
                    continue;
                patrolPoints.Add(t.position);
            }
        }

        agent = GetComponent<NavMeshAgent>();

    }
    private void Start()
    {
        player = WorldIntelligence.instance.Player;
        goalDestination = GetNearestNavMeshPoint(patrolPoints[patrolPointIterator]);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
   
    void Update()
    {
        if(health <= 0 && !isDead)
        {
            Kill();
        }
        else if (isDead)
        {
            return;
        }

        if(state != State.ATTACKING)
        {
            PlayerInSight();
        }


        // State behaviour
        switch(state)
        {
            case State.IDLE:
                {
                    if(idleTimer <= idleTime)
                    {
                        idleTimer += Time.deltaTime;
                    }
                    else
                    {
                        idleTimer = 0f;
                        state = State.PATROLLING;
                    }
                    break;
                }
            case State.PATROLLING:
                {
                    //Vector3 vecTo = goalDestination - transform.position;
                    //float sqrLen = vecTo.magnitude;

                    if (Vector3.Distance(goalDestination, transform.position)  < stoppingDist)
                    {
                        patrolPointIterator++;

                        //get next position in patrol points
                        if(patrolPointIterator == patrolPoints.Count)
                        {
                            patrolPointIterator = 0;
                        }

                        goalDestination = GetNearestNavMeshPoint(patrolPoints[patrolPointIterator]);
                        
                        state = State.IDLE;
                    }
                    else
                    {
                        agent.SetDestination(goalDestination);
                    }
                    
                    break;
                }
            case State.CHASING:
                {
                    //move towards target

                    //change to ATTACKING if in range



                    break;
                }
            case State.ATTACKING:
                {
                    agent.ResetPath(); // cancel movement

                    if(Time.time % 1 == 0) // update best target every second
                    {
                        target = GetNearestTarget();

                    }
                    
                    if( reloadTimer > 0)
                    {
                        reloadTimer -= Time.deltaTime;
                    }

                    if ( reloadTimer <= 0 && target )
                    {
                        // shoot

                        Vector3 dir = (target.transform.position - bulletSpawnPoint.transform.position).normalized;

                        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
                        bullet.GetComponent<BulletProjectile>().Fire(dir);

                        // reset reloadTimer to reloadTime
                        reloadTimer = reloadTime;
                    }

                    break;
                }
            default:
                {
                    state = State.IDLE;
                    break;
                }
        }
    }

    void Kill()
    {
        WorldIntelligence.instance.EnemyDied(this);
        //playDeathAnim
        isDead = true;
    }

    void PlayerInSight()
    {
        Vector3 direction = player.transform.position - this.transform.position; // cache direction w/ distance to player
        float angle = Vector3.Angle(direction, this.transform.forward); // calculate angle from direction to player and 

        if (direction.magnitude < viewDist && angle < fieldOfView / 2) // player is within view distance and the field of view of enemy
        {
            RaycastHit hit;
            if (Physics.Linecast(transform.position, player.transform.position, out hit, -1))
            {
                if (hit.transform.GetComponent<FPSController>())
                {
                    playerSpotted = true;
                    return;
                }
            }
        }
        playerSpotted = false;
    }

    public Vector3 GetNearestNavMeshPoint(Vector3 point)
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(point, out hit, point.y, -1);
        return hit.position;
    }

    private GameObject GetNearestTarget()
    {
        // shoot rays at squadies to get seen

        // pick closest squadie out of seen squadies
        
        return null;
    }

    private void OnDrawGizmos()
    {
        Vector3 leftViewLine = new Vector3(Mathf.Sin((-(this.fieldOfView / 2) + this.transform.eulerAngles.y) * Mathf.Deg2Rad), 0, Mathf.Cos((-(this.fieldOfView / 2) + this.transform.eulerAngles.y) * Mathf.Deg2Rad));
        Vector3 rightViewLine = new Vector3(Mathf.Sin(((this.fieldOfView / 2) + this.transform.eulerAngles.y) * Mathf.Deg2Rad), 0, Mathf.Cos(((this.fieldOfView / 2) + this.transform.eulerAngles.y) * Mathf.Deg2Rad));

        Gizmos.DrawLine(this.transform.position, this.transform.position + leftViewLine * this.viewDist);
        Gizmos.DrawLine(this.transform.position, this.transform.position + rightViewLine * this.viewDist);

        Gizmos.color = Color.blue;
        if(patrolPoints != null)
        {
            foreach(var p in patrolPoints)
            {
                Gizmos.DrawSphere(p, 0.2f);
            }

            Gizmos.DrawLine(goalDestination, goalDestination + (Vector3.up * 1.5f));
        }
    }

    public enum State { DEFAULT, IDLE, PATROLLING, CHASING, ATTACKING }
}
