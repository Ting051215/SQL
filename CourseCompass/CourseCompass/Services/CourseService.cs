using System.Collections.Generic;
using System.Data.SqlClient;
using CourseCompass.Models;

namespace CourseCompass.Services
{
    public class CourseService : BaseService
    {
        // ==========================================
        // 1. 取得所有課程清單
        // ==========================================
        public List<Course> GetAllCourses()
        {
            List<Course> list = new List<Course>();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string sql = "SELECT * FROM Course";

                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new Course()
                    {
                        courseID = dr["courseID"].ToString(),
                        courseName = dr["courseName"].ToString(),
                        teacher = dr["teacher"].ToString(),
                        credits = (int)dr["credits"],
                        department = dr["department"].ToString()
                    });
                }
            }

            return list;
        }

        // ==========================================
        // 2. 取得單一課程詳細資料
        // ==========================================
        public Course GetCourse(string courseID)
        {
            Course course = null;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string sql = @"SELECT *
                               FROM Course
                               WHERE courseID=@courseID";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@courseID", courseID);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    course = new Course()
                    {
                        courseID = dr["courseID"].ToString(),
                        courseName = dr["courseName"].ToString(),
                        teacher = dr["teacher"].ToString(),
                        credits = (int)dr["credits"],
                        department = dr["department"].ToString()
                    };
                }
            }

            return course;
        }

        // ==========================================
        // 3. 搜尋課程 (新增的方法)
        // ==========================================
        // ==========================================
        // 3. 搜尋課程 (支援課名、老師、系所)
        // ==========================================
        public List<Course> Search(string keyword)
        {
            List<Course> list = new List<Course>();

            // 如果使用者沒有輸入任何關鍵字就按下搜尋，直接回傳全部課程
            if (string.IsNullOrEmpty(keyword))
            {
                return GetAllCourses();
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // 【關鍵修改】：加上了 OR department LIKE @keyword
                string sql = @"SELECT * FROM Course 
                               WHERE courseName LIKE @keyword 
                                  OR teacher LIKE @keyword 
                                  OR department LIKE @keyword";

                SqlCommand cmd = new SqlCommand(sql, conn);

                // 在前後加上 % 符號進行模糊比對
                cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new Course()
                    {
                        courseID = dr["courseID"].ToString(),
                        courseName = dr["courseName"].ToString(),
                        teacher = dr["teacher"].ToString(),
                        credits = (int)dr["credits"],
                        department = dr["department"].ToString()
                    });
                }
            }

            return list;
        }
    }
}