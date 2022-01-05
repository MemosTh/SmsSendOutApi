using System.Text.RegularExpressions;
using DataAccess.Data;
using DataAccess.Models;
using SmsSendOutApi.Enums;

namespace SmsSendOutApi.SmsVendors;

public class SmsVendorGR : ISmsVendor
{
    private static ISmsRepository _data;

    public CountryCode CountryCode { get; } = CountryCode.Greece;


    public SmsVendorGR(ISmsRepository data)
    {
        _data = data;
    }

    public async Task<IResult> SmsInsert(SmsModel sms)
    {
        var match = Regex.Match(sms.Body, "^[α-ω Α-Ω0-9]*$", RegexOptions.IgnoreCase);

        if (!match.Success)
        {
            return Results.BadRequest("Only greek characters accepted");
        }
        else if (!(sms.Body.Length <= 480))
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