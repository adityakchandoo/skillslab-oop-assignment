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
	UserId					nvarchar(40)	     not null,
	FirstName				nvarchar(40)         not null,
	LastName				nvarchar(40)         not null,
	Password				nvarchar(64)         not null,
	Email					nvarchar(40)         not null,
	DOB						datetime             not null,
	NIC						nvarchar(20)         not null,
	MobileNumber			nvarchar(20)         not null,
	CreatedOn				datetime             not null,
	Status					tinyint         	 not null,
	Role					tinyint              not null,
	ManagerId				nvarchar(40)		 null,
	DepartmentId			int					 null,
	constraint PK_AppUser_UserId primary key (UserId),
	constraint FK_Training_ManagerId_AppUser_UserId foreign key (ManagerId)
      references AppUser(UserId),
	constraint FK_Training_DepartmentId_Department_DepartmentId foreign key (DepartmentId)
      references Department(DepartmentId)

)
/*==============================================================*/
/* Table: Training                                              */
/*==============================================================*/
create table Training (
	TrainingId				int                  identity,
	Name					nvarchar(40)         not null,
	Description				text         		 not null,
	Threshold				int         		 not null,
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
	Name					nvarchar(40)         not null unique,
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
	UserTrainingEnrollmentId	int                  identity,
	UserId						nvarchar(40)         not null,
	TrainingId					int       		     not null,
	ApplyDate					datetime     	     not null,
	EnrolledDate				datetime     	     null,
	Status						tinyint     	     not null,
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
	OriginalFilename					nvarchar(40)         not null,
	SystemFilename						nvarchar(40)         not null,
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
	Name					nvarchar(40)         not null,
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
	Name					nvarchar(40)         not null,
	Description				text    		     not null,
	PostDate				datetime     	     not null,
    constraint PK_TrainingContentAttachment_TrainingContentAttachmentId primary key (TrainingContentAttachmentId),
	constraint FK_TrainingContentAttachment_TrainingContentAttachmentId_TrainingContent_TrainingContentId foreign key (TrainingContentId)
      references TrainingContent(TrainingContentId)
)
