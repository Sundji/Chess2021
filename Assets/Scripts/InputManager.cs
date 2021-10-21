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
            if ((GameManager.GM.Status == Status.IN_PROGRESS || GameManager.GM.Status == Status.CHECK) && Input.GetMouseButtonDown(MOUSE_PRIMARY_BUTTON))
            {
                RaycastHit hit;
                Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit);
                Transform hitTransform = hit.transform;

                if (hitTransform != null)
                {
                    if (hitTransform.CompareTag("Piece"))
                    {
                        Piece piece = hitTransform.GetComponent<Piece>();
                        piece.Select();
                    }
                    else if (hitTransform.CompareTag("Cell"))
                    {
                        Cell cell = hitTransform.GetComponent<Cell>();
                        cell.Select();
                    }
                }
            }
        }
    }
}