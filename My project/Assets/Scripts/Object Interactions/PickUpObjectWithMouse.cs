using UnityEngine;
using System.Collections;

public class PickUpObjectWithMouse : MonoBehaviour {

	/// <summary>
    /// This script goes on the player object, a camera with tag MainCamera, a pickupable object with tag Pickupable, and a model root with tag ModelRoot is needed
    /// This script should only exist once 
    /// There are two pick up types, InFront and OnHand (Self Explanatory)
    /// 
    /// </summary>

	public enum mode { LeftClick, RightClick };
	public mode Mode;
	public enum pickUpType { InFront, OnHand };
	public pickUpType PickUpType;
	public float DistanceInFront;
	public float Height;
	public float Proximity;
	//public float Smoothness;
	public float Speed;
	public float Force;
	
	GameObject mainCamera;
	GameObject modelRoot;
	GameObject carriedObject;
	GameObject rightArm;
	GameObject leftArm;
	Rigidbody rigid;
	bool carrying;
	int inputIndex = 0;

	void Awake () {
		mainCamera = GameObject.FindWithTag("MainCamera");
		modelRoot = GameObject.FindWithTag("ModelRoot");
		rightArm = GameObject.FindWithTag("RightArm");
		leftArm = GameObject.FindWithTag("LeftArm");
		if(Mode == mode.RightClick)
        {
			inputIndex = 1;
        }
	}
	void Update () {
		if(carrying) {
			//ObjectBob();
			CheckDrop();
		} else {
			PickUp();
		}
	}
	void FixedUpdate()
    {
		if(carrying && carriedObject != null)
        {
			CarryObject(carriedObject);
        }
    }
	void ObjectBob() {
		carriedObject.transform.Rotate(5,10,15);
	}
	void CarryObject(GameObject o) {
		if(PickUpType == pickUpType.OnHand)
        {
			rigid.isKinematic = true;
			o.transform.position = leftArm.transform.position + leftArm.transform.forward * DistanceInFront + new Vector3(0f, Height, 0f);
			//o.transform.position = Vector3.Lerp(o.transform.position, leftArm.transform.position + leftArm.transform.forward * DistanceInFront + new Vector3(0f, Height, 0f), 1f);
		}
		else
        {
			Vector3 holdingPosition = (modelRoot.transform.position + modelRoot.transform.forward * DistanceInFront + new Vector3(0f, Height, 0f));
			rigid.velocity = (holdingPosition - rigid.position) * Time.deltaTime * Speed;
		}
	}
	void PickUp() {
		if(Input.GetMouseButtonDown(inputIndex)) {			
			Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit)) {
				if(hit.collider.tag == "Pickupable") { 
					if((hit.collider.GetComponent<Transform>().position - this.transform.position).magnitude < Proximity) {
						carrying = true;
						carriedObject = hit.collider.gameObject;
						rigid = hit.collider.gameObject.GetComponent<Rigidbody>();
						Physics.IgnoreCollision(hit.collider.gameObject.GetComponent<Collider>(), this.GetComponent<Collider>(), true);
					}
				}
			}
		}
	}
	void CheckDrop() {
		if(Input.GetMouseButtonDown(inputIndex)) {
			rigid.isKinematic = false;
			if(PickUpType == pickUpType.OnHand)
            {
				rigid.AddForce(modelRoot.transform.forward * Force, ForceMode.Impulse);
				Debug.Log("plop");
			}
			Physics.IgnoreCollision(carriedObject.GetComponent<Collider>(), this.GetComponent<Collider>(), false);
			carrying = false;
			rigid = null;
			carriedObject = null;
		}
	}
}

