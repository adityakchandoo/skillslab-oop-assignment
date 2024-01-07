CREATE TABLE AutoProcessLog
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    StartTime DATETIME,
    EndTime DATETIME
);

GO

CREATE OR ALTER PROCEDURE AutomaticProcess
AS
BEGIN
    SET NOCOUNT ON;

	-- Check if the procedure was run in the last hour
	IF NOT EXISTS (
		SELECT 1 
		FROM AutoProcessLog
		WHERE StartTime > DATEADD(HOUR, -1, GETDATE())
	)
	BEGIN
		DECLARE @LogId INT;
		INSERT INTO AutoProcessLog (StartTime)
		VALUES (GETDATE());

		SET @LogId = SCOPE_IDENTITY();

		BEGIN TRANSACTION;
		-- temp table to contain enrollment
		CREATE TABLE #PotencialEnrollments (
			UserTrainingEnrollmentId INT,
			ApplyDate DATE,
			DepartmentMatch BIT,
			TrainingId INT
		);

		CREATE TABLE #Selected (
			UserId int,
			Email nvarchar(255),
			TrainingName nvarchar(255)
		);

		-- fill temporary table with eligible users, marking department matches
		-- for trainings whose deadline has passed and EnrollStatus is not 0
		-- Populate the t
		INSERT INTO #PotencialEnrollments (UserTrainingEnrollmentId, ApplyDate, DepartmentMatch, TrainingId)
		SELECT 
			UTE.UserTrainingEnrollmentId, 
			UTE.ApplyDate, 
			CASE 
				WHEN T.PreferedDepartmentId IS NOT NULL AND U.DepartmentId = T.PreferedDepartmentId THEN 1 
				ELSE 0 
			END,
			T.TrainingId
		FROM 
			UserTrainingEnrollment UTE
		INNER JOIN 
			AppUser U ON UTE.UserId = U.UserId
		INNER JOIN 
			Training T ON UTE.TrainingId = T.TrainingId
		WHERE 
			T.Deadline < GETDATE() -- Deadline is passed
			AND UTE.ManagerApprovalStatus = 2 -- Manager has approved the training requests
			AND UTE.EnrollStatus = 1; -- status is pending (not yet processed)

		-- Iterate through each training to update enrollment statuses
		DECLARE @CurrentTrainingId INT;
		DECLARE training_cursor CURSOR FOR
		SELECT DISTINCT TrainingId FROM #PotencialEnrollments;

		OPEN training_cursor;
		FETCH NEXT FROM training_cursor INTO @CurrentTrainingId;

		WHILE @@FETCH_STATUS = 0
		BEGIN
			-- Get the MaxSeat value for the current training
			DECLARE @MaxSeat INT;
			SELECT @MaxSeat = MaxSeat FROM Training WHERE TrainingId = @CurrentTrainingId;

			-- Create temp table to hold n select enrollments depending on filter
			CREATE TABLE #CurrentSelection (UserTrainingEnrollmentId INT);

			-- Fill the table
			INSERT INTO #CurrentSelection (UserTrainingEnrollmentId)
			SELECT TOP (@MaxSeat) UserTrainingEnrollmentId
			FROM #PotencialEnrollments
			WHERE TrainingId = @CurrentTrainingId
			ORDER BY DepartmentMatch DESC, ApplyDate ASC;
			
			-- Update the selected employees enrollment status to 2 (Approved) - This is an enum in code
		    UPDATE UserTrainingEnrollment
		    SET EnrollStatus = 2
		    WHERE UserTrainingEnrollmentId IN (SELECT UserTrainingEnrollmentId FROM #CurrentSelection)
		    AND TrainingId = @CurrentTrainingId;

			--Insert to employee to inform later
			INSERT INTO #Selected
			SELECT AU.UserId, AU.Email, T.Name FROM #CurrentSelection CE
			INNER JOIN UserTrainingEnrollment UTE ON UTE.UserTrainingEnrollmentId = CE.UserTrainingEnrollmentId
			INNER JOIN AppUser AU ON AU.UserId = UTE.UserId
			INNER JOIN Training T ON T.TrainingId = UTE.TrainingId

			-- Update the remaining employees enrollment status to 3 (Rejected)
			UPDATE UserTrainingEnrollment
			SET EnrollStatus = 3
			WHERE 
			    TrainingId = @CurrentTrainingId
			    AND ManagerApprovalStatus = 2
			    AND EnrollStatus != 0
			    AND UserTrainingEnrollmentId NOT IN (SELECT UserTrainingEnrollmentId FROM #CurrentSelection);

			-- DELETE temp table for next training
			DROP TABLE #CurrentSelection;

			-- Get the next traning id into the cursor
			FETCH NEXT FROM training_cursor INTO @CurrentTrainingId;
		END

		CLOSE training_cursor;
		DEALLOCATE training_cursor;

		SELECT * FROM #Selected;

		DROP TABLE #PotencialEnrollments;
		DROP TABLE #Selected;
		COMMIT TRANSACTION;

		UPDATE AutoProcessLog
		SET EndTime = GETDATE()
		WHERE Id = @LogId;

	END
	ELSE
	BEGIN
		PRINT('already run');
	END
END