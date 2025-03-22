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
		}
	}
}
