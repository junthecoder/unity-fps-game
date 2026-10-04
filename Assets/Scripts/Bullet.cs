using UnityEngine;
using System.Collections;
using System.Linq;

[RequireComponent(typeof(LineRenderer))]
public class Bullet : MonoBehaviour
{
	public float speed;
	public float damage;
	public GameObject hole;
	public string shooterTag;
	LineRenderer lineRenderer;
	public float lineOffset;
	public float startTime;
	float residualTime = 0.08f;

	void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.enabled = false;
		startTime = Time.time;
		lineRenderer.SetPosition(0, transform.TransformPoint(Vector3.forward * lineOffset));
	}

	void Update()
	{
		if (speed * (Time.time - startTime - residualTime) < lineOffset)
			return;
	}

	void FixedUpdate()
	{
		// Destroy後に参照するのを抑制
		if (this == null) return;

		var prevPosition = transform.position;
		var gravity = 30000 / speed;

		var delta = transform.forward * speed * Time.deltaTime;
		transform.position += delta;
		
		lineRenderer.SetPosition(1, transform.position);

		foreach (var hit in Physics.RaycastAll(prevPosition, transform.forward, (transform.position - prevPosition).magnitude).OrderBy(h => h.distance))
		{
			if (hit.collider.tag == "Enemy")
			{
				SendHit(hit);
				break;
			}
			else if (hit.collider.transform.parent && hit.collider.transform.parent.tag == "Enemy")
			{
				SendHit(hit, hit.collider.transform.parent.gameObject);
				break;
			}
			else if (hit.collider.gameObject.tag == "Player" && shooterTag != "Player")
			{
				SendHit(hit);
				break;
			}
			else if (hit.collider.tag == "Stage" || (hit.collider.transform.parent && hit.collider.transform.parent.tag == "Stage"))
			{
				Instantiate(hole, hit.point, Quaternion.LookRotation(hit.normal));
				Destroy(gameObject);
				break;
			}
			else if (hit.collider.attachedRigidbody)
			{
				hit.collider.attachedRigidbody.AddForce((transform.position - prevPosition) * 40);
				Destroy(gameObject);
			}
		}
	}

	void SendHit(RaycastHit hit, GameObject collider = null)
	{
		var damage = new Damage(gameObject, shooterTag, this.damage);
		damage.point = hit.point;
		damage.direction = transform.forward;

		if (!collider) collider = hit.collider.gameObject;
		collider.SendMessage("Hit", damage);

		Destroy(gameObject);
	}
}
