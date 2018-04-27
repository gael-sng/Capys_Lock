using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SettingsMenu : MonoBehaviour {

	public AudioMixer audioMixer;
	private GameObject objectWithScript;
	private float volume;
	private Slider sliderVolume;

	public void Start(){
		objectWithScript = GameObject.FindGameObjectWithTag ("DATA");
		sliderVolume = Resources.FindObjectsOfTypeAll<Slider>()[0];
		volume = objectWithScript.GetComponent<DataToSave> ().LoadVolume () ;
		if (sliderVolume != null)
			sliderVolume.value = volume;
		else
			Debug.Log ("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAhhhh");
	}

	public void SetVolume(){
		audioMixer.SetFloat ("Volume", sliderVolume.GetComponent<Slider>().value );
		objectWithScript.GetComponent<DataToSave> ().SaveVolume (sliderVolume.GetComponent<Slider>().value );
	}
}
