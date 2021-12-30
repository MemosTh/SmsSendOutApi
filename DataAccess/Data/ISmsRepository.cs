using DataAccess.Models;

namespace DataAccess.Data;

public interface ISmsRepository
{
    Task InsertSms(SmsModel sms);
}