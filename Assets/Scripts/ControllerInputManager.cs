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
		//Debug.Log(trackedObject.index);

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

		//Grip
		if(device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
		{
			objectMenuManager.SpawnCurrentObject();
		}


	}

}
