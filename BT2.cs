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
    public partial class BT2 : Form
    {
        public BT2()
        {
            InitializeComponent();
        }

        private void BT2_Load(object sender, EventArgs e)
        {
            try
            {
                StudentDBContext context = new StudentDBContext();
                List<Faculty> danhSachKhoa = context.Faculties.ToList();
                BindGrid(danhSachKhoa);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void BindGrid(List<Faculty> danhSachKhoa)
        {
            dgvdanhsachkhoa.Rows.Clear();
            foreach (var khoa in danhSachKhoa)
            {
                int index = dgvdanhsachkhoa.Rows.Add();
                dgvdanhsachkhoa.Rows[index].Cells[0].Value = khoa.FacultyID;
                dgvdanhsachkhoa.Rows[index].Cells[1].Value = khoa.FacultyName;
                dgvdanhsachkhoa.Rows[index].Cells[2].Value = khoa.TotalProfessor;
            }
        }


        private void ResetFields()
        {
            txtmakhoa.Text = string.Empty;
            txttenkhoa.Text = string.Empty;
            txttongsogs.Text = string.Empty;
        }

        private void btnthemsua_Click(object sender, EventArgs e)
        {
            try
            {
                string maKhoa = txtmakhoa.Text;
                string tenKhoa = txttenkhoa.Text;
                string tongSoGS = txttongsogs.Text;

                int maKhoaInt;
                if (int.TryParse(maKhoa, out maKhoaInt))
                {
                    int? tongSoGSInt = null;
                    if (!string.IsNullOrEmpty(tongSoGS))
                    {
                        tongSoGSInt = int.Parse(tongSoGS);
                    }

                    StudentDBContext context = new StudentDBContext();
                    Faculty khoa = context.Faculties.Find(maKhoaInt);
                    if (khoa != null)
                    {
                        // Cập nhật khoa đã tồn tại
                        khoa.FacultyName = tenKhoa;
                        khoa.TotalProfessor = tongSoGSInt;
                    }
                    else
                    {
                        // Thêm khoa mới
                        khoa = new Faculty
                        {
                            FacultyID = maKhoaInt,
                            FacultyName = tenKhoa,
                            TotalProfessor = tongSoGSInt
                        };
                        context.Faculties.Add(khoa);
                    }

                    context.SaveChanges();

                    List<Faculty> danhSachKhoa = context.Faculties.ToList();
                    BindGrid(danhSachKhoa);
                    ResetFields();

                    MessageBox.Show("Lưu thành công", "Thông báo", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("Mã khoa không hợp lệ", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            try
            {
                string maKhoa = txtmakhoa.Text;

                int maKhoaInt;
                if (int.TryParse(maKhoa, out maKhoaInt))
                {
                    DialogResult dialogResult = MessageBox.Show("Bạn chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        StudentDBContext context = new StudentDBContext();
                        Faculty khoa = context.Faculties.Find(maKhoaInt);
                        if (khoa != null)
                        {
                            context.Faculties.Remove(khoa); 
                            context.SaveChanges();

                            List<Faculty> danhSachKhoa = context.Faculties.ToList();
                            BindGrid(danhSachKhoa);
                            ResetFields();

                            MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK);
                        }
                        else
                        {
                            MessageBox.Show("Khoa không tồn tại", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Mã khoa không hợp lệ", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btndong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvdanhsachkhoa_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvdanhsachkhoa.Rows[e.RowIndex];
                txtmakhoa.Text = row.Cells[0].Value.ToString();
                txttenkhoa.Text = row.Cells[1].Value.ToString();
                txttongsogs.Text = row.Cells[2].Value.ToString();
            }
        }
    }
}
