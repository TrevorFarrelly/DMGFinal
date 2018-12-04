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
	// when the grip is pressed, grab the object
	void OnGrip(SteamVR_Action_In actionIn) {
			if (gripAction.GetStateDown(hand.handType)) {
					Grab();
			}
	}

	// shoot the generator to play a note
	void PlayNote () {
		// init raycast variables
		rayOrigin = transform.position;
		forward = transform.forward;
		LineDrawer l = new LineDrawer(0.01f);
		l.DrawLineInGameView(rayOrigin, forward * range, Color.green);
		RaycastHit hit;
		// if we hit a sound generator, play its note
		if (Physics.Raycast(rayOrigin, forward, out hit, range) && hit.transform.tag == "SoundPanel") {
			hit.collider.GetComponent<PlayNote>().Play();
		}
		yield return new WaitForSeconds(0.3f);
		l.Destroy();
	}

	// grab the generator and bring it closer
	void Grab () {
		rayOrigin = transform.position;
		forward = transform.forward;
		LineDrawer l = new LineDrawer(0.01f);
		l.DrawLineInGameView(rayOrigin, forward * range, Color.red);
		RaycastHit hit;
		// if we hit a sound object, pull it to us
		if (Physics.Raycast(rayOrigin, forward, out hit, range) && hit.transform.tag == "SoundObject") {
			// TODO
		}
		yield return new WaitForSeconds(0.3f);
		l.Destroy();
	}
}

public struct LineDrawer
{
    private LineRenderer lineRenderer;
    private float lineSize;

    public LineDrawer(float lineSize = 0.2f)
    {
        GameObject lineObj = new GameObject("LineObj");
        lineRenderer = lineObj.AddComponent<LineRenderer>();
        //Particles/Additive
        lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));

        this.lineSize = lineSize;
    }

    private void init(float lineSize = 0.2f)
    {
        if (lineRenderer == null)
        {
            GameObject lineObj = new GameObject("LineObj");
            lineRenderer = lineObj.AddComponent<LineRenderer>();
            //Particles/Additive
            lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));

            this.lineSize = lineSize;
        }
    }

    //Draws lines through the provided vertices
    public void DrawLineInGameView(Vector3 start, Vector3 end, Color color)
    {
        if (lineRenderer == null)
        {
            init(0.2f);
        }

        //Set color
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        //Set width
        lineRenderer.startWidth = lineSize;
        lineRenderer.endWidth = lineSize;

        //Set line count which is 2
        lineRenderer.positionCount = 2;

        //Set the postion of both two lines
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    public void Destroy()
    {
        if (lineRenderer != null)
        {
            UnityEngine.Object.Destroy(lineRenderer.gameObject);
        }
    }
}
