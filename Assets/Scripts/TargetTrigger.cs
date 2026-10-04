using UnityEngine;
using System.Collections;
using System.Linq;

public class TargetTrigger : MonoBehaviour {

	bool hit = false;
	public Target[] targets;
	public TimeAttack timeAttack;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider collider)
	{
		if (hit || collider.gameObject.tag != "Player")
			return;

		hit = true;

		if (timeAttack)
			timeAttack.OnTriggered();

		foreach (var target in targets)
			target.Up();
	}
}
