using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPlayer : MonoBehaviour {
	// initial positions and scales
	private Vector3 initPos;
	private Vector3 initScale;
	// subdivisions of positions and scales for movement
	private Vector3 iterPos;
	private Vector3 iterScale;
	// object landing location
	private Vector3 finalPos;
	// iterations left when moving
	private int movingIn;
	private int movingOut;
	// public variables for checking/setting in editor
	public bool isSmall;
	public int iterations;
	public float scale;

	// Use this for initialization
	void Start () {
		// initialize position and scale
		initPos = transform.position;
		initScale = transform.localScale;
		// initialize change in position and scale
		iterScale = (initScale - initScale * scale) / iterations;
		finalPos = new Vector3(initPos.x / 7, 1, initPos.z / 7);
		// we are not moving at start
		movingIn = 0;
		movingOut = 0;
		isSmall = false;
	}

	void Update () {
		// every tick, check if we need to move
		if (movingIn > 0) {
			// move in if we have to
			transform.position += iterPos;
			transform.localScale -= iterScale;
			movingIn--;
		}
		if (movingOut > 0) {
			// move out if we have to
			transform.position += iterPos;
			transform.localScale += iterScale;
			movingOut--;
		}
	}

	public void MoveIn () {
		// find change in position
		iterPos = (finalPos - transform.position) / iterations;
		movingIn = iterations;
		isSmall = true;
	}

	public void MoveOut () {
		iterPos = (initPos - transform.position) / iterations;
		movingOut = iterations;
		isSmall = false;
	}
}
