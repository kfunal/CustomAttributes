using UnityEngine;

public class OnValueChangedAttribute : PropertyAttribute
{
    public string MethodName;

    public OnValueChangedAttribute(string MethodName)
    {
        this.MethodName = MethodName;
    }
}
