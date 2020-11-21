using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

public class OTPModel
{
    public OTPModel()
    {
        WebId = string.Empty;
        OTP = string.Empty;
    }

    public string WebId { get; set; }
    public string OTP { get; set; }
}