using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour {

	public AudioMixer audioMixer;
	public GameObject objectWithScript;
	private int volume;

	public void Start(){
		volume = objectWithScript.GetComponent<DataToSave> ().LoadVolume () ;

	}

	public void SetVolume(float volume){
		audioMixer.SetFloat ("Volume", volume);
	}


}
