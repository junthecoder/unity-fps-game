using UnityEngine;
using System.Collections;

public class HitBlood : MonoBehaviour
{

	public Texture texture;
	public float bulletAngle;
	public Transform playerTransform;
	private float startTime;
	public float vanishTime;

	void Start()
	{
		startTime = Time.time;
	}

	void Update()
	{
		if (Time.time - startTime >= vanishTime)
			Destroy(gameObject);
	}

	void OnGUI()
	{
		GUI.color = new Color(1, 1, 1, 1f - (Time.time - startTime) / vanishTime);
		var center = new Vector2(Screen.width / 2, Screen.height / 2);

		var angle = 180 + bulletAngle - playerTransform.eulerAngles.y;

		GUIUtility.RotateAroundPivot(angle, center);
		GUI.DrawTexture(new Rect(center.x - texture.width / 2, center.y - texture.height / 2 - 100, texture.width, texture.height), texture);
	}
}
