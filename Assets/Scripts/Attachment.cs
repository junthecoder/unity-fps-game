using UnityEngine;
using System.Collections;

public class Attachment : MonoBehaviour
{
	public string Name;
	public virtual Gun Gun { get; private set; }

	public virtual void Initialize(Gun gun)
	{
		this.Gun = gun;
	}

	public override string ToString()
	{
		return Name;
	}
}
