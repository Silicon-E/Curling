using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public Control control;
	public Text p1Turn;
	public Text p2Turn;
	private Text dispBanner;//Currently displayed banner
	private int stones1;
	private int stones2;
	private float bannerCool;//Banner cooldown, in seconds
	private bool player = true;//true=P1, false=P2
	// Use this for initialization
	void Start () {
		DisplayBanner(p1Turn,1f);
	}
	
	// Update is called once per frame
	void Update () {
		if(bannerCool>0)
			return;

		// What goes here? nothing?
	}

	public void StoneStopped(GameObject stone)
	{
		//(player ?stones1 :stones2)--;TODO this commented to silence compile error

		if(stones1==0 && stones2==0)
		{
			//TODO: tally the end
		}
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
			yield return null;
		}
		Debug.Log("STOPED");
		dispBanner.enabled = false;
		StopCoroutine("BannerCool");
	}
}
