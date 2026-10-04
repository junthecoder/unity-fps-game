using UnityEngine;
using System.Collections;
using System.Linq;

public class Scope : Optics
{
	public Texture scopeTexture;

	void Start()
	{
		HideOnZoom = true;
	}

	void OnGUI()
	{
		if (Gun && !Gun.IsZooming) return;

		var screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
		var center = Camera.main.WorldToScreenPoint(transform.FindChild("point_eye").transform.position);
		if (Vector2.Distance(screenCenter, center) > 300)
			return;
		
		var width = Screen.height / 2;
		var rect = new Rect();
		rect.width = rect.height = Screen.height;
		rect.center = center;

		var color = Color.black;

		// draw margin
		Drawing.DrawRect(new Rect(0, 0, center.x - width, Screen.height), color);
		Drawing.DrawRect(new Rect(center.x + width, 0, Screen.width, Screen.height), color);

		// draw cross
		Drawing.DrawLine(new Vector2(center.x, 0), new Vector2(center.x, Screen.height), color);
		Drawing.DrawLine(new Vector2(100, center.y), new Vector2(Screen.width * 2, center.y), color);

		// draw bold cross
		float w = 5;
		int r = 50;
		// horizontal
		Drawing.DrawRect(new Rect(0, center.y - w / 2, center.x - r, w), color);
		Drawing.DrawRect(new Rect(center.x + r, center.y - w / 2, center.x - r, w), color);
		// vertical
		Drawing.DrawRect(new Rect(center.x - w / 2, 0, w, center.y - r), color);
		Drawing.DrawRect(new Rect(center.x - w / 2, center.y + r, w, center.y - r), color);

		// circle
		GUI.DrawTexture(rect, scopeTexture);
	}
}
