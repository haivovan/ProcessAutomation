using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

public class RegisterAccountModel
{
    public RegisterAccountModel()
    {
        Id = 0;
        Name = string.Empty;
        Phone = string.Empty;
        WebId = string.Empty;
        IdNumber = string.Empty;
        Password = string.Empty;
        Status = 0;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string WebId { get; set; }
    public string IdNumber { get; set; }
    public string Password { get; set; }
    public int Status { get; set; }
}