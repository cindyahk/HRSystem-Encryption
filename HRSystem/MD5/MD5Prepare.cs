using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HrSystem.MD5
{
    public class MD5Prepare
    {
        private static char[] alphanum = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();
       
        public MD5Prepare()
        {
                   
        }

        public static String gen_randomKeyString(int randKeyLength)
        {
            //generate a a random string of length randKeyLength
            String randomKeyString = gen_randomString(randKeyLength);

            return randomKeyString;
        }

        public static String gen_ComplexPassword(byte[] passwordToHash,String randomKeyString,int randKeyLength)
        {
            /*Generate the password matrix*/
            int lenPassword = passwordToHash.Length;//length of password
            int passRows;//number of rows for password matrix

            /*Calculate the number of rows and columns needed for password matrix, depending on randonKeyLength*/
            if (lenPassword % randKeyLength == 0)
                passRows = (lenPassword / randKeyLength);
            else passRows = (lenPassword / randKeyLength) + 1;
            int passColumns = randKeyLength;//number of columns for password matrix

            byte[,] password = new byte[passRows, passColumns];
            byte[] passwordTemp = new byte[passRows * passColumns];

            //Fill the 1D passwordTemp array with alternating * and @
            for (int i = 0; i < passRows * passColumns; i++)
            {
                passwordTemp[i] = (Byte)'@';
                if (i % 2 == 0) passwordTemp[i] = (Byte)'*';
            }

            //Fill the passwordTemp array with the password entered
            for (int i = 0; i < passwordToHash.Length; i++)
                passwordTemp[i] = passwordToHash[i];

            //Fill the passRowsXpassColumns password matrix
            for (int i = 0; i < passRows; i++)
            {
                for (int j = 0; j < passColumns; j++)
                {
                    password[i, j] = passwordTemp[i * passColumns + j]; //passRows X passColumns password matrix
                }
            }

            //Carry out columnar transposition
            String complexPassword = columnTranspose(password, randomKeyString, randKeyLength);

            return complexPassword;

        }

        public static String gen_randomString(int len)
        {
            String str = "";
            Random rand = new Random();
            for (int i = 0; i < len; i++)
                str += alphanum[rand.Next() % (alphanum.Length - 1)];
            return str;
        }

        public static String columnTranspose(byte[,] password, String randomKeyString, int randomKeyLength)
        {

            /*Find the order in which the columns are to be transposed*/
            int[] index = new int[randomKeyLength];
            int[] indexTemp = new int[randomKeyLength];
            int ind = 0;
            foreach (char c in randomKeyString)
                index[ind++] = Array.IndexOf(alphanum, c);//find the order of each character in the random string

            Array.Copy(index, 0, indexTemp, 0, randomKeyLength);
            Array.Sort(indexTemp);

            for (int i = 0; i < randomKeyLength; i++)
            {
                int tempIndex = Array.IndexOf(index, indexTemp[i]);//find the position, hence order of the element at indexTemp[i]
                index[tempIndex] = i; //index[] now stores the order of the columns in the password matrix, for example
                //if index[0] = 1, index[1]=2, index[2]=0, index[3]=3, then it means that the 
                // columnar transposition will occur in the following order: 
                // Third column+1st column+2nd column+4th column
            }

            String complexPassword = "";
            for (int i = 0; i < randomKeyLength; i++)
            {
                int Column = Array.IndexOf(index, i);//the index of the array index corresponds to the column number in the password matrix
                for (int j = 0; j < password.GetLength(0); j++)
                    complexPassword += (Char)password[j, Column];

            }

            return complexPassword;

        }

        public static String gen_Salt(String randomKeyString, String complexPassword)
        {
            char[] randomKeyStringArray = randomKeyString.ToCharArray();
            char[] complexPasswordArray = complexPassword.ToCharArray();

            StringBuilder salt = new StringBuilder();

            for (int i = 0; i < complexPasswordArray.Length; )
            {
                for (int j = 0; j < randomKeyStringArray.Length; j++)
                {
                    salt.Append((Char)(randomKeyStringArray[j] ^ complexPasswordArray[i + j]));
                }

                i = (++i) * randomKeyStringArray.Length;
            }

            return salt.ToString();
        }

        public static String gen_XORStrings(String strOne,String strTwo)
        {
            //given the two strings are of the same length
            
            char[] strOneArray = strOne.ToCharArray();
            char[] strTwoArray = strTwo.ToCharArray();

            StringBuilder temp = new StringBuilder();

            for (int i = 0; i < strOne.Length;i++ )
            {
                temp.Append((Char)(strOne[i] ^ strTwo[i]));

            }

            return temp.ToString();
        }
    }
}
