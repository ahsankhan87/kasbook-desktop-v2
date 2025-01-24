using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Configuration;
using System.Data.SqlClient;

namespace pos.Reports
{
    public class ReportConnectionManager
    {
        // Method to apply database connection info to a report
        // Method to apply the database connection info to the report
        public static void SetDatabaseLogon(ReportDocument reportDoc)
        {
            // Retrieve the connection string from App.config
            string connectionString = ConfigurationManager.ConnectionStrings["cn"].ConnectionString;

            // Parse the connection string
            var builder = new SqlConnectionStringBuilder(connectionString);

            // Create and set the ConnectionInfo object
            ConnectionInfo connectionInfo = new ConnectionInfo
            {
                ServerName = builder.DataSource,   // Server name (from the connection string)
                DatabaseName = builder.InitialCatalog, // Database name
                UserID = builder.UserID,           // User ID
                Password = builder.Password        // Password
            };

            // Apply connection info to all tables in the main report
            foreach (Table table in reportDoc.Database.Tables)
            {
                TableLogOnInfo tableLogOnInfo = table.LogOnInfo;
                tableLogOnInfo.ConnectionInfo = connectionInfo;
                table.ApplyLogOnInfo(tableLogOnInfo);
            }

            // Apply connection info to all tables in subreports, if any
            foreach (ReportDocument subreport in reportDoc.Subreports)
            {
                foreach (Table subTable in subreport.Database.Tables)
                {
                    TableLogOnInfo tableLogOnInfo = subTable.LogOnInfo;
                    tableLogOnInfo.ConnectionInfo = connectionInfo;
                    subTable.ApplyLogOnInfo(tableLogOnInfo);
                }
            }
        }
    }

}
