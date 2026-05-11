using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //VARIABLES DE COMPORTAMIENTO
    enum E_ATTACKMOVEMENT {FORWARD, PARABOLIC, STATIC} //UN ENUMERADOR FUNCIONA COMO UN LISTADO DE ETIQUETAS
    [SerializeField]
    private E_ATTACKMOVEMENT attackMovementMode;
    [SerializeField]
    private float timeToDestroy = 3;
    private Rigidbody rb;

    //VARIABLES DE ATAQUE
    public Vector3 attackDirection;
    [SerializeField]
    private float attackForce;
    [SerializeField]
    private List<string> avoidAttackTags;
    [SerializeField]
    private List<string> damageTags;
    private Collider col;
    [SerializeField]
    private float parabolicColliderSizeMultiplier;

    //SISTEMA DE PARTICULAS
    [SerializeField]
    private GameObject vfxPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (attackMovementMode == E_ATTACKMOVEMENT.STATIC)
        {
            return;
        }

        Destroy(gameObject, timeToDestroy); //GAMEOBJECT HACE REFERENCIA AL GAMEOBJECT QUE TENGA ESTE SCRIPT
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }
    public void AttackNow()
    {
        if (rb==null)
        {
            rb = GetComponent<Rigidbody>();
        }
        if (col == null)
        {
            col = GetComponent<Collider>();
        }
        if (attackMovementMode == E_ATTACKMOVEMENT.FORWARD)  //SI VOY A LANZAR RECTO HACIA ADELANTE...
        {
            //EL ATAQUE SIEMPRE SERÁ HACIA DELANTE EXCEPTO SI TIENE AUTOAIM, QUE -SI ES EL PLAYER- BUSCARÁ AL ENEMIGO MAS CERCANO
            //attackDirection = GameManager.instance.playerController.transform.forward;
            rb.isKinematic = false; //DESACTIVO EL KINEMATIC Y LA GRAVEDAD PARA QUE NO CAIGA PERO SE MUEVA POR LA FUERZA DE DISPARO
            rb.useGravity = false;
            rb.AddForce(attackDirection * attackForce, ForceMode.Impulse);
        }
        if (attackMovementMode == E_ATTACKMOVEMENT.PARABOLIC)  //SI VOY A LANZAR CON UN MOVIMIENTO PARABOLICO...
        {
            attackDirection = (GameManager.instance.playerController.transform.forward + (GameManager.instance.playerController.transform.up * 1).normalized * attackForce);
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddForce(attackDirection * attackForce, ForceMode.Impulse);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //COMPRUEBO SI EL TAG DEL OBJETO QUE HA HECHO OVERLAP ESTA DENTRO DE LA LISTA A IGNORAR
        if(avoidAttackTags.Contains(other.tag) == true)
        {
            //SI TENGO QUE IGNORARLO HAGO RETURN Y SALGO DE LA FUNCION
            return;
        }
        if(attackMovementMode == E_ATTACKMOVEMENT.PARABOLIC)
        {
            Destroy(col);
            SphereCollider newCol = gameObject.AddComponent<SphereCollider>();
            newCol.center = Vector3.zero;
            newCol.isTrigger = true;
            newCol.radius = parabolicColliderSizeMultiplier;
            attackMovementMode = E_ATTACKMOVEMENT.FORWARD;
        }
        else
        {
            if(damageTags.Contains(other.tag) == true)
            {
                other.gameObject.SendMessage("DoDamage", gameObject);
                if (attackMovementMode == E_ATTACKMOVEMENT.FORWARD)
                {
                    Destroy(gameObject);
                }
                Instantiate(vfxPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                if (attackMovementMode == E_ATTACKMOVEMENT.FORWARD)
                {
                    Destroy(gameObject);
                }
                if (vfxPrefab != null)
                {
                    Instantiate(vfxPrefab, transform.position, Quaternion.identity);
                }
            }
        }
    }
}