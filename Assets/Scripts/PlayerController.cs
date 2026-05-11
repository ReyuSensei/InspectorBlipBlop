using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using System.Runtime.CompilerServices;
using NUnit.Framework.Internal.Filters;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    //REFERENCIAS A COMPONENTES
    //DEFINICION DE VARIABLES
    //tipoAcceso tipoVariable nombreVariable valorDefecto;
    //CAMELCASE:
    //VARIABLES: Almacenamiento de info. todas las palabras juntas primera letra de cada palabra en may, menos la 1a ( variableEjemplo; )
    //FUNCIONES: Conjuntos de pasos para realizar acciones. todas las palabras juntas primera letra de cada palabra en may ( FuncionEjemplo(); )
    //CLASES/COMPONENTES: todas las palabras juntas primera letra de cada palabra en may ( ClaseEjemplo )
    // = ASIGNA == COMPARA

    private CharacterController characterController;  // private = variable | CharacterController = Clase | characterController = funcion;
    private PlayerInput playerInput;
    private Animator anim;

    [Header("GROUNDED")]
    private bool isGrounded;
    [SerializeField] //PARA QUE LAS VARIABLES SEAN PRIVADAS PERO SE VEAN EN EL EDITOR
    private GameObject groundChecker;
    [SerializeField]
    private float groundDistance;
    [SerializeField]
    private LayerMask groundLayer;

    [Header("MOVEMENT / JUMP")]
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float crouchSpeed;
    private float actualSpeed; //Se usa esta y se cambia a la velocidad del resto de variables de movimiento en lugar de usar esas
    private Vector2 moveInput;
    private Vector3 moveVelocity;
    private Vector3 gravityVelocity;
    public float gravity = -9.81f;
    private Quaternion rotation;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float jumpHeight;
    [SerializeField]
    private float jumpDistance;
    [SerializeField]
    private int maxJumps;
    private int jumpsCount;

    //VARIABLES DE SALTO QUE SERÁN CALCULADAS
    private float timeToApex;
    private float initialVelocity;
    private float requiredGravity;

    [Header("DASH")]
    private bool isDashing;
    [SerializeField]
    private float dashSpeed;
    [SerializeField]
    private float dashDuration; //EL TIEMPO QUE TIENE QUE DURAR EL DASH
    private float dashTime; //EL TIEMPO QUE LLEVO HACIENDO EL DASH 
    private Vector3 dashDirection;
    [SerializeField]
    private AnimationCurve dashCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    private float dashCurveMultiplier;

    [Header("CROUCH")]
    [SerializeField]
    private float crouchedHeight = 1.0f;
    [SerializeField]
    private float crouchedCenterY = 0.5f;
    [SerializeField]
    private float crouchTransitionSpeed = 8f;
    private float targetHeight;
    private Vector3 targetCenter;
    private float originalHeight; //ALTURA DE COLLIDER A TAMAÑO NORMAL
    private Vector3 originalCenter; //CENTRO DEL COLLIDER A TAMAÑO NORMAL
    private bool isCrouching;

    //REFERENCIAS A ACCIONES DE INPUT
    private InputAction moveAction;
    private InputAction sprintAction;
    private InputAction jumpAction;
    private InputAction cameraSwitchAction;
    private InputAction dashAction;
    private InputAction crouchAction;
    private InputAction blockAction;
    private InputAction meleeAction;
    private InputAction rangedAction;
    private InputAction interactAction;
    private InputAction pauseAction;

    [Header("CAMERA CONTROL")]
    private Camera mainCamera;
    private Vector3 camForward;
    private Vector3 camRight;
    [SerializeField]
    private CinemachineCamera orbitalCam;
    [SerializeField]
    private CinemachineCamera isometricCam;
    private bool orbitalCamActive = true;

    [Header("HEALTH")]
    [SerializeField]
    private float maxHealth;
    private float currentHealth;
    private bool isInvincible;
    [SerializeField]
    private float invincibleTime;   //INDICA EL TIEMPO QUE EL JUGADOR SERA INVENCIBLE
    private float invincibleTimer;  //INDICA CUANTO TIEMPO LLEVA SIENDO INVENCIBLE EL JUGADOR

    [Header("ATTACK BLOCK")]
    private bool isBlocking;
    [SerializeField]
    private float blockDuration;
    [SerializeField]
    private float blockCooldownDuration;
    private float blockTimer;
    private float blockCooldownTimer;
    [SerializeField]
    private GameObject blockSocket; //PLACEHOLDER PARA UN ENEMIGO O DAMAGE DEALER
    [SerializeField, Range(0,1)]
    private float blockDamageReduction;
    [SerializeField, Range(-1, 1)]
    private float blockDotAngleThreshold;

    [Header("KICK")]
    [SerializeField]
    private float minKickForce;
    [SerializeField]
    private float maxKickForce;
    [SerializeField]
    private float maxKickUpForce;

    [Header("PUSH")]
    [SerializeField]
    private float pushForce;
    [SerializeField]
    private float pushRayDistance;
    [SerializeField]
    private float pushRayPivotOffset;
    [SerializeField]
    private float pushSpeed;
    private bool isPushing;

    [Header("HAPTIC RESPONSES")]
    [SerializeField]
    private float pushRumbleLow;
    [SerializeField]
    private float pushRumbleHigh;
    private Gamepad gamepad;
    [SerializeField]
    private LayerMask pushLayer;

    [Header("MELEE ATTACK")]
    [SerializeField]
    private GameObject meleeWeapon;
    [SerializeField]
    private Collider meleeCollider;
    private bool isMeleeAttacking;

    [Header("RANGED ATTACK")]
    [SerializeField]
    private GameObject rangedWeapon;
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private Transform projectileSpawnPoint;
    private bool isRangedAttacking;

    [Header("AUTOAIM")]
    [SerializeField]
    private float enemyDetectionRadius;
    [SerializeField]
    private LayerMask enemyLayer;

    [Header("INTERACT")]
    [SerializeField]
    private float interactDistance;
    [SerializeField]
    private LayerMask interactLayer;

    [Header("SOUND")]
    [SerializeField]
    private AudioClip stepClip;
    public AudioSource audioSource;

    [Header("RAGDOLL")]
    private List<Rigidbody> ragdollRbs = new List<Rigidbody>();
    private List<Collider> ragdollCols = new List<Collider>();

    private void InitRagdoll()
    {
        //BUSCO TODOS LOS RBs DEL OBJETO
        Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>();
        //PARA CADA RB ME QUEDO CON SU COLLIDER, DESACTIVO LOS 2 Y LOS GUARDO EN LAS LISTAS
        foreach (Rigidbody rb in rbs)
        {
            rb.isKinematic = true;
            ragdollRbs.Add(rb);
            Collider c = rb.GetComponent<Collider>();
            c.enabled = false;
            ragdollCols.Add(c);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        anim = GetComponentInChildren<Animator>();
        mainCamera = Camera.main;
        

        moveAction = playerInput.actions["Move"];
        sprintAction = playerInput.actions["Sprint"];
        jumpAction = playerInput.actions["Jump"];
        cameraSwitchAction = playerInput.actions["CameraSwitch"];
        dashAction = playerInput.actions["Dash"];
        crouchAction = playerInput.actions["Crouch"];
        blockAction = playerInput.actions["Block"];
        meleeAction = playerInput.actions["MeleeAttack"];
        rangedAction = playerInput.actions["RangedAttack"];
        interactAction = playerInput.actions["Interact"];
        pauseAction = playerInput.actions["Pause"];

        originalHeight = characterController.height;
        originalCenter = characterController.center;
        currentHealth = maxHealth;

        blockSocket.SetActive(false);
        meleeWeapon.SetActive(false);
        meleeCollider = meleeWeapon.GetComponentInChildren<Collider>();
        meleeCollider.enabled = false;
        rangedWeapon.SetActive(false);

        GameManager.instance.playerController = this; //  LE DIGO AL GAMEMANAGER QUE EL GESTOR DE PLAYERCONTROLLER SOY YO, CREANDO UNA REFERENCIA INDIRECTA
        gamepad = Gamepad.current;  //ASIGNO EL GAMEPAD ACTUAL A LA VARIABLE DE GAMEPAD

        audioSource = GetComponent<AudioSource>();

        InitRagdoll();
    }

    // Update is called once per frame
    void Update()
    {
        //GESTION DE CONTADORES

        if (isInvincible == true)
        {
            invincibleTimer = invincibleTimer - Time.deltaTime;
            if (invincibleTimer <= 0)
            {
                isInvincible = false;
            }
        }
        /*
         if(invincibleTimer > 0)
        {
           invincibleTimer = invincibleTimer - Time.deltaTime;
        }
        else
        {
            isInvincible = false;
        }
        */

        CheckGrounded();
        HandleMovement();
        HandleJump();
        HandleCamera();
        HandleDash();
        HandleCrouch();
        HandleBlock();
        HandleMeleeInput();
        HandleRangedInput();
        HandlePush();
        HandleInteract();
        HandlePause();

    }

    private void HandlePause()
    {
        if (pauseAction.WasPressedThisFrame())
        {
            GameManager.instance.gameplayUI.ShowPause();
        }
    }

    //DECLARACION DE FUNCIONES
    //tipo_de_acceso tipo_variable_retorno nombre_funcion (parametros_entrada)
    private void HandleMovement()
    {
        if (isBlocking == true || isMeleeAttacking == true || isRangedAttacking == true)
        {
            return;
        }
        if (DialogueManager.instance.isActive)
        {
            anim.SetFloat("_speed", 0);
            return;
        }
        moveInput = moveAction.ReadValue<Vector2>();
        //PALABRAS MAGICAS:
        //transform: el transform del gameObject que tiene este script
        //gameObject: el gameObject que tiene este script. todos los obj dentro de una escena son gameObjects. Todos por defecto tienen un transform (da posicion, rotacion y escala)
        //PASO DEL PLANO XY DEL INPUT AL PLANO XZ DEL MUNDO 3D EN FUNCION DE LA ROTACION DEL PERSONAJE
        moveVelocity = camRight * moveInput.x + camForward * moveInput.y;
        //ROTO AL PERSONAJE EN FUNCION DE LA DIRECCION DE LA CAMARA
        //sqrMagnitude -> CALCULA LA MAGNITUD DEL VECTOR HACIENDO UNA APROXIMACION POR RAIZ CUADRADA
        if (moveVelocity.sqrMagnitude > 0.01f)
        {
            //CALCULO LA ROTACION NECESARIA PARA MIRAR HACIA LA DIRECCION DE MOVIMIENTO
            rotation = Quaternion.LookRotation(moveVelocity);
            //ROTO PROGRESIVAMENTE HACIA ESA DIRECCION
            //LERP = LINEAL INTERPOLATION
            //SLERP = SPHERIC LINEAL INTERPOLATION
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }

        //COMPRUEBO SI ESTOY ANDANDO O CORRIENDO O SI ESTOY QUIETO
        if (moveInput.sqrMagnitude > 0.1f)
        {
            if (isCrouching == false)
            {
                if (isPushing == true)
                {
                    actualSpeed = pushSpeed;
                }
                else if (sprintAction.IsPressed())
                {
                    actualSpeed = runSpeed;
                }
                else
                {
                    actualSpeed = walkSpeed;
                }
            }
        }

        else
        {
            actualSpeed = 0;
        }

        if (isDashing == false)
        {
            //LE DIGO AL CHARACTER CONTROLLER QUE SE MUEVA EN LA DIRECCION Y VELOCIDAD MARCADAS
            //DELTATIME CALCULA EL TIEMPO ENTRE EL FRAME ACTUAL Y EL ANTERIOR, SE USA PARA QUE SE MUEVA A LA MISMA VELOCIDAD EN TODOS LOS DISPOSITIVOS
            //DELTATIME = CUANTOS SEGUNDOS HAY EN UN FRAME (1/60 SI VA A 60FPS, 1/120 SI VA A 120 ETC)
            //normalized ME DEVUELVE EL VECTOR UNITARIO DE UN VECTOR, OSEA, DE MAGNITUD 1 (porque si no al ir en diagonal se suman ambos vectores, haciendo que vaya mas rapido
            characterController.Move(moveVelocity.normalized * actualSpeed * Time.deltaTime);
        }

        if (transform.parent == null)
        {
            gravityVelocity.y = gravityVelocity.y + (gravity * Time.deltaTime);
            characterController.Move(gravityVelocity * Time.deltaTime);
        }

        anim.SetFloat("_speed", actualSpeed);
        anim.SetBool("_push", isPushing);
    }

    //COMPRUEBO SI ESTOY TOCANDO EL SUELO O ALGO
    private void CheckGrounded()
    {
        isGrounded = Physics.Raycast(groundChecker.transform.position, Vector3.down, groundDistance, groundLayer);
        if (isGrounded == true && gravityVelocity.y < 0)
        {
            gravityVelocity.y = -0.5f;
            jumpsCount = 0;
        }
        if (transform.parent == null)
            anim.SetBool("_grounded", isGrounded);
    }

    //CONTROL DE SALTO
    private void HandleJump()
    {
        if (jumpAction.WasPressedThisFrame() && jumpsCount < maxJumps && isDashing == false && isInvincible == false && isBlocking == false && isRangedAttacking == false && isMeleeAttacking == false && !DialogueManager.instance.isActive)
        {
            //CALCULO EL TIEMPO QUE TARDARÍA EN LLEGAR A LA ALTURA MÁXIMA (APEX)
            //NO HACEMOS EL CÁLCULO FÍSICO REAL, HACEMOS UNA APROXIMACIÓN A LA FÓRMULA
            timeToApex = jumpDistance / (2 * runSpeed);
            //FÓRMULA SIMPLIFICADA PARA EL CÁLCULO DE DISTANCIAS
            //Mathf -> DA ACCESO A LA LIBRERÍA DE FUNCIONES MATEMÁTICAS
            //Pow -> PARA CALCULAR POTENCIAS (AQUÍ ABAJO SERIA timeToApex^2)
            requiredGravity = -(2 * jumpHeight) / Mathf.Pow(timeToApex, 2);
            //CALCULO LA VELOCIDAD INICIAL QUE NECESITARIA PARA EL SALTO
            //Sqrt -> PARA CALCULAR LA RAÍZ CUADRADA
            initialVelocity = Mathf.Sqrt(jumpHeight * -2.0f * requiredGravity);
            //+ SUMA NÚMEROS O CONCATENA TEXTOS (PONE UNO DESTRAS DEL OTRO)
            Debug.Log("VELOCIDAD SALTO: " + initialVelocity);
            Debug.Log("GRAVEDAD CALCULADA: " + requiredGravity);

            gravity = requiredGravity;
            gravityVelocity.y = initialVelocity;
            jumpsCount = jumpsCount + 1;
            anim.SetTrigger("_jump");

        }

    }

    //CONTROL DE CAMARA
    private void HandleCamera()
    {
            //CALCULO LA DIRECCION FORWARD Y RIGHT DE LA CAMARA
            camRight = mainCamera.transform.right;
            camForward = Vector3.ProjectOnPlane(mainCamera.transform.forward, Vector3.up);
            if (cameraSwitchAction.WasPressedThisFrame())
            {
                orbitalCamActive = !orbitalCamActive;
                if (orbitalCamActive == true)
                {
                    orbitalCam.Priority = 20;
                    isometricCam.Priority = 0;
                }
                else
                {
                    orbitalCam.Priority = 0;
                    isometricCam.Priority = 20;
                }

            }
    }

    //CONTROL DE DASH
    private void HandleDash()
    {
        if (dashAction.WasPressedThisFrame() && isDashing == false && isInvincible == false && isBlocking == false && isRangedAttacking == false && isMeleeAttacking == false && !DialogueManager.instance.isActive)
        {
            isDashing = true;
            dashTime = 0;
            dashDirection = transform.forward;
        }

        if (dashTime <= 1.0f)
        {
            dashTime = (dashTime + Time.deltaTime) / dashDuration;
            dashCurveMultiplier = dashCurve.Evaluate(Mathf.Clamp01(dashTime));
            characterController.Move(dashDirection * dashSpeed * dashCurveMultiplier * Time.deltaTime);

            if (isGrounded == false)
            {
                gravityVelocity.y = gravityVelocity.y / 2;
            }
        }
        else
        {
            isDashing = false;
        }
        anim.SetBool("_dashing", isDashing);
    }

    private void HandleCrouch()
    {
        if (crouchAction.IsPressed() && isInvincible == false && isBlocking == false && isRangedAttacking == false && isMeleeAttacking == false && !DialogueManager.instance.isActive)
        {
            isCrouching = true;
        }
        else
        {
            isCrouching = false;
        }
        if (gravityVelocity.y < -5.0f || gravityVelocity.y > 0.1f || isDashing)
        {
            isCrouching = false;
        }
        anim.SetBool("_crouch", isCrouching);

        if (isCrouching == true)
        {
            actualSpeed = crouchSpeed;
            targetHeight = crouchedHeight;
            targetCenter = new Vector3(originalCenter.x, crouchedCenterY, originalCenter.z);
        }
        else
        {
            targetHeight = originalHeight;
            targetCenter = originalCenter;
        }
        characterController.height = Mathf.Lerp(characterController.height, targetHeight, Time.deltaTime * crouchTransitionSpeed);
        characterController.center = Vector3.Lerp(characterController.center, targetCenter, Time.deltaTime * crouchTransitionSpeed);
    }

    private void HandleBlock()
    {
        //SI INTENTO BLOQUEAR Y NO ESTOY BLOQUEANDO Y NO ESTOY EN COOLDOWN
        if(blockAction.WasPressedThisFrame() && isBlocking == false && blockCooldownTimer <= 0 && isDashing == false && isGrounded == true && isInvincible == false && isCrouching == false && isRangedAttacking == false && isMeleeAttacking == false && !DialogueManager.instance.isActive)
        {
            isBlocking = true;
            blockTimer = blockDuration;
            //ACTIVO EL GAMEOBJECT DE BLOQUEO
            blockSocket.SetActive(true);
            //CREAR ANIMACIÓN!!!!!!!!!!!!!!!
            anim.SetBool("_block", true);
        }

        if (isBlocking == true)
        {
            blockTimer = blockTimer - Time.deltaTime;
            if (blockTimer <= 0)
            {
                isBlocking = false;
                blockCooldownTimer = blockCooldownDuration; //ACTIVO EL COOLDOWN
                blockSocket.SetActive(false);
                anim.SetBool("_block", false);
            }
        }

        if (blockCooldownTimer > 0)
        {
            blockCooldownTimer = blockCooldownTimer - Time.deltaTime;
        }
    }

    //BOOL: DEVUELVE TRUE SI BLOQUEO EL ATAQUE, FALSE SI NO CONSIGO BLOQUEARLO  //TRANSFORM ATTACKER: PARA FUNCIONAR LA FUNCIÓN NECESITA UN TRANSFORM, EN ESTE CASO DEL OBJETO ATTACKER
    private bool IsAttackBlocked(Transform attacker)
    {
        //EL VECTOR3 AL SER CREADO DENTRO DE ESTA FUNCIÓN SOLO EXISTE DENTRO DE ESTA FUNCIÓN
        //ATTACKER POS. POSICIÓN DEL ATACANTE   //TRANSFORM POS. POSICIÓN DEL JUGADOR
        Vector3 directionToAttacker = (attacker.position - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, directionToAttacker);

        return dot > blockDotAngleThreshold;
    }

    public void DoDamage(GameObject other)
    {
        //COMPRUEBO SI EL OBJETO QUE HA HECHO OVERLAP TIENE LA ETIQUETA DE DAMAGE
        if (other.CompareTag("Damage"))
        {
            if (isInvincible == true)
            {
                return; //SALE DE LA FUNCION
            }

            CancelActions();
            //DEFINO UNA VARIABLE LOCAL DAMAGEDEALER Y LE ASIGNO EL VALOR DEL GETCOMPONENT
            //EL OTHER. PARA BUSCAR EL COMPONENTE DENTRO DEL COLLIDER OTHER EN VEZ DE DENTRO DEL SCRIPT PLAYERCONTROLLER
            //LO QUE TENGA QUE VER CON EL OBJETO QUE HACE DAÑO NO LO GESTIONA EL PLAYER, SINO EL PROPIO OBJETO
            DamageDealer dmg = other.GetComponent<DamageDealer>();
            //CREO UNA VARIABLE PARA GUARDAR EL DAÑO QUE DEBERIA RECIBIR
            float damageValue = dmg.damageValue; //CON DMG. ENTRO DENTRO DEL COMPONENTE DAMAGEDEALER Y SACO EL DAMAGEVALUE
            //COMPRUEBO SI ESTOY BLOQUEANDO Y ESTE HA SIDO EFECTIVO
            if (isBlocking == true && IsAttackBlocked(other.transform) == true)
            {
                damageValue = damageValue * blockDamageReduction;
                //OBLIGO AL JUGADOR A DEJAR DE BLOQUEAR
                blockTimer = 0;
            }

            //LE RESTO A LA VIDA EL DAÑO QUE DEBERÍA RECIBIR
            currentHealth = currentHealth - damageValue;
            //currentHealth -= dmg.damageValue; MISMO CODIGO PERO SIN TENER QUE ESCRIBIR EL CURRENTHEALTH OTRA VEZ, SE USA -= += ETC
            //GameManager.instance.UpdateHealthBar(currentHealth / maxHealth);
            Debug.Log(currentHealth);

            if (currentHealth <= 0)
            {
                EnableRagdoll();
            }
            else
            {
                anim.SetTrigger("_damage");
                isInvincible = true;
                invincibleTimer = invincibleTime;
            }

        }

    }

    private void HandlePush()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * pushRayPivotOffset;

        //out ME PERMITE MODIFICAR EL VALOR DE UNA VARIABLE DENTRO DE UNA FUNCION
        if(Physics.Raycast(rayOrigin, transform.forward, out hit, pushRayDistance, pushLayer))
        {
            //hit CONTIENE LA INFORMACION DE IMPACTO, ENTRE OTRAS COSAS EL COLLIDER DEL OBJETO CONTRA EL QUE HA IMPACTADO
            //COMPRUEBO QUE EL OBJETO TENGA RIGIDBODY Y NO SEA KINEMATIC
            Rigidbody pushRb = hit.collider.GetComponent<Rigidbody>();
            if(pushRb != null && pushRb.isKinematic == false)
                {
                    //CALCULO LA DIRECCION DEL PLAYER AL OBJETO
                    Vector3 pushObjectDirection = (hit.collider.transform.position - transform.position).normalized;
                    //MIRO SI EL OBJETO ESTA ALINEADO CON EL FORWARD DEL PLAYER
                    float dot = Vector3.Dot(transform.forward, pushObjectDirection);
                    if(dot > 0.7f)
                    {
                        isPushing = true;

                        if(moveVelocity.z != 0) //Mathf.Abs(moveVelocity.z) >= 0.1f
                        {
                            pushRb.AddForce(transform.forward * pushForce, ForceMode.Force);    //MODO DE FUERZA FORCE PORQUE SERA UNA FUERZA CONSTANTE
                            StartRumble();
                        }
                        return;
                    }
                }
        }
        isPushing = false;
        StopRumble();
    }

    private void HandleInteract()
    {
        if (interactAction.WasPressedThisFrame() == true && !isMeleeAttacking && !isRangedAttacking && !isBlocking && !isDashing && isGrounded == true && !isCrouching && !isBlocking && !DialogueManager.instance.isActive)
        {
            RaycastHit hit; //VARIABLE PARA GUARDAR EL RESULTADO DEL RAYCAST
            Vector3 rayOrigin = transform.position + Vector3.up * pushRayPivotOffset;
            if (Physics.Raycast(rayOrigin, transform.forward, out hit, interactDistance, interactLayer))
            {
                hit.collider.GetComponent<IInteractable>().Interact();
            }
        }
    }

    private void StartRumble()
    {
        if(gamepad != null)
        {
            gamepad.SetMotorSpeeds(pushRumbleLow, pushRumbleHigh);
        }
    }

    private void StopRumble()
    {
        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(0, 0);
        }
    }

    private void CancelActions()
    {
        isDashing = false;
        dashTime = 0;
        anim.SetBool("_dashing", false);

        isCrouching = false;
        anim.SetBool("_crouch", false);

        if(gravityVelocity.y > 0)
        {
            gravityVelocity.y = 0;
        }

    }

    private void HandleMeleeInput()
    {
        if(meleeAction.WasPressedThisFrame() && !isMeleeAttacking && !isRangedAttacking && !isBlocking && !isDashing && isGrounded && !DialogueManager.instance.isActive)
        {
            isMeleeAttacking = true;
            anim.SetTrigger("_melee");
        }
    }

    private void HandleRangedInput()
    {
        if(rangedAction.WasPressedThisFrame() && !isMeleeAttacking && !isRangedAttacking && !isBlocking && !isDashing && isGrounded && !DialogueManager.instance.isActive)
        {
            isRangedAttacking = true;
            anim.SetTrigger("_ranged");
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Kickable"))
        {
            Rigidbody kickRb = hit.gameObject.GetComponent<Rigidbody>();
            if(kickRb != null && kickRb.isKinematic == false)
            {
                Vector3 kickDirection = (hit.gameObject.transform.position - transform.position).normalized;
                kickDirection.y = Random.Range(0, maxKickForce);
                kickRb.AddForce(kickDirection * Random.Range(minKickForce, maxKickForce), ForceMode.Impulse);
            }
        }
    }

    public void EventShowMeleeWeapon()
    {
        meleeWeapon.SetActive(true);    //LOS GAMEOBJECTS SE ACTIVAN/DESACTIVAN CON UN .SETACTIVE
    }

    public void EventEnableMeleeCollider()
    {
        meleeCollider.enabled = true;   //LOS COMPONENTES SE ACTIVAN/DESACTIVAN CON .ENABLED
    }

    public void EventDisableMeleeCollider()
    {
        meleeCollider.enabled = false;
    }

    public void EventHideMeleeWeapon()
    {
        meleeWeapon.SetActive(false);
        isMeleeAttacking = false;
    }

    public void EventShowRangeWeapon()
    {
        rangedWeapon.SetActive(true);
    }

    public void EventFireRangeWeapon()
    {
        //CREO UN NUEVO PROYECTIL GENERANDO UNA INSTANCIA DE PREFAB, EN UN PUNTO CONCRETO Y CON UNA ROTACION CONCRETA
        GameObject projectileGO = Instantiate(projectilePrefab, projectileSpawnPoint.transform.position, Quaternion.identity); //QUATERNION.IDENTITY CREA UN QUATERNION CON LA ROTACION ORIGINAL DEL PREFAB
        Projectile projectile = projectileGO.GetComponent<Projectile>();
        if(GameManager.instance.autoAimActive == true)
        {
            Collider[] enemies = Physics.OverlapSphere(transform.position, enemyDetectionRadius, enemyLayer);
            if(enemies.Length > 0)
            {
                float distance = enemyDetectionRadius;  //COMPARO LA DISTANCIA DE CADA ENEMIGO CON LA LONGITUD DEL RADIO DE DETECCION
                int enemyIndex = 0;
                for(int i = 0; i < enemies.Length; i ++)    //POR CADA ELEMENTO DEL INDEX
                {
                    if(Vector3.Distance(transform.position, enemies[0].transform.position) < distance)   //COMPRUEBO QUE LA DISTANCIA DEL ENEMIGO SEA MENOR AL RANGO DE DETECCION
                    {
                        distance = Vector3.Distance(transform.position, enemies[0].transform.position);  //CONVIERTO LA DISTANCIA DEL ENEMIGO EN EL PUNTO DE COMPARACION PARA LA DISTANCIA DEL SIGUIENTE
                        enemyIndex = i;
                    }
                }
                projectile.attackDirection = (enemies[enemyIndex].transform.position - transform.position).normalized;
            }
            else
            {
                projectile.attackDirection = transform.forward;
            }
        }
        else
        {
            projectile.attackDirection = transform.forward;
        }
        if(projectile != null)
        {
            projectile.AttackNow();
        }
    }

    public void EventHideRangeWeapon()
    {
        rangedWeapon.SetActive(false);
        isRangedAttacking = false;
    }

    public void EventStep()
    {
        Play3DSound(stepClip);
    }

    public void Play3DSound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    [ContextMenu("Ragdoll")]
    private void EnableRagdoll()
    {
        playerInput.enabled = false;
        //DESACTIVO EL ANIMATOR
        anim.enabled = false;
        //ACTIVO TODOS LOS RBs Y COLs DEL RAGDOLL
        foreach(Rigidbody rb in ragdollRbs)
        {
            rb.isKinematic = false;
        }
        foreach(Collider c in ragdollCols)
        {
            c.enabled = true;
        }
    }
}