#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;


[CustomPropertyDrawer(typeof(ExpandableAttribute))]
public class ExpandableAttributeDrawer : PropertyDrawer
{
    private const string BORDER_STYLE = "expandable-background-border";

    private PropertyField propertyField;
    private VisualElement root;
    private VisualElement background;

    private Foldout foldout;
    private IMGUIContainer inspectorContainer;
    private Editor propertyEditor;

    private StyleSheet styleSheet;

    public override VisualElement CreatePropertyGUI(SerializedProperty _property)
    {
        return BuildUI(_property);
    }

    private VisualElement BuildUI(SerializedProperty _property)
    {
        root = new VisualElement();
        styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/StyleSheet.uss");

        propertyEditor = null;
        propertyField = new PropertyField(_property);
        propertyField.Bind(_property.serializedObject);
        propertyField.RegisterValueChangeCallback(OnObjectChanged);
        root.Add(propertyField);

        background = BackgroundPanel();

        foldout = new Foldout();
        foldout.value = false;
        return root;
    }

    private void OnObjectChanged(SerializedPropertyChangeEvent _evt)
    {
        if (_evt.changedProperty.objectReferenceValue != null && !background.Contains(foldout))
        {
            root.Add(background);
            background.Add(foldout);
        }

        if (_evt.changedProperty.objectReferenceValue == null && background.Contains(foldout))
        {
            background.Remove(foldout);
            root.Remove(background);
        }

        foldout.text = _evt.changedProperty.objectReferenceValue != null ? _evt.changedProperty.objectReferenceValue.GetType().ToString() : "";

        if (_evt.changedProperty.objectReferenceValue != null && propertyEditor == null)
        {
            propertyEditor = Editor.CreateEditor(_evt.changedProperty.objectReferenceValue);
            inspectorContainer = new IMGUIContainer(() => propertyEditor.OnInspectorGUI());

            if (!foldout.Contains(inspectorContainer))
                foldout.Add(inspectorContainer);
        }
        else if (_evt.changedProperty.objectReferenceValue == null)
        {
            if (foldout.Contains(inspectorContainer))
                foldout.Remove(inspectorContainer);

            propertyEditor = null;
            inspectorContainer = null;
        }
    }

    private VisualElement BackgroundPanel()
    {
        VisualElement panelElement = new VisualElement();
        panelElement.styleSheets.Add(styleSheet);
        panelElement.AddToClassList(BORDER_STYLE);
        return panelElement;
    }
}

#endif