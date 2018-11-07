using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OPMath {

	public static int FullLoop(int lower, int upper, int value) {
		int diff = 1 + upper - lower;
		while (value < lower)
			value += diff;
		while (value > upper)
			value -= diff;
		return value;
	}
}
