create or alter view vwAgencies
as
select agency.Id, entity.CompanyName from [dbo].[Agency_ProgramStateAgency] as programstateagency
join [dbo].[Agency_Agency] as agency on programstateagency.AgencyId = agency.Id
join [dbo].[Agency_Entity] as entity on agency.EntityId = entity.Id
where programstateagency.ProgramId = 2

GO

