using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSchedule : MonoBehaviour {

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
		ShowingEnemies,
		Building,
		CameraMovementToEnemie,
		CameraFollowProjectile,
		EndGame
	}

	[Header("Attack settings")]
	//Attack order
	public AttackType[] AttackOrder;
	//Attack target
	public AttackTarget[] AttackTargets;
    public bool isAtRight = false;
	[Header("Cosmedic")]
	//Distance between attackers
	public float DistanceBetweenAttackers = 5f;
	public float AirstrikeVerticalDistance = 10f;

    public float cameraMovementTime = 1f;
    public float projectileOvertime = 0.5f;
    public float showWaitTime = 3f;

    [Header("Scene references")]
	public Transform attackPosition;
	public Transform cameraEnemyPosition;
	public Transform cameraBuildPosition;

	//Prefabs
	[Header("Prefabs")]	
	public GameObject CannonPrefab;
	public GameObject PiercingPrefab;
	public GameObject ExplosivePrefab;
	public GameObject AirstrikePrefab;
	public GameObject NapalmPrefab;

    [Header("Pontuation")]
    public float remainingCapivarasHealthWeight = 100;
    public float remainingMoneyWeight = 0;
    public float remainingConstructionsWeight = 0;


	//
	private List<GameObject> enemyLine;
	private int currentEnemy;
	private Transform lastProjectile;

	CameraMovement cameraScript;
	public GameState currentState = GameState.ShowingEnemies;

	private List<Capivara> capivaras;
    private List<GameObject> explosivos;


	private void Awake() {
		cameraScript = Camera.main.GetComponent<CameraMovement>();
		capivaras = new List<Capivara>();
        explosivos = new List<GameObject>();
	}

	// Use this for initialization
	void Start () {
		currentEnemy = 0;
		enemyLine = new List<GameObject>();
		int i = 0;


		Vector3 distancing = new Vector3 (DistanceBetweenAttackers,0,0);
        if (isAtRight)
            distancing.x *= -1;
		foreach(AttackType a in AttackOrder){
			GameObject enemySpawned;
			switch (a)
			{
				case AttackType.Cannon:
					enemySpawned = GameObject.Instantiate(CannonPrefab, attackPosition.position - distancing * i, Quaternion.identity, attackPosition);
					break;
				case AttackType.Explosive:
					enemySpawned = GameObject.Instantiate(ExplosivePrefab, attackPosition.position - distancing * i, Quaternion.identity, attackPosition);
					break;
				case AttackType.Piercing:
					enemySpawned = GameObject.Instantiate(PiercingPrefab, attackPosition.position - distancing * i, Quaternion.identity, attackPosition);
					break;
				case AttackType.Airstrike:
					enemySpawned = GameObject.Instantiate(AirstrikePrefab, attackPosition.position - distancing * i, Quaternion.identity, attackPosition);
                    if (isAtRight) {
                        PlaneShot planeScript = enemySpawned.GetComponent<PlaneShot>();
                        planeScript.horizontalSpeed *= -1;
                    }
                    break;
				case AttackType.Napalm:
					enemySpawned = GameObject.Instantiate(NapalmPrefab, attackPosition.position - distancing * i, Quaternion.identity, attackPosition);
					break;
				default:
					enemySpawned = null;
					break;
			}
			enemyLine.Add(enemySpawned);
			i++;
            if (isAtRight) {
                Vector3 scale = enemySpawned.transform.localScale;
                scale.x *= -1;
                enemySpawned.transform.localScale = scale;
            }

            if (currentState == GameState.ShowingEnemies)
            {
                //cameraScript.goToLocation(cameraEnemyPosition.position, 0.1f);
                Vector3 dest = cameraEnemyPosition.position;
                dest.z = -10;
                cameraScript.transform.position = dest;
                StartCoroutine(waitTime(showWaitTime));
            }
		}

	}
	
	/// <summary>
	/// Move enemies one position forward
	/// </summary>
	private void advanceEnemyPostions() {
		if(currentEnemy != 0) {
			Destroy(enemyLine[currentEnemy-1]);
			Vector3 offset = new Vector3(DistanceBetweenAttackers,0,0);
            if (isAtRight)
                offset.x *= -1; 
			for (int  i = currentEnemy; i < AttackOrder.Length; i++) {
				enemyLine[i].transform.position += offset;
			}
		}
	}

	public bool nextAttack() {
		//advanceEnemyPostions();

		if (currentEnemy >= AttackOrder.Length) return false;

		Vector3 target = Vector3.zero;
		if(AttackTargets[currentEnemy] == AttackTarget.Capivaras) {

			if(capivaras.Count < 1){ 
				print("Não foi encontrado nenhum alvo");
				return false;
			}
			int maxCapivaras = capivaras.Count;
			//Choose a random target
			target = capivaras[Random.Range(0, maxCapivaras)].transform.position;
		}

		if(AttackOrder[currentEnemy] != AttackType.Airstrike) {
			EnemySimpleShoot shootScript = enemyLine[currentEnemy].GetComponent<EnemySimpleShoot>();
			if(shootScript == null) return false;
			lastProjectile = shootScript.ShootAtTarget(target).transform;
		}
		else if(AttackOrder[currentEnemy] == AttackType.Airstrike) {
			Vector3 verticalOffset = new Vector3(0, AirstrikeVerticalDistance);
            print("Is at: " + enemyLine[currentEnemy].transform.position);
			enemyLine[currentEnemy].transform.position += verticalOffset;
            print("Now at: " + enemyLine[currentEnemy].transform.position);
			PlaneShot planeScript = enemyLine[currentEnemy].GetComponent<PlaneShot>();
			if(planeScript == null) return false;
			planeScript.AttackTarget(target);
		}
		currentEnemy++;
		return true;
	}

	public void pressButton() {
		if(currentState == GameState.Building) {
			cameraScript.goToLocation(cameraEnemyPosition.position, cameraMovementTime);
			currentState = GameState.CameraMovementToEnemie;
			StartCoroutine(waitTime(cameraMovementTime));
			for (int i = 0; i < capivaras.Count; i++) {
				capivaras [i].StartPlay ();
			}

            //Activate physics for explosives
            for (int i = 0; i < explosivos.Count; i++) {
                Rigidbody2D rgb2d = explosivos[i].GetComponent<Rigidbody2D>();
                rgb2d.gravityScale = 1;
            }
		}
	}

	public void finishAnimation() {
		if(currentState == GameState.CameraMovementToEnemie) {
			currentState = GameState.CameraFollowProjectile;
			nextAttack();
			if(AttackOrder[currentEnemy-1] == AttackType.Airstrike) {
				PlaneShot planeScript = enemyLine[currentEnemy-1].GetComponent<PlaneShot>();
				//cameraScript.followTargetForTime(planeScript.transform, planeScript.getFlyTime() + projectileOvertime);
				cameraScript.goToLocation(planeScript.target, planeScript.getFlyTime() + projectileOvertime);
				StartCoroutine(waitTime(planeScript.getFlyTime() + projectileOvertime));
			}
			else {
				EnemySimpleShoot shootScript = enemyLine[currentEnemy - 1].GetComponent<EnemySimpleShoot>();
				if(lastProjectile != null) {
					cameraScript.followTargetForTime(lastProjectile.transform, shootScript.getFlyTime() + projectileOvertime);
					StartCoroutine(waitTime(shootScript.getFlyTime() + projectileOvertime));
				}
				else {
					cameraScript.goToLocation(shootScript.target, shootScript.getFlyTime() + projectileOvertime);
					StartCoroutine(waitTime(shootScript.getFlyTime() + projectileOvertime));
				}
			}
		}
		else if(currentState == GameState.CameraFollowProjectile) {
            advanceEnemyPostions();
			if(currentEnemy < AttackOrder.Length) {
				currentState = GameState.CameraMovementToEnemie;
				cameraScript.goToLocation(cameraEnemyPosition.position, cameraMovementTime);
				StartCoroutine(waitTime(cameraMovementTime));
			}
			else {				
                EndGame(false);
			}
		}
        else if(currentState == GameState.ShowingEnemies)
        {
            cameraScript.goToLocation(cameraBuildPosition.position, cameraMovementTime);
            currentState = GameState.Building;
        }
	}

    private void EndGame(bool hasLost) {
        currentState = GameState.EndGame;
		if (cameraBuildPosition == null)
			return;
		cameraScript.goToLocation(cameraBuildPosition.position, cameraMovementTime);
        StartCoroutine(waitTime(cameraMovementTime));
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
            if (dest != null) {
                score += remainingCapivarasHealthWeight * (dest.getActualLife() / dest.getMaxLife());
            }

        }

        //TODO: calculate remaining money

        //score due to remaining lives of buildings
        GameObject[] destructibles = GameObject.FindGameObjectsWithTag("Destructible");
        foreach (GameObject go in destructibles)
        {
            Destructible dest = go.GetComponent<Destructible>();
            if (dest != null) {
                if(dest.GetComponent<Capivara>() == null)
                {
                    score += remainingConstructionsWeight * (dest.getActualLife() / dest.getMaxLife());
                    print("Adicionou score: " + score);
                }
            }
        }


        EndScreenHUD hud = GetComponentInChildren<EndScreenHUD>();
        if(hud != null)
        {
            int roundedScore = (int)score;
            hud.endGame(hasLost, roundedScore);
        }

    }

	private IEnumerator waitTime(float finishTime) {
		float currentTime = 0;
		while(currentTime < finishTime){
			currentTime += Time.deltaTime;
			yield return null;
		}
		finishAnimation();
	}

	public void addCapivara(Capivara cap) {
		capivaras.Add(cap);
	}

	public void removeCapivara(Capivara cap) {
		capivaras.Remove(cap);
        if(capivaras.Count < 1)
        {
            EndGame(true);
        }
	}

	public List<Capivara> getCapivaras() {
		return capivaras;
	}

    public void addExplosive(GameObject explosive) {
        explosivos.Add(explosive);
    }

}
