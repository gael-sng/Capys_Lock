using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasMovement : MonoBehaviour {
	public GameObject button;
	public GameObject panel;
	private float veloc;
	private Vector2 buttonShowingposition ;
	private Vector2 panelShowingposition ;
	private Vector2 buttonNOTShowingposition ;
	private Vector2 panelNOTShowingposition ;
	private bool flag;
	//private float rangepanel;
	//private float rangebutton;
	// Use this for initialization
	void Start () {
		flag = true;
		veloc = 100f * Time.deltaTime;
		buttonShowingposition =  
			new Vector2(GameObject.Find("BtnFROMScrollListPosClicked").transform.position.x,GameObject.Find("BtnFROMScrollListPosClicked").transform.position.y);
		panelShowingposition =  
			new Vector2(GameObject.Find("BtnScrollListPosClicked").transform.position.x,GameObject.Find("BtnScrollListPosClicked").transform.position.y);
		buttonNOTShowingposition =  
			new Vector2(GameObject.Find("BtnFROMScrollListPosUnclicked").transform.position.x,GameObject.Find("BtnFROMScrollListPosUnclicked").transform.position.y );
		panelNOTShowingposition =  
			new Vector2(GameObject.Find("BtnScrollListPosUnclicked").transform.position.x, GameObject.Find("BtnScrollListPosUnclicked").transform.position.y);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Click(){
		
		print (flag);

		//rangebutton = Vector2.Distance(buttonNOTShowingposition, buttonShowingposition);
		//rangepanel = Vector2.Distance(panelNOTShowingposition, panelShowingposition);
		//if (panel.transform.position.x.Equals (panelNOTShowingposition.x) && button.transform.position.x.Equals (buttonNOTShowingposition.x)) {
		if (flag == true){
			flag = false;
		//isShowing = 1;
			//panel.transform.position = 
			//	Vector2.MoveTowards (panel.transform.position, panelShowingposition, veloc * Time.deltaTime);
			//button.transform.position = 
			//	Vector2.MoveTowards (button.transform.position, buttonShowingposition, veloc * Time.deltaTime);
			iTween.MoveTo(panel,panelShowingposition, veloc); 
			iTween.MoveTo(button,buttonShowingposition, veloc);
		} else if (flag == false) {
			flag = true;
			iTween.MoveTo(panel,panelNOTShowingposition, veloc); 
			iTween.MoveTo(button,buttonNOTShowingposition, veloc);
			//panel.transform.position = 
			//	Vector2.MoveTowards (panel.transform.position, panelNOTShowingposition, veloc * Time.deltaTime);
			//button.transform.position = 
			//	Vector2.MoveTowards (button.transform.position, buttonNOTShowingposition, veloc * Time.deltaTime);
		}
	}
}
