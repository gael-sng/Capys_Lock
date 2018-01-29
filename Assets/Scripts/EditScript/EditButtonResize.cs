using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EditButtonResize : MonoBehaviour {
	private RectTransform _transform;
	// Use this for initialization
	void Start () {
		_transform = GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update () {
		float height = _transform.sizeDelta.y;
		_transform.sizeDelta = new Vector2 (height, height);
	}
}
