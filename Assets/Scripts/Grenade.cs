using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Grenade : MonoBehaviour
{
	public string Name;
	public float explosionTime = 3;
	public GameObject explosion;
	public AudioClip sound;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(explosionTime);
		audio.PlayOneShot(sound);
		Instantiate(explosion, transform.position, transform.rotation);
		enabled = false;

		// TODO: use Rigidbody.AddExplosionForce
		var maxRadius = 5;
		foreach (var collider in Physics.OverlapSphere(transform.position, maxRadius))
		{
			if (collider.gameObject.tag != "Enemy" && collider.gameObject.tag != "Player")
				continue;

			var distance = Vector3.Distance(collider.transform.position, transform.position);
			collider.gameObject.SendMessage("Hit", new Damage(gameObject, null, (1 - Mathf.Pow(distance / maxRadius, 2)) * 150));
		}

		Destroy(gameObject, sound.length);
	}

	public override string ToString()
	{
		return Name;
	}
}
