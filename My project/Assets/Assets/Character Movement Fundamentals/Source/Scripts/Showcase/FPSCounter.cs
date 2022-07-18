using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CMF
{
	//This script calculates the average framerate and displays it in the upper right corner of the screen;
	public class FPSCounter : MonoBehaviour {

		public GameObject FPSct;
		

		//Framerate is calculated using this interval;
		public float checkInterval = 1f;

		//Variables to keep track of passed time and frames;
		int currentPassedFrames = 0;
		float currentPassedTime = 0f;

		//Current framerate;
		public float currentFrameRate = 0f;
		string currentFrameRateString = "";
		
		// Update;
		void Update () {

			//Increment passed frames;
			currentPassedFrames ++;

			//Increment passed time;
			currentPassedTime += Time.deltaTime;

			//If passed time has reached 'checkInterval', recalculate framerate;
			if(currentPassedTime >= checkInterval)
			{
				//Calculate frame rate;
				currentFrameRate = (float)currentPassedFrames/currentPassedTime;

				//Reset counters;
				currentPassedTime = 0f;
				currentPassedFrames = 0;

				//Clamp to two digits behind comma;
				currentFrameRate *= 100f;
				currentFrameRate = (int)currentFrameRate;
				currentFrameRate /= 100f;

				//Calculate framerate string to display later;
				currentFrameRateString = currentFrameRate.ToString();
			}

			if (FPSct)
				FPSct.GetComponent<TMPro.TextMeshProUGUI>().text = "FPS: " + currentFrameRateString;
		}

		//Render framerate in the upper right corner of the screen;
		void OnGUI()
		{
			if (!FPSct)
			{
				GUI.contentColor = Color.black;

				float labelSize = 30f;
				float offset = 2f;

				GUI.Label(new Rect(labelSize + offset, offset, Screen.width / 9, Screen.height/9), "FPS:  " + currentFrameRateString);

			}
		}
	}
}