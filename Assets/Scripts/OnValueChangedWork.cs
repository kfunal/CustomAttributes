using UnityEngine;

public class OnValueChangedWork : MonoBehaviour
{
    [OnValueChanged("TestMethod")]
    public int IntVariable;

    [OnValueChanged("OtherTestMethod")]
    public int OtherIntVariable;

    public void TestMethod()
    {
        Debug.Log("Test Method");
    }

    public void OtherTestMethod()
    {
        Debug.Log("Other Test Method");
    }
}
