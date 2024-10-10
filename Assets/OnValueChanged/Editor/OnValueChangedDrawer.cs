#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(OnValueChangedAttribute))]
public class OnValueChangedDrawer : PropertyDrawer
{
    private OnValueChangedAttribute attr;
    private MethodInfo methodInfo;
    private Object targetObject;
    private HelpBox errorBox;

    private VisualElement root;
    private PropertyField propertyField;

    public override VisualElement CreatePropertyGUI(SerializedProperty _property)
    {
        if (root == null)
            root = new VisualElement();

        if (attr == null)
            attr = attribute as OnValueChangedAttribute;

        if (targetObject == null)
            targetObject = _property.serializedObject.targetObject;

        if (methodInfo == null)
            methodInfo = GetMethodInfo();

        if (errorBox == null)
            errorBox = CreateErrorBox();

        return BuildUI(_property);
    }

    private VisualElement BuildUI(SerializedProperty _property)
    {
        if (propertyField == null)
            propertyField = DrawPropertyField(_property);

        if (!root.Contains(propertyField))
            root.Add(propertyField);

        return root;
    }

    private PropertyField DrawPropertyField(SerializedProperty _property)
    {
        PropertyField field = new PropertyField();
        field.BindProperty(_property);

        field.RegisterValueChangeCallback(OnValueChanged);

        return field;
    }

    private void OnValueChanged(SerializedPropertyChangeEvent evt)
    {
        if (Application.isPlaying) return;

        if (methodInfo == null)
        {
            errorBox.text = $"There is no method called {attr.MethodName}";
            if (!root.Contains(errorBox))
                root.Add(errorBox);

            return;
        }

        methodInfo?.Invoke(targetObject, null);
    }

    private MethodInfo GetMethodInfo()
    {
        MethodInfo info = targetObject.GetType().GetMethod(attr.MethodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        return info;
    }

    private HelpBox CreateErrorBox()
    {
        HelpBox helpBox = new HelpBox("", HelpBoxMessageType.Error);
        return helpBox;
    }
}

#endif