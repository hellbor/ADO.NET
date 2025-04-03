//#define HOMEWORK
//#define SWITCH
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
	public partial class Main : Form
	{
		Connector connector;

		Dictionary<string, int> d_directions;
		Dictionary<string, int> d_groups;
		DataGridView[] tables;
		Query[] queries = new Query[]
		{
			new Query
			(
				"last_name,first_name,middle_name,birth_date,group_name,direction_name",
				"Students JOIN Groups ON ([group]=group_id) JOIN Directions ON (direction=direction_id)"
				//"[group]=group_id AND direction=direction_id"
			),
			new Query
			(
				"group_name,dbo.GetLearningDaysFor(group_name) AS weekdays,start_time,direction_name",
				"Groups,Directions",
				"direction=direction_id"
			),
			new Query
			(
				"direction_name,COUNT(DISTINCT group_id) AS N'Количество групп', COUNT(stud_id) AS 'Количество студентов'",
				"Students RIGHT JOIN Groups ON([group]=group_id) RIGHT JOIN Directions ON(direction=direction_id)",
				"",
				"direction_name"
			),
			new Query("*", "Disciplines"),
			new Query("*", "Teachers")
		};

		string[] status_messagess = new string[]
		{
			$"Количество студентов: ",
			$"Количество групп: ",
			$"Количество направлений: ",
			$"Количество дисциплин: ",
			$"Количество преподавателей: ",
		};
		public Main()
		{
			InitializeComponent();

			tables = new DataGridView[]
			{
				dgvStudents,
				dgvGroups,
				dgvDirections,
				dgvDisciplines,
				dgvTeachers
			};

			//cbGroups.MouseClick += cbGroups_MouseClick;
			//cbGroups.SelectedIndexChanged += cbGroups_SelectedIndexChanged;

			connector = new Connector
				(
					ConfigurationManager.ConnectionStrings["PV_319_Import"].ConnectionString
				);
			d_directions = connector.GetDictionary("*", "Directions"); //d_ - Dictionary
			d_groups = connector.GetDictionary("group_id,group_name", "Groups");
			d_directions["Все направления"] = 0;
			d_groups["Все группы"] = 0;
			cbStudentsGroup.Items.AddRange(d_groups.Select(g => g.Key).ToArray());
			cbGroupsDirection.Items.AddRange(d_directions.Select(d => d.Key).ToArray()); //KeyValuePair
			cbStudentsDirection.Items.AddRange(d_directions.Select(d => d.Key).ToArray());
			cbStudentsGroup.Items.Insert(0, "Все группы");
			cbStudentsDirection.Items.Insert(0, "Все направления");
			cbStudentsGroup.SelectedIndex = cbStudentsDirection.SelectedIndex = 0;
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
		void LoadPage(int i, Query query = null)
		{
			if(query==null)query = queries[i];
			tables[i].DataSource = connector.Select(query.Columns, query.Tables, query.Condition, query.Group_by);
			toolStripStatusLabelCount.Text = status_messagess[i] + CountRecordsInDGV(tables[i]);
		}
		private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			//int i = tabControl.SelectedIndex;
			LoadPage(tabControl.SelectedIndex);

#if SWITCH
			switch (tabControl.SelectedIndex)
			{
				case 0:
					dgvStudents.DataSource = connector.Select
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
#endif
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

		private void cbStudentsDirection_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i = cbStudentsDirection.SelectedIndex;
			Dictionary<string, int> d_groups = connector.GetDictionary
				(
				"group_id,group_name",
				"Groups",
				i == 0 ? "" : $"direction={d_directions[cbStudentsDirection.SelectedItem.ToString()]}"
				);
			cbStudentsGroup.Items.Clear();
			cbStudentsGroup.Items.AddRange(d_groups.Select(g => g.Key).ToArray());

			//int t = tabControl.SelectedIndex;
			//dgvStudents.DataSource = 
			//	connector.Select(
			//		queries[0].Columns, 
			//		queries[0].Tables,
			//		i == 0 || cbStudentsDirection.SelectedItem == null ? "" : $"direction={ d_directions[cbGroupsDirection.SelectedItem.ToString()]}"
			//		);
			Query query = new Query(queries[0]);
			query.Condition = 
				(i == 0 || cbStudentsDirection.SelectedItem == null ? "" : $"direction={d_directions[cbGroupsDirection.SelectedItem.ToString()]}");
			LoadPage(0, query);
		}
	}
}
