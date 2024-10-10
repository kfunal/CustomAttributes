#if UNITY_EDITOR
using System.Collections;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static SerializedPropertyFieldHelper;

[CustomPropertyDrawer(typeof(SerializableDictionary<,>))]
public class SerializableDictionaryDrawer : PropertyDrawer
{
    private const string KEYS_PROPERTY_NAME = "keys";
    private const string VALUES_PROPERTY_NAME = "values";
    private const string BACKGROUND_STYLE = "dic-background";
    private const string STYLE_SHEET_PATH = "Assets/Editor/StyleSheet.uss";
    private const string BUTTON_STYLE = "dic-button-style";
    private const string ELEMENT_CONTAINER_STYLE = "element-container";
    private const string ELEMENT_STYLE = "dic-element";

    private VisualElement root;
    private StyleSheet styleSheet;
    private Foldout foldout;
    private ScrollView scrollView;

    private SerializedProperty dict;
    private SerializedProperty keys;
    private SerializedProperty values;

    private SerializedObject serializedObject;

    private System.Type keysType;
    private System.Type valuesType;

    public override VisualElement CreatePropertyGUI(SerializedProperty _property)
    {
        dict = _property;
        keys = _property.FindPropertyRelative(KEYS_PROPERTY_NAME);
        values = _property.FindPropertyRelative(VALUES_PROPERTY_NAME);
        serializedObject = _property.serializedObject;
        GetValueTypeFromDictionary(GetTargetObjectOfProperty(_property).GetType(), ref keysType, ref valuesType);

        return BuildUI(_property);
    }

    private VisualElement BuildUI(SerializedProperty _property)
    {
        root = new VisualElement();
        styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(STYLE_SHEET_PATH);

        root.styleSheets.Add(styleSheet);
        root.AddToClassList(BACKGROUND_STYLE);

        foldout = new Foldout();
        foldout.value = false;
        foldout.text = _property.displayName;
        foldout.Add(ButtonGroup());

        scrollView = new ScrollView(ScrollViewMode.VerticalAndHorizontal);
        foldout.Add(scrollView);
        root.Add(foldout);

        if (keys.arraySize > 0)
            DrawDictionary();

        return root;
    }

    private void DrawDictionary()
    {
        for (int i = 0; i < keys.arraySize; i++)
            CreateElementContainer(keys.GetArrayElementAtIndex(i), values.GetArrayElementAtIndex(i));
    }

    private ToolbarButton CreateButton(string _text, EventCallback<MouseUpEvent> _onClick)
    {
        ToolbarButton button = new ToolbarButton();
        button.AddToClassList(BUTTON_STYLE);
        button.text = _text;
        button.RegisterCallback(_onClick);
        return button;
    }

    private VisualElement ButtonGroup()
    {
        VisualElement group = new VisualElement();
        group.style.flexDirection = FlexDirection.Row;
        group.Add(CreateButton("Add Item", OnAddItemButtonClick));
        group.Add(CreateButton("Clear", OnClearButtonClick));
        return group;
    }

    private void OnAddItemButtonClick(MouseUpEvent evt)
    {
        keys.InsertArrayElementAtIndex(keys.arraySize);
        values.InsertArrayElementAtIndex(values.arraySize);

        SerializedProperty newKey = keys.GetArrayElementAtIndex(keys.arraySize - 1);
        SerializedProperty newValue = values.GetArrayElementAtIndex(values.arraySize - 1);

        SetDefaultValue(newKey, keys.arraySize - 1);
        SetDefaultValue(newValue, values.arraySize - 1);

        serializedObject.ApplyModifiedProperties();

        CreateElementContainer(newKey, newValue);
    }

    private void OnClearButtonClick(MouseUpEvent evt)
    {
        keys.ClearArray();
        values.ClearArray();

        serializedObject.ApplyModifiedProperties();

        scrollView.Clear();
    }

    private void CreateElementContainer(SerializedProperty _key, SerializedProperty _value)
    {
        VisualElement container = new VisualElement();
        container.AddToClassList(ELEMENT_CONTAINER_STYLE);
        container.Add(ElementByType(_key, keysType));
        container.Add(ElementByType(_value, valuesType));
        scrollView.Add(container);
    }

    private VisualElement ElementByType(SerializedProperty _property, System.Type _objectType)
    {
        return _property.propertyType switch
        {
            SerializedPropertyType.Integer => CreateIntegerField(_property, ELEMENT_STYLE),
            SerializedPropertyType.Boolean => CreateToggleField(_property, ELEMENT_STYLE),
            SerializedPropertyType.Float => CreateFloatField(_property, ELEMENT_STYLE),
            SerializedPropertyType.String => CreateTextField(_property, ELEMENT_STYLE),
            SerializedPropertyType.Color => CreateColorField(_property, ELEMENT_STYLE),
            SerializedPropertyType.ObjectReference => CreateObjectField(_property, OnObjectChanged, _objectType, ELEMENT_STYLE),
            SerializedPropertyType.Enum => CreateEnumField(_property, ELEMENT_STYLE),
            SerializedPropertyType.Vector2 => CreateVector2Field(_property, ELEMENT_STYLE),
            SerializedPropertyType.Vector3 => CreateVector3Field(_property, ELEMENT_STYLE),
            SerializedPropertyType.Generic => CreateGenericViewElement(_property),

            _ => new PropertyField(_property)
        };
    }

    private void OnObjectChanged(Object _newObject)
    {
        if (_newObject != null)
        {
            var element = GetTargetObjectOfProperty(dict);
            element.GetType().GetMethod("OnAfterDeserialize").Invoke(element, null);
            serializedObject.ApplyModifiedProperties();
        }
    }

    private Foldout CreateGenericViewElement(SerializedProperty _property)
    {
        Foldout foldout = new Foldout();
        foldout.text = _property.displayName;

        SerializedProperty iterator = _property.Copy();
        SerializedProperty endProperty = _property.GetEndProperty();

        while (iterator.NextVisible(true) && !SerializedProperty.EqualContents(iterator, endProperty))
        {
            System.Type type = GetTargetObjectOfProperty(_property).GetType();
            foldout.Add(ElementByType(iterator, type));
        }

        return foldout;
    }

    private void SetDefaultValue(SerializedProperty _property, int _index)
    {
        switch (_property.propertyType)
        {
            case SerializedPropertyType.Integer:
                int intValue = 0;
                while (SerializedPropertyContainElement(keys, intValue))
                    intValue++;
                _property.intValue = intValue;
                break;
            case SerializedPropertyType.Boolean:
                _property.boolValue = false;
                break;
            case SerializedPropertyType.Float:
                float floatValue = 0;
                while (SerializedPropertyContainElement(keys, floatValue))
                    floatValue++;
                _property.floatValue = floatValue;
                break;
            case SerializedPropertyType.String:
                int index = _index;
                while (SerializedPropertyContainElement(keys, $"Element {index}"))
                    index++;
                _property.stringValue = $"Element {index}";
                break;
            case SerializedPropertyType.Color:
                Color colorValue = Color.black;
                while (SerializedPropertyContainElement(keys, colorValue))
                    colorValue = GenerateRandomColor();
                _property.colorValue = Color.black;
                break;
            case SerializedPropertyType.ObjectReference:
                _property.objectReferenceValue = null;
                break;
            case SerializedPropertyType.Enum:
                int enumValue = 0;
                while (SerializedPropertyContainElement(keys, enumValue))
                    enumValue++;
                _property.enumValueIndex = enumValue;
                break;
            case SerializedPropertyType.Vector2:
                Vector2 vectorTwoValue = Vector2.zero;
                while (SerializedPropertyContainElement(keys, vectorTwoValue))
                    vectorTwoValue = GenerateRandomVector2();
                _property.vector2Value = vectorTwoValue;
                break;
            case SerializedPropertyType.Vector3:
                Vector3 vectorThreeValue = Vector3.zero;
                while (SerializedPropertyContainElement(keys, vectorThreeValue))
                    vectorThreeValue = GenerateRandomVector2();
                _property.vector3Value = vectorThreeValue;
                break;
        }
    }
}

#endif


