using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour {

	public float lifetime;

	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds(lifetime);
		Destroy(gameObject);
	}
}
