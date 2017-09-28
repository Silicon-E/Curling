using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Control : MonoBehaviour {

	public Camera camera;
	public Transform cameraPivot;
	public GameObject sPrefab;
	public Transform stoneSpawn;
	public float launchSens;
	public float turnSens;
	public float friction;
	public float curlMul;

	public Text hogged;

	private string mode = "none";
	private bool hasRebounded; //Has rebounded off another stone?
	//private bool 
	private GameObject stone;
	private Rigidbody physics;
	private Collider collider;
	private Vector3 launch;
	private float turn;


	void Start ()
	{
		//sPrefab = (GameObject)Resources.Load("Prefabs/Stone", typeof(GameObject));
		//Debug.Log(sPrefab);
		beginThrow();//TODO: remove when game is structured
	}

	void Update ()
	{
		cameraPivot.position = stone.transform.position;
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
			turn += Input.GetAxis("Mouse X")*turnSens;
			turn = Mathf.Clamp(turn, -1f, 1f);
			Debug.DrawRay(stone.transform.position, new Vector3(turn/1f, 0f, 0f), Color.red);
			if(Input.GetMouseButtonDown(0))
			{
				physics.AddTorque(0f, turn, 0f, ForceMode.Impulse);
				mode="watch";
			}
		}else if(mode=="watch")
		{
			physics.velocity = Quaternion.Euler(physics.angularVelocity*curlMul *Mathf.Rad2Deg) * physics.velocity;
			if(physics.velocity==Vector3.zero)
			{
				if(/*isXollidingWithFarHogBox && */!hasRebounded)
					HogStone();
				else
					mode="next";
			}
		}
	}

	public void StoneTriggerEnter(Collider other)
	{
		if(mode=="throw" && other.tag=="Near Hog")
		{
			HogStone();
		}
	}

	public void StoneTriggerExit(Collider other)
	{
		//TODO: maybe far hog tracking
	}

	void HogStone()
	{
		collider.material.dynamicFriction = friction;
		foreach(Collider c in stone.GetComponents<Collider>())//Disable colliders
			c.enabled= false;
		mode="hogged";
		hogged.enabled=true;
	}

	public void StoneCollisionEnter(Collider other)
	{
		if(other.tag=="Stone")
			hasRebounded=true;
	}

	public void beginThrow()
	{
		//spawn stone
		stone = GameObject.Instantiate(sPrefab, stoneSpawn);
		stone.GetComponent<Stone>().control = this;
		physics = stone.GetComponent<Rigidbody>();
		collider = stone.GetComponent<Collider>();
		collider.material.dynamicFriction = 0f;

		hasRebounded = false;
		//camera.transform.parent = stone.transform;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		launch = new Vector3(0f,0f,1f);

		mode = "launch";
	}
}
