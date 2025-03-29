using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        private float cameraCap;
        private Vector2 currentMouseDelta;
        private Vector2 currentMouseDeltaVelocity;

        [Header("Movement Properties")]
        public RaycastHit groundHit;
        public float spherecastRadius = 1f;
        public float moveSpeed;
        public Vector3 moveDirection;

        [Header("Components")]
        public Rigidbody rb;
        public Collider bodyCollider;
        public Camera cam;
        
        public InputActionReference move;
        
        
        
        

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            bodyCollider = GetComponentInChildren<Collider>();
            cam = GetComponentInChildren<Camera>();
        }

        private void Update()
        {
            Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, StaticData.mouseSmoothTime);
            
            cameraCap -= currentMouseDelta.y * StaticData.mouseSensitivity;
            cameraCap = Mathf.Clamp(cameraCap, -90.0f, 90.0f);
            cam.transform.localEulerAngles = Vector3.right * cameraCap;
            transform.Rotate(Vector3.up * currentMouseDelta.x * StaticData.mouseSensitivity);
        }

        private void Movement()
        {
           moveDirection = transform.forward * move.action.ReadValue<Vector2>().y + transform.right * move.action.ReadValue<Vector2>().x;
           
           rb.AddForce(moveDirection.normalized * moveSpeed);
        }

        // Currently NOT WORKING, idk what's wrong
        public bool IsGrounded()
        {
            float halfScaleY = transform.localScale.y / 2f;
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - halfScaleY, transform.position.z);

            return Physics.SphereCast(spherePosition, spherecastRadius, Vector3.down, out groundHit, 1f);
        }
        
        private void FixedUpdate()
        {
            Movement();
        }
    }
}