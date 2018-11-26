using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogicScript : MonoBehaviour {
	public GameObject ball;
	public List<GameObject> stars;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//Ball hit a star
	public void CollectStar(GameObject star)
	{
    	stars.Add(star);
    	star.GetComponent<Star>().Collect();
	}

	public void HitGoal()
	{
		//Collect All Stars?
    	if(stars.Count == 3){
           Debug.Log("You Win!");
           //load next level
           NextLevel();
        }
    	else
    	{
        	Debug.Log("Try Again");
        	//reset level
        	Reset();
    	}
	}

	//All stars collected, load next level
	public void NextLevel()
	{
		//SteamVR_LoadLevel.Begin("Level_1");
        Scene scene = SceneManager.GetActiveScene();
        Debug.Log("Active scene is '" + scene.name + "'.");
	}

	public void Reset()
	{
		//Respawn Stars
        foreach(GameObject star in stars)
        {
        	star.GetComponent<Star>().Respawn();
        }
        //reset star array
        stars.Clear();
	}

	public bool isCheating()
	{
		return true;
	}
}
