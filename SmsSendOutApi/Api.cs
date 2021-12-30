using DataAccess.Data;
using DataAccess.Models;
using SmsSendOutApi.Enums;

namespace SmsSendOutApi;

public static class Api
{
 
    public static void ConfigureApi(this WebApplication app)
    {
        app.MapPost("/sendSms", InsertSms);
    }


    private static async Task<IResult> InsertSms(SmsModel sms, ISmsRepository _data)
    {
        try
        {
            var _smsVendor = new SmsVendor(_data);
            return await _smsVendor.SmsVendorManager(sms);

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

   
}
