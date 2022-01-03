using DataAccess.Data;
using DataAccess.Models;

namespace SmsSendOutApi.SmsVendors;

public class SmsVendorCY : ISmsVendor
{
    private static ISmsRepository _data;

    public SmsVendorCY(ISmsRepository data)
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
            var bodyMessage = sms.Body;
            while (bodyMessage.Length > 160)
            {
                SmsModel subSmsModel = new SmsModel();
                subSmsModel.Recipient = sms.Recipient;
                subSmsModel.Body = bodyMessage.Substring(0, 160);
                await _data.InsertSms(subSmsModel);
                bodyMessage = bodyMessage.Remove(0, 160);
            }

            if (bodyMessage.Length > 0)
            {
                SmsModel subSmsModel = new SmsModel();
                subSmsModel.Recipient = sms.Recipient;
                subSmsModel.Body = bodyMessage;
                await _data.InsertSms(subSmsModel);
            }
            else
            {
                return Results.NoContent();
            }

            return Results.Ok();
        }
    }
}