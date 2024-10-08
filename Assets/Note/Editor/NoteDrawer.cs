#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(NoteAttribute))]
public class NoteDrawer : DecoratorDrawer
{
    private NoteAttribute attr;
    private HelpBox helpBox;

    public override VisualElement CreatePropertyGUI()
    {
        return HelpBoxDrawer();
    }

    private HelpBox HelpBoxDrawer()
    {
        if (attr == null)
            attr = attribute as NoteAttribute;

        if (helpBox == null)
        {
            helpBox = new HelpBox(attr.Text, (HelpBoxMessageType)attr.MessageType)
            {
                style =
                    {
                        marginTop = attr.TopMargin,
                        marginBottom = attr.BottomMargin,
                        marginLeft = attr.LeftMargin,
                        marginRight = attr.RightMargin
                    }
            };
        }

        return helpBox;
    }
}

#endif