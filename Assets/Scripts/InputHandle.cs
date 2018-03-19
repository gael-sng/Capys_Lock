using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandle : MonoBehaviour {
	//Atributos de seleção
	[SerializeField] private int MaxNumberSelected;
	private int currentNumberSelected;
	private bool selecting;
	private List<GameObject> SelectedTiles;
	
	// Use this for initialization
	void Start () {
		if (MaxNumberSelected <= 0) MaxNumberSelected = 4;
		currentNumberSelected = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Ray r = Camera.main.ScreenPointToRay (Input.mousePosition);
			GameObject Tile =  Physics2D.Raycast(r.origin, r.direction).collider.gameObject;
			//Verificar se esta clicando numa tile do spawn grid
			if (Tile != null && Tile.tag == "TileSpawn"){
				//Verificar se se essa tile esta livre

				//Se não stiver Livrar ele

			}
		}
		// Se estiver selecionando 
		if(selecting && Input.GetMouseButton(0)){
			Ray r = Camera.main.ScreenPointToRay (Input.mousePosition);
			GameObject Tile = Physics2D.Raycast(r.origin, r.direction).collider.gameObject;
			//Verificar se esta clicando numa tile do spawn grid
			if (Tile != null && Tile.tag == "TileSpawn") {
				//verificar se o bloco acertado ja não esta selecionado
				if(!SelectedTiles.Contains(Tile) && currentNumberSelected > 0){
					//Subtrair do contador maximo de bloco selecionados
					currentNumberSelected--;
					SelectedTiles.Add (Tile);
					//Selecionalo

				}
			}
		}
		//Se estiver levantando o botão do mouse
	}
		
}
