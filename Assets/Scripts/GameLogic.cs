using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour {
	public GameObject ball;
	public List<GameObject> stars;
    //public GameObject factory;

	//Start Zone 
	public GameObject StartZone;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {	
	}

	//Check if ball in start zone to prevent cheating
	public bool checkStartZone()
	{
		return StartZone.GetComponent<Collider>().bounds.Contains(ball.transform.position);
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

        switch(scene.name)
        {
        	case "Level_1":
        		SceneManager.LoadScene("Level_2");
        		break;
        	case "Level_2":
        		SceneManager.LoadScene("Level_3");
        		break;
        	case "Level_3":
        		SceneManager.LoadScene("Level_4");
        		break;
        	case "Level_4":
        		SceneManager.LoadScene("Level_1");
        		break;
        }
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

    	//reset ball
    	ball.GetComponent<Ball>().Respawn();
	}

	public bool isCheating()
	{
		return true;
	}
}