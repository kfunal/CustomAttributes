using UnityEngine;

[System.Serializable]
public class NoteWork : MonoBehaviour
{
    [Note("Note Attribute  Test", TopMargin: 10f, BottomMargin: 10f)]
    public int Variable;

    [Note("Note Attribute Info Test", MessageType.Info, 10f, 10f)]
    public int OtherVariable;

    [Note("Note Attribute Warning Test", MessageType.Warning, 10f, 10f)]
    public int OtherOtherVariable;

    [Note("Note Attribute Error Test", MessageType.Error, 10f, 10f)]
    public int OtherOtherOtherVariable;
}
