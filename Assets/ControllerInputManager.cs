using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInputManager : MonoBehaviour {
	public SteamVR_TrackedObject trackedObject;
    public SteamVR_Controller.Device device;

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


	// Update is called once per frame
	void Update () {
		device = SteamVR_Controller.Input((int)trackedObject.index);

		//on press
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
		
		//on press up
		if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
		{
			laser.gameObject.SetActive(false);
			teleportAimerObject.SetActive(false);
			//move player
			player.transform.position = teleportLocation;
		}	
	}
}
