USE [LondonTransport]
GO
/****** Object:  StoredProcedure [dbo].[UpdateTransactionOnSwipeIn]    Script Date: 18/05/2024 22:07:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[UpdateTransactionOnSwipeIn] 
	-- Add the parameters for the stored procedure here
	@customerID uniqueidentifier, 
	@modeID int,
	@stationID int = null,
	@isAllowed bit OUT
AS
BEGIN

DECLARE
	@maxFare money,
	@minFare money,
	@zoneID int,
	@currentAvailableBalance money,
	@travelDescription nvarchar(max),
	@modeTableName nvarchar(max),
	@sql nvarchar(max)

	Select @currentAvailableBalance = Balance from Customer where ID = @customerID

	Select @modeTableName = TableName, @maxFare = MaxFare from Mode where ID = @modeID

	--This is for Mode - BUS
	if( @modeID = 1 )
		BEGIN
			if( @currentAvailableBalance < @maxFare )
				SET @isAllowed = 0
			else
				begin
					Insert into CustomerTransaction (DateTime, CustomerID,ModeID,ZoneID, Decription, Debit, CurrentBalance )
					Values (getdate(),@customerID,@modeID,null,'By bus' ,@maxFare,(@currentAvailableBalance-@maxFare))

					Update Customer set Balance = (@currentAvailableBalance-@maxFare) where ID = @customerID

					if(@@ROWCOUNT> 0 ) begin set @isAllowed = 1 end

					else begin set @isAllowed = 0 end
				end
		END
	
	ELSE
		BEGIN
			--This is for mode- Tube
			select @minFare = min(minFare), @zoneID = SZ.ZoneID from TubeZones SZ 
			inner join Zones Z on Z.ID = SZ.ZoneID
			where SZ.StationID = @stationID
			group by StationID,ZoneID

			if( @currentAvailableBalance < @minFare )
				begin
					set @isAllowed = 0
				end
			else
				begin
					--WHAT IF your balance is EQUAL to minimum account, will you balance go into negative when you swipe out?
					Update Customer set Status = 1 where ID = @customerID

					Insert into CustomerTransaction (DateTime, CustomerID,ModeID,ZoneID, Decription, Debit, CurrentBalance )
					Values (getdate(),@customerID,@modeID,@zoneID, 'By Tube',@maxFare,(@currentAvailableBalance-@maxFare))
			
					if(@@ROWCOUNT > 0 ) begin set @isAllowed = 1 end

					else begin set @isAllowed = 0 end
				end
			END
SELECT @isAllowed as IsAllowed
END
