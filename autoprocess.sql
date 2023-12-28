CREATE PROCEDURE AutomaticProcess
AS
BEGIN
    SET NOCOUNT ON;

    -- temp table to contain enrollment
    CREATE TABLE #PotencialEnrollments (
        UserTrainingEnrollmentId INT,
        ApplyDate DATE,
        DepartmentMatch BIT,
        TrainingId INT
    );

    -- Populate the temporary table with eligible users, marking department matches
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
        AND UTE.EnrollStatus != 0; -- -- This Requests has not already been processed

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

        -- Update the top N eligible users' enrollment status to 1, based on MaxSeat, priority, and ApplyDate
        WITH CTE AS (
            SELECT TOP (@MaxSeat) UserTrainingEnrollmentId
            FROM #PotencialEnrollments
            WHERE TrainingId = @CurrentTrainingId
            ORDER BY DepartmentMatch DESC, ApplyDate ASC
        )
		
		-- Update the selected employees enrollment status to 2 (Approved) - This is an enum in code
        UPDATE UserTrainingEnrollment
        SET EnrollStatus = 1
        WHERE UserTrainingEnrollmentId IN (SELECT UserTrainingEnrollmentId FROM CTE)
        AND TrainingId = @CurrentTrainingId;

        -- Update the remaining employees enrollment status to 3 (Rejected)
        UPDATE UserTrainingEnrollment
        SET EnrollStatus = 3
        WHERE 
            TrainingId = @CurrentTrainingId
            AND ManagerApprovalStatus = 2
            AND EnrollStatus != 0
            AND UserTrainingEnrollmentId NOT IN (SELECT UserTrainingEnrollmentId FROM CTE);

		-- Get the next traning id into the cursor
        FETCH NEXT FROM training_cursor INTO @CurrentTrainingId;
    END

    CLOSE training_cursor;
    DEALLOCATE training_cursor;

    DROP TABLE #PotencialEnrollments;
END
