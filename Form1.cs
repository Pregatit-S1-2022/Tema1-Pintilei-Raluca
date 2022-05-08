using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tema1_PregatIT
{
    public partial class Form1 : Form
    {

        public void inregistrare()
        {
            string password = string.Empty;
            string username = string.Empty;

            if (txtPassword.Text == "" || txtUsername.Text == "")
            {
                MessageBox.Show("Username or Password fields are empty. Please enter username/password");
            }
            else
            {
                if (IsValidUsername(txtUsername.Text))
                {
                    MessageBox.Show("This is a valid username");
                    if (IsValidPassword(txtPassword.Text) == true)
                    {

                        string sirDeConectare;
                        sirDeConectare = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C://BD//Tema1.accdb;Persist Security Info=False;";
                        OleDbConnection conexiune;
                        conexiune = new OleDbConnection();
                        conexiune.ConnectionString = sirDeConectare;
                        conexiune.Open();
                        MessageBox.Show("The connection was made successfully!");

                        OleDbCommand comanda;
                        comanda = new OleDbCommand();
                        comanda.Connection = conexiune;
                        comanda.CommandText = "INSERT into tbl_Users values ('" + txtUsername.Text + "','" + txtPassword.Text + "') ";
                        comanda.ExecuteNonQuery();
                        MessageBox.Show("Account created successfully");
                        conexiune.Close();
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid username / password");
                    }
                }
                else
                {
                    MessageBox.Show("This is not a valid username");

                }
            }
        }
        public Form1()
        {
            InitializeComponent();
        }


      

        private void btnRegister_Click(object sender, EventArgs e)
        {

            inregistrare();
        }

        public int number_attempts = 0;
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            

            List<InformationBD> user = new List<InformationBD>();
            string sirDeConectare;
            sirDeConectare = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C://BD//Tema1.accdb;Persist Security Info=False;";
            OleDbConnection conexiune;
            conexiune = new OleDbConnection();
            conexiune.ConnectionString = sirDeConectare;
            conexiune.Open();
            MessageBox.Show("The connection was made successfully!");

            OleDbCommand comanda;
            comanda = new OleDbCommand();
            comanda.Connection = conexiune;
            comanda.CommandText = "SELECT * FROM tbl_Users WHERE Username = '" + txtUsername.Text + "' and Password = '" + txtPassword.Text + "'";
            OleDbDataReader cititor;
            cititor = comanda.ExecuteReader();

            while (cititor.Read())
            {
                user.Add(new InformationBD()
                {
                    UsernameBD = cititor["Username"].ToString(), //the name of the db column
                    PasswordBD = cititor["Password"].ToString(),

                
                }
              );
            }

            
                cititor.Close();
            conexiune.Close();

            foreach (InformationBD utilizator in user)
            {
             
                if (username.Equals(utilizator.UsernameBD))
                {
                    if (password.Equals(utilizator.PasswordBD))
                    {
                        MessageBox.Show("Welcome " + txtUsername.Text);
                        Console.WriteLine(utilizator.UsernameBD.Contains("pintilei.raluca@yahoo.com"));
                    }
                    else
                    {
                        number_attempts++;
                        MessageBox.Show("Incercarea: " +number_attempts);
                        if (number_attempts >= 3)
                        {
                            MessageBox.Show("User has been blocked");
                            Application.Exit();
                        }
                    }
                }
                else
                {
                    number_attempts++;
                    MessageBox.Show("Incercarea: " + number_attempts);
                    if (number_attempts >= 3)             
                    {
                        MessageBox.Show("User has been blocked");
                        Application.Exit();

                    }
                }
            }

        }

        public class InformationBD
        {
            public string UsernameBD { get; set; }
            public string PasswordBD { get; set; }
  
        }

        private void btnChPassword_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            List<InformationBD> user1 = new List<InformationBD>();
            string sirDeConectare;
            sirDeConectare = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C://BD//Tema1.accdb;Persist Security Info=False;";
            OleDbConnection conexiune;
            conexiune = new OleDbConnection();
            conexiune.ConnectionString = sirDeConectare;
            conexiune.Open();
            MessageBox.Show("The connection was made successfully!");

            OleDbCommand comanda;
            comanda = new OleDbCommand();
            comanda.Connection = conexiune;
            comanda.CommandText = "SELECT * FROM tbl_Users WHERE Username = '" + txtUsername.Text + "' and Password = '" + txtPassword.Text + "'";
            OleDbDataReader cititor;
            cititor = comanda.ExecuteReader();

            while (cititor.Read())
            {
                string name= cititor["Username"].ToString();
                string pw = cititor["Password"].ToString();
                user1.Add(new InformationBD()
                {
                    UsernameBD = name, //the name of the db column
                    PasswordBD = pw,
                  
                });
                MessageBox.Show(name);
            }
            cititor.Close();
            bool ok = false;
            foreach (InformationBD utilizator in user1)  
            {
                if (username.Equals(utilizator.UsernameBD))
                {
                    ok = true;
                    break;
                }
            }
            MessageBox.Show(ok +"");

            if (ok == false)
                inregistrare();
            else
            {
                OleDbCommand comandaa;
                comandaa = new OleDbCommand();
                comandaa.Connection = conexiune;
                comandaa.CommandText = "UPDATE tbl_Users SET  Password ='" + txtPassword.Text + "' WHERE Username='" + txtUsername.Text + "'";



                if (IsValidPassword(password) == true)
                {
                    MessageBox.Show("Your password was changed successfuly.");
                    comandaa.ExecuteNonQuery();
                }
                else
                {
                    MessageBox.Show("The password is invalid.");
                }
                conexiune.Close();
            }

        }



















        public static bool IsValidUsername(string username)
        {
            string validare = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            var regex = new Regex(validare, RegexOptions.IgnoreCase);
            return regex.IsMatch(username);
        }
        public static bool IsValidPassword(string password)
        {
            var containNumber = new Regex(@"[0-9]+");
            if (password.Length >= 10 && containNumber.IsMatch(password))
                return true;
            else
                return false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
} 
