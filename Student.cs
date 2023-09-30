using System;

namespace NguyenHoangPhuong_2180601200_BaiTapTuan6
{
    public class Student
    {
        internal object FullName;
        internal object AverageScore;

        public object StudentID { get; internal set; }
        public object FacultyID { get; internal set; }

        public static implicit operator Student(Models.Student v)
        {
            throw new NotImplementedException();
        }
    }
}