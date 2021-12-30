using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DbAccess;
using DataAccess.Models;

namespace DataAccess.Data;

public class SmsRepository : ISmsRepository
{
    private readonly  ISqlDataAccess _db;

    public SmsRepository(ISqlDataAccess db)
    {
        _db = db;
    }

    public Task InsertSms(SmsModel sms) =>
        _db.SaveData("dbo.spSms_Insert", new
        {
            sms.Recipient,
            sms.Body,
        });


}
