CREATE TYPE PrerequisitesType AS TABLE
(
    PrerequisiteId INT
);

GO

CREATE OR ALTER PROCEDURE CreateTrainingWithPrerequisite
@Name nvarchar(255),
@Description text,
@MaxSeat int,
@Deadline date,
@PreferedDepartmentId int,
@Prerequisites PrerequisitesType READONLY
AS
BEGIN
    -- Start transaction
    BEGIN TRANSACTION;

    BEGIN TRY
        -- Inserting a new record into MyTable
        INSERT INTO Training (Name, Description, MaxSeat, Deadline, PreferedDepartmentId)
		VALUES (@Name, @Description, @MaxSeat, @Deadline, @PreferedDepartmentId);

        -- Getting the last inserted ID
        DECLARE @LastID int;
        SET @LastID = SCOPE_IDENTITY();

        -- Insert Attachments
        INSERT INTO TrainingPrerequisite (TrainingId, PrerequisiteId)
		SELECT @LastID, PrerequisiteId FROM @Prerequisites;

        -- Commit the transaction
        COMMIT TRANSACTION;
        PRINT 'Transaction committed successfully';
    END TRY
    BEGIN CATCH
        -- Rollback the transaction in case of an error
        ROLLBACK TRANSACTION;
        PRINT 'Error occurred: ' + ERROR_MESSAGE();
    END CATCH
END
