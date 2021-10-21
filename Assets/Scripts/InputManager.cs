using UnityEngine;

namespace Practice.Chess
{
    public class InputManager : MonoBehaviour
    {
        public const int MOUSE_PRIMARY_BUTTON = 0;

        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(MOUSE_PRIMARY_BUTTON))
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