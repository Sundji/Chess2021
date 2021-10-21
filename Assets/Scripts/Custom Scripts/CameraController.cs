using UnityEngine;

namespace Practice.Chess
{
    public class CameraController : MonoBehaviour
    {
        [Header("Rotation Information")]
        [SerializeField] private Vector3 _rotationAxis;
        [SerializeField] private Vector3 _rotationCenter;
        [SerializeField] private float _rotationSpeed;

        [Header("Zoom Information")]
        [SerializeField] private Vector2 _zoomRange;
        [SerializeField] private float _zoomSpeed;

        private Camera _camera;
        private Transform _transform;

        private float _originalFieldOfView;
        private Vector3 _originalPosition;
        private Quaternion _originalRotation;

        private Vector3 _mousePositionStart;
        private bool _mousePositionTracking;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            _transform = transform;

            _originalFieldOfView = _camera.fieldOfView;
            _originalPosition = _transform.position;
            _originalRotation = _transform.rotation;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(InputManager.MOUSE_MIDDLE_CLICK))
            {
                _mousePositionStart = Input.mousePosition;
                _mousePositionTracking = true;
            }
            if (Input.GetMouseButton(InputManager.MOUSE_MIDDLE_CLICK) && _mousePositionTracking)
            {
                Rotate((Input.mousePosition - _mousePositionStart).x);
                _mousePositionStart = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(InputManager.MOUSE_MIDDLE_CLICK))
            {
                _mousePositionTracking = false;
            }

            float scrollAxis = Input.GetAxisRaw("Mouse ScrollWheel");
            if (!Mathf.Approximately(scrollAxis, 0.0f))
                Zoom(scrollAxis);
        }

        private void Rotate(float positionDifference)
        {
            _transform.RotateAround(_rotationCenter, _rotationAxis, positionDifference * _rotationSpeed * Time.deltaTime);
        }

        private void Zoom(float scrollAxis) 
        {
            float fieldOfView = _camera.fieldOfView + (scrollAxis > 0.0 ? -1.0f : 1.0f) * _zoomSpeed * Time.deltaTime;
            _camera.fieldOfView = Mathf.Clamp(fieldOfView, _zoomRange.x, _zoomRange.y);
        }

        public void ResetCamera()
        {
            _camera.fieldOfView = _originalFieldOfView;
            _transform.position = _originalPosition;
            _transform.rotation = _originalRotation;

            if (GameManager.GM.ActivePlayerColor == PlayerColor.BLACK)
                _transform.RotateAround(_rotationCenter, _rotationAxis, 180);
        }
    }
}