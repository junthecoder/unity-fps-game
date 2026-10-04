using UnityEngine;
using System.Collections;

public class KnifeCollider : MonoBehaviour
{
	void Start()
	{
	}
	
	void Update()
	{
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "Enemy")
			collider.SendMessage("Hit", new Damage(gameObject, "Player", 100));
		else if (collider.transform.parent && collider.transform.parent.tag == "Enemy")
			collider.transform.parent.SendMessage("Hit", new Damage(gameObject, "Player", 100));
	}
}
