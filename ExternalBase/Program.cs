using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Configuration;
using System.ComponentModel.Design;

namespace ExternalBase
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Connector.Select("*", "Disciplines");
			Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine();
            Connector.Select("*", "Teachers");
			Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine();
            Connector.Select("*", "Students");
			Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine(Connector.DisciplineID("Системное программирование"));
            Console.WriteLine(Connector.TeacherID("Ковтун"));
			Console.WriteLine(Connector.Count("Students"));
		}
	}
}
