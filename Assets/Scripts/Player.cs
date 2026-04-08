using UnityEngine;

public class Player_Gr2 : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float m_TranslationSpeed = 5.0f;

    [Header("FPS Camera")]
    [SerializeField] private Transform m_CameraTransform;
    [SerializeField] private Vector3 m_CameraOffset = new Vector3(0f, 1.65f, 0f);
    [SerializeField] private float m_MouseSensitivity = 2.2f;
    [SerializeField] private float m_MaxPitch = 85f;

    private Rigidbody m_Rb;
    private float m_CameraPitch;

    private void Awake()
    {
        m_Rb = GetComponent<Rigidbody>();
        if (m_Rb != null)
        {
            // Keep movement script independent from physics/gravity.
            m_Rb.isKinematic = true;
            m_Rb.useGravity = false;
        }
        if (m_CameraTransform == null && Camera.main != null)
        {
            m_CameraTransform = Camera.main.transform;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        UpdateLook();

        float vInput = Input.GetAxisRaw("Vertical");
        float hInput = Input.GetAxisRaw("Horizontal");

        Vector3 moveDirection = (transform.forward * vInput + transform.right * hInput).normalized;
        transform.position += moveDirection * m_TranslationSpeed * Time.deltaTime;
    }

    private void LateUpdate()
    {
        if (m_CameraTransform == null)
        {
            return;
        }

        m_CameraTransform.position = transform.position + transform.TransformDirection(m_CameraOffset);
    }

    private void UpdateLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * m_MouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * m_MouseSensitivity;

        transform.Rotate(Vector3.up, mouseX, Space.Self);

        m_CameraPitch -= mouseY;
        m_CameraPitch = Mathf.Clamp(m_CameraPitch, -m_MaxPitch, m_MaxPitch);

        if (m_CameraTransform != null)
        {
            m_CameraTransform.rotation = Quaternion.Euler(m_CameraPitch, transform.eulerAngles.y, 0f);
        }

    }
}
