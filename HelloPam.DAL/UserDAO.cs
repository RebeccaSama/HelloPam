﻿using HelloPam.BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace HelloPam.DAL
{
    public class UserDAO
    {
        private readonly Sql sql;
        public UserDAO()
        {
            sql = new Sql("HelloPam");
        }
        public void Add(User user)
        {
            sql.Execute
                (
                    "Sp_User_Insert",
                    GetParameters(user),
                    true
                );
        }
        public void Set(User user)
        {
            sql.Execute
                (
                    "Sp_User_Update",
                    GetParameters(user),
                    true
                );
        }
        public User Get(int id)
        {
            var reader = sql.Read
                (
                    "Sp_User_Update",
                    GetParameters(new User { Id = id }),
                    true
                );
            while (reader.Read())
                return GetObject(reader);
            reader.Close();

            return null;
        }
        public User Login(string username, string password)
        {
            var reader = sql.Read
                (
                    "Sp_User_Login",
                    GetParameters(new User { Username = username, Password = password }),
                    true
                );
            while (reader.Read())
                return GetObject(reader);
            reader.Close();

            return null;
        }
        public IEnumerable<User> Find(User user)
        {
            var reader = sql.Read
                (
                    "Sp_User_Update",
                    GetParameters(user),
                    true
                );
            var users = new List<User>();
            while (reader.Read())
                users.Add (GetObject(reader));
            reader.Close();

            return users;
        }
        public void Delete(int id)
        {
            sql.Execute
                (
                    "Sp_User_Delete",
                    GetParameters(new User { Id = id}),
                    true
                );
        }
        public void Fin(int id)
        {
            sql.Execute
                (
                    "Sp_User_Delete",
                    GetParameters(new User { Id = id }),
                    true
                );
        }
        private User GetObject(DbDataReader reader)
        {
            return new User
                (
                    reader["Id"] == null ? 0 : int.Parse(reader["Id"].ToString()),
                    reader["Username"] == null ? null : reader["Username"].ToString(),
                    reader["Password"] == null ? null : reader["Password"].ToString(),
                    reader["Fullname"] == null ? null : reader["Fullname"].ToString(),
                    reader["Profil"] == null ? User.ProfileOptions.Visitor : (User.ProfileOptions)int.Parse(reader["Profile"].ToString()),
                    reader["Status"] == null ? false : bool.Parse(reader["Status"].ToString()),
                    reader["Picture"] == null ? null : (byte[])reader["Picture"],
                    reader["CreatedAt"] == null ? null : (DateTime?)DateTime.Parse(reader["CreatedAt"].ToString())

                );
        }
        private IEnumerable <Sql.Parameter> GetParameters(User user)
        {
            return new Sql.Parameter[]
            {
                new Sql.Parameter("@Id", DbType.Int32, (user.Id == 0 ? (Object)DBNull.Value : user.Id)),
                new Sql.Parameter("@Username", DbType.String, (String.IsNullOrEmpty (user.Username) ? (object) DBNull.Value : user.Id)),
                new Sql.Parameter("@Password", DbType.String, (String.IsNullOrEmpty (user.Password) ? (object)DBNull.Value : user.Id)),
                new Sql.Parameter("@Fullname", DbType.String, (String.IsNullOrEmpty (user.Fullname) ? (object)DBNull.Value : user.Id)),
                new Sql.Parameter("@Profil", DbType.Int32, (user.Profile == null ? (Object)DBNull.Value : (int)user.Profile)),
                new Sql.Parameter("@Statut", DbType.Boolean, (user.Status == null ? (Object)DBNull.Value : user.Status)),
                new Sql.Parameter("@Picture", DbType.Binary, (user.Picture == null ? (Object)DBNull.Value : user.Picture)),
                new Sql.Parameter("@CreatedAt", DbType.DateTime, (user.CreatedAt == null ? (Object)DBNull.Value : user.CreatedAt)),

            };
        }
    }
}
