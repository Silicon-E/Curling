using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour {

	public Camera camera;
	private GameObject sPrefab;
	private string mode = "none";

	void Start ()
	{
		sPrefab = (GameObject)Resources.Load("Prefabs/Stone", typeof(GameObject));
	}

	void Update ()
	{
		
	}

	void FixedUpdate ()
	{
		if(mode=="throw")
		{

		}
	}

	public void beginThrow()
	{
		//spawn stone
	}
}
