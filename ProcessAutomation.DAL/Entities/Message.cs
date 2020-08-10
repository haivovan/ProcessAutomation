using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

public class Message
{
    public Message()
    {
        Id = new ObjectId();
        Account = string.Empty;
        Money = string.Empty;
        Web = string.Empty;
        RecievedDate = string.Empty;
        MessageContent = string.Empty;
        IsSatisfied = false;
        IsProcessed = false;
        Error = string.Empty;
        DateExcute = null;
    }

    public ObjectId Id { get; set; }
    public string Account { get; set; }
    public string Money { get; set; }
    public string Web { get; set; }
    public string RecievedDate { get; set; }
    public string MessageContent { get; set; }
    public bool IsSatisfied { get; set; }
    public bool IsProcessed { get; set; }
    public string Error { get; set; }
    [BsonElement]
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime? DateExcute { get; set; }
    public bool IsKeepSession { get; set; }
}