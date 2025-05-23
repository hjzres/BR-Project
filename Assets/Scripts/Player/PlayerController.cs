using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

namespace Assets.Scripts.Player
{
    public class PlayerController : NetworkBehaviour
    {
        private float cameraCap;
        private Vector2 currentMouseDelta;
        private Vector2 currentMouseDeltaVelocity;

        [Header("Movement Properties")]
        public float moveSpeed;
        public Vector3 moveDirection;
        
        [Header("Ground Check")]
        [SerializeField] bool grounded;
        public LayerMask whatIsGround;
        [SerializeField] private float playerHeight;
        

        [Header("Components")]
        public Rigidbody rb;
        public Collider bodyCollider;
        public Camera cam;
        
        public InputActionReference move;


        public override void OnNetworkSpawn()
        {
            if (!IsOwner)
            {
                enabled = false;
                return;
            }
        }
        

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            bodyCollider = GetComponentInChildren<Collider>();
            cam = GetComponentInChildren<Camera>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, StaticData.mouseSmoothTime);
            
            cameraCap -= currentMouseDelta.y * StaticData.mouseSensitivity;
            cameraCap = Mathf.Clamp(cameraCap, -90.0f, 90.0f);
            cam.transform.localEulerAngles = Vector3.right * cameraCap;
            transform.Rotate(Vector3.up * currentMouseDelta.x * StaticData.mouseSensitivity);

            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight / 2 + 2f, whatIsGround);

            rb.linearDamping = grounded ? 5f : 0f;
        }

        private void Movement()
        {
           moveDirection = transform.forward * move.action.ReadValue<Vector2>().y + transform.right * move.action.ReadValue<Vector2>().x;
           
           rb.AddForce(moveDirection.normalized * moveSpeed);
        }
        
        private void FixedUpdate()
        {
            Movement();
        }
    }
}