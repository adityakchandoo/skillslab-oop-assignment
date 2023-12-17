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
/*==============================================================*/

INSERT INTO Role (RoleId, Name) VALUES (1, 'Admin');
INSERT INTO Role (RoleId, Name) VALUES (2, 'Manager');
INSERT INTO Role (RoleId, Name) VALUES (3, 'Employee');