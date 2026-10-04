using UnityEngine;
using System.Collections;

public class GameGUI
{
	public static Rect ConvertRect(Rect rect)
	{
		rect.x *= Screen.width;
		rect.width *= Screen.width;
		rect.y *= Screen.height;
		rect.height *= Screen.height;
		return rect;
	}

	public static bool BackButton()
	{
		return GUI.Button(ConvertRect(new Rect(0.7f, 0.7f, 0.15f, 0.07f)), "BACK");
	}
}
