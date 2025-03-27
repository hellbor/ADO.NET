using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Management;
using System.Configuration;

namespace Academy
{
	public partial class Main : Form
	{
		Connector connector;
		public Main()
		{
			InitializeComponent();

			connector = new Connector
				(
					ConfigurationManager.ConnectionStrings["PV_319_Import"].ConnectionString
				);

			//dgv - DataGridView
			dgvStudents.DataSource = connector.Select("*", "Students");
			dgvGroups.DataSource = connector.Select("*", "Groups");
			dgvDirections.DataSource = connector.Select("*", "Directions");
			dgvDisciplines.DataSource = connector.Select("*", "Disciplines");
			dgvTeachers.DataSource = connector.Select("*", "Teachers");
		}

		private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
		{

		}
	}
}
