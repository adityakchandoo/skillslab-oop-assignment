/*==============================================================*/
/* Table: Department                                            */
/*==============================================================*/
create table Department (
	DepartmentId			int                  identity,
	Name					nvarchar(40)         not null unique,
	Description				text        		 null,
	constraint PK_Department_DepartmentId primary key (DepartmentId)
)

/*==============================================================*/
/* Table: AppUser                                               */
/*==============================================================*/
create table AppUser (
	UserId					int	   			     identity,
	UserName				nvarchar(50)	     not null unique,
	Password				nvarchar(64)         not null,
	FirstName				nvarchar(255)        not null,
	LastName				nvarchar(255)        not null,	
	Email					nvarchar(255)        not null,
	DOB						date             	 not null,
	NIC						varchar(14)          not null unique,
	MobileNumber			varchar(20)          not null,
	CreatedOn				date            	 not null,
	Status					tinyint         	 not null,
	DepartmentId			int					 null,
	constraint PK_AppUser_UserId primary key (UserId),
	constraint FK_Training_DepartmentId_Department_DepartmentId foreign key (DepartmentId)
      references Department(DepartmentId)
)

/*==============================================================*/
/* Table: Role                                                  */
/*==============================================================*/
create table Role (
	RoleId				int,
	Name				nvarchar(40)         not null unique,
	constraint PK_Role_RoleId primary key (RoleId)
)
/*==============================================================*/
/* Table: UserRole                                              */
/*==============================================================*/
CREATE TABLE UserRole (
	UserRoleId			int	     identity,
    UserId      		int		 NOT NULL,
    RoleId        		int      NOT NULL,
    CONSTRAINT PK_UserRole_UserRoleId PRIMARY KEY (UserRoleId),
    CONSTRAINT FK_UserRole_UserId_AppUser_UserId FOREIGN KEY (UserId)
        REFERENCES AppUser(UserId),
	CONSTRAINT FK_UserRole_RoleId_Role_RoleId FOREIGN KEY (RoleId)
        REFERENCES Role(RoleId)
);

/*==============================================================*/
/* Table: UserManager                                              */
/*==============================================================*/
CREATE TABLE UserManager (
	UserManagerId		int          identity,
	UserId      		int          NOT NULL,
	ManagerId     		int 		 NOT NULL,
    CONSTRAINT PK_UserManager_UserManagerId PRIMARY KEY (UserManagerId),
    CONSTRAINT FK_UserManager_UserId_AppUser_UserId FOREIGN KEY (UserId)
        REFERENCES AppUser(UserId),
    CONSTRAINT FK_UserManager_ManagerId_AppUser_UserId FOREIGN KEY (ManagerId)
        REFERENCES AppUser(UserId)
);

/*==============================================================*/
/* Table: Training                                              */
/*==============================================================*/
create table Training (
	TrainingId				int                  identity,
	Name					nvarchar(255)        not null,
	Description				text         		 not null,
	MaxSeat					int         		 not null,
	Deadline				datetime             not null,
	PreferedDepartmentId	int					 null,
    constraint PK_Training_TrainingId primary key (TrainingId),
	constraint FK_Training_PreferedDepartmentId_Department_DepartmentId foreign key (PreferedDepartmentId)
	references Department(DepartmentId)
)


/*==============================================================*/
/* Table: Prerequisite                                          */
/*==============================================================*/
create table Prerequisite (
	PrerequisiteId			int                  identity,
	Name					nvarchar(255)        not null unique,
	Description				text    		     not null,
    constraint PK_Prerequisite_PrerequisiteId primary key (PrerequisiteId)
)

/*==============================================================*/
/* Table: TrainingPrerequisite                                  */
/*==============================================================*/
create table TrainingPrerequisite (
	TrainingPrerequisiteId  int                  identity,
	TrainingId				int                  not null,
	PrerequisiteId			int                  not null,
	constraint PK_TrainingPrerequisite_TrainingPrerequisiteId primary key (TrainingPrerequisiteId),
	constraint FK_TrainingPrerequisite_TrainingId_Training_TrainingId foreign key (TrainingId)
      references Training(TrainingId),
	constraint FK_TrainingPrerequisite_PrerequisiteId_Training_PrerequisiteId foreign key (PrerequisiteId)
      references Prerequisite(PrerequisiteId)
)


/*==============================================================*/
/* Table: UserTrainingEnrollment	                            */
/*==============================================================*/
create table UserTrainingEnrollment (
	UserTrainingEnrollmentId	int              identity,
	UserId						int         	 not null,
	TrainingId					int       		 not null,
	ApplyDate					date     	     not null,
	EnrolledDate				date     	     null,
	Status						tinyint     	 not null,
    constraint PK_UserTrainingEnrollment_UserTrainingEnrollmentId primary key (UserTrainingEnrollmentId),
	constraint FK_UserTrainingEnrollment_UserId_AppUser_UserId foreign key (UserId)
      references AppUser(UserId),
	constraint FK_UserTrainingEnrollment_TrainingId_Training_TrainingId foreign key (TrainingId)
      references Training(TrainingId)
)

/*==============================================================*/
/* Table: EnrollmentPrerequisiteAttachment                      */
/*==============================================================*/
create table EnrollmentPrerequisiteAttachment (
	EnrollmentPrerequisiteAttachmentId	int                  identity,
	EnrollmentId						int        			 not null,
	TrainingPrerequisiteId  			Int					 not null,
	OriginalFilename					nvarchar(255)        not null,
	FileKey								uniqueidentifier     not null,
    constraint EnrollmentPrerequisiteAttachmentId primary key (EnrollmentPrerequisiteAttachmentId),
	constraint FK_EnrollmentPrerequisiteAttachment_EnrollmentId_UserTrainingEnrollment_UserTrainingEnrollmentId foreign key (EnrollmentId)
      references UserTrainingEnrollment(UserTrainingEnrollmentId),
	constraint FK_EnrollmentAttachment_TrainingPrerequisiteId_TrainingPrerequisite_TrainingPrerequisiteId foreign key (TrainingPrerequisiteId)
      references TrainingPrerequisite(TrainingPrerequisiteId)
)

/*==============================================================*/
/* Table: TrainingContent                                       */
/*==============================================================*/
create table TrainingContent (
	TrainingContentId		int                  identity,
	TrainingId				int         		 not null,
	Name					nvarchar(255)        not null,
	Description				text    		     not null,
	PostDate				datetime     	     not null,
    constraint PK_TrainingContent_TrainingContentId primary key (TrainingContentId),
	constraint FK_TrainingContent_TrainingId_Training_TrainingId foreign key (TrainingId)
      references Training(TrainingId)
)

/*==============================================================*/
/* Table: TrainingContentAttachment                             */
/*==============================================================*/
create table TrainingContentAttachment (
	TrainingContentAttachmentId		int                  identity,
	TrainingContentId				int         		 not null,
	OriginalFilename				nvarchar(255)        not null,
	FileKey							uniqueidentifier     not null,
    constraint PK_TrainingContentAttachment_TrainingContentAttachmentId primary key (TrainingContentAttachmentId),
	constraint FK_TrainingContentAttachment_TrainingContentAttachmentId_TrainingContent_TrainingContentId foreign key (TrainingContentId)
      references TrainingContent(TrainingContentId)
)