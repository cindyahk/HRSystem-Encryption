using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using DBSqlLib;

namespace HrSystem.MD5
{
    public class ProcessPassword
    {
        MD5Calculate md5Calculate = new MD5Calculate();
        string connectionName = "CnStr";
        public Tuple<string,string> ProcessPasswordToMD5(string passwordText,string username,bool save)//save=TRUE if resetting/creating password, save=FALSE if 
        {
            Hashtable ht = new Hashtable();
            byte[] passwordToHash = Encoding.ASCII.GetBytes(passwordText);//Encoding.ASCII.GetBytes(ClearText.Text);//load the password to be hashed
          
            String randomKeyString;
            int randKeyLength;
            
            if (save)
            {
                //calculate randomKeyString
                //Generate a random length for randomKey range between 3 and 6 characters
                Random randKey = new Random();
                randKeyLength = randKey.Next(3, 6);
                randomKeyString = MD5Prepare.gen_randomKeyString(randKeyLength);
            }
            else
            {
                ht.Add("@UserName",username);
                DataTable dt = DBSql.GetDataTable("select randomKey from Users where UserName=@UserName",ht,connectionName);
                randomKeyString = dt.Rows[0]["randomKey"].ToString().Trim();
                randKeyLength = randomKeyString.Length;
            }

            //calculate complex password
            String complexPassword = MD5Prepare.gen_ComplexPassword(passwordToHash, randomKeyString, randKeyLength);

            //Calculate the salt
            String salt = MD5Prepare.gen_Salt(randomKeyString, complexPassword);

            /***********************************************************
             * Calculate hash value
             * finalHashValue = hashValue0 ^ hashValue1 ^.......^hashValue1000
             * where hashValue0 = Hash(key,cpxpassword,salt)
             *       hashValue1 = Hash(hashValue0,cpxpassword,salt)
             *       hashValue1000 = Hash(hashValue999,cpxpassword,salt)
             ***********************************************************/
            StringBuilder valueToHash = new StringBuilder();
            StringBuilder hashValue = new StringBuilder();

            valueToHash.Append(complexPassword + salt);
            hashValue.Append(md5Calculate.GetMD5(Encoding.ASCII.GetBytes(valueToHash.ToString()), (uint)valueToHash.Length));

            String finalHashValue = hashValue.ToString();

            for (int i = 0; i < 1000; i++)
            {
                valueToHash.Remove(0, valueToHash.Length);
                valueToHash.Append(hashValue + complexPassword + salt);
                hashValue.Remove(0, hashValue.Length);
                hashValue.Append(md5Calculate.GetMD5(Encoding.UTF8.GetBytes(valueToHash.ToString()), (uint)valueToHash.Length));
                finalHashValue = MD5Prepare.gen_XORStrings(hashValue.ToString(), finalHashValue.ToString());
            }

           if (!save) randomKeyString = null;//if we are verifying login password, no need to return randomKeyString
           return new Tuple<string, string>(randomKeyString, finalHashValue);
            
        }
    }
}