#if UNITY_EDITOR
using System.Globalization;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(HorizontalLineAttribute))]
public class HorizontalLineDrawer : DecoratorDrawer
{
    private HorizontalLineAttribute attr;

    private Color defaultLineColor => EditorGUIUtility.isProSkin ? new Color(.3f, .3f, .3f, 1f) : new Color(.7f, .7f, .7f, 1f);

    private Color lineColor;

    private string[] colorString;

    public override float GetHeight()
    {
        if (attr == null)
            attr = attribute as HorizontalLineAttribute;

        return Mathf.Max(attr.Padding, attr.Thickness);
    }

    public override void OnGUI(Rect position)
    {
        if (attr == null)
            attr = attribute as HorizontalLineAttribute;

        position.height = attr.Thickness;
        position.y += attr.Padding * .5f;

        GetColor();

        EditorGUI.DrawRect(position, lineColor);
    }

    private void GetColor()
    {
        if (string.IsNullOrEmpty(attr.Color))
        {
            lineColor = defaultLineColor;
            return;
        }

        try
        {
            colorString = attr.Color.Split(",");

            if (colorString == null || colorString.Length < 4)
            {
                lineColor = defaultLineColor;
                return;
            }

            lineColor = new Color(StringToFloat(colorString[0]), StringToFloat(colorString[1]), StringToFloat(colorString[2]), StringToFloat(colorString[3]));
        }
        catch
        {
            lineColor = defaultLineColor;
        }
    }

    private float StringToFloat(string _string) => float.Parse(_string, CultureInfo.InvariantCulture.NumberFormat);
}
#endif