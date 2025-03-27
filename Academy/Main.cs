//#define HOMEWORK
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

			dgvStudents.DataSource = connector.Select
				(
					"last_name,first_name,middle_name,birth_date,group_name,direction_name",
					"Students,Groups,Directions",
					"[group]=group_id AND direction=direction_id"
				);
			toolStripStatusLabelCount.Text = $"количество студентов:{dgvStudents.RowCount - 1}.";

#if HOMEWORK
			dgvGroups.DataSource = connector.Select("*", "Groups");
			dgvDirections.DataSource = connector.Select("*", "Directions");
			dgvDisciplines.DataSource = connector.Select("*", "Disciplines");
			dgvTeachers.DataSource = connector.Select("*", "Teachers"); 
#endif
		}

		private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch (tabControl.SelectedIndex)
			{
				case 0:
					dgvStudents.DataSource =
						connector.Select
						(
							"last_name,first_name,middle_name,birth_date,group_name,direction_name",
							"Students,Groups,Directions",
							"[group]=group_id AND direction=direction_id"
						);
					toolStripStatusLabelCount.Text = $"количество студентов:{dgvStudents.RowCount - 1}.";
					break;
				case 1:
					dgvGroups.DataSource = connector.Select
						(
							"group_name,dbo.GetLearningDaysFor(group_name) AS weekdays,start_time,direction_name", 
							"Groups,Directions",
							"direction=direction_id"
						);
					toolStripStatusLabelCount.Text = $"количество групп:{dgvGroups.RowCount - 1}.";
					break;
				case 2:
					dgvDirections.DataSource = connector.Select("*", "Directions");
					toolStripStatusLabelCount.Text = $"количество направлений:{dgvDirections.RowCount - 1}.";
					break;
				case 3:
					dgvDisciplines.DataSource = connector.Select("*", "Disciplines");
					toolStripStatusLabelCount.Text = $"количество дисциплин:{dgvDisciplines.RowCount - 1}.";
					break;
				case 4:
					dgvTeachers.DataSource = connector.Select("*", "Teachers");
					toolStripStatusLabelCount.Text = $"количество преподавателей:{dgvTeachers.RowCount - 1}.";
					break;
			}
		}
	}
}
