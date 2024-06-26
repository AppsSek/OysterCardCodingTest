USE [LondonTransport]
GO
/****** Object:  StoredProcedure [dbo].[UpdateTransactionOnSwipeOut]    Script Date: 19/05/2024 18:59:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[UpdateTransactionOnSwipeOut] 
	-- Add the parameters for the stored procedure here
	@customerID uniqueidentifier, 
	@modeID int,
	@stationID int,
	@success bit OUT
AS
BEGIN

DECLARE
	@actualFare money,
	@minFare money,
	@outZoneID int,
	@InZoneID int,
	@currentAvailableBalance money,
	@noOfZonesTravelled int,
	@status bit,
	@travelDescription nvarchar(max),
	@modeTableName nvarchar(max),
	@sql nvarchar(max),
	@ParmDefinition nvarchar(500)

	Select @currentAvailableBalance = Balance, @status = Status from Customer where ID = @customerID

	--This is for Mode - BUS
	if( @modeID = 1 )
		BEGIN
			SET @success = 1
		END
	
	ELSE
		BEGIN

			--This is for mode- Tube
			--Assumption - Taking the OUT Zone as the zone which will have LOWEST fare,
			--For example: If travelling from Holborn to Earl'sCourt, Out Zone of Earl's Court will be 1
			--BUT If travelling from Wimbledon to Earl'sCourt, Out Zone of Earl's Court will be 2
			select Top 1 @InZoneID = zoneId, @actualFare = CurrentBalance from CustomerTransaction where CustomerID = @customerID and ZoneID is not null and ZoneID != 99 and Debit > 0 order by DateTime Desc

			if ( @status = 0 )
				begin
					Update Customer set Balance = @actualFare where ID = @customerID
					if(@@ROWCOUNT > 0 ) begin set @success = 1 end
					else begin set @success = 0 end
				end
			else
				begin
					select @minFare = min(minFare), @outZoneID = SZ.ZoneID from TubeZones SZ 
					inner join Zones Z on Z.ID = SZ.ZoneID
					where SZ.StationID = @stationID
					group by StationID,ZoneID
			
					set @noOfZonesTravelled = Abs(@InZoneID-@outZoneID) + 1

					set @sql = N'select @actualFare = _' + CAST(@noOfZonesTravelled as varchar) + N' from Zones where ID = ' + CAST(@outZoneID as nchar)
					SET @ParmDefinition = N'@actualFare money OUTPUT';
					EXEC sp_executesql  @sql, @ParmDefinition, @actualFare OUT
					select @noOfZonesTravelled as ZONES
					Update Customer set Status = 0, Balance = (@currentAvailableBalance-@actualFare) where ID = @customerID

					Insert into CustomerTransaction (DateTime, CustomerID,ModeID,ZoneID, Decription, Debit, CurrentBalance )
					Values (getdate(),@customerID,@modeID,@outZoneID, 'By Tube',@actualFare,(@currentAvailableBalance-@actualFare))
			
					if(@@ROWCOUNT > 0 ) begin set @success = 1 end

					else begin set @success = 0 end
				end
			END
SELECT @success as Success
END
