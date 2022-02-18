namespace FireChickenGames.Combat.Core
{
    using System;
    using UnityEngine;

    [Serializable]
    public class UnityLayer
    {
        [SerializeField]
        private int layerIndex = 0;
        public int LayerIndex { get { return layerIndex; } }
        public int Mask { get { return 1 << layerIndex; } }

        public UnityLayer() { }

        public UnityLayer(int _layerIndex) : this()
        {
            Set(_layerIndex);
        }

        public void Set(int _layerIndex)
        {
            if (_layerIndex > 0 && _layerIndex < 32)
                layerIndex = _layerIndex;
        }
    }
}
