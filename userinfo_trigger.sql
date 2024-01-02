create table AppUserAudit (
	AppUserAuditId			int	identity,
	UserId					int,
	UserName				nvarchar(50),
	Password				varchar(64),
	Email					nvarchar(255),
	MobileNumber			varchar(20),
	Status					tinyint,
	TriggerDate				datetime,
	constraint PK_AppUserAudit_AppUserAuditId primary key (AppUserAuditId),
)
go

create trigger AppUserAuditTrigger on AppUser
after update, insert
as
begin
  insert into AppUserAudit 
  (UserId, UserName, Password, Email, MobileNumber, Status, TriggerDate)
  select i.UserId, i.UserName, i.Password, i.Email, i.MobileNumber, i.Status, getdate() 
  from  AppUser au 
  inner join inserted i on  au.UserId = i.UserId 
end
go