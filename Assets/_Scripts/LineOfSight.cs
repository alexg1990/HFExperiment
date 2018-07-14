using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour {

	private RaycastHit vision;
	public float rayLength;
	private bool isGrabbed;
	private Rigidbody grabbedObject;

	// Use this for initialization
	void Start () {
		rayLength = 4.0f;
		isGrabbed = false;
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawRay (Camera.main.transform.position, Camera.main.transform.forward * rayLength, Color.red, 0.5f);
		if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out vision, rayLength))
		{
			if(vision.collider.tag == "target")
			{
				Debug.Log (vision.collider.name);
				if (Input.GetKeyDown (KeyCode.E) && !isGrabbed) {
					grabbedObject = vision.rigidbody;
					grabbedObject.isKinematic = true;
					grabbedObject.transform.SetParent (gameObject.transform);
					isGrabbed = true;
				} else if (isGrabbed && Input.GetKeyDown(KeyCode.E)){
					grabbedObject.transform.parent = null;
					grabbedObject.isKinematic = false;
					isGrabbed = false;
				}
			}
		}
	}
}