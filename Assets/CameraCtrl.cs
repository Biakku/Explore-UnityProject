using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour 
{

	public Transform player;
	public float yOffset;
	public float xOffset;


	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position = new Vector3 (player.position.x + xOffset, player.position.y + yOffset, transform.position.z);
	}
}
