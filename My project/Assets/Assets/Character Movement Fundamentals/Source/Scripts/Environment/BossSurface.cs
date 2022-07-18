using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CMF
{
	//This script moves a rigidbody along a set of waypoints;
	//It also moves any controllers on top along with it;
	public class BossSurface: MonoBehaviour {

		//Movement speed;
		public float movementSpeed = 10f;

		//Check to reverse order of waypoints;
		public bool reverseDirection = false;

		//Wait time after reaching a waypoint;
		public float waitTime = 1f;

		//This boolean is used to stop movement while the platform is waiting;
		//References to attached components;
		Rigidbody r;
		TriggerArea triggerArea;

		//List of transforms used as waypoints;
		public List<Transform> waypoints = new List<Transform>();

		//Start;
		void Start () {

			//Get references to components;
			r = GetComponent<Rigidbody>();
			triggerArea = GetComponentInChildren<TriggerArea>();

			StartCoroutine(LateFixedUpdate());
		}

		//This coroutine ensures that platform movement always occurs after Fixed Update;
		IEnumerator LateFixedUpdate()
		{
			WaitForFixedUpdate _instruction = new WaitForFixedUpdate();
			while(true)
			{
				yield return _instruction;
				MovePlatform();
			}
		}
		Vector3 movement;
		Vector3 lastPosition;
		void MovePlatform () {
			movement = (transform.position - lastPosition) / Time.fixedDeltaTime;

			Debug.Log(movement);
			if(triggerArea == null)
				return;

			//Move all controllrs on top of the platform the same distance;

			for(int i = 0; i < triggerArea.rigidbodiesInTriggerArea.Count; i++) 
			{
				triggerArea.rigidbodiesInTriggerArea[i].MovePosition(triggerArea.rigidbodiesInTriggerArea[i].position + movement);

				Debug.Log("Moving!");
			}


			lastPosition = transform.position;

		}

		//This function is called after the current waypoint has been reached;
		//The next waypoint is chosen from the list of waypoints;
		
	}
}