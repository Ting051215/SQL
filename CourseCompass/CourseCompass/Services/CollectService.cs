using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using CourseCompass.Models;

namespace CourseCompass.Services
{
    public class CollectService : BaseService
    {
        // 1. 查詢特定學生的所有收藏
        public List<Collect> GetCollect(string studentID)
        {
            List<Collect> list = new List<Collect>();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sql = @"SELECT * FROM Collect WHERE studentID=@studentID";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@studentID", studentID);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new Collect()
                    {
                        studentID = dr["studentID"].ToString(),
                        courseID = dr["courseID"].ToString(),
                        collectTime = (DateTime)dr["collectTime"],
                        note = dr["note"].ToString()
                    });
                }
            }
            return list;
        }

        // 💡 新增：取得「單一筆」收藏紀錄 (供修改備註與刪除畫面使用)
        public Collect GetSingleCollect(string studentID, string courseID)
        {
            Collect collect = null;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sql = @"SELECT * FROM Collect WHERE studentID=@studentID AND courseID=@courseID";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@studentID", studentID);
                cmd.Parameters.AddWithValue("@courseID", courseID);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    collect = new Collect()
                    {
                        studentID = dr["studentID"].ToString(),
                        courseID = dr["courseID"].ToString(),
                        collectTime = (DateTime)dr["collectTime"],
                        note = dr["note"].ToString()
                    };
                }
            }
            return collect;
        }

        // 2. 新增一筆收藏紀錄
        public void AddCollect(Collect collect)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sql = @"INSERT INTO Collect (studentID, courseID, collectTime, note) 
                               VALUES (@studentID, @courseID, @collectTime, @note)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@studentID", collect.studentID);
                cmd.Parameters.AddWithValue("@courseID", collect.courseID);
                cmd.Parameters.AddWithValue("@collectTime", collect.collectTime);
                cmd.Parameters.AddWithValue("@note", collect.note ?? "");
                cmd.ExecuteNonQuery();
            }
        }

        // 💡 新增：修改特定收藏的備註內容
        public void UpdateCollectNote(string studentID, string courseID, string note)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sql = @"UPDATE Collect SET note = @note 
                               WHERE studentID = @studentID AND courseID = @courseID";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@note", note ?? "");
                cmd.Parameters.AddWithValue("@studentID", studentID);
                cmd.Parameters.AddWithValue("@courseID", courseID);
                cmd.ExecuteNonQuery();
            }
        }

        // 3. 刪除特定的收藏紀錄
        public void DeleteCollect(string studentID, string courseID)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sql = @"DELETE FROM Collect WHERE studentID=@studentID AND courseID=@courseID";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@studentID", studentID);
                cmd.Parameters.AddWithValue("@courseID", courseID);
                cmd.ExecuteNonQuery();
            }
        }
    }
}