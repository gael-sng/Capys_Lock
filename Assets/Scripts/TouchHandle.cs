using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchHandle : MonoBehaviour {
	enum TouchState{Default, Selected};
	private PseudoBlock _SelectedBlock;

	private TouchState _MyTouchState = TouchState.Default;
	private Touch _ActualTouch;

	//Time Variable
	private float _HoldCount;
	private float _TimeCount;
	[SerializeField] float _HoldMaxTime = 0.3f;
	[SerializeField] float _HoldMinTime = 0.1f;
	[SerializeField] float _MaxDistToMove = 10f;

	private CameraMovement _m_camera;
	private Vector3 _RelativePosition;
	private Transform _target;
	void Start () {
		_target = new GameObject ("CameraMovementTarget").transform;
		_target.position = Vector3.zero;
		_TimeCount = -1;
		_m_camera = Camera.main.GetComponent<CameraMovement> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0 && Input.GetTouch (0).fingerId == 0) {
			Touch touch = Input.GetTouch (0);
			if(_MyTouchState == TouchState.Default){
				Ray r = Camera.main.ScreenPointToRay (touch.position);
				Collider2D BlockCollider =  Physics2D.Raycast(r.origin, r.direction).collider;
				//verificando se um bloco esta sendo selecionado
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
					}
				}

				//Movendo a camera
				//Calculando a variação do toque
				Vector2 aux = touch.position - touch.deltaPosition;
				Vector3 touchInitialPosition = Camera.main.ScreenToWorldPoint(new Vector3 (aux.x, aux.y, Camera.main.nearClipPlane));
				Vector3 touchFinalPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.nearClipPlane));
				Vector3 deltaPosition = touchFinalPosition - touchInitialPosition;
				deltaPosition.z = 0;
				if (touch.phase == TouchPhase.Began){
					_m_camera.targetPoint (_target);
				}
				if (touch.phase == TouchPhase.Moved){
					_target.position -= deltaPosition; //touchPosition + _RelativePosition;
				}
			}else if(_MyTouchState == TouchState.Selected){


				if(touch.phase == TouchPhase.Moved){
					//Make Camera Move
					if((_SelectedBlock.transform.position - _target.position).magnitude > _MaxDistToMove){
						_target.position = _SelectedBlock.transform.position;
					}
				}
				if(touch.phase == TouchPhase.Began){
					_TimeCount = 0;
					_m_camera.targetPoint(_target);
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
