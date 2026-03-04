using UnityEngine;

public class Shield : MonoBehaviour
{
    private Renderer renderer; //EL COMPONENTE QUE RENDERIZA EL OBJETO
    [SerializeField]
    private float displacementForce; //VARIABLE PARA INDICAR LA FUERZA MAXIMA DE LA DEFORMACION
    [SerializeField]
    private AnimationCurve displacementCurve; //CURVA DE ANIMACION PARA EL EFECTO DE RECUPERACION DE LA DEFORMACION
    [SerializeField]
    private float displacementSpeed;   //VELOCIDAD DE RECUPERACION DE LA ANIMACION
    private float displacementLerpValue;    //TIEMPO DE EVALUACION DE LA CURVA
    private bool shieldOn;  //SEMAFORO PARA SABER SI ESTA EL ESCUDO ACTIVADO

    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    public void Hit(Vector3 hitPos)
    {
        renderer.material.SetVector("_HitPos", hitPos);
        shieldOn = true;
    }

    private void HandleHit()
    {
        if (shieldOn == true)
        {
            //MIENTRAS VAYA HACIENDO LA ANIMACION
            if (displacementLerpValue < 1)
            {
                //MODIFICO EL VALOR DE DESPLAZAMIENTO MEDIANTE UN LERP Y LO ANIMO SEGUN LA CURVA
                renderer.material.SetFloat("_DisplacementStrength", displacementCurve.Evaluate(displacementLerpValue) * displacementForce);
                displacementLerpValue = displacementLerpValue + Time.deltaTime * displacementSpeed;
            }
            else
            {
                //CUANDO EL LERP ACABA DESACTIVO EL ESCUDO DE FORMA INTERNA
                shieldOn = false;
                displacementLerpValue = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Damage"))
        {
            //LE PASO A LA FUNCION DE HIT EL PUNTO MAS CERCANO DEL COLLIDER
            Hit(other.ClosestPoint(transform.position));
        }
    }

    void Update()
    {
        HandleHit();
    }
}
