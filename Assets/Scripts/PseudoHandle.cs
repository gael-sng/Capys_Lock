using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PseudoHandle : MonoBehaviour {
	public List<PseudoBlock> _PseudoBlockList;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void _StartSimulation (){
		for (int i = 0; i < _PseudoBlockList.Count; i++) {
            if(_PseudoBlockList[i] != null)
			    _PseudoBlockList [i].StartPlay();
		}

		_PseudoBlockList.RemoveAll (RemoveAllPredicate);
	}

	bool RemoveAllPredicate(PseudoBlock p){
		return true;
	}
}
