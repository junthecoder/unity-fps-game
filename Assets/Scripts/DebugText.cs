using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(GUIText))]
public class DebugText : MonoBehaviour
{
	public static DebugText main { get; private set; }

	public float updateInterval = 0;

	private Dictionary<string, string> texts = new Dictionary<string, string>();
	private float nextUpdateTime;

	public void Start()
	{
		if (!Application.isEditor)
			main.guiText.enabled = false;
	}

	public DebugText()
	{
		main = this;
	}

	public void Set(string name, object text)
	{
		texts[name] = text.ToString();
		UpdateText();
	}

	void UpdateText()
	{
		if (Time.time < nextUpdateTime) return;

		var text = ""; 
		foreach (var item in texts)
			text += item.Key + ": " + item.Value + "\n";

		guiText.text = text;
		nextUpdateTime = Time.time + updateInterval;
	}
}
