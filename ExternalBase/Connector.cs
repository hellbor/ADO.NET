using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Configuration;

namespace ExternalBase
{
	static class Connector
	{
		static readonly int PADDING = 16;
		static readonly string CONNECTION_STRING = ConfigurationManager.ConnectionStrings["PV_319_Import"].ConnectionString;
		static SqlConnection connection;
		static Connector()
		{
            Console.WriteLine(CONNECTION_STRING);
			connection = new SqlConnection(CONNECTION_STRING);
        }
		public static void Select(string fields, string tables, string condition = "")
		{
			string cmd = $"SELECT {fields} FROM {tables}";
			if (condition != "") cmd += $" WHERE {condition}";
			SqlCommand command = new SqlCommand(cmd, connection);
			connection.Open();

			SqlDataReader reader = command.ExecuteReader();

			if(reader.HasRows)
			{
				for (int i = 0; i < reader.FieldCount; i++)
				{
					Console.Write(reader.GetName(i).PadRight(PADDING));
				}
			}
            Console.WriteLine();
            while (reader.Read())
			{
				for (int i = 0; i < reader.FieldCount; i++)
				{
                    Console.Write(reader[i].ToString().PadRight(PADDING));
                }
                Console.WriteLine();
            }
			reader.Close();
			connection.Close();
		}
		public static int DisciplineID(string discipline_name)
		{
			int discipline_id = 0;
			try
			{
				string cmd = $"SELECT discipline_id FROM Disciplines WHERE discipline_name LIKE N'%{discipline_name}%'";
				connection.Open();
				SqlCommand command = new SqlCommand(cmd, connection);
				SqlDataReader reader = command.ExecuteReader();
				if (reader.Read())
				{
					discipline_id = (Int16)reader["discipline_id"];
				}
				connection.Close();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return discipline_id;
		}
		public static int TeacherID(string last_name)
		{
			int teacher_id = 0;
			try
			{
				string cmd = $"SELECT teacher_id FROM Teachers WHERE last_name LIKE N'%{last_name}%'";
				connection.Open();

				SqlCommand command = new SqlCommand(cmd, connection);
				SqlDataReader reader = command.ExecuteReader();
				if (reader.Read())
				{
					teacher_id = (Int16)reader["teacher_id"];
				}

				connection.Close();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return teacher_id;
		}
		public static int Count(string table) 
		{
			int count = 0;
			string cmd = $"SELECT COUNT(*) FROM {table}";
			SqlCommand command = new SqlCommand(cmd, connection);
			connection.Open();
			count = Convert.ToInt32(command.ExecuteScalar());
			connection.Close();
			return count;
		}
	}
}
