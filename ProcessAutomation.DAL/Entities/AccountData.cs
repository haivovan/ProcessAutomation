using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AccountData
{
    public AccountData()
    {
        Id = new ObjectId();
        IDAccount = string.Empty;
        Name = string.Empty;
        Phone = string.Empty;
        BK = string.Empty;
        CB = string.Empty;
        HLC = string.Empty;
        GD = string.Empty;
        NT = string.Empty;
        LQ = string.Empty;
    }

    public ObjectId Id { get; set; }
    public string IDAccount { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string BK { get; set; }
    public string CB { get; set; }
    public string HLC { get; set; }
    public string GD { get; set; }
    public string NT { get; set; }
    public string LQ { get; set; }
}