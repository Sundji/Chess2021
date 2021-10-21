using UnityEngine;

namespace Practice.Chess
{
    public class DesignManager : MonoBehaviour
    {
        [SerializeField] private DesignData _designData;

        private static DesignManager _DM;

        public static DesignManager DM
        {
            get
            {
                if (_DM == null)
                    _DM = FindObjectOfType<DesignManager>();
                return _DM;
            }
        }

        public DesignData DesignData { get { return _designData; } }

        private void Awake()
        {
            if (_DM == null)
                _DM = this;
            else if (_DM != this)
                Destroy(gameObject);
        }
    }
}