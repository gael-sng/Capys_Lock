using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SettingsMenu : MonoBehaviour {

	public AudioMixer audioMixer;
	public GameObject objectWithScript;
	private int volume;
	private GameObject sliderVolume;

	public void Start(){
		sliderVolume = GameObject.Find ("SliderVolume");
		volume = objectWithScript.GetComponent<DataToSave> ().LoadVolume () ;
		sliderVolume.GetComponent<Slider>().value = volume;
	}

	public void SetVolume(){
		
		audioMixer.SetFloat ("Volume", sliderVolume.GetComponent<Slider>().value );
		objectWithScript.GetComponent<DataToSave> ().SaveVolume ((int) sliderVolume.GetComponent<Slider>().value );

	
	}


}
