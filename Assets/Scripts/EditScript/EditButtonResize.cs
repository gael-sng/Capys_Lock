using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EditButtonResize : MonoBehaviour {
	private RectTransform _transform;
    public bool useHeight = true;
    
    // Use this for initialization
	void Start () {
		_transform = GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update () {
		float height = _transform.sizeDelta.y;
        float width = _transform.sizeDelta.x;
        if (useHeight)
		    _transform.sizeDelta = new Vector2 (height, height);
        else
            _transform.sizeDelta = new Vector2(width, width);
    }
}
