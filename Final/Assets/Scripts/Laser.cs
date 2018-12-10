using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Laser : MonoBehaviour {

	// raycasting variables
	private Vector3 rayOrigin;
	private Vector3 forward;
	private float range = 20;
	private RaycastHit hit;

	// custom line object
	private LineDrawer l;

	// steamVR controller variables
	private Hand hand;
	public SteamVR_Action_Boolean PlayAction;
	public SteamVR_Action_Boolean PullAction;
	public SteamVR_Action_Boolean GrabAction;
	public SteamVR_Action_Vector2 RotationAction;

	// misc variables
	private Color currColor;
	private int colorCooldown = 0;
	private int colorReturnTime = 20;
	private bool showLine;

	// initialize objects
	void OnEnable() {
		// get hand
    hand = this.GetComponent<Hand>();
		// create line
		l = new LineDrawer();
		// get actions
		PlayAction.AddOnChangeListener(onPlay, hand.handType);
		PullAction.AddOnChangeListener(onPull, hand.handType);
		GrabAction.AddOnChangeListener(onGrab, hand.handType);
		RotationAction.AddOnChangeListener(onRotate, hand.handType);
		// set default color
		currColor = Color.blue;
		showLine = true;
	}

	void OnDisable() {
		// delete  line object on exit
    l.Destroy();
	}

	void Update() {
		// every tick, cast a ray
		rayOrigin = transform.position;
		forward = transform.forward;
		Physics.Raycast(rayOrigin, forward, out hit, range);
		// show it if we want to
		if (showLine)
			l.DrawLineInGameView(rayOrigin, forward*5 + rayOrigin, currColor);
		// if a different color has been set, iterate downwards until it changes back to default
		if (colorCooldown > 0) { colorCooldown--; }
		else { currColor = Color.blue; }
	}

	// when the trigger is pressed, play a note
	void onPlay(SteamVR_Action_In actionIn) {
			if (showLine && PlayAction.GetStateDown(hand.handType)) {
					Play();
			}
	}
	// when the touchpad is clicked, Pull/release the object
	void onPull(SteamVR_Action_In actionIn) {
			if (showLine && PullAction.GetStateDown(hand.handType)) {
					Pull();
			}
	}
	// when the grip is pressed/released enable/disable the laser
	void onGrab(SteamVR_Action_In actionIn) {
			if (GrabAction.GetStateDown(hand.handType)) {
				l.Destroy();
				showLine = false;
			}
			if (GrabAction.GetStateUp(hand.handType)) {
				l = new LineDrawer();
				showLine = true;
			}
	}
	// rotate the object we are pointing at
	void onRotate(SteamVR_Action_In actionIn) {
		Vector2 addRot = RotationAction.GetLastAxisDelta(hand.handType);
		Rotate(addRot);
	}

	// shoot the generator to play a note
	void Play () {
		// set line colors
		currColor = Color.green;
		colorCooldown = colorReturnTime;
		// if we hit a sound generator, play its note
		if (hit.transform.tag == "SoundPanel") {
			hit.rigidbody.GetComponent<PlayNote>().Play();
		}
	}

	// pull the generator closer
	void Pull () {
		// set line colors
		currColor = Color.red;
		colorCooldown = colorReturnTime;
		// if we hit a sound object, move it
		if (hit.transform.tag == "SoundObject" || hit.transform.tag == "SoundPanel") {
			// if the object is small, send it back to its default position
			if (hit.rigidbody.GetComponentInParent<MoveToPlayer>().isSmall) {
				hit.rigidbody.GetComponentInParent<MoveToPlayer>().MoveOut();
			}
			// otherwise, bring it forward to us
			else {
				hit.rigidbody.GetComponentInParent<MoveToPlayer>().MoveIn();
			}
		}
	}

	// rotate the generator using touch input
	void Rotate (Vector2 addRot) {
		if (hit.transform.tag == "SoundObject" || hit.transform.tag == "SoundPanel") {
			hit.rigidbody.GetComponentInParent<Rotator>().AddRotation(72 * addRot.y, -72 * addRot.x, 0);
		}
	}
}
