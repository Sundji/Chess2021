using UnityEngine;

namespace Practice.Chess
{
    [CreateAssetMenu(fileName = "New Design Data", menuName = "Scriptable Objects/New Design Data", order = 1)]
    public class DesignData : ScriptableObject
    {
        public Material MaterialBlack;
        public Material MaterialWhite;
        public Material MaterialHightlight;

        public Color EmissionColorBlack;
        public Color EmissionColorWhite;

        public Color ColorPanelBackground;
        public Color ColorButtonBackground;
        public Color ColorTextMain;
        public Color ColorText;
    }
}