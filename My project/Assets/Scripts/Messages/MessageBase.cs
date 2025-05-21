[System.Serializable]
public class MessageBase
{
    public MessageType Type;
}

public enum MessageType
{
    Empty,
    TextMessage
}