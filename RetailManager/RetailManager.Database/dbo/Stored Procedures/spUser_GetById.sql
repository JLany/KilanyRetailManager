﻿CREATE PROCEDURE [dbo].[spUser_GetById]
	@Id nvarchar(128)

AS
BEGIN

	SET NOCOUNT ON;

	SELECT Id, FirstName, LastName, EmailAddress, CreatedDate
	FROM [dbo].[User] u
	WHERE u.Id = @Id;

END
