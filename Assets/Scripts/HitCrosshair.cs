using UnityEngine;
using System.Collections;

public class HitCrosshair : MonoBehaviour {

	public Texture hair;
	private float max = 10, min = 0;
	private float spreadPixels = 0;

	public static HitCrosshair main { get; private set; }

	void Start()
	{
		main = this;
	}
	
	void Update()
	{
		spreadPixels -= Time.deltaTime * 25;
	}

	public void Spread()
	{
		spreadPixels = max;
	}

	void OnGUI()
	{
		if (spreadPixels < min)
			return;

		GUI.color = new Color(1, 1, 1, (spreadPixels - min) / (max - min));

		var centerX = Screen.width / 2f;
		var centerY = Screen.height / 2f;

		GUIUtility.RotateAroundPivot(-45, new Vector2(Screen.width / 2f, Screen.height / 2f));
		for (int i = 0; i < 4; ++i)
		{
			GUI.DrawTexture(new Rect(centerX - hair.width - spreadPixels, centerY - hair.height / 2f, hair.width, hair.height), hair);
			GUIUtility.RotateAroundPivot(-90, new Vector2(Screen.width / 2f, Screen.height / 2f));
		}
	}
}
