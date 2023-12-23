/*==============================================================*/
CREATE VIEW AllTrainingWithDepartmentName AS
SELECT 
    T.TrainingId,
    D.Name AS DepartmentName,
    COUNT(UTE.UserId) AS NumberOfEmployeesEnrolled
FROM 
    Training T
LEFT JOIN 
    UserTrainingEnrollment UTE ON T.TrainingId = UTE.TrainingId
LEFT JOIN 
    AppUser AU ON UTE.UserId = AU.UserId
LEFT JOIN 
    Department D ON AU.DepartmentId = D.DepartmentId
GROUP BY 
    T.TrainingId, D.Name;
GO
/*==============================================================*/
CREATE VIEW UserRolesInline AS
SELECT 
    u.UserId,
    STRING_AGG(r.Name, ',') WITHIN GROUP (ORDER BY r.Name) AS Roles
FROM 
    AppUser u
INNER JOIN 
    UserRole ur ON u.UserId = ur.UserId
INNER JOIN 
    Role r ON ur.RoleId = r.RoleId
GROUP BY 
    u.UserId
GO