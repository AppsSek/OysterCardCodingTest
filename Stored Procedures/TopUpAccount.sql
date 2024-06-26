USE [LondonTransport]
GO
/****** Object:  StoredProcedure [dbo].[TopUpAccount]    Script Date: 18/05/2024 21:54:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[TopUpAccount]
	-- Add the parameters for the stored procedure here
	@customerID uniqueidentifier, 
	@amount money,
	@success bit OUT,
	@balance money OUT
AS
BEGIN
	update Customer set Balance = Balance + @amount where ID = @customerID
	if(@@ROWCOUNT > 0 ) 
		begin 
			set @success = 1 
			select @balance = Balance from Customer where ID = @customerID
			Insert into CustomerTransaction (DateTime, CustomerID,ModeID,ZoneID, Decription, Debit, CurrentBalance )
			Values (getdate(),@customerID,null,null, 'TopUp for amount £' + cast(@amount as nvarchar),-@amount,@balance)
		end
	
	
	else 
		begin 
			set @success = 0 
		end

	
			
	
	select @success as IsSuccess, @balance as Balance
END
