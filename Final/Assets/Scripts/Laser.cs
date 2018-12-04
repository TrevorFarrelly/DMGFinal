using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Laser : MonoBehaviour {

	// vectors describing ray origin and direction
	private Vector3 rayOrigin;
	private Vector3 forward;
	private float range = 100;

	// steamVR controller variables
	private Hand hand;

	// get tracked objects
	void Start() {
	    hand = GetComponent<Hand>();
	}

	// Update is called once per frame
	void Update () {
		// update direction we are pointing
		rayOrigin = transform.position;
		forward = transform.forward;
		// draw ray
		//if (hand.controller.GetHairTriggerDown()) {
    	Debug.DrawRay(rayOrigin, forward * range, Color.green);
		//}
	}

	// Raycast function
	void PlayNote () {
		RaycastHit hit;
		// if we hit a sound generator, return it
		if (Physics.Raycast(rayOrigin, forward, out hit, range) && hit.transform.tag == "SoundPanel") {
			hit.collider.GetComponent<PlayNote>().Play();
		}
	}

	// grab the generator and bring it closer
	void Grab () {
		RaycastHit hit;
		// if we hit a sound generator, return it
		if (Physics.Raycast(rayOrigin, forward, out hit, range) && hit.transform.tag == "SoundPanel") {
			// TODO
		}
	}
}
