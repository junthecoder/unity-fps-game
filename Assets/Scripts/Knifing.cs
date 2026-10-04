using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class Knifing : MonoBehaviour
{

	public GameObject knifePrefab;
	public KnifeCollider knifeCollider;

	private GameObject knife;
	private bool pressingKey;
	private Sequence currentSequence;
	private Transform previousTarget;
	private IKLimb ikRightArm;

	void Start()
	{
		knife = Instantiate(knifePrefab) as GameObject;
		knife.transform.parent = Camera.main.transform;
		knife.SetActive(false);
		knifeCollider.gameObject.SetActive(false);
	}

	void Update()
	{
		if (Input.GetButtonDown("Knife"))
		{
			pressingKey = true;
			StartCoroutine("Knife");
		}

		if (Input.GetButtonUp("Knife"))
			pressingKey = false;
	}

	void Play(string name)
	{
		if (currentSequence != null)
			currentSequence.Kill();

		currentSequence = new Sequence();

		var player = GameObject.FindWithTag("Player");
		var ikRightArm = player.GetComponent<Soldier>().ikRightArm;
		//var ikLeftArm = player.GetComponent<Soldier>().ikLeftArm;
		ikRightArm.target = knife.transform;

		if (name == "Default")
		{
			currentSequence.Append(HOTween.To(knife.transform, 0.05f, "localPosition", new Vector3(0, 1.4f, 0.8f)));
		}
		else if (name == "Slash")
		{
			currentSequence.Append(HOTween.To(knife.transform, 0.05f, new TweenParms()
				.Prop("localPosition", new Vector3(0.15f, -0.1f, 0.3f))
				.Prop("localRotation", Quaternion.Euler(90, 220, 0))
				));
			
			currentSequence.Append(HOTween.To(knife.transform, 0.1f, new TweenParms()
				 .Prop("localPosition", new Vector3(-0.15f, -0.1f, 0.3f))
				 .Prop("localRotation", Quaternion.Euler(90, 140, 0))
				 ));
		}
		else
			throw new System.NotImplementedException();

		
		currentSequence.Play();
	}

	IEnumerator Knife()
	{
		var gun = GetComponent<Soldier>().CurrentEquipment.gameObject;
		gun.SetActive(false);

		var player = GameObject.FindWithTag("Player");
		ikRightArm = transform.FindChild("ikRightArm").GetComponent<IKLimb>();
		previousTarget = ikRightArm.target;
		ikRightArm.target = knife.transform;

		knife.SetActive(true);
		//knifeCollider.gameObject.SetActive(true);
		Play("Slash");

		yield return new WaitForSeconds(0.2f);

		//if (pressingKey)
		//{
		//	Play("Default");
		//}
		//else
		{
			ikRightArm.target = previousTarget;
			knife.SetActive(false);
			ikRightArm.target = previousTarget;
			//knifeCollider.gameObject.SetActive(false);
			gun.SetActive(true);
		}
	}
}
