using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SnapOnEditor : MonoBehaviour {

    private BoxCollider2D _myColl;

	// Use this for initialization
	void Start () {
        _myColl = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = _MakeSnap(transform.position);
	}

    private Vector3 _MakeSnap(Vector3 pos)
    {
        //Arredondando a posição;
        float newX;
        float newY;
        if (transform.rotation.eulerAngles.z >= 1)
        {
            newX = (float)((Mathf.Round(pos.x))) + (_myColl.size.y + 0.1f) / 2 - 0.5f;
            newY = (float)((Mathf.Round(pos.y))) + (_myColl.size.x + 0.1f) / 2 - 0.5f;
        }
        else
        {
            newX = (float)((Mathf.Round(pos.x))) + (_myColl.size.x + 0.1f) / 2 - 0.5f;
            newY = (float)((Mathf.Round(pos.y))) + (_myColl.size.y + 0.1f) / 2 - 0.5f;
        }

        return new Vector3(newX, newY, pos.z);
    }
}
