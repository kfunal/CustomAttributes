using UnityEngine;

public class HorizontalLineAttribute : PropertyAttribute
{
    /// <summary>
    /// Line Thickness
    /// </summary>
    public int Thickness = 2;

    /// <summary>
    /// Line Padding
    /// </summary>
    public float Padding = 20f;

    /// <summary>
    /// r,g,b,a all has to be between 0 and 1<br /><br />
    /// Example Format<br /><br />
    /// "0.5,0.5,0.5,0.5";<br /><br />
    /// </summary>
    public string Color = string.Empty;

    public HorizontalLineAttribute() { }
}