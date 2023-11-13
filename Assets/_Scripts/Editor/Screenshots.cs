using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class Screenshots {

#if UNITY_EDITOR
	[MenuItem("Useful/Take screenshot", priority = 9)]
	public static void TakeScreenshot() {
		string currentTime = System.DateTime.Now.ToString("MM-dd-yy (HH-mm-ss)");
		ScreenCapture.CaptureScreenshot($"{Application.persistentDataPath}/screenshot({currentTime}).png");
		Debug.Log("A screenshot was taken!");
	}
#endif
}
