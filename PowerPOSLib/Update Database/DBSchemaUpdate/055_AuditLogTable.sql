IF  NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[AuditLog]') AND type in (N'U'))

BEGIN

CREATE TABLE [dbo].[AuditLog](
	[AuditLogID] [uniqueidentifier] NOT NULL,
	[LogDate] [datetime] NULL,
	[Operation] [varchar](MAX) NULL,	
	[TableName] [varchar](MAX) NULL,
	[PrimaryKeyCol] [varchar](MAX) NULL,	
	[PrimaryKeyVal] [varchar](MAX) NULL,		
	[OldValues] [nvarchar](MAX) NULL,	
	[NewValues] [nvarchar](MAX) NULL,	
	[Remarks] [nvarchar](MAX) NULL
 CONSTRAINT [PK_AuditLog] PRIMARY KEY CLUSTERED 
(
	[AuditLogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

END

IF EXISTS(SELECT * FROM sys.columns WHERE [name] = N'CreatedOn' AND [object_id] = OBJECT_ID(N'AuditLog'))
BEGIN
    ALTER TABLE AuditLog DROP COLUMN CreatedOn
END
IF EXISTS(SELECT * FROM sys.columns WHERE [name] = N'CreatedBy' AND [object_id] = OBJECT_ID(N'AuditLog'))
BEGIN
    ALTER TABLE AuditLog DROP COLUMN CreatedBy
END
IF EXISTS(SELECT * FROM sys.columns WHERE [name] = N'ModifiedOn' AND [object_id] = OBJECT_ID(N'AuditLog'))
BEGIN
    ALTER TABLE AuditLog DROP COLUMN ModifiedOn
END
IF EXISTS(SELECT * FROM sys.columns WHERE [name] = N'ModifiedBy' AND [object_id] = OBJECT_ID(N'AuditLog'))
BEGIN
    ALTER TABLE AuditLog DROP COLUMN ModifiedBy
END
IF EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Deleted' AND [object_id] = OBJECT_ID(N'AuditLog'))
BEGIN
    ALTER TABLE AuditLog DROP COLUMN Deleted
END
