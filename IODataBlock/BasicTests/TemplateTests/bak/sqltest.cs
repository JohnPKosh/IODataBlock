using Business.Templates;
using Data.DbClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace BasicTests.TemplateTests.bak
{
    [TestClass]
    public class sqltest
    {
        private const string SqlServer = @"CLEHBSQL0301";

        private const string NewSqlServer = @"172.16.5.82";

        private const string SqlServerDatabase = @"master";

        private static string SqlServerConnectionString => Database.CreateSqlConnectionString(SqlServer, SqlServerDatabase);

        private static string NewSqlServerConnectionString => Database.CreateSqlConnectionString(NewSqlServer, SqlServerDatabase);

        [TestMethod]
        public void TestMethod1()
        {
            dynamic model = new ExpandoObject();
            model.name = "LNP";

            var parser = new TemplateParser();
            var templateString = System.IO.File.ReadAllText(@"C:\Users\jkosh\Documents\GitHub\IODataBlock\IODataBlock\BasicTests\TemplateTests\bak\sql.cshtml");

            var sql = parser.RenderTemplate(model, typeof(ExpandoObject), templateString);

            Assert.IsNotNull(sql);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var data = GetModels();
            foreach (var o in data)
            {
                var sql = ParseTemplate(o.name, o.dataFileName, o.logFileName, @"C:\Users\jkosh\Documents\GitHub\IODataBlock\IODataBlock\BasicTests\TemplateTests\bak\sql.cshtml");
                Assert.IsNotNull(sql);
                System.IO.File.WriteAllText(System.IO.Path.Combine(@"C:\junk\migration", string.Format(@"{0}.sql", o.name)), sql);
            }

            var sqlcmd = ParseTemplates(data.Select(x => x.name as string).Distinct(), @"C:\Users\jkosh\Documents\GitHub\IODataBlock\IODataBlock\BasicTests\TemplateTests\bak\sqlcmd.cshtml");
            System.IO.File.WriteAllText(System.IO.Path.Combine(@"C:\junk\migration", @"sqlcmd.sql"), sqlcmd);

            var checkdb = ParseTemplates(data.Select(x => x.name as string).Distinct(), @"C:\Users\jkosh\Documents\GitHub\IODataBlock\IODataBlock\BasicTests\TemplateTests\bak\checkdb.cshtml");
            System.IO.File.WriteAllText(System.IO.Path.Combine(@"C:\junk\migration", @"checkdb.sql"), checkdb);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var data = GetNewServerModels();
            foreach (var o in data)
            {
                var sql = ParseTemplate(o.name, o.dataFileName, o.logFileName, @"C:\Users\jkosh\Documents\GitHub\IODataBlock\IODataBlock\BasicTests\TemplateTests\bak\dropNewDbs.cshtml");
                Assert.IsNotNull(sql);
                System.IO.File.WriteAllText(System.IO.Path.Combine(@"C:\junk\migration\deletes", string.Format(@"{0}.sql", o.name)), sql);
            }

            var sqlcmd = ParseTemplates(data.Select(x => x.name as string).Distinct(), @"C:\Users\jkosh\Documents\GitHub\IODataBlock\IODataBlock\BasicTests\TemplateTests\bak\sqlcmd_dropNewDbs.cshtml");
            System.IO.File.WriteAllText(System.IO.Path.Combine(@"C:\junk\migration\deletes", @"dropNewDbs.sql"), sqlcmd);
        }

        private IEnumerable<dynamic> GetModels()
        {
            #region sql

            var sql = @"
SELECT a.[name]
,b.[name] as [dataFileName]
,c.[name] as [logFileName]
FROM sys.databases as a
JOIN sys.master_files as b
ON a.[database_id] = b.[database_id]
JOIN sys.master_files as c
ON a.[database_id] = c.[database_id]
WHERE a.[state] = 0
AND b.[type] = 0
AND c.[type] = 1
AND a.[name] NOT IN('master', 'model', 'msdb', 'tempdb','dbadb01','LiteSpeedLocal','db_manager','db_manager_CLR')
AND a.[name] IN(
	'Applications'
	,'BulkData'
	,'CDRRECORD'
	,'Costar'
	,'CovadServices'
	,'CustomerDB'
	,'db_manager'
	,'db_manager_CLR'
	,'db01'
	,'dbadb01'
	,'DIDLegacyState'
	,'DirectoryListingOrders'
	,'E911'
	,'E911_New'
	,'EventLog'
	,'HeatTemp'
	,'LCA'
	,'Level3B2B'
	,'LNP'
	,'PaeTecLNP'
	,'ReportManager'
	,'SonusToolsLog'
	,'Syslog'
	,'Transactions'
	,'VendorLCR'
)
ORDER BY 1,2
";

            #endregion sql

            return Database.Query(SqlServerConnectionString, "System.Data.SqlClient", sql, 120);
        }

        private IEnumerable<dynamic> GetNewServerModels()
        {
            #region sql

            var sql = @"
SELECT a.[name]
,b.[name] as [dataFileName]
,c.[name] as [logFileName]
FROM sys.databases as a
JOIN sys.master_files as b
ON a.[database_id] = b.[database_id]
JOIN sys.master_files as c
ON a.[database_id] = c.[database_id]
WHERE a.[state] = 0
AND b.[type] = 0
AND c.[type] = 1
AND a.[name] NOT IN('master', 'model', 'msdb', 'tempdb','OMEssentials', 'OnvoyDB','dbadb01','LiteSpeedLocal','db_manager','db_manager_CLR')

ORDER BY 1,2
";

            #endregion sql

            return Database.Query(NewSqlServerConnectionString, "System.Data.SqlClient", sql, 120);
        }

        private string ParseTemplate(string name, string dataFileName, string logFileName, string razorfile)
        {
            dynamic model = new ExpandoObject();
            model.name = name;
            model.dataFileName = dataFileName;
            model.logFileName = logFileName;

            var parser = new TemplateParser();
            var templateString = System.IO.File.ReadAllText(razorfile);

            return parser.RenderTemplate(model, typeof(ExpandoObject), templateString);
        }

        private string ParseTemplates(IEnumerable<string> items, string filePath)
        {
            dynamic model = new ExpandoObject();
            model.items = items;

            var parser = new TemplateParser();
            var templateString = System.IO.File.ReadAllText(filePath);

            return parser.RenderTemplate(model, typeof(ExpandoObject), templateString);
        }
    }
}