using System.Text.RegularExpressions;
using DataAccess.Data;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using SmsSendOutApi.Enums;


namespace SmsSendOutApi;

public class SmsVendor 
{
    private static ISmsRepository _data;

    public SmsVendor(ISmsRepository data)
    {
        _data = data;
    }

    public async Task<IResult> SmsVendorManager(SmsModel sms)
    {
        
        var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();
        var phoneNumber = phoneNumberUtil.Parse(sms.Recipient, null);
        CountryCode countryCode = (CountryCode)phoneNumber.CountryCode;

        switch (countryCode)
        {
            case CountryCode.Greece:
                return await SMSVendorGR(sms);
                break;
            case CountryCode.Cyprus:
                return await SMSVendorCY(sms);
                break;
            default:
                return await SMSVendorGeneral(sms);
                break;
        }
    }

    private async Task<IResult> SMSVendorGR(SmsModel sms)
    {
        
        var match = Regex.Match(sms.Body, "^[α-ω Α-Ω0-9]*$", RegexOptions.IgnoreCase);

        if (!match.Success)
        {
            return Results.BadRequest("Only greek characters accepted");
        }
        else if (!CharactersCountValidation(sms.Body))
        {
            return Results.BadRequest("Sorry you are not able to send a message with more than 480 characters");
        }
        else
        {
            await _data.InsertSms(sms);
            return Results.Ok();
        }
        
    }

    private async Task<IResult> SMSVendorCY(SmsModel sms)
    {
        if (!CharactersCountValidation(sms.Body))
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

    private async Task<IResult> SMSVendorGeneral(SmsModel sms)
    {
        if (!CharactersCountValidation(sms.Body))
        {
            return Results.BadRequest("Sorry you are not able to send a message with more than 480 characters");
        }
        else
        {
            await _data.InsertSms(sms);
            return Results.Ok();
        }
        
    }

    private bool CharactersCountValidation(string smsBody)
    {
        return smsBody.Length <= 480;
    }

   
}
