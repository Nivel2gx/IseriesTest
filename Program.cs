using System;
using IBM.Data.DB2.iSeries;
using System.Configuration;

namespace iseries
{
	public class Test
	{
		public static void Main(string[] args)
		{
			iDB2Connection con = null;
			try
			{
				Console.WriteLine("Open connection...");
				con = new iDB2Connection(ConfigurationManager.AppSettings["connectionString"]);
				con.Open();
				iDB2Transaction trn = con.BeginTransaction();

				string select = "SELECT PerRegS, PerPrv FROM VGPER WHERE PerPrv = ? and PerRegS = ? ORDER BY PerPrv, PerRegS FOR FETCH ONLY";

				iDB2Command cmd = new iDB2Command(select, con, trn);
				iDB2Parameter p1 = new iDB2Parameter("AV8PerPrv", iDB2DbType.iDB2Integer, 4);
				iDB2Parameter p2 = new iDB2Parameter("AV9PerRegS", iDB2DbType.iDB2Char, 1);
				cmd.Parameters.Add(p1);
				cmd.Parameters.Add(p2);
				Console.WriteLine("Execute select...");
				cmd.ExecuteReader();
				Console.WriteLine("Commit...");
				trn.Commit();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error:" + ex.Message);
				Console.WriteLine(ex.StackTrace);
			}
			finally
			{
				if (con != null)
					con.Close();
			}
		}
	}

}
