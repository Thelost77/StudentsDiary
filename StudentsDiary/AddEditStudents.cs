using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace StudentsDiary
{
    public partial class AddEditStudents : Form
    {
        private int _studentId;
        private FileHelper<List<Student>> fileHelper = new FileHelper<List<Student>>(Program.FilePath);
        private Student _student = new Student();
        public AddEditStudents(int id = 0)
        {
            InitializeComponent();
            _studentId = id;
            var students = fileHelper.DeserializeFromFile();
            _student = students.FirstOrDefault(x => x.Id == _studentId);
            tbName.Select();
            GetStudentData();
        }

        private void GetStudentData()
        {
            if (_studentId != 0)
            {
                Text = "Edytowanie danych ucznia";

                var students = fileHelper.DeserializeFromFile();
                _student = students.FirstOrDefault(x => x.Id == _studentId);

                if (_student == null)
                    throw new Exception("Nie ma takiego ucznia");

                FillTextBoxes();
            }
                        
        }
        private void FillTextBoxes()
        {
            tbId.Text = _student.Id.ToString();
            cbGroupId.SelectedItem = _student.GroupId;
            tbName.Text = _student.FirstName;
            tbLastName.Text = _student.LastName;
            tbMath.Text = _student.Math;
            tbPhys.Text = _student.Physics;
            tbPolish.Text = _student.PolishLang;
            tbForeignLang.Text = _student.ForeignLang;
            tbBio.Text = _student.Bio;
            cbExtraCurr.Checked = _student.IfExtracurricularActivities;
            rtbComments.Text = _student.Comments;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            var students = fileHelper.DeserializeFromFile();

            if (_studentId != 0)
                students.RemoveAll(x => x.Id == _studentId);            
            else  
                AssignIdToNewStudent(students);

            AddNewStudentToList(students);
            fileHelper.SerializeToFile(students);
            Close();
        }

        private void AddNewStudentToList(List<Student> students)
        {
            var student = new Student
            {
                Id = _studentId,
                GroupId = cbGroupId.SelectedItem.ToString(),
                FirstName = tbName.Text,
                LastName = tbLastName.Text,
                Math = tbMath.Text,
                Bio = tbBio.Text,
                Physics = tbPhys.Text,
                PolishLang = tbPolish.Text,
                ForeignLang = tbForeignLang.Text,
                Comments = rtbComments.Text,
                IfExtracurricularActivities = cbExtraCurr.Checked,

            };
            students.Add(student);
        }

        private void AssignIdToNewStudent(List<Student> students)
        {
                var studentWithHighestId = students.OrderByDescending(x => x.Id).FirstOrDefault();
                _studentId = studentWithHighestId == null ? 1 : studentWithHighestId.Id + 1;
        }
    }
} 
