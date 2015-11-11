

Alter Procedure spTest
(
	@UserName varchar(20),
@UserPwd varchar(max)
)
AS
Begin
Declare @sql nvarchar(max)
set @sql = N'select UserName,UserPwd from Users where UserName='''+@UserName+''' And UserPwd = '''+@UserPwd+''''
Exec sp_executesql @sql
End

