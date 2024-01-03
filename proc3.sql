CREATE TYPE ContentAttachmentType AS TABLE
(
    OriginalFilename nvarchar(255),
	FileKey uniqueidentifier
);

GO

CREATE OR ALTER PROCEDURE CreateTrainingContentWithAttachment
@TrainingId int,
@Name nvarchar(255),
@Description text,
@ContentAttachment ContentAttachmentType READONLY
AS
BEGIN
    -- Start transaction
    BEGIN TRANSACTION;

    BEGIN TRY
        -- Inserting a new record into MyTable
        INSERT INTO TrainingContent (TrainingId, Name, Description)
		VALUES (@TrainingId, @Name, @Description);

        -- Getting the last inserted ID
        DECLARE @LastID int;
        SET @LastID = SCOPE_IDENTITY();

        -- Insert Attachments
        INSERT INTO TrainingContentAttachment (TrainingContentId, OriginalFilename, FileKey)
		SELECT @LastID, OriginalFilename, FileKey FROM @ContentAttachment;

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
