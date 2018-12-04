using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftInput : MonoBehaviour {
	public SteamVR_TrackedObject trackedObject;
    public SteamVR_Controller.Device device;

    //GameLogic
	public GameObject GameLogic;

    //Interaction Variables
    public float throwForce = 1.5f;

    //Access ball to set 'Grabbed' variable
    private GameObject ball;

    // Teleporter
    private LineRenderer laser; //laser pointer
    public GameObject teleportAimerObject; //where to teleport
    public Vector3 teleportLocation; //position to teleport
    public GameObject player; //player 
    public LayerMask laserMask; //choose which layer raycast to collide with
    public float yNudge = 0f;

	
	// Use this for initialization
	void Start () {
		//asign 
        trackedObject = GetComponent<SteamVR_TrackedObject>();
        //access LineRenderer despite being private
        laser = GetComponentInChildren<LineRenderer>();
        //assign ball
        ball = GameLogic.GetComponent<GameLogic>().ball;
    }


	// Teleport and Swipe Logic
	void Update () 
	{
		device = SteamVR_Controller.Input((int)trackedObject.index);
		//Debug.Log(trackedObject.index);

		//deploy laser on press
		if (device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
		{
			laser.gameObject.SetActive(true);
			teleportAimerObject.SetActive(true);

<<<<<<< HEAD
			//on ray hit
			laser.SetPosition(0, gameObject.transform.position);
			//create variable to store ray info
			RaycastHit hit;
			if(Physics.Raycast(transform.position, transform.forward, out hit, 15, laserMask))
			{
=======
				//on ray hit
				laser.SetPosition(0, gameObject.transform.position);
				//create variable to store ray info
				RaycastHit hit;
				if(Physics.Raycast(transform.position, transform.forward, out hit, 15, laserMask))
				{
<<<<<<< HEAD
<<<<<<< HEAD
>>>>>>> parent of 7cfbbe9... Factory Bounds
=======
>>>>>>> parent of 7cfbbe9... Factory Bounds
=======
>>>>>>> parent of 7cfbbe9... Factory Bounds
				//true on collision within 15
				teleportLocation = hit.point;
				laser.SetPosition(1, teleportLocation);
				//aimer position
				teleportAimerObject.transform.position = new Vector3(teleportLocation.x, teleportLocation.y + yNudge, teleportLocation.z);
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
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
=======
=======
>>>>>>> parent of 7cfbbe9... Factory Bounds
=======
>>>>>>> parent of 7cfbbe9... Factory Bounds
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
>>>>>>> parent of 7cfbbe9... Factory Bounds
		
		//teleport on press up
		if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
		{
			laser.gameObject.SetActive(false);
			teleportAimerObject.SetActive(false);
			//move within factory and on floor
			if (GameLogic.GetComponent<GameLogic>().factory.GetComponent<Collider>().bounds.Contains(teleportLocation) && teleportLocation.y < 1)
			{
<<<<<<< HEAD
=======
				laser.gameObject.SetActive(false);
				teleportAimerObject.SetActive(false);
<<<<<<< HEAD
<<<<<<< HEAD
>>>>>>> parent of 7cfbbe9... Factory Bounds
=======
>>>>>>> parent of 7cfbbe9... Factory Bounds
=======
>>>>>>> parent of 7cfbbe9... Factory Bounds
				//move player
				player.transform.position = teleportLocation;
			}
		}
	}

	//Hold Objects
	void OnTriggerStay(Collider col)
	{
		//Interact with "Ball"
		if(col.gameObject.CompareTag("Ball"))
		{
			//grab 
			if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
			{
				//Only allow grab if ball is inside startZone
				if (GameLogic.GetComponent<GameLogic>().checkStartZone())
				{
					ball.GetComponent<Ball>().Grabbed = true;
					GrabObject(col);
				}
			}
			//release if outside startZone
			else if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
			{
				col.gameObject.GetComponent<Rigidbody>().useGravity = true;
				ball.GetComponent<Ball>().Grabbed = false;
				ThrowObject(col);
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
	}

	//Throw "Throwables" with trigger release
	void ThrowObject(Collider coli)
	{
		coli.transform.SetParent(null);
		Rigidbody rigidBody = coli.GetComponent<Rigidbody>();
		rigidBody.isKinematic = false;
		rigidBody.velocity = device.velocity * throwForce;
		rigidBody.angularVelocity = device.angularVelocity;
	}
	
	//Release "structures" with trigger release
	void ReleaseObject(Collider coli)
	{
		coli.transform.SetParent(null);
		Rigidbody rigidBody = coli.GetComponent<Rigidbody>();
		rigidBody.isKinematic = false;
		rigidBody.velocity = new Vector3(0, 0, 0);
		rigidBody.angularVelocity = new Vector3(0, 0, 0);
		//Debug.Log("You have released the trigger");
	}
}
