using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace CMF
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyMovement : MonoBehaviour
    {

        public Transform Target;
        public float UpdateSpeed = 0.1f;

        public float minimumDistance;
        public float maxDistance;
        public int currentWaypoint;

        private NavMeshAgent Agent;

        public List<GameObject> waypoints = new List<GameObject>();

        public bool TrackOnSight = false;

        public bool InfiniteTrack = false;

        public float detectionRange = 50;

        public List<GameObject> Waypoints { get => waypoints; set => waypoints = value; }

        // Start is called before the first frame update

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Waypoints");

            //Debug.Log(objects);
            /*
            for (int i = 0; i < objects.Length; i++)
            {
                Waypoints.Add(objects[i]);
            }*/

        }

        void OnEnable()
        {
            StartCoroutine(FollowTarget());
        }

        // Update is called once per frame


        public float stopDistance = 9;

        private IEnumerator FollowTarget()
        {
            while (enabled)
            {
                // Updates the variable each iteration

                float update = Mathf.Abs((this.transform.position - Target.transform.position).magnitude);

                /* Checks to see if the player is within the range of the distance designated and  then chases them. 
                   maxDistance makes sure the AI is not too close and minimumDistance is making sure it's not too far.*/
                RaycastHit hit;
                
                if (InfiniteTrack)
                {
                    Agent.SetDestination(Target.transform.position);
                    continue;
                }

                if (TrackOnSight)
                {
                    if (Physics.Raycast(transform.position, Target.position - transform.position, out hit, detectionRange, ~(1<<2)))
                    {
                        if (hit.collider.gameObject.GetComponent<Mover>() || hit.collider.gameObject.GetComponentInParent<Mover>())
                        {
                            //Debug.Log("Seeing player, Following player");
                            //Debug.Log(hit.collider.gameObject.name);
                            Agent.SetDestination(Target.transform.position);
                            if (Mathf.Abs((Target.position - transform.position).magnitude) <= stopDistance)
                            {

                                Agent.SetDestination(transform.position);
                            }


                        } 
                        else
                        {
                            //Debug.Log("Following waypoints");
                            FollowWayPoints();
                        }
                    }
                }
                else if (update < minimumDistance)
                {
                    
                    Agent.SetDestination(Target.transform.position);
                }
                else //if (update > minimumDistance)
                {
                    //Debug.Log("Following waypoints");
                    FollowWayPoints();
                }

                //Debug.Log(Agent.destination + ", " + Mathf.Abs((transform.position - Target.position).magnitude));


                yield return new WaitForSeconds(UpdateSpeed);
            }
        }

        private void FollowWayPoints()
        {
            Agent.SetDestination(waypoints[currentWaypoint].transform.position);
            // If the AI has reached the waypoint
            if (Mathf.Abs((waypoints[currentWaypoint].transform.position - this.transform.position).magnitude) < 5f)
            {
                currentWaypoint++;
                if (currentWaypoint >= waypoints.Count) { currentWaypoint = 0; }
                Agent.SetDestination(waypoints[currentWaypoint].transform.position);
            }
        }


        // possible AI behaviours:
        // Follow on sight within range
        // Follow regardless of sight within range
        // Follow until can shoot player, then stop



    }
}