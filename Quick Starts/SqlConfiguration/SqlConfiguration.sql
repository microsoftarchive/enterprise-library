if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EntLib_GetConfig]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[EntLib_GetConfig]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EntLib_SetConfig]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[EntLib_SetConfig]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Configuration_Parameter]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Configuration_Parameter]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[configparam_insupd]') and OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [dbo].[configparam_insupd]
GO

CREATE TABLE [dbo].[Configuration_Parameter] (
	[section_name] [nvarchar] (50) NOT NULL ,
	[section_type] [nvarchar] (300) NOT NULL,
	[section_value] [ntext] NOT NULL ,
	[lastmoddate] [datetime] NOT NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Configuration_parameter] WITH NOCHECK ADD 
	CONSTRAINT [PK_Configuration_parameter] PRIMARY KEY  CLUSTERED 
	(
		[section_name]
	)  ON [PRIMARY] 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE [dbo].[EntLib_GetConfig] 
 @sectionName varchar(50) 
AS
SELECT section_type, section_value, lastmoddate
FROM Configuration_parameter 
WHERE section_name = @sectionName

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE [dbo].[EntLib_SetConfig] 
(
	@section_name nvarchar(50),
	@section_type nvarchar(300),
	@section_value ntext
)
AS
IF( ( SELECT COUNT(*) FROM Configuration_parameter WHERE section_name = @section_name ) = 1 )
BEGIN
	UPDATE Configuration_parameter SET section_value = @section_value, section_type = @section_type
	WHERE section_name = @section_name 
END
ELSE
BEGIN
	DECLARE @thisdate DATETIME
	SELECT @thisdate = GETDATE()
	INSERT INTO Configuration_parameter ( section_name, section_type, section_value, lastmoddate) 
	VALUES ( @section_name, @section_type, @section_value, @thisdate)
END

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE EntLib_RemoveSection
(
	@section_name NVARCHAR(50)
)
AS
	DELETE FROM Configuration_Parameter WHERE section_name = @section_name

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE UpdateSectionDate
(
@section_name NVARCHAR(50),
@mod_date DATETIME = NULL
)
AS
	IF (@mod_date IS NULL) SET @mod_date = GETDATE()
	UPDATE    Configuration_Parameter
	SET       lastmoddate = @mod_date
	WHERE     (section_name = @section_name) 

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE TRIGGER configparam_insupd ON [dbo].[Configuration_Parameter] 
FOR UPDATE
AS
   IF (COLUMNS_UPDATED() & 4)  > 0
  BEGIN
	UPDATE Configuration_Parameter 
	SET lastmoddate = GETDATE()
	WHERE section_name = (SELECT ins.section_name FROM inserted ins)
  END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

delete from Configuration_parameter 
