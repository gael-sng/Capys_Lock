using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryBehaviour : MonoBehaviour {
	public float Offset; 
	private float _ScaleOffset;
	// Use this for initialization
	void Start () {
		_ScaleOffset = (Offset + 0.5f) * 2;
		if (gameObject.name == "BoundaryU") {
			gameObject.transform.position = new Vector2 (0f, GetMaxVerticalPosition () + Offset);
			gameObject.transform.localScale = new Vector2 (1f, GetMaxHorizontalPosition () - GetMinHorizontalPosition () + _ScaleOffset);
		}else if (gameObject.name == "BoundaryD") {
			gameObject.transform.position = new Vector2 (0f, GetMinVerticalPosition () - Offset);
			gameObject.transform.localScale = new Vector2 (1f, GetMaxHorizontalPosition () - GetMinHorizontalPosition () + _ScaleOffset);
		}else if (gameObject.name == "BoundaryR") {
			gameObject.transform.position = new Vector2 (GetMaxHorizontalPosition() + Offset,0f);
			gameObject.transform.localScale = new Vector2 (GetMaxVerticalPosition() - GetMinVerticalPosition() + _ScaleOffset, 1f);
		}else if (gameObject.name == "BoundaryL") {
			gameObject.transform.position = new Vector2 (GetMinHorizontalPosition() - Offset, 0f);
			gameObject.transform.localScale = new Vector2 (GetMaxVerticalPosition() - GetMinVerticalPosition() + _ScaleOffset, 1f);
		}
	}

	public float GetMaxVerticalPosition(){
		return Camera.main.orthographicSize;
	}
	public float GetMinVerticalPosition(){
		return - Camera.main.orthographicSize;
	}
	public float GetMaxHorizontalPosition(){
		return Camera.main.orthographicSize * Screen.width / Screen.height;
	}
	public float GetMinHorizontalPosition(){
		return -Camera.main.orthographicSize * Screen.width / Screen.height;
	}	
}
