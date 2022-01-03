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


    private static async Task<IResult> InsertSms(SmsModel sms, ISmsVendor smsVendor, ISmsRepository data)
    {
        Dictionary<CountryCode, ISmsVendor> strategies = new Dictionary<CountryCode, ISmsVendor>()
        {
            {CountryCode.Greece, new SmsVendorGR(data)},
            {CountryCode.Cyprus, new SmsVendorCY(data)},
            {CountryCode.General, new SmsVendor(data)}
        };

        var countryCode = GetCountryCode(sms);

        try
        {
            return strategies.ContainsKey(countryCode)
                ? await strategies[countryCode].SmsInsert(sms)
                : await strategies[CountryCode.General].SmsInsert(sms);
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