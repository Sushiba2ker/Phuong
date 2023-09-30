using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NguyenHoangPhuong_2180601200_BaiTapTuan6.Models;

namespace NguyenHoangPhuong_2180601200_BaiTapTuan6
{
    public partial class Form1 : Form
    {
        private StudentDBContext context;

        public Form1()
        {
            InitializeComponent();
            context = new StudentDBContext();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<Faculty> listFaculty = context.Faculties.ToList();
                List<Student> listStudent = context.Students.ToList();

                fillFacultyToComboBox(listFaculty);
                BindGrid(listStudent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void fillFacultyToComboBox(List<Faculty> listFaculty)
        {
            cbxkhoa.DataSource = listFaculty;
            cbxkhoa.DisplayMember = "FacultyName";
            cbxkhoa.ValueMember = "FacultyID";
        }

        public void BindGrid(List<Student> listStudents)
        {
            dlvDanhsachSV.Rows.Clear();
            foreach (var item in listStudents)
            {
                int index = dlvDanhsachSV.Rows.Add();
                dlvDanhsachSV.Rows[index].Cells[0].Value = item.StudentID;
                dlvDanhsachSV.Rows[index].Cells[1].Value = item.FullName;

                Faculty faculty = context.Faculties.Find(item.FacultyID);
                dlvDanhsachSV.Rows[index].Cells[2].Value = faculty?.FacultyName;
                dlvDanhsachSV.Rows[index].Cells[3].Value = item.AverageScore;
            }
        }

        private void resetNull()
        {
            txtMSSV.Text = string.Empty;
            txthoten.Text = string.Empty;
            cbxkhoa.SelectedIndex = -1;
            txtDTB.Text = string.Empty;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                string studentID = txtMSSV.Text;
                string fullName = txthoten.Text;
                int facultyID;
                if (cbxkhoa.SelectedValue != null && int.TryParse(cbxkhoa.SelectedValue.ToString(), out facultyID))
                {
                    float averageScore;
                    if (float.TryParse(txtDTB.Text, out averageScore))
                    {
                        Student newStudent = new Student
                        {
                            StudentID = studentID,
                            FullName = fullName,
                            FacultyID = facultyID,
                            AverageScore = averageScore
                        };

                        context.Students.Add(newStudent);
                        context.SaveChanges();

                        MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK);
                        LoadData();
                        resetNull();
                    }
                    else
                    {
                        MessageBox.Show("Điểm trung bình không hợp lệ", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn khoa", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string studentID = txtMSSV.Text;

                DialogResult dialogResult = MessageBox.Show("Bạn chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    Student existingStudent = context.Students.Find(studentID);
                    if (existingStudent != null)
                    {
                        context.Students.Remove(existingStudent);
                        context.SaveChanges();

                        MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK);
                        LoadData();
                        resetNull();
                    }
                    else
                    {
                        MessageBox.Show("Sinh viên không tồn tại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                string studentID = txtMSSV.Text;
                string fullName = txthoten.Text;
                int facultyID;
                if (cbxkhoa.SelectedValue != null && int.TryParse(cbxkhoa.SelectedValue.ToString(), out facultyID))
                {
                    float averageScore;
                    if (float.TryParse(txtDTB.Text, out averageScore))
                    {
                        Student existingStudent = context.Students.Find(studentID);
                        if (existingStudent != null)
                        {
                            existingStudent.FullName = fullName;
                            existingStudent.FacultyID = facultyID;
                            existingStudent.AverageScore = averageScore;
                            context.SaveChanges();

                            MessageBox.Show("Cập nhật thành công", "Thông báo", MessageBoxButtons.OK);
                            LoadData();
                            resetNull();
                        }
                        else
                        {
                            MessageBox.Show("Sinh viên không tồn tại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Điểm trung bình không hợp lệ", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn khoa", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dlvDanhsachSV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dlvDanhsachSV.Rows[e.RowIndex];
                txtMSSV.Text = row.Cells[0].Value.ToString();
                txthoten.Text = row.Cells[1].Value.ToString();
                cbxkhoa.SelectedValue = row.Cells[2].Value; // Assuming that cell 2 contains the FacultyID
                txtDTB.Text = row.Cells[3].Value.ToString();
            }
        }
    }
}
