#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class SerializedPropertyFieldHelper
{
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
}

#endif