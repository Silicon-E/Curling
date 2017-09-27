using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour {

	public Camera camera;
	public GameObject sPrefab;
	public Transform stoneSpawn;
	public float launchSens;

	private string mode = "none";
	private GameObject stone;
	Vector3 launch;

	void Start ()
	{
		//sPrefab = (GameObject)Resources.Load("Prefabs/Stone", typeof(GameObject));
		//Debug.Log(sPrefab);
		beginThrow();//TODO: remove when game is structured
	}

	void Update ()
	{
		
	}

	void FixedUpdate ()
	{
		if(mode=="launch")
		{
			launch.x += Input.GetAxis("Mouse X")*launchSens;
			launch.z += Input.GetAxis("Mouse Y")*launchSens;
			launch.x = Mathf.Clamp(launch.x, -1f, 1f);
			launch.z = Mathf.Clamp(launch.z, 0, 2f);
			Debug.DrawRay(stone.transform.position, launch);
			//launch = launch.normalized*(launch.magnitude+Input.GetAxis("Mouse Y")*launchSensZ);
		}
	}

	public void beginThrow()
	{
		//spawn stone
		stone = GameObject.Instantiate(sPrefab, stoneSpawn);
		camera.transform.parent = stone.transform;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		launch = new Vector3(0f,0f,1f);

		mode = "launch";
	}
}
