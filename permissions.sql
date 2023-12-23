INSERT INTO Role (RoleId, Name) VALUES (1, 'Admin');
INSERT INTO Role (RoleId, Name) VALUES (2, 'Manager');
INSERT INTO Role (RoleId, Name) VALUES (3, 'Employee');

CREATE PROCEDURE CheckUserPermission
    @UserId INT,
    @Permission VARCHAR(100)
AS
BEGIN
    SELECT 
        COALESCE(
            MAX(CASE 
                    WHEN rp.Permission = @Permission THEN 1 
                    ELSE 0 
                END), 
            0
        ) AS HasPermission
    FROM 
        AppUser u
    LEFT JOIN 
        UserRole ur ON u.UserId = ur.UserId
    LEFT JOIN 
        Role r ON ur.RoleId = r.RoleId
    LEFT JOIN 
        RolePermission rp ON r.RoleId = rp.RoleId
    WHERE 
        u.UserId = @UserId
END


INSERT INTO RolePermission (RoleId, Permission) VALUES
(1, 'admin.dash'),
(1, 'user.profile'),
(1, 'file.download'),
(1, 'department.view'),
(1, 'department.add'),
(1, 'prerequisite.view'),
(1, 'prerequisite.add'),
(1, 'training.viewall'),
(1, 'training.viewone'),
(1, 'training.add'),
(1, 'training.addcontent'),
(1, 'user.viewall')

INSERT INTO RolePermission (RoleId, Permission) VALUES
(2, 'manager.dash'),
(2, 'user.profile'),
(2, 'file.download'),
(2, 'training.viewrequests'),
(2, 'training.processrequests'),
(2, 'user.viewallemployee'),
(2, 'user.viewsubordinates'),
(2, 'user.viewpendingsubordinates')

INSERT INTO RolePermission (RoleId, Permission) VALUES
(3, 'employee.dash'),
(3, 'user.profile'),
(3, 'file.download'),
(3, 'training.dash'),
(3, 'training.viewone'),
(3, 'training.apply')