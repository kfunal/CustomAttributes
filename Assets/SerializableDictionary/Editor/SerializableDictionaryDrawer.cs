#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(SerializableDictionary<,>))]
public class SerializableDictionaryDrawer : PropertyDrawer
{
    private const string ITEMS_PROPERTY_NAME = "items";
    private const string BACKGROUND_STYLE = "dic-background";
    private const string STYLE_SHEET_PATH = "Assets/Editor/StyleSheet.uss";
    private const string BUTTON_STYLE = "dic-button-style";
    private SerializedProperty items;

    private VisualElement root;
    private StyleSheet styleSheet;
    private Foldout foldout;
    private ScrollView scrollView;

    public override VisualElement CreatePropertyGUI(SerializedProperty _property)
    {
        items = _property.FindPropertyRelative(ITEMS_PROPERTY_NAME);
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

        return root;
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

    private void OnClearButtonClick(MouseUpEvent evt)
    {
    }

    private void OnAddItemButtonClick(MouseUpEvent evt)
    {
    }

}

#endif