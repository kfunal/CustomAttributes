using UnityEngine;

public class SerializableDictionaryWork : MonoBehaviour
{
    [SerializeField]
    private SerializableDictionary<string, int> stringIntDictionary;
    [SerializeField]
    private SerializableDictionary<string, Color> stringColorDictionary;
    [SerializeField]
    private SerializableDictionary<int, HorizontalLineWork> intScriptDictionary;
    [SerializeField]
    private SerializableDictionary<Vector2, HorizontalLineWork> vector2ScriptDictionary;
    [SerializeField]
    private SerializableDictionary<NoteWork, NoteWork> scriptScriptDictionary;
}
