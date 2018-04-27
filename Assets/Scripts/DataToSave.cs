using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DataToSave : MonoBehaviour {
	//public static int Dinheiro;
	//public Text textvalue;
	private int Number_Level = 1 ;
	public GameObject[] Datas;
	public static int[] Scores;
	public static int Money;
	public static int Volume;


	void Awake(){
		Datas = GameObject.FindGameObjectsWithTag ("DATA");

		if (Datas.Length >= 2) {
			Destroy (Datas [0]);
		}
		DontDestroyOnLoad (transform.gameObject);
	}

	public void SaveMoney(int value){
		Money = value;
		PlayerPrefs.SetInt ("Money", Money);

	}
	public int LoadMoney(){
		if (PlayerPrefs.HasKey ("Money")) {
			Money = PlayerPrefs.GetInt ("Money");
		} else {
			Money = 0;
		}
		return Money;
	}
	public void SaveVolume(int value){
		Volume = value;
		PlayerPrefs.SetInt ("Volume", Volume);

	}
	public int LoadVolume(){
		if (PlayerPrefs.HasKey ("Volume")) {
			Volume = PlayerPrefs.GetInt ("Volume");
		} else {
			Volume = 0;
		}
		return Volume;
	}
	public void SaveScore(int value, int fase){
		Scores [fase] = value;
		PlayerPrefs.SetInt ("Scores" + fase, Scores[fase]);
	}
	public int LoadScore(){
		if (PlayerPrefs.HasKey ("Scores")) {
			for (int x = 0; x < Number_Level; x++) {
				Scores[x] =  PlayerPrefs.GetInt("Scores"+ x);
			}
		} else {
			for (int x = 0; x < Number_Level; x++) {
				Scores[x] =  0;
			}
		}
		return Scores;
	}
		/*
	void Start(){
		if (PlayerPrefs.HasKey ("Money")) {
			Money = PlayerPrefs.GetInt ("Money");
			//textvalue = Money.ToString ();
			print("Yes");
		} else {
			PlayerPrefs.SetInt ("Money", Money);
		}

		//for (int x = 0; x < Number_Level; x++) {
		//	PlayerPrefs.SetInt("Scores"+ x, Scores[x]);
		//		}
		//for (int x = 0; x < Number_Level; x++) {
		//	Scores[x] =  PlayerPrefs.GetInt("Scores"+ x);
		//}
				
		}
*/
	
}