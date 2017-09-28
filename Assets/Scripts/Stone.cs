using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour {
	public Control control;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		control.StoneTriggerEnter(other);
	}

	void OnTriggerExit(Collider other)
	{
		control.StoneTriggerExit(other);
	}

	void OnCollisionEnter(Collider other)
	{
		control.StoneCollisionEnter(other);
	}
}
