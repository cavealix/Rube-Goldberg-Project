using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInputManager : MonoBehaviour {
	public SteamVR_TrackedObject trackedObject;
    public SteamVR_Controller.Device device;

    //Interaction Variables
    public float throwForce = 1.5f;

    //Swipe
	public float touchTolerance = 0.7f;
	private Vector2 touch;
	public ObjectMenuManager objectMenuManager;
	
	// Use this for initialization
	void Start () {
		//asign 
        trackedObject = GetComponent<SteamVR_TrackedObject>();
    } 


	// Teleport and Swipe Logic
	void Update () {
		device = SteamVR_Controller.Input((int)trackedObject.index);
		Debug.Log(trackedObject.index);

		//detect touch
		if(device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
		{
			touch = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);

			if(touch.x > touchTolerance)
			{
				objectMenuManager.MenuRight();
				Debug.Log("MenuRight");
			}
			if(touch.x < -touchTolerance)
			{
				objectMenuManager.MenuLeft();
				Debug.Log("MenuLeft");
			}
		}

		if(device.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
		{
			objectMenuManager.SpawnCurrentObject();
		}


	}


	//Hold Objects
	void OnTriggerStay(Collider col)
	{
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
	}
	//Grab objects with Trigger
	void GrabObject(Collider coli)
	{
		coli.transform.SetParent(gameObject.transform);
		coli.GetComponent<Rigidbody>().isKinematic = true;
		device.TriggerHapticPulse(2000);
		//Debug.Log("you are touching down on the trigger on an object");
	}
	//Throw objects with trigger release
	void ThrowObject(Collider coli)
	{
		coli.transform.SetParent(null);
		Rigidbody rigidBody = coli.GetComponent<Rigidbody>();
		rigidBody.isKinematic = false;
		rigidBody.velocity = device.velocity * throwForce;
		rigidBody.angularVelocity = device.angularVelocity;
		//Debug.Log("You have released the trigger");
	}
}
