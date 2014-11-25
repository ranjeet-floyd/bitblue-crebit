using MySql.Data.MySqlClient;
using School.src.db;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace School.src.model
{
    public class Validation
    {
        public int ValidateUser(string userName, string pwd)
        {
            int _Valid = 0;
            string qtext = "SELECT  pwd  FROM admin_sch where uname = @UserName ";
            MySqlParameter[] mySqlParameter = new MySqlParameter[1];
            mySqlParameter[0] = new MySqlParameter("@UserName", userName);
            DataSet ds = (new DataBase()).GetDataSet(qtext, mySqlParameter);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                string hashPwd = ds.Tables[0].Rows[0]["pwd"].ToString();
                if (!string.IsNullOrEmpty(hashPwd))
                {
                    if (VerifyMD5Hash(MD5.Create(), pwd, hashPwd))//valid
                        _Valid = 1;
                    else
                        _Valid = 2; //password not matched

                }
                else
                    _Valid = 3; //user not exist
            }

            return _Valid;

        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64DecodedData)
        {
            var base64DecodedBytes = System.Convert.FromBase64String(base64DecodedData);
            return System.Text.Encoding.UTF8.GetString(base64DecodedBytes);
        }

        private static string GetMD5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuildder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuildder.Append(data[i].ToString("x2"));
            }
            return sBuildder.ToString();
        }

        public static bool VerifyMD5Hash(MD5 md5, string input, string hash)
        {
            string hashInput = GetMD5Hash(md5, input);
            StringComparer comaprer = StringComparer.Ordinal;
            if (0 == comaprer.Compare(hashInput, hash))
                return true;
            return false;
        }
    }
}