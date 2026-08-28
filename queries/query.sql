-- ============================================================================
-- Script: create_kanannon_user (query.sql)
-- Description: Creates a SQL Server Login and Database User with access
--              restricted exclusively to the 'kanannon' database.
-- ============================================================================

-- 1. Switch to 'master' to create the Server-level Login
USE [master];
GO

-- Set variables (or replace placeholders directly)
-- NOTE: Please replace 'YourStrongPasswordHere!123' with your secure password.
IF NOT EXISTS (SELECT 1 FROM sys.server_principals WHERE name = N'kanannon_user')
BEGIN
    CREATE LOGIN [kanannon_user] 
    WITH PASSWORD = N'YourStrongPasswordHere!123',
         DEFAULT_DATABASE = [kanannon],
         CHECK_EXPIRATION = OFF,
         CHECK_POLICY = ON;
    PRINT 'Login [kanannon_user] created successfully.';
END
ELSE
BEGIN
    PRINT 'Login [kanannon_user] already exists.';
END
GO

-- 2. Switch to 'kanannon' database to create and configure the Database User
USE [kanannon];
GO

-- Create the database user mapped to the login
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = N'kanannon_user')
BEGIN
    CREATE USER [kanannon_user] FOR LOGIN [kanannon_user] WITH DEFAULT_SCHEMA = [dbo];
    PRINT 'Database User [kanannon_user] created successfully in [kanannon].';
END
ELSE
BEGIN
    PRINT 'Database User [kanannon_user] already exists in [kanannon].';
END
GO

-- 3. Grant access roles inside 'kanannon'
-- db_datareader: allows reading data from all tables/views
ALTER ROLE [db_datareader] ADD MEMBER [kanannon_user];

-- db_datawriter: allows inserting, updating, and deleting data in all tables
ALTER ROLE [db_datawriter] ADD MEMBER [kanannon_user];

-- Optional: Allow executing stored procedures/functions if needed
GRANT EXECUTE TO [kanannon_user];

-- Optional (Alternative): If the user needs full control (including DDL/migrations) over 'kanannon' only:
-- ALTER ROLE [db_owner] ADD MEMBER [kanannon_user];

PRINT 'Permissions granted to [kanannon_user] on [kanannon] database.';
GO

-- 4. Verification queries
-- Verify login and its default database:
SELECT name, default_database_name, type_desc, is_disabled 
FROM sys.server_principals 
WHERE name = 'kanannon_user';

-- Verify database user in kanannon:
USE [kanannon];
SELECT dp.name AS DatabaseUser, dp.type_desc, sp.name AS ServerLogin
FROM sys.database_principals dp
LEFT JOIN sys.server_principals sp ON dp.sid = sp.sid
WHERE dp.name = 'kanannon_user';
GO
