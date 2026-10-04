using UnityEngine;
using System.Collections;

public class Sight : Optics
{
	public Texture texture;
	public float heightProportion; // textureが画面の高さに占める割合
	public bool canShake;

	private Rect textureRect;
	private Vector2 currentShake;

	void Start()
	{
		var scale = (Screen.height * heightProportion) / texture.height;
		textureRect = new Rect();
		textureRect.width = texture.width * scale;
		textureRect.height = texture.height * scale;
		textureRect.center = new Vector2(Screen.width / 2f, Screen.height / 2f);
	}

	protected override void OnGunFired()
	{
		if (canShake)
			StartCoroutine(Shake());
	}

	IEnumerator Shake()
	{
		var shake = new Vector2(Random.Range(1f, 3f), Random.Range(1f, 3f));
		currentShake -= shake;
		yield return new WaitForSeconds(0.05f);
		currentShake += shake;
	}

	void OnGUI()
	{
		if (!IsZooming) return;

		Vector2 point = Camera.main.WorldToScreenPoint(transform.FindChild("point_eye").transform.position);
		point.y = Screen.height - point.y;
		textureRect.center = point + currentShake;

		GUI.DrawTexture(textureRect, texture);
	}
}