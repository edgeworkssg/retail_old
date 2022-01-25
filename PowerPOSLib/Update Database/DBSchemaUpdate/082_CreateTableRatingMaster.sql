IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RatingMaster]') AND type in (N'U'))
BEGIN

CREATE TABLE [dbo].[RatingMaster](
	[Rating] [int] IDENTITY(1,1) NOT NULL,
	[RatingName] [varchar](50) NULL,
	[RatingImage] [varbinary](max) NULL,
	[RatingType] [varchar](50) NULL,
	[Weight] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[Deleted] [bit] NULL,
 CONSTRAINT [PK_RatingMaster] PRIMARY KEY CLUSTERED 
(
	[Rating] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


END


IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'RatingImageUrl' AND [object_id] = OBJECT_ID(N'RatingMaster'))
BEGIN
    ALTER TABLE RatingMaster ADD RatingImageUrl varchar(MAX) NULL
END

IF((SELECT COUNT(*) FROM RatingMaster WHERE Rating = 1)<=0)
BEGIN 
	SET IDENTITY_INSERT RatingMaster ON;	
	
	INSERT into RatingMaster (Rating, RatingName, RatingType, Weight, RatingImageUrl, CreatedBy, CreatedOn,
	ModifiedBy, ModifiedOn, Deleted) 
	VALUES (1,'Very Poor','Bad Rating', 0, 'Very Poor.png', 'SCRIPTS',GETDATE(),'SCRIPTS',GETDATE(),0) 
	
	SET IDENTITY_INSERT RatingMaster OFF;
END 


IF((SELECT COUNT(*) FROM RatingMaster WHERE Rating = 2)<=0)
BEGIN 
	SET IDENTITY_INSERT RatingMaster ON;	

	INSERT into RatingMaster (Rating, RatingName, RatingType, Weight, RatingImageUrl, CreatedBy, CreatedOn,
	ModifiedBy, ModifiedOn, Deleted) 
	VALUES (2,'Poor','Bad Rating', 25, 'Poor.png', 'SCRIPTS',GETDATE(),'SCRIPTS',GETDATE(),0) 
		
	SET IDENTITY_INSERT RatingMaster OFF;
END 

IF((SELECT COUNT(*) FROM RatingMaster WHERE Rating = 3)<=0)
BEGIN 
	SET IDENTITY_INSERT RatingMaster ON;

	INSERT into RatingMaster (Rating, RatingName, RatingType, Weight, RatingImageUrl, CreatedBy, CreatedOn,
	ModifiedBy, ModifiedOn, Deleted) 
	VALUES (3,'Satisfactory','Good Rating', 50, 'Average.png', 'SCRIPTS',GETDATE(),'SCRIPTS',GETDATE(),0) 
		
	SET IDENTITY_INSERT RatingMaster OFF;
END 

IF((SELECT COUNT(*) FROM RatingMaster WHERE Rating = 4)<=0)
BEGIN 
	SET IDENTITY_INSERT RatingMaster ON;

	INSERT into RatingMaster (Rating, RatingName, RatingType, Weight, RatingImageUrl, CreatedBy, CreatedOn,
	ModifiedBy, ModifiedOn, Deleted) 
	VALUES (4,'Good','Good Rating', 75, 'Good.png', 'SCRIPTS',GETDATE(),'SCRIPTS',GETDATE(),0) 
		
	SET IDENTITY_INSERT RatingMaster OFF;
END 

IF((SELECT COUNT(*) FROM RatingMaster WHERE Rating = 5)<=0)
BEGIN 
	SET IDENTITY_INSERT RatingMaster ON;

	INSERT into RatingMaster (Rating, RatingName, RatingType, Weight, RatingImageUrl, CreatedBy, CreatedOn,
	ModifiedBy, ModifiedOn, Deleted) 
	VALUES (5,'Excellent','Good Rating', 100, 'Excellent.png', 'SCRIPTS',GETDATE(),'SCRIPTS',GETDATE(),0) 
		
	SET IDENTITY_INSERT RatingMaster OFF;
END 