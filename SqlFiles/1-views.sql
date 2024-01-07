/*==============================================================*/
CREATE VIEW AllTrainingWithDepartmentName AS
SELECT 
    T.TrainingId,
    D.Name AS DepartmentName,
    COUNT(UTE.UserId) AS NumberOfEmployeesApplied,
	SUM(CASE WHEN UTE.ManagerApprovalStatus = 2 AND UTE.EnrollStatus = 2 THEN 1 ELSE 0 END) AS NumberOfEmployeesEnrolled
FROM 
    Training T
LEFT JOIN 
    UserTrainingEnrollment UTE ON T.TrainingId = UTE.TrainingId
LEFT JOIN 
    Department D ON T.PreferedDepartmentId = D.DepartmentId
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