using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftInput : MonoBehaviour {
	public SteamVR_TrackedObject trackedObject;
    public SteamVR_Controller.Device device;

    //Interaction Variables
    public float throwForce = 1.5f;

    // Teleporter
    private LineRenderer laser; //laser pointer
    public GameObject teleportAimerObject; //where to teleport
    public Vector3 teleportLocation; //position to teleport
    public GameObject player; //player 
    public LayerMask laserMask; //choose which layer raycast to collide with
    public float yNudge = 1f;

	
	// Use this for initialization
	void Start () {
		//asign 
        trackedObject = GetComponent<SteamVR_TrackedObject>();
        //access LineRenderer despite being private
        laser = GetComponentInChildren<LineRenderer>();
    }


	// Teleport and Swipe Logic
	void Update () {
		device = SteamVR_Controller.Input((int)trackedObject.index);
		//Debug.Log(trackedObject.index);

		//Teleportation Logic (need to include logic to only allow for left controller)
		//{
			//deploy laser on press
			if (device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
			{
				laser.gameObject.SetActive(true);
				teleportAimerObject.SetActive(true);

				//on ray hit
				laser.SetPosition(0, gameObject.transform.position);
				//create variable to store ray info
				RaycastHit hit;
				if(Physics.Raycast(transform.position, transform.forward, out hit, 15, laserMask))
				{
				//true on collision within 15
				teleportLocation = hit.point;
				laser.SetPosition(1, teleportLocation);
				//aimer position
				teleportAimerObject.transform.position = new Vector3(teleportLocation.x, teleportLocation.y + yNudge, teleportLocation.z);
				}
				//on no hit, move forward 15
				else
				{
				teleportLocation = new Vector3(transform.forward.x * 15 + transform.position.x, transform.forward.y * 15 + transform.position.y, transform.forward.z * 15 + transform.position.z);
				RaycastHit groundRay;
				if(Physics.Raycast(teleportLocation, -Vector3.up, out groundRay, 17, laserMask))
				{
					teleportLocation = new Vector3(transform.forward.x * 15 + transform.position.x, groundRay.point.y, transform.forward.z*15 + transform.position.z);

				}
				laser.SetPosition(1, transform.forward * 15 + transform.position);
				//aimer 
				teleportAimerObject.transform.position = teleportLocation + new Vector3(0, yNudge, 0);
				}
			}	
		
			//teleport on press up
			if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
			{
				laser.gameObject.SetActive(false);
				teleportAimerObject.SetActive(false);
				//move player
				player.transform.position = teleportLocation;
			}
		//}

	
	}

	//Hold Objects
	void OnTriggerStay(Collider col)
	{
		//Interact with "Throwables"
		if(col.gameObject.CompareTag("Throwable"))
		{
			if(device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
			{
				//if object is ball, instantiate gravity
				if(col.gameObject.name == "Ball")
				{
					col.gameObject.GetComponent<Rigidbody>().useGravity = true;
				}
				ThrowObject(col);
			}
			else if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
			{
				GrabObject(col);
			}
		}

		//Interact with "Structure" or "Trampoline"
		if(col.gameObject.CompareTag("Structure") || col.gameObject.CompareTag("Trampoline"))
		{
			//Grab and Move
			if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
			{
				GrabObject(col);
			}
			//Release
			else if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
			{
				ReleaseObject(col);
			}
		}
	}
	//Grab objects with Trigger
	void GrabObject(Collider coli)
	{
		coli.transform.SetParent(gameObject.transform);
		coli.GetComponent<Rigidbody>().isKinematic = true;
		device.TriggerHapticPulse(2000);
		//Debug.Log("you are touching down on the trigger on an object");
	}
	//Throw "Throwables" with trigger release
	void ThrowObject(Collider coli)
	{
		coli.transform.SetParent(null);
		Rigidbody rigidBody = coli.GetComponent<Rigidbody>();
		rigidBody.isKinematic = false;
		rigidBody.velocity = device.velocity * throwForce;
		rigidBody.angularVelocity = device.angularVelocity;
		//Debug.Log("You have released the trigger");
	}
	//Release "structures" with trigger release
	void ReleaseObject(Collider coli)
	{
		coli.transform.SetParent(null);
		Rigidbody rigidBody = coli.GetComponent<Rigidbody>();
		rigidBody.isKinematic = true;
		rigidBody.velocity = new Vector3(0, 0, 0);
		rigidBody.angularVelocity = new Vector3(0, 0, 0);
		//Debug.Log("You have released the trigger");
	}
}
