using StudentsDiary.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace StudentsDiary
{
    public partial class Main : Form
    {
        private FileHelper<List<Student>> fileHelper = new FileHelper<List<Student>>(Program.FilePath);
        private string _groupId;
        public bool IsMaximize 
        {
            get
            {
                return Settings.Default.IsMaximize;
            }
            set
            {
                Settings.Default.IsMaximize = value;
            }
        }

        public Main()
        {       
            InitializeComponent();
            cobSetGroupView.SelectedItem = "Wszyscy";
            RefreshDiary();
            SetColumnsHeaders();
            if (IsMaximize)            
                WindowState = FormWindowState.Maximized;
            

        }

        private void RefreshDiary()
        {            
            var students = fileHelper.DeserializeFromFile();
            _groupId = cobSetGroupView.SelectedItem.ToString();
            if (_groupId == "Wszyscy")
                dgvDiary.DataSource = students.OrderBy(x => x.Id).ToList();
            else
                dgvDiary.DataSource = students.Where(x => x.GroupId == _groupId).ToList();            
        }             
            
        
        private void SetColumnsHeaders()
        {
            dgvDiary.Columns[0].HeaderText = "Numer";
            dgvDiary.Columns[1].HeaderText = "Numer grupy";
            dgvDiary.Columns[2].HeaderText = "Imię";
            dgvDiary.Columns[3].HeaderText = "Nazwisko";
            dgvDiary.Columns[4].HeaderText = "Matematyka";
            dgvDiary.Columns[5].HeaderText = "Fizyka";
            dgvDiary.Columns[6].HeaderText = "Biologia";
            dgvDiary.Columns[7].HeaderText = "Język polski";
            dgvDiary.Columns[8].HeaderText = "Język obcy";
            dgvDiary.Columns[9].HeaderText = "Zajęcia dodatkowe";
            dgvDiary.Columns[10].HeaderText = "Komentarze";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

                var addEditStudent = new AddEditStudents();
                addEditStudent.FormClosing += AddEditStudent_FormClosing;
                addEditStudent.ShowDialog();       
                     
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę zaznacz ucznia, którego dane chcesz edytować");
                return;
            }

            var addEditStudent = new AddEditStudents(Convert.ToInt32(dgvDiary.SelectedRows[0].Cells[0].Value));
            addEditStudent.FormClosing += AddEditStudent_FormClosing;
            addEditStudent.ShowDialog();

        }

        private void AddEditStudent_FormClosing(object sender, FormClosingEventArgs e)
        {
            RefreshDiary();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę zaznacz ucznia, którego chcesz usunąć");
                return;
            }

            var selectedStudent = dgvDiary.SelectedRows[0];

            var confirmDelete =  MessageBox.Show($"Czy napewno chcesz usunąć ucznia {(selectedStudent.Cells[1].Value + " " + selectedStudent.Cells[2].Value).Trim()}", "Usuwanie ucznia", MessageBoxButtons.OKCancel);

            if (confirmDelete == DialogResult.OK)
            {
                DeleteStudent(Convert.ToInt32(selectedStudent.Cells[0].Value));
                RefreshDiary();
            }
        }

        private void DeleteStudent(int id)
        {
            var students = fileHelper.DeserializeFromFile();
            students.RemoveAll(x => x.Id == id);
            fileHelper.SerializeToFile(students);

        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDiary();
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
                IsMaximize = true;
            else
                IsMaximize = false;

            Settings.Default.Save();
        }

    }
}
