// Historical input experiment: toggles an input flag and time scale, but does not implement network chat.
// The original ChatA class name is preserved.

using UnityEngine;
using System.Collections;

public class ChatA : MonoBehaviour {

	public bool IsInputting { get; private set; }

	void Update()
	{
		if (Input.GetButtonDown("Chat"))
		{
			IsInputting = !IsInputting;
			if (IsInputting)
				Time.timeScale = 0;
			else
				Time.timeScale = 1;
		}
	}
}
