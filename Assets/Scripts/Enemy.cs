using UnityEngine;
using System.Collections;

public class Enemy : Soldier
{
	public bool showInformation;

	private HitCrosshair hitCrosshair;

    new void Start()
	{
        base.Start();

		hitCrosshair = Player.GetComponent<HitCrosshair>();

		OnDamaged += (Damage damage) => { hitCrosshair.Spread(); };
		OnDied += Enemy_OnDied;
	}

	void Enemy_OnDied(Damage damage)
	{
		//Instantiate((equipments[0] as Gun).Data, transform.position, transform.rotation);
		Destroy(gameObject, 5);
	}
	
	void Update()
	{
		if (IsDead) return;
        rigidbody.MoveRotation(Quaternion.LookRotation(Player.transform.position - transform.position));
	}

    void OnGUI()
    {
		if (IsDead || !showInformation) return;

        var position = transform.position;
        position.y += 1.9f;

        var direction = (transform.position - Camera.main.transform.position).normalized;
        var origin = Camera.main.transform.position + direction * 1.2f;
        var end = transform.position - direction * 1.2f;
        RaycastHit hit;

        if (Physics.Raycast(origin, direction, out hit, Vector3.Distance(origin, end)))
            if (hit.collider.tag == "Stage" || (hit.collider.transform.parent && hit.collider.transform.parent.tag == "Stage"))
                return;

        var screenPoint = Camera.main.WorldToScreenPoint(position);
        if (screenPoint.z <= 0) return;

        var rect = new Rect();
        rect.width = 40;
        rect.height = 8;
        screenPoint.y = Screen.height - screenPoint.y;
        rect.center = screenPoint;
        Drawing.DrawRect(rect, Color.white);

        ++rect.x;
        ++rect.y;
        rect.width -= 2;
        rect.height -= 2;
        rect.width *= Health / 100;

        Drawing.DrawRect(rect, Color.red);
    }
}
