using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DataToSave : MonoBehaviour {
	//public static int Dinheiro;

	private int Number_Level = 1 ;
	public GameObject[] Datas;
	public static int[] Scores;


	void Awake(){
		Datas = GameObject.FindGameObjectsWithTag ("DATA");

		if (Datas.Length >= 2) {
			Destroy (Datas [0]);
		}
		DontDestroyOnLoad (transform.gameObject);
	}
	void Start(){
		for (int x = 0; x < Number_Level; x++) {
			PlayerPrefs.SetInt("Scores"+ x, Scores[x]);
				}
		for (int x = 0; x < Number_Level; x++) {
			Scores[x] =  PlayerPrefs.GetInt("Scores"+ x);
		}
				
		}

	}
	
