using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawn : MonoBehaviour {

	public static EnemySpawn main;
	public GameObject enemy;
	float nextSpawnTime;
	public List<GameObject> enemies = new List<GameObject>();
	public Vector3[] spwanPoints;
	public GUIText informationText;
	int numEnemyKilled;
	int currentLevel;

	void Start ()
	{
		main = this;
		currentLevel = 1;
		UpdateText();
		Spawn();
	}
	
	void Update () {
		if (Time.time >= nextSpawnTime)
			Spawn();
	}

	void Spawn()
	{
		nextSpawnTime = Time.time + Mathf.Max(3, 5.5f - (float)currentLevel / 2);
		var position = spwanPoints[Random.Range(0, spwanPoints.Length)];
		var rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

		var obj = Instantiate(enemy, position, rotation) as GameObject;
		var robot = obj.GetComponent<Robot>();
		robot.OnDied += EnemySpawn_OnDied;
		robot.minShotInterval = Mathf.Max(2, 8 - currentLevel * 0.8f);
		robot.maxShotInterval = robot.minShotInterval + 2;

		enemies.Add(obj);
	}

	void EnemySpawn_OnDied()
	{
		++numEnemyKilled;
		if (numEnemyKilled % 10 == 0)
			++currentLevel;

		UpdateText();
	}

    void UpdateText()
    {
        informationText.text =
            "LEVEL  : " + currentLevel + "\n" +
            "KILLED : " + numEnemyKilled;
    }
}
