using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGridBehaviour : MonoBehaviour {
	[SerializeField] List<GameObject> _ListOfBlocks;

	//Variaveis voltada para o controle de recursos disponiveis 
		//(numero de blocos disponiveiws a serem utilizados)
	[SerializeField] int _MaxNumberOfBlocks = 20;
	private int _ActualNumberOfBlocks;


	//Variaveis voltadas para a seleção da grid
	[SerializeField] int _MaxNumberSelected = 4;
	private bool _selecting;
	private List<SpawnTileBehaviour> _SelectedTiles;

	// Use this for initialization
	void Start () {
		_ActualNumberOfBlocks = 0;
		_selecting = false;
		_SelectedTiles = new List<SpawnTileBehaviour> ();
		if (_MaxNumberSelected <= 0)
			_MaxNumberSelected = 4;
		if (_MaxNumberOfBlocks <= 0)
			_MaxNumberOfBlocks = 20;
	}
	
	// Update is called once per frame
	void Update () {
		if (_selecting && Input.GetMouseButtonUp (0) && _SelectedTiles.Count > 0) {
			_TrySpawn ();
			_ResetSelection ();
		}
	}

	public bool IsSeleting(){
		return _selecting;
	}

	public void DecressActualNumberOfBlocks(){
		_ActualNumberOfBlocks--;
	}

	bool _TrySpawn(){
		float meanX = 0f;
		float meanY = 0f;

		//Achando o pivot e calculando a media dos blocos selecinados e verificando se ja esses blocos ja estão ocupados
		//O pivot sera as cordenadas deltaX deltaY que seram utilizadad para calcular a posição na matriz de boleano
		float deltaX = _SelectedTiles[0].transform.position.x;
		float deltaY = _SelectedTiles[0].transform.position.y;
		for (int i = 0; i < _SelectedTiles.Count; i++) {
			//Verificando se ja não tem um bloco spawnado nesta posição
			if (_SelectedTiles [i]._IsOccupied)
				return false;
			//Calculando a média das posições
			meanX += _SelectedTiles [i].transform.position.x;
			meanY += _SelectedTiles [i].transform.position.y;
			//Selecionando o pivot
			if (_SelectedTiles [i].transform.position.x < deltaX){
				deltaX = (int)_SelectedTiles[i].transform.position.x;
			}
			if (_SelectedTiles [i].transform.position.y < deltaY) {
				deltaY = (int)_SelectedTiles [i].transform.position.y;
			}
		}

		meanX /= _SelectedTiles.Count;
		meanY /= _SelectedTiles.Count;

		//Converter os tiles para uma matriz
		bool[,] matrix = new bool[_SelectedTiles.Count, _SelectedTiles.Count];

		for (int i = 0; i < _SelectedTiles.Count; i++) {
			int x = (int)(_SelectedTiles [i].transform.position.x - deltaX); 
			int y = (int)(_SelectedTiles [i].transform.position.y - deltaY); 
			if (0 <= x && x < _SelectedTiles.Count && 0 <= y && y < _SelectedTiles.Count) {
				matrix [x, y] = true;
			} else {
				Debug.Log ("ERRO em _TrySpawn codigo 0");
				return false;
			}
		}
		int Blockindex = -1;
		bool WillRotate = false;
		switch (_SelectedTiles.Count) {
		case 1:
			//instanciar bloco 1x1
			Debug.Log ("1x1");
			Blockindex = 0;
			break;
		case 2:
			//instanciar bloco 2x1 ou  1x2
			if(matrix[0,0] && matrix[0,1]){
				Debug.Log("1x2");
				Blockindex = 1;
				WillRotate = true;
			}
			if(matrix[0,0] && matrix[1,0]){
				Debug.Log("2x1");
				Blockindex = 1;
			}
			break;
		case 3:
			//instanciar bloco 3x1 ou 1x3
			if(matrix[0,0] && matrix[0,1] && matrix[0,2]){
				Debug.Log("1x3");
				Blockindex = 2;
				WillRotate = true;
			}
			if(matrix[0,0] && matrix[1,0] && matrix[2,0]){
				Debug.Log("3x1");
				Blockindex = 2;
			}
			break;
		case 4:
			if(matrix[0,0] && matrix[0,1] && matrix[0,2] && matrix[0,3]){
				Debug.Log("1x4");
				Blockindex = 3;
				WillRotate = true;
			}
			if(matrix[0,0] && matrix[1,0] && matrix[2,0] && matrix[3,0]){
				Debug.Log("4x1");
				Blockindex = 3;
			}
			if(matrix[0,0] && matrix[1,0] && matrix[0,1] && matrix[1,1]){
				Debug.Log("2x2");
				Blockindex = 4;
			}
			//instanciar bloco 4x1 ou 1x4 ou 2x2
			break;
		default:
			break;
		}
		//verificando se encontrou algum bloco valido
		if(Blockindex < 0)return false;

		//Spawnar um bloco valido
		PseudoBlock block = Instantiate (_ListOfBlocks [Blockindex]).GetComponent<PseudoBlock>();
		block.transform.position = new Vector3 (meanX,meanY,0);
		if (WillRotate)	block.transform.Rotate (0, 0, 90f);
		for (int i = 0; i < _SelectedTiles.Count; i++) {
			block._ListOfTiles.Add (_SelectedTiles[i]);
			//_SelectedTiles [i]._IsOccupied = true;
			_SelectedTiles [i].gameObject.SetActive (false);
		}
		block._MyGrid = this;

		//adicionando o numero de blocos a contagem de blocos
		_ActualNumberOfBlocks += _SelectedTiles.Count;
		return true;
	}

	public bool _SelectTile(SpawnTileBehaviour tile){
		//verificar se não estou tentando selecionar uma tile que ja foi selecionada
		if (_SelectedTiles.Contains (tile)) {
			//se ja tiver selecionado, desselecioanr todas as selecionadas depois desta
			int ActualIndex = _SelectedTiles.IndexOf (tile);
			while (_SelectedTiles.Count - 1 != ActualIndex){
				_SelectedTiles [_SelectedTiles.Count - 1].UnSelect ();
				_SelectedTiles.RemoveAt (_SelectedTiles.Count - 1);
			}
			return false; 
		}
		//Se for possivel adicionar então adicionar
		if (_SelectedTiles.Count < _MaxNumberSelected && _SelectedTiles.Count + _ActualNumberOfBlocks < _MaxNumberOfBlocks) {
			_SelectedTiles.Add (tile);
			if (!IsSeleting ()) {
				_selecting = true;
			}
			return true;
		}
		return false;
	}
		
	public void _ResetSelection(){
		//Chamar desselecionar as tiles
		for (int i = 0; i < _SelectedTiles.Count; i++){
			_SelectedTiles [i].UnSelect ();
		}
		//Apagar os elementos da lista
		_SelectedTiles.Clear();

		_selecting = false;
	}
}
