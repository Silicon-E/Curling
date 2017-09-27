using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour {

	public Camera camera;
	public GameObject sPrefab;
	public Transform stoneSpawn;
	public float launchSens;

	public GUIText hogged;

	private string mode = "none";
	private GameObject stone;
	private Rigidbody physics;
	private Collider collider;
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
			launch.z = Mathf.Clamp(launch.z, 0, 4f);
			Debug.DrawRay(stone.transform.position, launch, Color.red);
			//launch = launch.normalized*(launch.magnitude+Input.GetAxis("Mouse Y")*launchSensZ);
			if(Input.GetMouseButtonDown(0))
			{
				physics.AddForce(launch, ForceMode.Impulse);
				mode="throw";
			}
		}else if(mode=="throw")
		{
			
		}else if(mode=="watch")
		{
			if(physics.velocity==Vector3.zero)
				mode="next";
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(mode=="throw" && other.tag=="Near Hog")
		{Debug.Log("HOGGED");
			mode="hogged";
			hogged.enabled=true;
		}
	}

	public void beginThrow()
	{
		//spawn stone
		stone = GameObject.Instantiate(sPrefab, stoneSpawn);
		physics = stone.GetComponent<Rigidbody>();
		collider = stone.GetComponent<Collider>();
		collider.material.dynamicFriction = 0f;

		camera.transform.parent = stone.transform;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		launch = new Vector3(0f,0f,1f);

		mode = "launch";
	}
}
