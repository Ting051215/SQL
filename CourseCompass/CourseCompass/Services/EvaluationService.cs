using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using CourseCompass.Models;

namespace CourseCompass.Services
{
    // 用來裝四項平均分數的小幫手類別
    public class CourseAverages
    {
        public double Recommend { get; set; }
        public double Sweetness { get; set; }
        public double Difficult { get; set; }
        public double Workload { get; set; }
    }

    public class EvaluationService : BaseService
    {
        // 1. 取得特定課程的評論清單
        public List<Evaluation> GetEvaluationsByCourse(string courseID)
        {
            List<Evaluation> list = new List<Evaluation>();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sql = @"SELECT e.* FROM Evaluation e
                               INNER JOIN Commend c ON e.evalID = c.evalID
                               WHERE c.courseID = @courseID";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@courseID", courseID);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new Evaluation()
                    {
                        evalID = (int)dr["evalID"],
                        recommendations = (int)dr["recommendations"],
                        sweetness = (int)dr["sweetness"],
                        difficult = (int)dr["difficult"],
                        workload = (int)dr["workload"],
                        comment = dr["comment"].ToString()
                    });
                }
            }
            return list;
        }

        // 2. 一次計算特定課程的「四項」平均分數
        public CourseAverages GetCourseAverages(string courseID)
        {
            CourseAverages avgs = new CourseAverages();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sql = @"SELECT 
                                AVG(CAST(e.recommendations AS FLOAT)) AS AvgRec,
                                AVG(CAST(e.sweetness AS FLOAT)) AS AvgSweet,
                                AVG(CAST(e.difficult AS FLOAT)) AS AvgDiff,
                                AVG(CAST(e.workload AS FLOAT)) AS AvgWork
                               FROM Evaluation e
                               INNER JOIN Commend c ON e.evalID = c.evalID
                               WHERE c.courseID = @courseID";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@courseID", courseID);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        // 檢查如果沒有人評論，就預設給 0 分
                        avgs.Recommend = dr["AvgRec"] != DBNull.Value ? Convert.ToDouble(dr["AvgRec"]) : 0;
                        avgs.Sweetness = dr["AvgSweet"] != DBNull.Value ? Convert.ToDouble(dr["AvgSweet"]) : 0;
                        avgs.Difficult = dr["AvgDiff"] != DBNull.Value ? Convert.ToDouble(dr["AvgDiff"]) : 0;
                        avgs.Workload = dr["AvgWork"] != DBNull.Value ? Convert.ToDouble(dr["AvgWork"]) : 0;
                    }
                }
            }
            return avgs;
        }

        // 3. 新增評價 (不變)
        public void AddEvaluation(Evaluation eva, string courseID, string studentID)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sql = @"
                    DECLARE @newID INT;
                    SELECT @newID = ISNULL(MAX(evalID), 0) + 1 FROM Evaluation;

                    INSERT INTO Evaluation (evalID, recommendations, sweetness, difficult, workload, comment) 
                    VALUES (@newID, @recommendations, @sweetness, @difficult, @workload, @comment);

                    INSERT INTO Commend (studentID, courseID, evalID)
                    VALUES (@studentID, @courseID, @newID);";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@recommendations", eva.recommendations);
                cmd.Parameters.AddWithValue("@sweetness", eva.sweetness);
                cmd.Parameters.AddWithValue("@difficult", eva.difficult);
                cmd.Parameters.AddWithValue("@workload", eva.workload);
                cmd.Parameters.AddWithValue("@comment", eva.comment ?? "");
                cmd.Parameters.AddWithValue("@courseID", courseID);
                cmd.Parameters.AddWithValue("@studentID", studentID);

                cmd.ExecuteNonQuery();
            }
        }
    }
}