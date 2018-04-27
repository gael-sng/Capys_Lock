using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;


public class FlexAttack : MonoBehaviour {

	public enum AttackType
	{
		Cannon,
		Piercing,
		Explosive,
		Airstrike,
		Napalm
	};

	public enum AttackTarget
	{
		Capivaras
	};

	public enum GameState
	{
		ShowingEnemy,
		ShowingMovement,
		Building,
		PlayerAnimation,
		CameraMovementToEnemy,
		EnemyAnimation,
		CameraFollowProjectile,
		ShowFinal,
		EndGame
	}

	[SerializeField]
	[HideInInspector]
	public List<Attacker> attackers;

	[Header("Camera")]
	[Tooltip("Tempo de movimento da camera entre pontos")]
	public float cameraMovementTime = 1f;
	[Tooltip("Tempo que a camera vai esperar após o projétil terminar a trajetoria")]
	public float projectileOvertime = 0.5f;
	[Tooltip("Tempo para tocar as animações do jogador(as capivaras)")]
	public float playerAnimationWaitTime = 2f;
	[Tooltip("Tempo que vai ficar em cada inimigo quando estiver mostrando eles")]
	public float waitOnEnemyTime = 1f;
	[Tooltip("Tempo que vai mostrar o final do jogo")]
	public float endWaitTime = 2f;

	[Header("Scene references")]
	public Transform cameraBuildPosition;

	[Header("Pontuation")]
	public float remainingCapivarasHealthWeight = 100;
	public float remainingMoneyWeight = 0;
	public float remainingConstructionsWeight = 0;


	//
	private List<Attacker> enemyOrder;
	private int currentEnemy;
	private int showIndex;
	private Transform lastProjectile;

	CameraMovement cameraScript;
	public GameState currentState = GameState.ShowingEnemy;

	private List<Capivara> capivaras;
	private List<GameObject> explosivos;


	private void Awake()
	{
		cameraScript = Camera.main.GetComponent<CameraMovement>();
		capivaras = new List<Capivara>();
		explosivos = new List<GameObject>();
		enemyOrder = new List<Attacker>();
	}

	// Use this for initialization
	void Start()
	{
		currentEnemy = 0;
		showIndex = 0;

		if (currentState == GameState.ShowingEnemy) {
			currentState = GameState.ShowingMovement;
			Vector3 offset = new Vector3(attackers[currentEnemy].cameraOffset.x, attackers[currentEnemy].cameraOffset.y, 0);
			Vector3 position = attackers[currentEnemy].transform.position + offset;
			cameraScript.goToLocation(position, cameraMovementTime);
			StartCoroutine(waitTime(cameraMovementTime));
			showIndex++;
		}

	}

	/// <summary>
	/// Função que começa o ataque
	/// </summary>
	/// <returns>Retorna se foi possível atacar</returns>
	public bool nextAttack()
	{
		Vector3 target = Vector3.zero;
		if (enemyOrder[currentEnemy].targetType == AttackTarget.Capivaras)
		{

			if (capivaras.Count < 1)
			{
				print("Não foi encontrado nenhum alvo");
				EndGame(true);
				return false;
			}

			//Choose closest capivara
			float lowerDistance = float.PositiveInfinity;
			int lowerIndex = -1;
			for(int i = 0; i < capivaras.Count; i++)
			{
				if (capivaras [i] != null) {
					Vector3 dif = capivaras[i].transform.position - enemyOrder[currentEnemy].transform.position;
					float newDistance = dif.sqrMagnitude;
					if (newDistance < lowerDistance)
					{
						lowerDistance = newDistance;
						lowerIndex = i;
					}
				}
			}

			target = capivaras[lowerIndex].transform.position;
		}

		enemyOrder[currentEnemy].attackTarget(target);

		currentEnemy++;
		return true;
	}

	/// <summary>
	/// Função chamada quando o jogador pressiona o botão para começar o ataque
	/// </summary>
	public void pressButton()
	{
		if(currentState == GameState.Building)
		{
			//Copiar lista
			for(int i = 0; i < attackers.Count; i++)
			{
				enemyOrder.Add(attackers[i]);
			}
			cameraScript.goToLocation(cameraBuildPosition.position, cameraMovementTime);

			//Começa animações e física
			for (int i = 0; i < capivaras.Count; i++)
			{
				capivaras[i].StartPlay();
			}

			//Activate physics for explosives
			for (int i = 0; i < explosivos.Count; i++)
			{
				Rigidbody2D rgb2d = explosivos[i].GetComponent<Rigidbody2D>();
				rgb2d.gravityScale = 1;
			}

			StartCoroutine(waitTime(playerAnimationWaitTime));
			currentState = GameState.PlayerAnimation;
		}

	}

	/// <summary>
	/// Pula a animação inicial de mostrar os inimigos
	/// </summary>
	public void skipAnimation()
	{
		if(currentState == GameState.ShowingEnemy || currentState == GameState.ShowingMovement){
			currentState = GameState.Building;
			StopAllCoroutines();
			cameraScript.goToLocation(cameraBuildPosition.position, cameraMovementTime);
		}
	}

	/// <summary>
	/// Função chamada toda vez que o timer estora. O timer é chamado entre transições de camera
	/// </summary>
	public void finishAnimation()
	{
		//Debug.Log("Transição de estado: " + currentState.ToString());
		//Lógica de mostrar os inimigos. Se ainda tem inimigos que não foram mostrados, vai mover a camera até ele
		if(currentState == GameState.ShowingEnemy)
		{
			//Enquanto ainda tiver atacantes não revelados
			if (showIndex < attackers.Count)
			{
				//Estado de movimento de camera até o inimigo
				currentState = GameState.ShowingMovement;
				//Calcula posição da câmera onde está o inimigo
				Vector3 offset = new Vector3(attackers[showIndex].cameraOffset.x, attackers[showIndex].cameraOffset.y, 0);
				Vector3 position = attackers[showIndex].transform.position + offset;
				cameraScript.goToLocation(position, cameraMovementTime);
				//Começa o timer
				StartCoroutine(waitTime(cameraMovementTime));
				showIndex++;
			}
			//Se não tem mais quem mostrar, mover a câmera para a posição inicial
			else
			{
				currentState = GameState.Building;
				cameraScript.goToLocation(cameraBuildPosition.position, cameraMovementTime);
			}
		}
		//Se a camera terminou de mover até o inimigo, vai realizar a animação de mostra do inimigo
		else if(currentState == GameState.ShowingMovement)
		{
			currentState = GameState.ShowingEnemy;

			//Indice foi alterado no estado anterior
			attackers[showIndex-1].playShowAnimation();

			StartCoroutine(waitTime(waitOnEnemyTime));
		}
		else if(currentState == GameState.PlayerAnimation || currentState == GameState.CameraFollowProjectile)
		{
			if(currentEnemy >= enemyOrder.Count)
			{
				EndGame(false);
			}
			else
			{
				currentState = GameState.CameraMovementToEnemy;
				Vector3 offset = new Vector3(enemyOrder[currentEnemy].cameraOffset.x, enemyOrder[currentEnemy].cameraOffset.y, 0);
				Vector3 position = enemyOrder[currentEnemy].transform.position + offset;
				cameraScript.goToLocation(position, cameraMovementTime);
				StartCoroutine(waitTime(cameraMovementTime));
			}
		}
		else if(currentState == GameState.CameraMovementToEnemy)
		{
			currentState = GameState.EnemyAnimation;
			enemyOrder[currentEnemy].setupAttack();
			float time = enemyOrder[currentEnemy].timeTillAnimation + enemyOrder[currentEnemy].animationTime;
			//Debug.Log("Wait time: " + time);
			StartCoroutine(waitTime(time));
		}
		else if(currentState == GameState.EnemyAnimation)
		{
			currentState = GameState.CameraFollowProjectile;
			nextAttack();
			if (enemyOrder[currentEnemy-1].scriptAssociado == Attacker.ScriptAssoc.PlaneShot)
			{
				PlaneShot planeScript = enemyOrder[currentEnemy-1].GetComponent<PlaneShot>();
				cameraScript.goToLocation(planeScript.target, planeScript.getFlyTime() + projectileOvertime);
				StartCoroutine(waitTime(planeScript.getFlyTime() + projectileOvertime));
			}
			else if (enemyOrder[currentEnemy-1].scriptAssociado == Attacker.ScriptAssoc.Shoot)
			{
				EnemySimpleShoot shootScript = enemyOrder[currentEnemy - 1].GetComponent<EnemySimpleShoot>();
				Transform lastProjectile = shootScript.projectile;
				if (lastProjectile != null)
				{
					cameraScript.followTargetForTime(lastProjectile.transform, shootScript.getFlyTime() + projectileOvertime);
					StartCoroutine(waitTime(shootScript.getFlyTime() + projectileOvertime));
				}
				else
				{
					cameraScript.goToLocation(shootScript.target, shootScript.getFlyTime() + projectileOvertime);
					StartCoroutine(waitTime(shootScript.getFlyTime() + projectileOvertime));
				}
			}
		}

	}

	private void EndGame(bool hasLost)
	{
		currentState = GameState.EndGame;
		if (cameraBuildPosition == null)
			return;
		cameraScript.goToLocation(cameraBuildPosition.position, cameraMovementTime);
		StartCoroutine(waitTime(cameraMovementTime + endWaitTime));
		if (hasLost)
			print("Perdeu");
		else
			print("Ganhou");

		//Calculate score
		float score = 0;

		//Score due to capivaras health remaining
		foreach (Capivara cap in capivaras)
		{
			Destructible dest = cap.GetComponent<Destructible>();
			if (dest != null)
			{
				score += remainingCapivarasHealthWeight * (dest.getActualLife() / dest.getMaxLife());
			}

		}

		//TODO: calculate remaining money

		//score due to remaining lives of buildings
		GameObject[] destructibles = GameObject.FindGameObjectsWithTag("Destructible");
		foreach (GameObject go in destructibles)
		{
			Destructible dest = go.GetComponent<Destructible>();
			if (dest != null)
			{
				if (dest.GetComponent<Capivara>() == null)
				{
					score += remainingConstructionsWeight * (dest.getActualLife() / dest.getMaxLife());
					print("Adicionou score: " + score);
				}
			}
		}


		EndScreenHUD hud = GetComponentInChildren<EndScreenHUD>();
		if (hud != null)
		{
			int roundedScore = (int)score;
			hud.endGame(hasLost, roundedScore);
		}

	}

	private IEnumerator waitTime(float finishTime)
	{
		float currentTime = finishTime;
		while (currentTime > 0)
		{
			currentTime -= Time.deltaTime;
			yield return null;
		}
		finishAnimation();
	}

	public void addCapivara(Capivara cap)
	{
		capivaras.Add(cap);
	}

	public void removeCapivara(Capivara cap)
	{
		capivaras.Remove(cap);
		if (capivaras.Count < 1)
		{
			EndGame(true);
		}
	}

	public List<Capivara> getCapivaras()
	{
		return capivaras;
	}

	public void addExplosive(GameObject explosive)
	{
		explosivos.Add(explosive);
	}

	// Update is called once per frame
	void Update () {
		if(currentState == GameState.ShowingEnemy || currentState == GameState.ShowingMovement){
			if(Input.GetMouseButtonDown(0)){
				skipAnimation();
			}
		}
	}
}

[CustomEditor(typeof(FlexAttack))]
public class FlexAttackEditor : Editor
{
	private ReorderableList reorderableList;

	private FlexAttack flexAttack
	{
		get
		{
			return target as FlexAttack;
		}
	}

	private void OnEnable()
	{
		reorderableList = new ReorderableList(flexAttack.attackers, typeof(Attacker), true, true, true, true);

		// This could be used aswell, but I only advise this your class inherrits from UnityEngine.Object or has a CustomPropertyDrawer
		// Since you'll find your item using: serializedObject.FindProperty("list").GetArrayElementAtIndex(index).objectReferenceValue
		// which is a UnityEngine.Object
		// reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("list"), true, true, true, true);

		// Add listeners to draw events
		reorderableList.drawHeaderCallback += DrawHeader;
		reorderableList.drawElementCallback += DrawElement;

		reorderableList.displayAdd = false;
		reorderableList.onRemoveCallback += RemoveItem;
	}

	private void OnDisable()
	{
		// Make sure we don't get memory leaks etc.
		reorderableList.drawHeaderCallback -= DrawHeader;
		reorderableList.drawElementCallback -= DrawElement;

		reorderableList.onRemoveCallback -= RemoveItem;
	}

	/// <summary>
	/// Draws the header of the list
	/// </summary>
	/// <param name="rect"></param>
	private void DrawHeader(Rect rect)
	{
		GUI.Label(rect, "Ordem dos ataques");
	}

	/// <summary>
	/// Draws one element of the list (ListItemExample)
	/// </summary>
	/// <param name="rect"></param>
	/// <param name="index"></param>
	/// <param name="active"></param>
	/// <param name="focused"></param>
	private void DrawElement(Rect rect, int index, bool active, bool focused)
	{
		Attacker item = flexAttack.attackers[index];

		EditorGUI.BeginChangeCheck();
		if (item != null)
			EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, rect.height), item.gameObject.name);
		else
			flexAttack.attackers.RemoveAt(index);
		if (EditorGUI.EndChangeCheck())
		{
			EditorUtility.SetDirty(target);
		}

		// If you are using a custom PropertyDrawer, this is probably better
		// EditorGUI.PropertyField(rect, serializedObject.FindProperty("list").GetArrayElementAtIndex(index));
		// Although it is probably smart to cach the list as a private variable ;)
	}


	private void RemoveItem(ReorderableList list)
	{
		Attacker a = list.list[list.index] as Attacker;
		list.list.RemoveAt(list.index);

		DestroyImmediate(a.gameObject);

		EditorUtility.SetDirty(target);
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		// Actually draw the list in the inspector
		reorderableList.DoLayoutList();
	}
}