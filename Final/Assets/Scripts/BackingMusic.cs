using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackingMusic : MonoBehaviour {

	private ChuckSubInstance myChuck;

	void Start () {
		// get chuck subinstance
		myChuck = GetComponent<ChuckSubInstance>();
		// start music
		Play();
	}

	void Play() {
		myChuck.RunCode(@"
		  // consts
			float mods[3];
			float intro[3];
			float loop[6];
			[ 2.0022, 1.9978, 2.0000 ] @=> mods;
			[ 18.35, 21.83, 24.50 ] @=> intro;
			[ 27.50, 32.70, 36.71, 27.50, 21.83, 24.50 ] @=> loop;

			// init oscillators
			SawOsc osc[3];
			0 => osc[0].gain;
			0 => osc[1].gain;
			0 => osc[2].gain;

			// init filters
			LPF f;
			NRev r;

			650 => f.freq;
			0.6 => r.mix;

			// set up routing
			osc[0] => r => f => dac;
			osc[1] => r => f => dac;
			osc[2] => r => f => dac;

			// functions
			// set frequency
			fun void note(float n) {
			    n * mods[0] => osc[0].freq;
			    n * mods[1] => osc[1].freq;
			    n * mods[2] => osc[2].freq;
			}

			// fade volume at start
			fun void fadein() {
			    for ( 0=> int i; i < 30; i++ ) {
			        (i + 1) * 0.00006 => osc[0].gain;
			        (i + 1) * 0.00006 => osc[1].gain;
			        (i + 1) * 0.00006 => osc[2].gain;
			        500::ms => now;
			    }
			}

			// main logic
			spork ~ fadein();

			// play the intro
			for ( 0=> int i; i < 3; i++ ) {
			    note(intro[i]);
			    5::second => now;
			}

			// infinitely play  the loop
			0 => int i;
			while( true ) {
			    note(loop[i % 6]);
			    if ( i % 3 == 0) {
			        5::second => now;
			    }
			    else {
			        2.5::second => now;
			    }
			    i++;
			}
		");
	}
}
