﻿using DataAccess.Data;
using DataAccess.Models;

namespace SmsSendOutApi.SmsVendors;

public interface ISmsVendor
{
    Task<IResult> SmsInsert(SmsModel sms);
}

public class SmsVendor : ISmsVendor
{
    private static ISmsRepository _data;

    public SmsVendor(ISmsRepository data)
    {
        _data = data;
    }
    public async Task<IResult> SmsInsert(SmsModel sms)
    {
        if (!(sms.Body.Length <= 480))
        {
            return Results.BadRequest("Sorry you are not able to send a message with more than 480 characters");
        }
        else
        {
            await _data.InsertSms(sms);
            return Results.Ok();
        }
    }
}
