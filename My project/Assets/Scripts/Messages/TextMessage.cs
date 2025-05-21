[System.Serializable]
public class TextMessage : MessageBase
{
    public string Text;

    public TextMessage(string text)
    {
        Type = MessageType.TextMessage;
        Text = text;
    }
}