using UnityEngine;

namespace Practice.Chess
{
    public class InputManager : MonoBehaviour
    {
        public const int MOUSE_PRIMARY_BUTTON = 0;
        public const int MOUSE_MIDDLE_CLICK = 2;

        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (!GameManager.GM.ShouldPauseInput && Input.GetMouseButtonDown(MOUSE_PRIMARY_BUTTON))
            {
                Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);
                Transform hitTransform = hit.transform;

                if (hitTransform != null && hitTransform.CompareTag("Cell"))
                {
                    Cell cell = hitTransform.GetComponent<Cell>();
                    cell.Select();
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();
        }
    }
}