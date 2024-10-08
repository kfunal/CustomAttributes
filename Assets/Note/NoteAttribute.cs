using UnityEngine;

public class NoteAttribute : PropertyAttribute
{
    public string Text { get; private set; } = string.Empty;
    public MessageType MessageType { get; private set; } = MessageType.None;
    public float TopMargin { get; private set; } = 0f;
    public float BottomMargin { get; private set; } = 0f;
    public float LeftMargin { get; private set; } = 0f;
    public float RightMargin { get; private set; } = 0f;

    public NoteAttribute(string _text, MessageType MessageType = MessageType.None, float TopMargin = 0f, float BottomMargin = 0f, float LeftMargin = 0f, float RightMargin = 0f)
    {
        Text = _text;
        this.TopMargin = TopMargin;
        this.BottomMargin = BottomMargin;
        this.LeftMargin = LeftMargin;
        this.RightMargin = RightMargin;
        this.MessageType = MessageType;
    }
}