using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasMovement : MonoBehaviour {
	public GameObject button;
	public GameObject panel;
	private int isShowing;
	private Vector2 buttonShowingposition = new Vector2 (145.8881f,98f) ;
	private Vector2 panelShowingposition =  new Vector2 (265f, 0f) ;
	private Vector2 buttonNOTShowingposition = new Vector2 (335.6779f,98f) ;
	private Vector2 panelNOTShowingposition = new Vector2 (454.7898f,0f) ;
	private float rangepanel;
	private float rangebutton;
	// Use this for initialization
	void Start () {
		isShowing = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Click(){
		rangebutton = Vector2.Distance(buttonNOTShowingposition, buttonShowingposition);
		rangepanel = Vector2.Distance(panelNOTShowingposition, panelShowingposition);
		if (panel.Equals (panelNOTShowingposition) && button.Equals (buttonNOTShowingposition)) {
			//isShowing = 1;
			panel.transform.position = 
				Vector2.MoveTowards (panel.transform.position, panelShowingposition, rangepanel * 0.5f * Time.deltaTime);
			button.transform.position = 
				Vector2.MoveTowards (button.transform.position, buttonShowingposition, rangebutton * 0.5f * Time.deltaTime);
			

		} else {
			panel.transform.position = 
				Vector2.MoveTowards (panel.transform.position, panelNOTShowingposition, rangepanel * 0.5f * Time.deltaTime);
			button.transform.position = 
				Vector2.MoveTowards (button.transform.position, buttonNOTShowingposition, rangebutton * 0.5f * Time.deltaTime);
			
		}


	
	}

}
