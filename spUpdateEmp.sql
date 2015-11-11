Create procedure spUpdateEmp
(@EmpID int,@FamilyName varchar(50),@FirstName varchar(50),@Gender char(1),@Nationality varchar(50),
@Birth smalldatetime,@MaritalStatus varchar(20),@Address varchar(50),@HireDate smalldatetime,
@Department varchar(40),@Salary int,@SocialSecurityNum varchar(MAX),@Position varchar(50))
AS
BEGIN
Declare @DeptID int
Declare @PositionID int
select @DeptID=DeptID from Departments where Department=@Department;
select @PositionID=PositionID from Positions where Position=@Position;
Update Employees
Set FamilyName = @FamilyName, FirstName = @FirstName,
    Gender = @Gender, Nationality = @Nationality, DateOfBirth = @Birth, MaritalStatus= @MaritalStatus,
    Address = @Address, HireDate = @HireDate, DeptID = @DeptID, PositionID = @PositionID,
    Salary = @Salary, SocialSecurityNum = @SocialSecurityNum
Where EmpID = @EmpID;
END

