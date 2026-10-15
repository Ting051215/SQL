using System.Data.SqlClient;
using CourseCompass.Models;

namespace CourseCompass.Services
{
    public class UserService : BaseService
    {
        public User Login(string account, string password)
        {
            User user = null;

            using (SqlConnection conn =
                new SqlConnection(connStr))
            {
                conn.Open();

                string sql =
                @"SELECT *
                  FROM [User]
                  WHERE account=@account
                  AND password=@password";

                SqlCommand cmd =
                    new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@account", account);
                cmd.Parameters.AddWithValue("@password", password);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    user = new User()
                    {
                        studentID = dr["studentID"].ToString(),
                        grade = (int)dr["grade"],
                        account = dr["account"].ToString(),
                        password = dr["password"].ToString(),
                        department = dr["department"].ToString()
                    };
                }
            }

            return user;
        }

        public bool AccountExists(string account)
        {
            using (SqlConnection conn =
                new SqlConnection(connStr))
            {
                conn.Open();

                string sql =
                    @"SELECT COUNT(*)
                      FROM [User]
                      WHERE account=@account";

                SqlCommand cmd =
                    new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@account", account);

                int count = (int)cmd.ExecuteScalar();

                return count > 0;
            }
        }

        public void Register(User user)
        {
            using (SqlConnection conn =
                new SqlConnection(connStr))
            {
                conn.Open();

                string sql =
                @"INSERT INTO [User]
                VALUES
                (
                    @studentID,
                    @grade,
                    @account,
                    @password,
                    @department
                )";

                SqlCommand cmd =
                    new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@studentID", user.studentID);
                cmd.Parameters.AddWithValue("@grade", user.grade);
                cmd.Parameters.AddWithValue("@account", user.account);
                cmd.Parameters.AddWithValue("@password", user.password);
                cmd.Parameters.AddWithValue("@department", user.department);

                cmd.ExecuteNonQuery();
            }
        }
    }
}