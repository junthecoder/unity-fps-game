using UnityEngine;
using System.Collections;
using System;

public class Crosshair : MonoBehaviour
{
	public Texture hair;

	public float Spread { get; set; }
	public static Crosshair main { get; private set; }

	void Start()
	{
		main = this;
	}

	void OnGUI()
	{
		var centerX = Screen.width / 2f;
		var centerY = Screen.height / 2f;

		var distance = 1;
		var v = (Quaternion.Euler(0, Spread, 0) * Vector3.forward) * distance;
		v = Camera.main.transform.TransformPoint(v);
		v = Camera.main.WorldToScreenPoint(v);

		var spreadPixels = v.x - centerX;

		for (int i = 0; i < 4; ++i)
		{
			GUI.DrawTexture(new Rect(centerX - hair.width - spreadPixels, centerY - hair.height / 2f, hair.width, hair.height), hair);
			GUIUtility.RotateAroundPivot(-90, new Vector2(Screen.width / 2f, Screen.height / 2f));
		}
	}
}
