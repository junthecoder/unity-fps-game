using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MiniMap : MonoBehaviour
{

	public Texture backgroundTexture;
	public Texture playerTexture;
	public Texture enemyTexture;

	// Use this for initialization
	void Start()
	{
		//for (int i = 0; i < 20; ++i)
		//{
		//	enemies.Add(new Enemy(new Vector2(Random.value, Random.value), Random.value * 360));
		//}
	}

	void OnGUI()
	{
		var markerSize = new Vector2(16, 16);
		var rect = new Rect(40, Screen.height - 240, 200, 200);

		GUI.DrawTexture(rect, backgroundTexture);

		GUI.DrawTexture(new Rect(rect.center.x - markerSize.x / 2, rect.center.y - markerSize.y / 2, markerSize.x, markerSize.y), playerTexture);

		if (!EnemySpawn.main) return;

		foreach (var enemy in EnemySpawn.main.enemies)
		{
			if (enemy == null) continue;

			var toEnemy = enemy.transform.position - Camera.main.transform.position;
			var posOnMap = new Vector2(toEnemy.x, -toEnemy.z) * 2 + rect.center;

			if (!rect.Contains(posOnMap))
				continue;

			var originalMatrix = GUI.matrix;

			GUIUtility.RotateAroundPivot(enemy.transform.localEulerAngles.y, posOnMap);
			GUIUtility.RotateAroundPivot(-Camera.main.transform.eulerAngles.y, rect.center);
			GUI.DrawTexture(new Rect(posOnMap.x - markerSize.x / 2, posOnMap.y - markerSize.y / 2, markerSize.x, markerSize.y), enemyTexture, ScaleMode.ScaleToFit);

			GUI.matrix = originalMatrix;
		}
	}
}
