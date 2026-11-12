using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Data;
using MySql.Data.MySqlClient;
using BCrypt.Net;
using System.Runtime.CompilerServices;

namespace Api_call
{
    public class AuthViewModel : INotifyPropertyChanged
    {
        // Tells the window that a value changed, so the bound TextBlock is refreshed
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public AuthViewModel()
        {
        }

        public bool Register()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Email))
            {
                ErrorMessage = "Veuillez remplir tous les champs.";
                return false;
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(Password);

            using (var conn = new MySqlConnection(Config.ConnectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("INSERT INTO users (Username, PasswordHash, Email) VALUES (@username, @passwordHash, @email)", conn);
                cmd.Parameters.AddWithValue("@username", Username);
                cmd.Parameters.AddWithValue("@passwordHash", passwordHash);
                cmd.Parameters.AddWithValue("@email", Email);

                try
                {
                    cmd.ExecuteNonQuery();
                    ErrorMessage = "Inscription réussie !";
                    return true;
                }
                catch (MySqlException ex)
                {
                    ErrorMessage = ex.Message;
                    return false;
                }
            }
        }

        public int Login()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Veuillez remplir tous les champs.";
                return -1;
            }

            using (var conn = new MySqlConnection(Config.ConnectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT Id, PasswordHash FROM users WHERE Username = @username", conn);
                cmd.Parameters.AddWithValue("@username", Username);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int userId = reader.GetInt32("Id");
                        string passwordHash = reader.GetString("PasswordHash");

                        if (BCrypt.Net.BCrypt.Verify(Password, passwordHash))
                        {
                            ErrorMessage = "Connexion réussie !";
                            return userId;
                        }
                        else
                        {
                            ErrorMessage = "Mot de passe incorrect.";
                            return -1;
                        }
                    }
                    else
                    {
                        ErrorMessage = "Utilisateur non trouvé.";
                        return -1;
                    }
                }
            }
        }

    }
    }
