using UnityEngine;
using System.Collections;

public class Robot : MonoBehaviour {

	GameObject target;
	public float maxSpeed = 10;
	public GameObject explosion;
	public GUITexture markerPrefab;
	public GameObject bulletPrefab;
	static int numDied;
	GUITexture marker;
	public float health;
	float maxForce = 5;
	public float minShotInterval;
	public float maxShotInterval;

	public delegate void DiedEventHandler();
	public event DiedEventHandler OnDied;

	private GameObject player;

	void Start () {
		player = target = GameObject.FindWithTag("Player");
		marker = Instantiate(markerPrefab) as GUITexture;
		rigidbody.centerOfMass = new Vector3(0, -0.6f, 0);
	}

	float nextShotTime;

	void Update()
	{
		if (Time.time >= nextShotTime)
		{
			var bulletObject = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
			var bullet = bulletObject.GetComponent<Bullet>();
			bullet.shooterTag = this.gameObject.tag;
			bullet.speed = 100;
			bullet.damage = 5;
			nextShotTime = Time.time + Random.Range(minShotInterval, maxShotInterval);
		}
	}	

	void FixedUpdate()
	{
		RaycastHit hit;
		if (Physics.Raycast(transform.position, transform.forward, out hit, 1) ||
			Physics.Raycast(transform.position, Quaternion.Euler(0, 45, 0) * transform.forward, out hit, 1) ||
			Physics.Raycast(transform.position, Quaternion.Euler(0, -45, 0) * transform.forward, out hit, 1))
		{
			rigidbody.AddRelativeForce(Vector3.back * 100);
			rigidbody.AddRelativeTorque(0, 50, 0);
			if (hit.collider.tag == "Player")
				hit.collider.SendMessage("Hit", new Damage(gameObject, tag, 1));
		}
		else
		{
			var a = target.transform.position - rigidbody.transform.position;
			a.y = 0;
			var b = rigidbody.transform.right;
			b.y = 0;

			var angle = Vector3.Dot(a, b);
			var rotation = Mathf.Clamp(angle, -1f, 1f);

			rigidbody.AddRelativeTorque(0, rotation, 0);

			rigidbody.AddForce(rigidbody.transform.forward * maxForce);
			if (rigidbody.velocity.magnitude > maxSpeed)
				rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;
		}

		var angle2 = Vector3.Angle(player.transform.forward, transform.position - player.transform.position);
		marker.enabled = angle2 <= 90;

		if (marker.enabled)
		{
			var position = transform.position;
			// TODO: Use collider max
			position.y += 1.1f; // collider.bounds.size.y;
			marker.transform.position = Camera.main.WorldToViewportPoint(position);
		}
	}

	void Hit(Damage damage)
	{
		// bullet.shooter が　Robotのとき、すでにDestroyされていることがあるのでnullチェック
		if (damage.ownerTag == "Player")
			HitCrosshair.main.Spread();

		health -= damage.damage;

		if (health <= 0)
			Die();
	}

	void Die()
	{
		if (OnDied != null) OnDied();

		Instantiate(explosion, transform.position, transform.rotation);

		foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
			renderer.enabled = false;

		Destroy(marker.gameObject);
		Destroy(gameObject);
	}
}
