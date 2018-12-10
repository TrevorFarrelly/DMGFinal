using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Rotator : MonoBehaviour {
	// change in rotation each tick
	public Vector3 RotationChange;

	void Start () {
		// initialize change in rotation
		RotationChange = new Vector3(1.5f, 3.0f, 0);
	}

	void Update () {
		// transform rotation by amount specified per tick
		transform.Rotate(RotationChange * Time.deltaTime, Space.World);
	}

	public void AddRotation(float x, float y, float z) {
		// modify rotation change
		RotationChange += new Vector3(x, y, z);
	}

}
