using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TransformAnimation : MonoBehaviour {

	public struct Transform
	{
		public Vector3 position;
		public Quaternion rotation;
		public float length;
		public bool repeat;
		public bool useRotation;

		public Transform(Vector3 position, Quaternion rotation, float length, bool repeat = false)
		{
			this.position = position;
			this.rotation = rotation;
			this.length = length;
			this.repeat = repeat;
			this.useRotation = true;
		}

		public Transform(Vector3 position, float length, bool repeat = false)
			: this(position, Quaternion.identity, length, repeat)
		{
			this.useRotation = false;
		}
	}

	public class Clip
	{
		public Transform[] transforms;

		public Clip(Transform transform)
		{
			this.transforms = new Transform[] { transform };
		}
		public Clip(Transform[] transforms)
		{
			this.transforms = transforms.Clone() as Transform[];
		}
	}

	Dictionary<string, Clip> clips = new Dictionary<string, Clip>();
	//Clip currentClip;
	float startTime;
	bool isPlaying;
	Transform[] transforms;
	int currentIndex;
	string[] playStack;

	void Start()
	{
	}
	
	void Update()
	{
		if (!isPlaying) return;

		if (Time.time - startTime >= transforms[currentIndex + 1].length)
		{
			if (transforms[currentIndex + 1].repeat)
			{
				startTime = startTime + transforms[currentIndex + 1].length;
				// swap
				var tmp = transforms[currentIndex];
				transforms[currentIndex] = transforms[currentIndex + 1];
				transforms[currentIndex + 1] = tmp;

				transforms[currentIndex + 1].repeat = true;
				transforms[currentIndex + 1].length = transforms[currentIndex].length;
			}
			else
			{
				if (currentIndex < transforms.Length - 2)
				{
					startTime += transforms[currentIndex + 1].length;
					++currentIndex;
				}
				else
					isPlaying = false; // returnせずに以下で最後の更新
			}
		}

		var t = Mathf.Min(1, (Time.time - startTime) / transforms[currentIndex + 1].length);

		transform.localPosition = Vector3.Lerp(transforms[currentIndex].position, transforms[currentIndex + 1].position, t);
		if (transforms[currentIndex + 1].useRotation)
			transform.localRotation = Quaternion.Lerp(transforms[currentIndex].rotation, transforms[currentIndex + 1].rotation, t);
	}

	public void Add(string name, params Transform[] transforms)
	{
		clips.Add(name, new Clip(transforms));
	}

	public void Play(string name)
	{
		startTime = Time.time;
		isPlaying = true;

		var clip = clips[name];
		if (clip == null) throw new System.ArgumentException();

		transforms = new Transform[clip.transforms.Length + 1];
		transforms[0] = new Transform(transform.localPosition, transform.localRotation, 0);

		for (int i = 0; i < clip.transforms.Length; ++i)
			transforms[i + 1] = clip.transforms[i];
		
		currentIndex = 0;
	}

	//public void Play(params string[] names)
	//{
	//	playStack = names;
	//	Play(names.
	//}
}
