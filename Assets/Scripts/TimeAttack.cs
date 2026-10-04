using UnityEngine;
using System.Collections;

public class TimeAttack : MonoBehaviour {

	bool started = false;
	float startTime;
	public GUIText timeText;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void OnGUI() {
		if (started)
			timeText.text = string.Format("{0:f2}", Time.time - startTime);
	}

	public void OnTriggered()
	{
		if (started)
		{
			started = false;
		}
		else
		{
			started = true;
			startTime = Time.time;
		}
	}
}
