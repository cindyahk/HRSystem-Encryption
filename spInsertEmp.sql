alter procedure spInsertEmp
(@EmpID int,@FamilyName varchar(50),@FirstName varchar(50),@Gender char(1),@Nationality varchar(50),
@Birth smalldatetime,@MaritalStatus varchar(20),@Address varchar(50),@HireDate smalldatetime,
@Department varchar(40),@Salary int,@SocialSecurityNum varchar(MAX),@Position varchar(50))
AS
BEGIN
Declare @DeptID int
Declare @PositionID int
select @DeptID=DeptID from Departments where Department=@Department;
select @PositionID=PositionID from Positions where Position=@Position;
INSERT INTO Employees Values(@EmpID,@FamilyName,@FirstName,@Gender,@Nationality,@Birth,@MaritalStatus,@Address,
@HireDate,@DeptID,@Salary,@SocialSecurityNum,@PositionID)
END

exec spInsertEmp 110,'as','as','F','mauritius','1/2/1980','Married','as','2/12/2013','Management',14000,'213','Manager'

select * from Employees