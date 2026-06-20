using System;
using System.Text.RegularExpressions;
using Hospital_Management_System.Models;
using Hospital_Management_System.Repositories;
using Hospital_Management_System.Helpers;

namespace Hospital_Management_System.Services
{
    public class AuthService
    {
        private readonly UserRepository _userRepository = new UserRepository();

        public (bool ok, string msg, string role) Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return (false, "Please enter both Username and Password.", "");
            }

            try
            {
                var user = _userRepository.GetByUsername(username);
                if (user == null)
                {
                    return (false, "Invalid Username or Password.", "");
                }

                bool isValid = PasswordHelper.VerifyPassword(password, user.PasswordHash);
                if (!isValid)
                {
                    return (false, "Invalid Username or Password.", "");
                }

                return (true, $"Login Successful! Welcome {user.Username} ({user.Role}).", user.Role);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Login error for user: {username}");
                return (false, "An error occurred during login. Please try again.", "");
            }
        }

        public (bool ok, string msg) Register(User user, string email, string plainPassword, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(user.Username) || 
                string.IsNullOrWhiteSpace(email) || 
                string.IsNullOrWhiteSpace(plainPassword) || 
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                return (false, "All fields are required.");
            }

            if (plainPassword != confirmPassword)
            {
                return (false, "Password and Confirm Password must match.");
            }

            // Simple email validation regex
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                return (false, "Please enter a valid email address.");
            }

            try
            {
                if (_userRepository.UsernameExists(user.Username))
                {
                    return (false, "Username is already taken.");
                }

                if (_userRepository.EmailExists(email))
                {
                    return (false, "Email is already registered.");
                }

                user.PasswordHash = PasswordHelper.HashPassword(plainPassword);
                _userRepository.Create(user, email);
                return (true, "Account Created Successfully");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Signup error for user: {user.Username}");
                return (false, "An error occurred while creating the account.");
            }
        }
    }
}
