using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteButtonScript : MonoBehaviour {

    public GameObject buttonRef;

    public void setVisibility(bool visible) {
        if(buttonRef != null)
        {
            buttonRef.SetActive(visible);
        }

    }
}
