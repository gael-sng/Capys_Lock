using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchHandle : MonoBehaviour {
	enum TouchState{Default, Selected};
	[SerializeField] TouchState _MyTouchState = TouchState.Default;
	private float _HoldCount;
	private PseudoBlock _SelectedBlock;

	private float _TimeCount;
	private Touch _ActualTouch;
	[SerializeField] float _HoldMaxTime = 0.3f;
	[SerializeField] float _HoldMinTime = 0.1f;
	// Use this for initialization
	void Start () {
		_TimeCount = -1;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0 && Input.GetTouch (0).fingerId == 0) {
			Touch touch = Input.GetTouch (0);
			Debug.Log ("Touch ID: " + touch.fingerId);
			if(_MyTouchState == TouchState.Default){
				Ray r = Camera.main.ScreenPointToRay (touch.position);
				Collider2D BlockCollider =  Physics2D.Raycast(r.origin, r.direction).collider;
				if (BlockCollider != null && BlockCollider.tag == "PseudoBlock") {
					if (touch.phase == TouchPhase.Began)
						_TimeCount = 0;//Começar a contar quanto tmepo o touch esta segurando o bloco
					if (touch.phase == TouchPhase.Stationary && _TimeCount >= 0f){
						_TimeCount += Time.deltaTime;
						if (_TimeCount > _HoldMaxTime) {
							_SelectBlock (BlockCollider);
						}
					} else if (touch.phase == TouchPhase.Ended) {
						_SelectBlock (BlockCollider);
					} else if(touch.phase == TouchPhase.Moved){
						//Make Camera Move
					}
				}
			}else if(_MyTouchState == TouchState.Selected){
				if(touch.phase == TouchPhase.Began){
					_TimeCount = 0;
				}
				if (touch.phase == TouchPhase.Ended){
					if (_TimeCount >= 0 && _TimeCount <= _HoldMinTime && _SelectedBlock._CanUnSelect ()) {
						_SelectedBlock._UnSelect ();
						_MyTouchState = TouchState.Default;
					}
					_TimeCount = -1;
				}
				if (_TimeCount >= 0) {
					_TimeCount += Time.deltaTime;
				}
			}	
		}
	}

	public void _SelectBlock(Collider2D BlockCollider){
		_TimeCount = -1;
		_SelectedBlock = BlockCollider.gameObject.GetComponent<PseudoBlock> ();
		_SelectedBlock._Select ();
		_MyTouchState = TouchState.Selected;
	}
}
