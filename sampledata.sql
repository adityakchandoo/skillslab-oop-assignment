INSERT INTO Department (DepartmentId, Name, Description) VALUES
('Human Resources', 'Handles employee related matters'),
('IT', 'Manages IT infrastructure and support'),
('Finance', 'Handles financial operations'),
('Sales', 'Responsible for sales and customer relationships'),
('Marketing', 'Manages marketing strategies and campaigns');


INSERT INTO AppUser (UserName, Password, FirstName, LastName, Email, DOB, NIC, MobileNumber, CreatedOn, Status, DepartmentId) VALUES
('aditya', 'bc182bb5c12e3ac6c226888d0a5eb1be0d4cb3b8186bfa48e2d2ae9cff6f8998', 'FirstName1', 'LastName1', 'user1@example.com', '1995-01-18', '86345800229449', '+12345678901', '2023-12-17', 1, 5),
('antman', 'bc182bb5c12e3ac6c226888d0a5eb1be0d4cb3b8186bfa48e2d2ae9cff6f8998', 'FirstName2', 'LastName2', 'user2@example.com', '1986-01-10', '85143035745811', '+12345678902', '2023-12-17', 1, 3),
('tony', 'bc182bb5c12e3ac6c226888d0a5eb1be0d4cb3b8186bfa48e2d2ae9cff6f8998', 'FirstName2', 'LastName2', 'user2@example.com', '1986-01-10', '85143035745812', '+12345678902', '2023-12-17', 1, 3),
('strange', 'bc182bb5c12e3ac6c226888d0a5eb1be0d4cb3b8186bfa48e2d2ae9cff6f8998', 'FirstName2', 'LastName2', 'user2@example.com', '1986-01-10', '85143035745813', '+12345678902', '2023-12-17',1, 3);

INSERT INTO UserRole (UserId, RoleId) VALUES
(4, 2),
(5, 2),
(6, 1),
(7, 3);


