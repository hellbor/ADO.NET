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

using System.Configuration;


namespace Academy
{
	public partial class Group : Form
	{
		Connector connector;
		Dictionary<string, int> d_directions;
		public Group()
		{
			InitializeComponent();

			//cbGroups.MouseClick += cbGroups_MouseClick;
			//cbGroups.SelectedIndexChanged += cbGroups_SelectedIndexChanged;

			connector = new Connector
				(
					ConfigurationManager.ConnectionStrings["PV_319_Import"].ConnectionString
				);
			d_directions = connector.GetDictionary("*", "Directions"); //d_ - Dictionary
			cbGroupsDirection.Items.AddRange(d_directions.Select(k => k.Key).ToArray()); //KeyValuePair
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
					//dgvDirections.DataSource = connector.Select
					//	(
					//		"direction_name,COUNT(DISTINCT group_id) AS N'Количество групп', COUNT(stud_id) AS 'Количество студентов'",
					//		"Students,Directions,Groups",
					//		"[group]=group_id AND direction=direction_id",
					//		"direction_name"
					//	);
					dgvDirections.DataSource = connector.Select
						(
							"direction_name,COUNT(DISTINCT group_id) AS N'Количество групп', COUNT(stud_id) AS 'Количество студентов'",
							"Students RIGHT JOIN Groups ON([group]=group_id) RIGHT JOIN Directions ON(direction=direction_id)",
							"",
							"direction_name"
						);

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

		private void cbGroupsDirection_SelectedIndexChanged(object sender, EventArgs e)
		{
					dgvGroups.DataSource = connector.Select
					(
						"group_name,dbo.GetLearningDaysFor(group_name) AS weekdays,start_time,direction_name",
						"Groups,Directions",
						$"direction=direction_id AND direction = N'{d_directions[cbGroupsDirection.SelectedItem.ToString()]}'"
					);
			toolStripStatusLabelCount.Text = $"Количество групп: {CountRecordsInDGV(dgvGroups)}.";
		}
		int CountRecordsInDGV(DataGridView dgv)
		{
			return dgv.RowCount == 0 ? 0 : dgv.Rows.Count - 1;
		}
	}
}
