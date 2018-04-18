using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[ExecuteInEditMode]
public class Attacker : MonoBehaviour {

    public enum ScriptAssoc
    {
        Unknow,
        Shoot,
        PlaneShot
    }


    [SerializeField]
    [Tooltip("Script associado a esse atacante")]
    public ScriptAssoc scriptAssociado = ScriptAssoc.Unknow;
    [Tooltip("Alvo que o atacante vai atacar")]
    public FlexAttack.AttackTarget targetType = FlexAttack.AttackTarget.Capivaras;

    [Header("Camera")]
    [Tooltip("Tempo até o inimigo começar a animação de ataque")]
    public float timeTillAnimation = 0.5f;
    [Tooltip("Tempo que a câmera vai ficar parada no alvo e ele vai esperar para atirar")]
    public float animationTime = 2f;
    [Tooltip("Offset que a câmera vai possuir do centro do personagem")]
    public Vector2 cameraOffset = Vector2.zero;

    [Header("Placeholder")]
    public float AirstrikeVerticalDistance = 3f;

    [SerializeField]
    private bool addedToArray = false;
    [HideInInspector]
    public FlexAttack flexAttack;
    private EnemySimpleShoot shootScript;
    private PlaneShot planeScript;
    private Vector3 target;

    private void Awake()
    {
        planeScript = GetComponent<PlaneShot>();
        shootScript = GetComponent<EnemySimpleShoot>();
        if (planeScript != null)
        {
            scriptAssociado = ScriptAssoc.PlaneShot;
        }
        else if (shootScript != null)
        {
            scriptAssociado = ScriptAssoc.Shoot;
        }
        else if (Application.isPlaying)
        {
            DestroyImmediate(gameObject);    
        }

        //Verifica se o atacante está na lista, se não tiver, adiciona
        updateFlexAttackRef();
        if(!flexAttack.attackers.Contains(this))
            flexAttack.attackers.Add(this);
    }

    public void setupAttack()
    {
        this.target = target;
        //Tocar animação de começar a atirar

        StartCoroutine(waitToBeginAnimation());
    }

    /// <summary>
    /// Realiza a animação de ataque do inimigo
    /// </summary>
    private void playAttackAnimation()
    {
        //Logica de animacao

    }

    /// <summary>
    /// Realiza a animação de apresentação do inimigo
    /// </summary>
    public void playShowAnimation(){

    }

    /// <summary>
    /// Função chamada para atacar o alvo na posição enviada
    /// </summary>
    /// <param name="target">Posição que o atacante deve atacar</param>
    public void attackTarget(Vector3 target)
    {
        if(scriptAssociado == ScriptAssoc.Shoot)
        {
            shootScript.ShootAtTarget(target);
        }
        else if(scriptAssociado == ScriptAssoc.PlaneShot)
        {
            Vector3 verticalOffset = new Vector3(0, AirstrikeVerticalDistance);
            transform.position += verticalOffset;
            planeScript.AttackTarget(target);
        }
    }

    /// <summary>
    /// Tempo de espera até começar a animação de ataque
    /// </summary>
    /// <returns></returns>
    private IEnumerator waitToBeginAnimation()
    {
        float counter = timeTillAnimation;
        while(counter > 0)
        {
            counter -= Time.deltaTime;
            yield return null;
        }

        playAttackAnimation();
    }

    /// <summary>
    /// Atualiza referência do flex attack
    /// </summary>
    /// <returns>Retorna a referência do FlexAttack</returns>
    public FlexAttack updateFlexAttackRef()
    {
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
        if (gameManager != null)
            flexAttack = gameManager.GetComponent<FlexAttack>();
        return flexAttack;
    }


    //Verificações de consistência de ordem
    //Chamado toda vez que uma variável é alterada
    private void OnValidate()
    {
        if(flexAttack == null)
        {
            updateFlexAttackRef();
        }
        if(flexAttack.attackers == null)
        {
            flexAttack.attackers = new List<Attacker>();
        }
        if(Application.isEditor && !addedToArray)
        {
            addedToArray = true;
            flexAttack.attackers.Add(this);
        }

        if(scriptAssociado == ScriptAssoc.Unknow)
        {
            planeScript = GetComponent<PlaneShot>();
            shootScript = GetComponent<EnemySimpleShoot>();
            if(planeScript != null)
            {
                scriptAssociado = ScriptAssoc.PlaneShot;
            }
            else if (shootScript != null)
            {
                scriptAssociado = ScriptAssoc.Shoot;
            }
        }
    }



    private void OnDestroy()
    {
        if(flexAttack.attackers != null)
        {
            flexAttack.attackers.Remove(this);
        }
    }

}

[CustomEditor(typeof(Attacker))]
public class AttackerEditor : Editor
{
    private ReorderableList reorderableList;

    private Attacker attacker
    {
        get
        {
            return target as Attacker;
        }
    }

    private FlexAttack flexAttack;

    private void OnEnable()
    {
        flexAttack = attacker.updateFlexAttackRef();
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
        {
            if(item == attacker)
                EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, rect.height), item.gameObject.name + " (This)");
            else
                EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, rect.height), item.gameObject.name);
        }
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