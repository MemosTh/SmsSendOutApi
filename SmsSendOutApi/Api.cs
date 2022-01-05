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


    private static async Task<IResult> InsertSms(SmsModel sms, IEnumerable<ISmsVendor> smsVendors)
    {

        var countryCode = GetCountryCode(sms);

        try
        {
            return smsVendors.FirstOrDefault(x => x.CountryCode == countryCode) != null
                ? await smsVendors.FirstOrDefault(x => x.CountryCode == countryCode).SmsInsert(sms)
                : await smsVendors.FirstOrDefault(x => x.CountryCode == CountryCode.General).SmsInsert(sms);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    private static CountryCode GetCountryCode(SmsModel sms)
    {
        var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();
        var phoneNumber = phoneNumberUtil.Parse(sms.Recipient, null);
        CountryCode countryCode = (CountryCode)phoneNumber.CountryCode;

        return countryCode;
    }
}