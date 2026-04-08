using UnityEngine;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class Player_Gr2 : MonoBehaviour
{
    // class field
    [Header("Motion")]
    [SerializeField] private float m_TranslationSpeed = 1.0f; // m.s-1

    [Tooltip("en �.s-1")]
    [SerializeField] float m_RotationSpeed = 1.0f;  // �.s-1

    // camel case ->translationSpeed
    // upper camel case , Pascal case -> TranslationSpeed

    Rigidbody m_Rb;

    [Header("Ball Shooting")]
    [SerializeField] GameObject m_BallPrefab;
    [SerializeField] Transform m_BallSpawnPos;
    [SerializeField] float m_BallShootSpeed = 20.0f;
    [SerializeField] float m_BallLifeTime = 3.0f;
    [SerializeField] float m_ShootingPeriod = .25f;

    float m_NextShootingTime;

    private void Awake()
    {
        m_Rb = GetComponent<Rigidbody>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_NextShootingTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        float vInput = Input.GetAxis("Vertical");
        float hInput = Input.GetAxis("Horizontal");

        // kinematic behaviour
        // transform, Update(), Time.deltaTime
        // l'objet change de position et d'orientation imm�diatement apr�s l'appel aux m�thodes Translate et Rotate

        Vector3 moveWorldVect = vInput * transform.forward * m_TranslationSpeed * Time.deltaTime;

        // transform.Translate(transform.forward* m_TranslationSpeed * Time.deltaTime,Space.World);    // r�f�rentiel du monde

        //transform.Translate(moveLocalVect, Space.Self); // r�f�rentiel du Player
        transform.position+= moveWorldVect;

        transform.Rotate(Vector3.up, hInput*m_RotationSpeed * Time.deltaTime, Space.Self); 
        */

        if (Input.GetButton("Fire1") && Time.time > m_NextShootingTime)
        {
            m_NextShootingTime = Time.time + m_ShootingPeriod;

            GameObject newBallGO = Instantiate(m_BallPrefab, m_BallSpawnPos.position, Quaternion.identity);
            Rigidbody ballRb = newBallGO.GetComponent<Rigidbody>();
            ballRb.linearVelocity = m_BallSpawnPos.forward * m_BallShootSpeed;
            Destroy(newBallGO, m_BallLifeTime);
        }

    }

    private void FixedUpdate()
    {
        // dynamic (physic) behaviour
        // PhysX (nvidia)
        // rigidbody, FixedUpdate(), Time.fixedDeltaTime
        // l'objet n'aura chang� de position et/ou d'orientation qu'� la frame prochaine

        float vInput = Input.GetAxis("Vertical");
        float hInput = Input.GetAxis("Horizontal");

        Vector3 moveWorldVect = vInput * transform.forward * m_TranslationSpeed * Time.fixedDeltaTime;

        // POSITION MODE AVEC REDRESSEMENT DYNAMIQUE -> t�l�portation
        // cocher Freeze Rotation
        // n'annule pas l'inertie physique de l'objet
        // les frottements entre objets ne sont pas pris en compte

        
        m_Rb.MovePosition(m_Rb.position + moveWorldVect);

        Quaternion qUprightRot = Quaternion.FromToRotation(transform.up, Vector3.up);
        Quaternion qUprightOrient = qUprightRot * m_Rb.rotation;

        Quaternion qUprightLerpedOrient = Quaternion.Lerp(m_Rb.rotation, qUprightOrient, Time.fixedDeltaTime*6);

        Quaternion qRot = Quaternion.AngleAxis(hInput * m_RotationSpeed * Time.fixedDeltaTime, transform.up);
        Quaternion newOrient = qRot * qUprightLerpedOrient;

        m_Rb.MoveRotation(newOrient);

        m_Rb.linearVelocity = Vector3.zero;
        m_Rb.angularVelocity = Vector3.zero;
        

        // VELOCITY MODE -> d�cocher Freeze Rotation
        // prise en compte des frottements entre objets
        //Vector3 targetVelocity = vInput * transform.forward * m_TranslationSpeed;
        //m_Rb.AddForce(targetVelocity - m_Rb.velocity, ForceMode.VelocityChange);

        //Vector3 targetAngularVelocity = hInput * transform.up * m_RotationSpeed * Mathf.Deg2Rad;
        //m_Rb.AddTorque(targetAngularVelocity - m_Rb.angularVelocity, ForceMode.VelocityChange);

        //Quaternion qUprightRot = Quaternion.FromToRotation(transform.up, Vector3.up);
        //Quaternion qUprightOrient = qUprightRot * m_Rb.rotation;
        //Quaternion qUprightLerpedOrient = Quaternion.Lerp(m_Rb.rotation, qUprightOrient, Time.fixedDeltaTime * 6);
        //m_Rb.MoveRotation(qUprightLerpedOrient);

        // ACCELERATION MODE
        //Vector3 targetAcceleration = vInput*...;
        //m_Rb.AddForce(targetAcceleration, ForceMode.Acceleration);

        // FORCE MODE
        //Vector3 force = ...;
        //m_Rb.AddForce(force, ForceMode.Force);

    }
}
