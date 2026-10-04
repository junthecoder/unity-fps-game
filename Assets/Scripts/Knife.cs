using UnityEngine;
using System.Collections;

public class Knife : MonoBehaviour
{
	void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "Enemy")
			collider.SendMessage("Hit", new Damage(gameObject, "Player", 100));
		else if (collider.transform.parent && collider.transform.parent.tag == "Enemy")
			collider.transform.parent.SendMessage("Hit", new Damage(gameObject, "Player", 100));
	}
}
