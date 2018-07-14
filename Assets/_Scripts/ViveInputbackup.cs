using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ViveInputbackup : MonoBehaviour {
	SteamVR_TrackedObject trackedObj;
	SteamVR_Controller.Device device;

	private bool orient = true;

	void Awake ()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
	}
	// Use this for initialization
	void Start () {
		
	}

	void FixedUpdate()
	{
		device = SteamVR_Controller.Input ((int)trackedObj.index);
	}

	// Update is called once per frame
	void Update () {
		if (device.GetTouchDown (SteamVR_Controller.ButtonMask.Trigger)) {
			orient = !orient;
			Debug.Log(GameObject.Find ("Camera (eye)").transform.position.ToString("F4"));
		}
		if (device.GetPress (SteamVR_Controller.ButtonMask.Touchpad)) {
			Vector2 touchpad = (device.GetAxis (Valve.VR.EVRButtonId.k_EButton_Axis0));
			if (orient == true) {
				if (touchpad.y > 0.7f) {
					GameObject.Find ("fingertip").transform.localPosition = GameObject.Find ("fingertip").transform.localPosition + new Vector3 (0.0f, 0.0f, 0.001f);
				} else if (touchpad.y < -0.7f) {
					GameObject.Find ("fingertip").transform.localPosition = GameObject.Find ("fingertip").transform.localPosition + new Vector3 (0.0f, 0.0f, -0.001f);
				} else if (touchpad.x > 0.7f) {
					GameObject.Find ("fingertip").transform.localPosition = GameObject.Find ("fingertip").transform.localPosition + new Vector3 (0.001f, 0.0f, 0.0f);
				} else if (touchpad.x < -0.7f) {
					GameObject.Find ("fingertip").transform.localPosition = GameObject.Find ("fingertip").transform.localPosition + new Vector3 (-0.001f, 0.0f, 0.0f);
				}
			} else if (orient == false) {
				if (touchpad.y > 0.7f) {
					GameObject.Find ("fingertip").transform.localPosition = GameObject.Find ("fingertip").transform.localPosition + new Vector3 (0.0f, 0.001f, 0.0f);
				} else if (touchpad.y < -0.7f) {
					GameObject.Find ("fingertip").transform.localPosition = GameObject.Find ("fingertip").transform.localPosition + new Vector3 (0.0f, -0.001f, 0.0f);
				} else if (touchpad.x > 0.7f) {
					GameObject.Find ("fingertip").transform.localPosition = GameObject.Find ("fingertip").transform.localPosition + new Vector3 (0.001f, 0.0f, 0.0f);
				} else if (touchpad.x < -0.7f) {
					GameObject.Find ("fingertip").transform.localPosition = GameObject.Find ("fingertip").transform.localPosition + new Vector3 (-0.001f, 0.0f, 0.0f);
				}
			}
		}
	}
}
