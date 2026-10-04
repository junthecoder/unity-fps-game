// Historical experiment: the controller implementation below is commented out.
// Retained for context; see README.md for the main gameplay reading path.

using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class EnemyController : MonoBehaviour
{
	//CharacterMotor motor;
	////float maxSpeed = 1;
	//public Vector3 path;
	//float health = 100;
	//bool died;
	//GameObject player;
	//Vector3 destination;
	//public GunData gunData;

	//void Start()
	//{
	//	motor = GetComponent<CharacterMotor>();
	//	player = GameObject.FindWithTag("Player");
	//	Gun gun = gunData.gameObject.AddComponent<Gun>();
	//	gun.Initialize();
	//}

	//static Vector2 xz(Vector3 v)
	//{
	//	return new Vector2(v.x, v.z);
	//}

	//void Update()
	//{
	//	if (died) return;

	//	// TODO: 上下もチェックしなくてはならない
	//	// Vector3.Angle(transform.forward, (player.transform.position - transform.position)) < 30)
	//	var angle = Vector2.Angle(xz(transform.forward), xz(player.transform.position - transform.position)) < 30;
	//	if (angle)
	//	{
	//		//destination = player.transform.position;
	//		var euler = transform.rotation.eulerAngles;
	//		var toTarget = player.transform.position - transform.position;
	//		euler.y = Mathf.Atan2(toTarget.x, toTarget.z) * Mathf.Rad2Deg;
	//		transform.rotation = Quaternion.Euler(euler);
	//	}

	//	motor.SetVelocity(transform.forward);
	//	//motor.SetVelocity((destination - transform.position).normalized * maxSpeed);
	//}

	//void OnExternalVelocity() { }

	//void Hit(Damage damage)
	//{
	//	health -= damage.damage;
	//	if (health <= 0)
	//		Die();
	//}

	//void Die()
	//{
	//	died = true;
	//	motor.SetVelocity(Vector3.zero);
	//	// TOOD: below
	//	//animation.Stop();

	//	var duration = 1f;

	//	var euler = transform.localRotation.eulerAngles;
	//	euler.x = 90;
	//	HOTween.To(transform, duration, "localRotation", Quaternion.Euler(euler));

	//	var position = transform.position;
	//	position.y = -0.4f;
	//	HOTween.To(transform, duration, "position", position);

	//	Destroy(gameObject, duration);
	//}
}
