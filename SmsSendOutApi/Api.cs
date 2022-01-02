using DataAccess.Data;
using DataAccess.Models;
using SmsSendOutApi.Enums;
using SmsSendOutApi.SmsVendors;

namespace SmsSendOutApi;

public static class Api
{
    public static void ConfigureApi(this WebApplication app)
    {
        app.MapPost("/sendSms", InsertSms);
    }


    private static async Task<IResult> InsertSms(SmsModel sms, ISmsVendor smsVendor)
    {
        try
        {
            return await smsVendor.SmsVendorManager(sms);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
}