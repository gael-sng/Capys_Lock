using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGridBehaviour : MonoBehaviour {
	[SerializeField] private int _MaxNumberSelected = 4;
	private int _currentNumberSelected;
	private bool _selecting;

	private List<SpawnTileBehaviour> SelectedTiles;

	public List<GameObject> Blocks;
	// Use this for initialization
	void Start () {
		_selecting = false;
		_currentNumberSelected = _MaxNumberSelected;
		SelectedTiles = new List<SpawnTileBehaviour> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (_selecting && Input.GetMouseButtonUp (0)) {
			_TrySpawn ();
			_ResetSelection ();
		}
	}

	public bool IsSeleting(){
		return _selecting;
	}

	bool _TrySpawn(){
		//Verificar se a seleção é valida

		//spawnar um bloco valido

		return true;
	}
		
	public bool _DecressCurrentNumberSelected(){
		if (_currentNumberSelected <= 0)
			return false;
		_currentNumberSelected--;
		return true;
	}

	public bool _SelectTile(SpawnTileBehaviour tile){
		if (!IsSeleting ()) {
			_selecting = true;
		}
		if (_DecressCurrentNumberSelected ()) {
			SelectedTiles.Add (tile);
			return true;
		}
		return false;
	}


	public void _ResetSelection(){
		//Chamar desselecionar as tiles
		SelectedTiles.Clear();
		//Apagar os elementos da lista
		_selecting = false;
		_currentNumberSelected = _MaxNumberSelected;
	}
}
