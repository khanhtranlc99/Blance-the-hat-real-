using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ConditionalFieldAttribute))]
public class ConditionalFieldAttributeDrawer : PropertyDrawer
{
    private ConditionalFieldAttribute Attribute
    {
        get { return _attribute ?? (_attribute = attribute as ConditionalFieldAttribute); }
    }

    private ConditionalFieldAttribute _attribute;

    private bool _toShow = true;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        _toShow = Attribute.CheckPropertyVisible(property);

        return _toShow ? EditorGUI.GetPropertyHeight(property) : 0;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (_toShow) EditorGUI.PropertyField(position, property, label, true);
    }
}
#endif
