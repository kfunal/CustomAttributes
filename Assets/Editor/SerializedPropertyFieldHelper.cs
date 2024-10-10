#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public static class SerializedPropertyFieldHelper
{
    public static VisualElement CreateIntegerField(SerializedProperty _property, params string[] _styles)
    {
        IntegerField field = new IntegerField();
        field.BindProperty(_property);

        if (_styles != null && _styles.Length > 0)
        {
            for (int i = 0; i < _styles.Length; i++)
                field.AddToClassList(_styles[i]);
        }

        return field;
    }

    public static VisualElement CreateToggleField(SerializedProperty _property, params string[] _styles)
    {
        Toggle field = new Toggle();
        field.BindProperty(_property);

        if (_styles != null && _styles.Length > 0)
        {
            for (int i = 0; i < _styles.Length; i++)
                field.AddToClassList(_styles[i]);
        }

        return field;
    }

    public static VisualElement CreateFloatField(SerializedProperty _property, params string[] _styles)
    {
        FloatField field = new FloatField();
        field.BindProperty(_property);

        if (_styles != null && _styles.Length > 0)
        {
            for (int i = 0; i < _styles.Length; i++)
                field.AddToClassList(_styles[i]);
        }

        return field;
    }

    public static VisualElement CreateTextField(SerializedProperty _property, params string[] _styles)
    {
        TextField field = new TextField();
        field.BindProperty(_property);

        if (_styles != null && _styles.Length > 0)
        {
            for (int i = 0; i < _styles.Length; i++)
                field.AddToClassList(_styles[i]);
        }

        return field;
    }

    public static VisualElement CreateColorField(SerializedProperty _property, params string[] _styles)
    {
        ColorField field = new ColorField();
        field.BindProperty(_property);

        if (_styles != null && _styles.Length > 0)
        {
            for (int i = 0; i < _styles.Length; i++)
                field.AddToClassList(_styles[i]);
        }

        return field;
    }

    public static VisualElement CreateObjectField(SerializedProperty _property, System.Action<Object> _onValueChanged, System.Type _objectType, params string[] _styles)
    {
        ObjectField field = new ObjectField
        {
            allowSceneObjects = true,
            objectType = _objectType
        };

        field.RegisterValueChangedCallback(evt =>
        {
            _onValueChanged?.Invoke(evt.newValue);

            if (evt.newValue != null)
                _property.serializedObject.ApplyModifiedProperties();
        });

        field.BindProperty(_property);

        if (_styles != null && _styles.Length > 0)
        {
            for (int i = 0; i < _styles.Length; i++)
                field.AddToClassList(_styles[i]);
        }

        return field;
    }

    public static VisualElement CreateEnumField(SerializedProperty _property, params string[] _styles)
    {
        EnumField field = new EnumField();
        field.BindProperty(_property);

        if (_styles != null && _styles.Length > 0)
        {
            for (int i = 0; i < _styles.Length; i++)
                field.AddToClassList(_styles[i]);
        }

        return field;
    }

    public static VisualElement CreateVector2Field(SerializedProperty _property, params string[] _styles)
    {
        Vector2Field field = new Vector2Field();
        field.BindProperty(_property);

        if (_styles != null && _styles.Length > 0)
        {
            for (int i = 0; i < _styles.Length; i++)
                field.AddToClassList(_styles[i]);
        }

        return field;
    }

    public static VisualElement CreateVector3Field(SerializedProperty _property, params string[] _styles)
    {
        Vector3Field field = new Vector3Field();
        field.BindProperty(_property);

        if (_styles != null && _styles.Length > 0)
        {
            for (int i = 0; i < _styles.Length; i++)
                field.AddToClassList(_styles[i]);
        }

        return field;
    }

    public static ToolbarButton CreateButton(string _text, EventCallback<MouseUpEvent> _onClick, params string[] _styles)
    {
        ToolbarButton field = new ToolbarButton();
        field.text = _text;
        field.RegisterCallback(_onClick);

        if (_styles != null && _styles.Length > 0)
        {
            for (int i = 0; i < _styles.Length; i++)
                field.AddToClassList(_styles[i]);
        }

        return field;
    }

    public static bool SerializedPropertyContainElement(SerializedProperty _property, object _targetElement)
    {
        if (_property.isArray)
        {
            for (int i = 0; i < _property.arraySize; i++)
            {
                SerializedProperty element = _property.GetArrayElementAtIndex(i);
                if (CheckElementValue(element, _targetElement))
                    return true;
            }
        }
        else if (_property.propertyType == SerializedPropertyType.Generic)
        {
            for (int i = 0; i < _property.arraySize; i++)
            {
                SerializedProperty childProperty = _property.GetArrayElementAtIndex(i);
                if (CheckElementValue(childProperty, _targetElement))
                    return true;
            }
        }
        else
            return CheckElementValue(_property, _targetElement);

        return false;
    }

    private static bool CheckElementValue(SerializedProperty _element, object _targetElement)
    {
        if (_element.propertyType == SerializedPropertyType.ObjectReference)
            return _element.objectReferenceValue == (Object)_targetElement;

        if (_element.propertyType == SerializedPropertyType.Integer)
            return _element.intValue.Equals(_targetElement);

        if (_element.propertyType == SerializedPropertyType.Boolean)
            return _element.boolValue.Equals(_targetElement);

        if (_element.propertyType == SerializedPropertyType.Float)
            return _element.floatValue.Equals(_targetElement);

        if (_element.propertyType == SerializedPropertyType.String)
            return _element.stringValue.Equals(_targetElement);

        if (_element.propertyType == SerializedPropertyType.Enum)
            return _element.enumValueIndex.Equals(_targetElement);

        if (_element.propertyType == SerializedPropertyType.Vector2)
            return _element.vector2Value.Equals((Vector2)_targetElement);

        if (_element.propertyType == SerializedPropertyType.Vector3)
            return _element.vector3Value.Equals((Vector3)_targetElement);

        return false;
    }

    public static Color GenerateRandomColor() => new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    public static Vector2 GenerateRandomVector2() => new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
    public static Vector3 GenerateRandomVector3() => new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

    public static void GetValueTypeFromDictionary(System.Type dictionaryType, ref System.Type _keyType, ref System.Type _valueType)
    {
        if (dictionaryType.IsGenericType && dictionaryType.GetGenericTypeDefinition() == typeof(SerializableDictionary<,>))
        {
            System.Type[] genericArguments = dictionaryType.GetGenericArguments();

            _keyType = genericArguments[0];
            _valueType = genericArguments[1];
        }
    }

    public static object GetTargetObjectOfProperty(SerializedProperty prop)
    {
        var path = prop.propertyPath.Replace(".Array.data[", "[");
        object obj = prop.serializedObject.targetObject;
        var elements = path.Split('.');
        foreach (var element in elements)
        {
            if (element.Contains("["))
            {
                var elementName = element.Substring(0, element.IndexOf("["));
                var index = System.Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                obj = GetValue_Imp(obj, elementName, index);
            }
            else
            {
                obj = GetValue_Imp(obj, element);
            }
        }
        return obj;
    }

    private static object GetValue_Imp(object source, string name)
    {
        if (source == null)
            return null;
        var type = source.GetType();

        while (type != null)
        {
            var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (f != null)
                return f.GetValue(source);

            var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (p != null)
                return p.GetValue(source, null);

            type = type.BaseType;
        }
        return null;
    }

    private static object GetValue_Imp(object source, string name, int index)
    {
        var enumerable = GetValue_Imp(source, name) as System.Collections.IEnumerable;
        if (enumerable == null) return null;
        var enm = enumerable.GetEnumerator();

        for (int i = 0; i <= index; i++)
        {
            if (!enm.MoveNext()) return null;
        }
        return enm.Current;
    }

    public static object GetPropertyTargetObject(this SerializedProperty property)
    {
        string path = property.propertyPath;
        var target = property.serializedObject.targetObject;

        string[] parts = path.Split('.');
        for (int i = 0; i < parts.Length - 1; i++)
        {
            var fieldInfo = target.GetType().GetField(parts[i]);
            if (fieldInfo != null)
            {
                target = fieldInfo.GetValue(target) as Object;
            }
        }

        var lastFieldInfo = target.GetType().GetField(parts[parts.Length - 1]);
        return lastFieldInfo.GetValue(target);
    }

    // private static void SetDefaultValue(SerializedProperty _property)
    // {
    //     switch (_property.propertyType)
    //     {
    //         case SerializedPropertyType.Integer:
    //             int intValue = 0;
    //             while (SerializedPropertyContainElement(keys, intValue))
    //                 intValue++;
    //             _property.intValue = intValue;
    //             break;
    //         case SerializedPropertyType.Boolean:
    //             _property.boolValue = false;
    //             break;
    //         case SerializedPropertyType.Float:
    //             float floatValue = 0;
    //             while (SerializedPropertyContainElement(keys, floatValue))
    //                 floatValue++;
    //             _property.floatValue = floatValue;
    //             break;
    //         case SerializedPropertyType.String:
    //             int index = 0;
    //             while (SerializedPropertyContainElement(keys, $"Element {index}"))
    //                 index++;
    //             _property.stringValue = $"Element {index}";
    //             break;
    //         case SerializedPropertyType.Color:
    //             Color colorValue = Color.black;
    //             while (SerializedPropertyContainElement(keys, colorValue))
    //                 colorValue = GenerateRandomColor();
    //             _property.colorValue = Color.black;
    //             break;
    //         case SerializedPropertyType.ObjectReference:
    //             _property.objectReferenceValue = null;
    //             break;
    //         case SerializedPropertyType.Enum:
    //             int enumValue = 0;
    //             while (SerializedPropertyContainElement(keys, enumValue))
    //                 enumValue++;
    //             _property.enumValueIndex = enumValue;
    //             break;
    //         case SerializedPropertyType.Vector2:
    //             Vector2 vectorTwoValue = Vector2.zero;
    //             while (SerializedPropertyContainElement(keys, vectorTwoValue))
    //                 vectorTwoValue = GenerateRandomVector2();
    //             _property.vector2Value = vectorTwoValue;
    //             break;
    //         case SerializedPropertyType.Vector3:
    //             Vector3 vectorThreeValue = Vector3.zero;
    //             while (SerializedPropertyContainElement(keys, vectorThreeValue))
    //                 vectorThreeValue = GenerateRandomVector2();
    //             _property.vector3Value = vectorThreeValue;
    //             break;
    //     }
    // }
}

#endif