#########################################################################
#																		#
#  WebApi Service for party radar android-app and web-app				#
#  																		#
#########################################################################

For database
==============
-- Create new Datatbase called: PartyDb
-- goto Security/logins and right click on [New Logins...] to create
-- 		user: partyservice
-- 		pwd: partyservice.pwd
-- 		set default database to PartyDb
-- goto Security/logins and right click on user partyWebApi select properties
-- 		goto User Mapping and map to PartyDb
--		goto "Database role membership" and select db_datareader, db_datawriter, db_ddladmin, db_owner, public
--		finish with clicking [ok]
-- errors: for Microsoft SQL Server Error 18456 Login Failed for User see https://www.youtube.com/watch?v=aU8RhjdkCoE
--		or look at this site (same content but presented textual): http://bjtechnews.org/2012/03/20/microsoft-sql-server-error-18456-login-failed-for-user/


-- login to get token:
http://www.asp.net/web-api/overview/security/individual-accounts-in-web-api

for creating db structure from inside the visual studio
package manager console:
// to make this happens:
PM> enable-migrations -ContextTypeName PartyService.Models.ApplicationDbContext -MigrationsDirectory:Migrations

PM> Add-Migration <init>  // initialize model but build project first; creates an class with update code like convention: YYYYMMDDHHmmss_<init>.cs
// as next update script (YYYYMMDDHHmmss_<init>.cs) is created insert update script:
PM> Update-Database

//roll back hole database -> deletes all; what I don't know is how to roll back to specific version...
// maybe instead of "-TargetMigration $InitialDatabase" use "-TargetMigration YYYYMMDDHHmmss_<init>" ?
// $InitialDatabase is used to roll back all
PM>Update-Database -TargetMigration $InitialDatabase

