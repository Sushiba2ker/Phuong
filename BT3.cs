using NguyenHoangPhuong_2180601200_BaiTapTuan6.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NguyenHoangPhuong_2180601200_BaiTapTuan6
{
    public partial class BT3 : Form
    {
        private object cbxkhoa;
        private object cmbKhoa;
        private object previousForm;

        public BT3()
        {
            InitializeComponent();
        }

        public void fillFacultyToComboBox(List<Faculty> listFaculty)
        {
            this.cbxkhoa.DataSource = listFaculty;
            this.cbxkhoa.DisplayMember = "FacultyName";

            this.cbxkhoa.ValueMember = "FacultyID";
        }

        public void BindGrid(List<Student> listStudents, StudentDBContext context)
        {
            dgvDanhSachSV.Rows.Clear();
            foreach (var item in listStudents)
            {

                int index = dgvDanhSachSV.Rows.Add();
                dgvDanhSachSV.Rows[index].Cells[0].Value = item.StudentID;
                dgvDanhSachSV.Rows[index].Cells[1].Value = item.FullName;

                // Tìm thông tin về khoa từ cơ sở dữ liệu
                Faculty faculty = context.Faculties.Find(item.FacultyID);
                dgvDanhSachSV.Rows[index].Cells[2].Value = faculty?.FacultyName;
                cbxkhoa.SelectedIndex = -1;
                dgvDanhSachSV.Rows[index].Cells[3].Value = item.AverageScore;
            }
        }

        private void BT3_Load(object sender, EventArgs e)
        {
            try
            {
                StudentDBContext context = new StudentDBContext();
                List<Faculty> listFaculty = context.Faculties.ToList();
                List<Student> listStudent = context.Students.ToList();


                fillFacultyToComboBox(listFaculty);
                BindGrid(listStudent, context);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btntimkiem_Click(object sender, EventArgs e)
        {
            try
            {
                StudentDBContext context = new StudentDBContext();
                int selectedFacultyId = Convert.ToInt32(cmbKhoa.SelectedValue);

                // Lấy giá trị MSSV, Họ Tên và Khoa từ các textbox
                string selectedMSSV = txtMSSV.Text.ToUpper();
                string selectedTen = txthoten.Text.ToLower();

                // Thực hiện tìm kiếm sinh viên theo các điều kiện
                List<Models.Student> students = context.Students.Where(s => (string.IsNullOrEmpty(selectedMSSV) || s.StudentID.ToUpper().Contains(selectedMSSV)) && (string.IsNullOrEmpty(selectedTen) || s.FullName.ToLower().Contains(selectedTen)) && (selectedFacultyId == 0 || s.FacultyID == selectedFacultyId)).ToList();
                List<Student> listStudents = students;

                // Cập nhật DataGridView với danh sách sinh viên tìm thấy
                BindGrid(listStudents, context);
                SetKetQuaTimKiem(listStudents.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            try
            {
                StudentDBContext context = new StudentDBContext();
                List<Student> listStudents = context.Students.ToList();

                // Cập nhật DataGridView với danh sách sinh viên ban đầu
                BindGrid(listStudents, context);
                SetKetQuaTimKiem(listStudents.Count);
                ClearInputFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ClearInputFields()
        {
            txtMSSV.Text = string.Empty;
        }

        private void BindGrid1(List<Student> listStudents, StudentDBContext context)
        {
            dgvDanhSachSV.Rows.Clear();
            foreach (var item in listStudents)
            {

                int index = dgvDanhSachSV.Rows.Add();
                dgvDanhSachSV.Rows[index].Cells[0].Value = item.StudentID;
                dgvDanhSachSV.Rows[index].Cells[1].Value = item.FullName;

                // Tìm thông tin về khoa từ cơ sở dữ liệu
                Faculty faculty = context.Faculties.Find(item.FacultyID);
                dgvDanhSachSV.Rows[index].Cells[2].Value = faculty?.FacultyName;
                cmbKhoa.SelectedIndex = -1;
                dgvDanhSachSV.Rows[index].Cells[3].Value = item.AverageScore;
            }
        }

        private void fillFacultyToComboBox1(List<Faculty> listFaculty)
        {
            this.cmbKhoa.DataSource = listFaculty;
            this.cmbKhoa.DisplayMember = "FacultyName";

            this.cmbKhoa.ValueMember = "FacultyID";
        }


        private void ClearInputFields()
        {
            txtMSSV.Text = string.Empty;
            txthoten.Text = string.Empty;
            cbxkhoa.SelectedIndex = -1;
        }


        public void SetPreviousForm(Form form)
        {
            previousForm = form;
        }

        private void btntrove_Click(object sender, EventArgs e)
        {
            if (previousForm != null)
            {
                previousForm.Show();
                this.Close();
            }
        }


        public void SetKetQuaTimKiem(int ketQua)
        {
            txtketquatimkiem.Text = ketQua.ToString();
        }



    }
}
