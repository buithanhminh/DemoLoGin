using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoLoginForm1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Lấy giá trị nhập từ người dùng
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            // Tạo chuoi cho việc mã hóa bcrypt
            string salt = BCrypt.Net.BCrypt.GenerateSalt();

            // Mã hóa mật khẩu sử dụng bcrypt
            string hash = BCrypt.Net.BCrypt.HashPassword(password, salt);

            // Kết nối tới cơ sở dữ liệu
            string connectionString = "Server=LAPTOP-FIJM5Q5I\\MINHTHANH;Database=demologin;Trusted_Connection=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Mở kết nối tới cơ sở dữ liệu
                connection.Open();

                // Thêm người dùng mới vào cơ sở dữ liệu
                SqlCommand command = new SqlCommand("INSERT INTO users (username, password) VALUES (@username, @password)", connection);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", hash);           
                command.ExecuteNonQuery();

                // Đóng kết nối tới cơ sở dữ liệu
                connection.Close();
            }
        }
        private void btnCheckLogin_Click(object sender, EventArgs e)
        {
            // Lấy giá trị nhập từ người dùng 
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            // Kết nối tới cơ sở dữ liệu 
            string connectionString = "Server=LAPTOP-FIJM5Q5I\\MINHTHANH;Database=demologin;Trusted_Connection=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Mở kết nối tới cơ sở dữ liệu 
                connection.Open();

                // Kiểm tra người dùng có tồn tại trong cơ sở dữ liệu hay không
                SqlCommand command = new SqlCommand("SELECT hash FROM users WHERE username=@username", connection);
                command.Parameters.AddWithValue("@username", username);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // Lấy mật khẩu đã được mã hóa từ cơ sở dữ liệu 
                    string hashedPassword = reader.GetString(0);

                    // Xác minh mât khẩu nhập vào với mật khâu đã mã hóa sử dụng Bcrypt 
                    bool passwordMatch = BCrypt.Net.BCrypt.Verify(password, hashedPassword);

                    if (passwordMatch)
                    {
                        // Mật khẩu khớp, người dùng được xác thực
                        MessageBox.Show("Login successful!");
                        var frm = new FrmMain(username);
                        this.Hide();
                    }
                    else
                    {
                        // Mật khẩu không khớp, người dùng không được xác thực
                        MessageBox.Show("mat khau khong chinh xac");
                    }
                }
                else
                {
                    //Người dùng không được tìm thấy trong cơ sở dữ liệu
                    MessageBox.Show("nguoi dung ko ton tai.");
                }

                // Đóng kết nối tới cơ sở dữ liệu và đóng đối tượng reader
                reader.Close();
                connection.Close();
            }

        }    
    }   
}
