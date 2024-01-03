CREATE TYPE EnrollmentAttachmentType AS TABLE
(
    TrainingPrerequisiteId INT,
    OriginalFilename nvarchar(255),
	FileKey uniqueidentifier
);

GO

CREATE OR ALTER PROCEDURE CreateEnrollmentWithAttachment
@UserId int,
@TrainingId int,
@ApplyDate date,
@ManagerApprovalStatus tinyint,
@EnrollStatus tinyint,
@EnrollmentAttachment EnrollmentAttachmentType READONLY
AS
BEGIN
    -- Start transaction
    BEGIN TRANSACTION;

    BEGIN TRY
        -- Inserting a new record into MyTable
        INSERT INTO UserTrainingEnrollment (UserId, TrainingId, ApplyDate, ManagerApprovalStatus, EnrollStatus)
		VALUES (@UserId, @TrainingId, @ApplyDate, @ManagerApprovalStatus, @EnrollStatus);

        -- Getting the last inserted ID
        DECLARE @LastID int;
        SET @LastID = SCOPE_IDENTITY();

        -- Insert Attachments
        INSERT INTO EnrollmentPrerequisiteAttachment (EnrollmentId, TrainingPrerequisiteId, OriginalFilename, FileKey)
		SELECT @LastID, TrainingPrerequisiteId, OriginalFilename, FileKey FROM @EnrollmentAttachment;

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
