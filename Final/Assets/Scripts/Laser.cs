using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Laser : MonoBehaviour {

	// vectors describing ray origin and direction
	private Vector3 rayOrigin;
	private Vector3 forward;
	private float range = 20;

	// steamVR controller variables
	public Hand hand;
	public SteamVR_Action_Boolean triggerAction;
	public SteamVR_Action_Boolean gripAction;

	// get tracked objects
	void OnEnable() {
	    hand = this.GetComponent<Hand>();
			triggerAction.AddOnChangeListener(OnTrigger, hand.handType);
			gripAction.AddOnChangeListener(OnGrip, hand.handType);
	}

	// when the trigger is pressed, play a note
	void OnTrigger(SteamVR_Action_In actionIn) {
			if (triggerAction.GetStateDown(hand.handType)) {
					PlayNote();
			}
	}
	// when the grip is pressed/released, grab/release the object
	void OnGrip(SteamVR_Action_In actionIn) {
			if (gripAction.GetStateDown(hand.handType)) {
					Grab();
			}
			if (gripAction.GetStateUp(hand.handType)) {
					Release();
			}
	}

	// shoot the generator to play a note
	void PlayNote () {
		// init raycast variables
		rayOrigin = transform.position;
		forward = transform.forward;
		LineDrawer l = new LineDrawer(0.001f);
		l.DrawLineInGameView(rayOrigin, forward * range, Color.green);
		RaycastHit hit;
		// if we hit a sound generator, play its note
		if (Physics.Raycast(rayOrigin, forward, out hit, range) && hit.transform.tag == "SoundPanel") {
			hit.collider.GetComponent<PlayNote>().Play();
		}
		l.Destroy();
	}

	// grab the generator and bring it closer
	void Grab () {
		// init raycast variables
		rayOrigin = transform.position;
		forward = transform.forward;
		LineDrawer l = new LineDrawer(0.001f);
		l.DrawLineInGameView(rayOrigin, forward * range, Color.red);
		RaycastHit hit;
		// if we hit a sound object, pull it to us
		if (Physics.Raycast(rayOrigin, forward, out hit, range) && hit.transform.tag == "SoundObject") {
			// TODO
		}
		l.Destroy();
	}

	// release the generator, letting it float back to its normal position
	void Release () {
		// TODO
	}
}


