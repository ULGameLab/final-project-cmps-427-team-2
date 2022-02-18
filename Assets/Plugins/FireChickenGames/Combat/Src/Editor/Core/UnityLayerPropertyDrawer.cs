namespace FireChickenGames.Combat.Editor.Core
{
    using FireChickenGames.Combat.Core;
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(UnityLayer))]
    public class UnityLayerPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            EditorGUI.BeginProperty(_position, GUIContent.none, _property);
            var layerIndex = _property.FindPropertyRelative("layerIndex");

            _position = EditorGUI.PrefixLabel(_position, GUIUtility.GetControlID(FocusType.Passive), _label);

            if (layerIndex != null)
                layerIndex.intValue = EditorGUI.LayerField(_position, layerIndex.intValue);

            EditorGUI.EndProperty();
        }
    }
}
