using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayNote : MonoBehaviour {

	public float freq;
	public float volume = 1f;

	private string type;
	private ChuckSubInstance myChuck;

	// Use this for initialization
	void Start () {
		// get instrument type from parent
		type = gameObject.GetComponentInParent<SoundID>().SoundType;
		myChuck = GetComponentInParent<ChuckSubInstance>();

	}

	// play note
	public void Play () {
		myChuck.RunCode(string.Format(@"

			// consts
			float mods[3];
			[ 2.002, 1.998, 2.0000 ] @=> mods;

			// init oscillators
			{0} osc[3];
			{2} * 0.002 => osc[0].gain;
			{2} * 0.002 => osc[1].gain;
			{2} * 0.004 => osc[2].gain;

			// init filters
			NRev r;
			ADSR e;
			0.1 => r.mix;
			e.set( 20::ms, 80::ms, .5, 50::ms );

			// set up routing
			osc[0] => e => r => dac;
			osc[1] => e => r => dac;
			osc[2] => e => r => dac;

			// set frequency
			fun void note(float n) {{
				n * mods[0] => osc[0].freq;
				n * mods[1] => osc[1].freq;
				n * mods[2] => osc[2].freq;
			}}

			// set note freq
			note( {1} );

			// play note
			e.keyOn();
			500::ms => now;
			e.keyOff();
			5::second => now;

		", type, freq, volume));
	}
}
