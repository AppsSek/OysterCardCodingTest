USE [LondonTransport]
GO
/****** Object:  StoredProcedure [dbo].[GetCurrentBalance]    Script Date: 18/05/2024 21:25:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[GetCurrentBalance] 
	-- Add the parameters for the stored procedure here
	@customerID uniqueidentifier,
	@balance money OUT
AS
BEGIN
	DECLARE
	@status bit

	select @balance = Balance, @status = Status from Customer where ID = @customerID

	if( @status = 1 )
		begin
			Select @balance = CurrentBalance from CustomerTransaction where CustomerID = @customerID and ModeID != null and Debit > 0
		end
select @balance as Balance
END
