using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class SquadMemberAI : MonoBehaviour
{
    //For now we'll just make a fsm for the different behaviour
    [SerializeField] new string             name;
    [SerializeField] NavMeshAgent           agent;
    [SerializeField] SquadController        manager;
    
    public SquadController                  Manager { set { manager = value; } }
    [SerializeField] private SquadState                      m_currentState = SquadState.NULL;

    Animator                                anim;

    float                                   rotationSpeed = 2.0f;
    float                                   speed = 2.0f;
    float                                   visDist = 20.0f;
    float                                   visAngle = 30.0f;
    float                                   shootDist = 15.0f;

    [SerializeField] bool                   enemiesInSight = false;
    [SerializeField] bool                   isDead = false;
    [SerializeField] int                    health = 100;

    Vector3                                 previousDestination;
    [SerializeField] Vector3                desiredFollowPos = Vector3.zero;
    [SerializeField] float                  followStoppingDist = 1.0f;
    [SerializeField] bool                   hasCommand = false;

    public Vector3                          FollowPosition { set { desiredFollowPos = value; } }
    public NavMeshAgent                     Agent { get { return agent; } }
    public SquadState                       State { get { return m_currentState; } set { m_currentState = value; } }
    public bool                             HasCommand { get { return hasCommand; } set { hasCommand = value; } }
    
    private void Start()
    {

        agent = this.GetComponent<NavMeshAgent>();
        if (!agent)
            Debug.LogError("Navmesh Agent not found.");
        else
        {
            Debug.Log("Nav mesh agent set up");
        }
    }

    /// <summary>
    ///     If in follow state, go to desired pos.
    ///     If in fight state, shoot & follow any other orders.
    /// </summary>

    private void Update()
    {
        if(m_currentState == SquadState.FOLLOW)
        {
            if(agent.hasPath)
            {
                Vector3 toTarget = agent.steeringTarget - this.transform.position;
                float turnAngle = Vector3.Angle(this.transform.forward, toTarget);
                agent.acceleration = turnAngle * agent.speed;
            }
            if(!agent.hasPath && !hasCommand)
            {
               
                //agent.SetDestination(desiredFollowPos);  
            }
            else
            {

            }
            if(hasCommand && agent.hasPath && agent.remainingDistance < followStoppingDist)
            {
                //hasCommand = false;
            }
        }
        if(m_currentState == SquadState.FIGHT)
        {
            if(enemiesInSight && Vector3.Distance(this.transform.position, manager.RequestBestTarget(this).gameObject.transform.position) < shootDist)
            {
                ShootAtTarget(manager.RequestBestTarget(this));
                //enemy = manager.RequestBestTarget(this)
                //ShootAtTarget(Enemy enemy)

                

            }
            else
            {
                Ray ray = new Ray(this.transform.position, manager.RequestBestTarget(this).gameObject.transform.position);
                RaycastHit hit;
                
                if(Physics.Raycast(ray, out hit, LayerMask.NameToLayer("Enemy")))
                {
                    enemiesInSight = true;
                }

                //if(manager.RequestBestTarget(this))
                //FIND CLOSEST enemy
                GoToLocation(manager.RequestBestTarget(this).gameObject.transform.position, 0.1f);
            }
        }
    }

    public void SetDestination(Vector3 pos)
    {

    }
    ////private 

    private void ShootAtTarget(EnemyAI enemy)
    {
        // if relode time = 0, shoot
    }

    public void ChangeState(SquadState change)
    {
        m_currentState = change;
    }

    public void GoToPoint(Vector3 point)
    {
        Debug.Log("Navmesh agent past status: " + agent.pathStatus.ToString());
        Debug.Log("Navmesh agent GoToPoint x: " + point.x.ToString() + ", y: "
        + point.y.ToString() + ",z: " + point.z.ToString());
        agent.SetDestination(point);
        Debug.Log("Navmesh agent past status: " + agent.pathStatus.ToString());
    }

    public bool GoToLocation(Vector3 position, float sphere_radius)
    {
        if (agent)
        {
            for (int i = 0; i < 20; i++)
            {
               
                Vector3 sample_pos = Random.insideUnitSphere * sphere_radius;
                sample_pos += position;

                sample_pos.y += sphere_radius; // add the minimum height to raise above any initial ocluded position
                RaycastHit hitInfo;
                Ray ray = new Ray(sample_pos, -Vector3.up);

                //if it hits
                if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
                {
                    sample_pos = hitInfo.point; // redefine sample pos to an environment surface location

                    
                    NavMeshHit hit;
                    if(NavMesh.SamplePosition(sample_pos, out hit, 1.0f, NavMesh.AllAreas))
                    {
                        sample_pos = hit.position; // redefine sample pos to be an appropriate navmesh position
                        agent.SetDestination(sample_pos);
                        return true; // return true for safe position
                    }
                    else
                    {
                        i++;
                    }
                }
                else
                {
                    i++;
                }
            }
            return false; // returns false for bad position
        }
        else
        {
            return false; // returns true for bad position
        }
    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    public override string ToString()
    {
        return m_currentState.ToString();
    }

    private void OnDrawGizmos()
    {
        Vector3 left = this.transform.forward - this.transform.right;
        Vector3 right = this.transform.forward + this.transform.right;

        Gizmos.DrawLine(this.transform.position, this.transform.position + (left * shootDist));
        Gizmos.DrawLine(this.transform.position, this.transform.position + (right * shootDist));
    }

}
