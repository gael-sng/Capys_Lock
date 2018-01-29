using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehaviour : MonoBehaviour {
	[SerializeField] string _SceneNameToLoad = "None";
	[SerializeField] bool _WillQuit = false;
	[SerializeField] GameObject Activate;
	[SerializeField] GameObject Deactivate;

	public void _Click(){
		if (_WillQuit)
			Application.Quit ();
		else if (_SceneNameToLoad != "None")
			SceneManager.LoadScene (_SceneNameToLoad);
		else {
			if (Activate != null)
				Activate.SetActive (true);
			if (Deactivate != null)
				Deactivate.SetActive (false);
		}
	}
}
