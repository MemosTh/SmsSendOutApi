CREATE PROCEDURE [dbo].[spSms_Insert]
	@Recipient nvarchar(15),
	@Body text
	
AS
begin
	insert into dbo.[Sms]  (Recipient  , Body )
	values (@Recipient , @Body);
end
