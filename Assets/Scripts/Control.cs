using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Control : MonoBehaviour {

	public Control control;
	public Text p1Turn;
	public Text p2Turn;
	private Text dispBanner;//Currently displayed banner
	private int stones1 = 3;//TODO: wire stone count to... something
	private int stones2 = 3;
	private int score1;
	private int score2;
	private float bannerCool = 0f;//Banner cooldown, in seconds
	private bool player = true;//true=P1, false=P2
	public int ends = 1; //How many ends to play

	//public GameManager manager;
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
	private bool clearedFarHog = false;
	private GameObject stone;
	private Rigidbody physics;
	private Collider collider;
	private Vector3 launch;
	private float turn;


	void Start ()
	{
		DisplayBanner(p1Turn,1f);
		mode = "next";
		//sPrefab = (GameObject)Resources.Load("Prefabs/Stone", typeof(GameObject));
		//Debug.Log(sPrefab);
		//beginThrow();//TODO: remove when game is structured
	}

	void DisplayBanner(Text b, float t)
	{
		dispBanner = b;
		bannerCool = t;

		dispBanner.enabled = true;
		StartCoroutine("BannerCool");
	}

	IEnumerator BannerCool()
	{
		while(bannerCool>0)
		{
			bannerCool -= Time.deltaTime;
			Debug.Log("UPD "+bannerCool);
			if(bannerCool>0)
				yield return null;
		}
		Debug.Log("STOPED");
		dispBanner.enabled = false;
		StopCoroutine("BannerCool");
	}

	void Update ()
	{
		if(stone)
			cameraPivot.position = stone.transform.position;
	}

	void FixedUpdate ()
	{
		if(bannerCool<0f)
			bannerCool = 0f;
		if(mode=="launch" && bannerCool==0)
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
				collider.material.dynamicFriction = friction;
				mode="watch";
			}
		}else if(mode=="watch")
		{
			physics.velocity = Quaternion.Euler(physics.angularVelocity*curlMul *Mathf.Rad2Deg) * physics.velocity;
			if(physics.velocity==Vector3.zero)
			{
				//(player ?stones1 :stones2)--;
				if(!clearedFarHog && !hasRebounded)//If hasn't cleared far hog AND has not rebounded off another stone
					HogStone();
				else
					StoneStopped();
			}
		}else if(mode=="hogged" && bannerCool==0)
		{
			GameObject.Destroy(stone);
			StoneStopped();
		}else if(mode=="next" && bannerCool==0)
		{
			mode="launch";
			beginThrow();
		}
	}

	void StoneStopped()
	{
		if(player) stones1--;
		else stones2--;

		mode="next";
		player = !player;

		if(stones1==0 && stones2==0)
		{
			//TODO: tally the end
			ends--;
			if(ends==0)
			{
				//TODO: game over, who won?
			}
		}
		DisplayBanner(player ?p1Turn :p2Turn,1);
	}

	public void StoneTriggerEnter(Collider other)
	{
		if(mode=="throw" && other.tag=="Near Hog")
		{
			HogStone();
		}else if(other.tag=="Far Hog")
		{Debug.Log("enter far");
			clearedFarHog = false;
		}
	}

	public void StoneTriggerExit(Collider other)
	{
		if(other.tag=="Far Hog")
		{Debug.Log("exit far");
			clearedFarHog = true;
		}
	}

	void HogStone()
	{
		DisplayBanner(hogged,1f);
		collider.material.dynamicFriction = friction;
		foreach(Collider c in stone.GetComponents<Collider>())//Disable colliders
			c.enabled= false;
		mode="hogged";
		//hogged.enabled=true;
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
