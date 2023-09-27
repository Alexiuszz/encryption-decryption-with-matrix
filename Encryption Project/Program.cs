using MatrixLibrary;
using System;
using System.Collections.Generic;

namespace Encryption_Project
{
    class Program
    {
        static char[] alphArr = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        static List<char> alpha = new List<char>(alphArr);
        static public List<char> Message()
        {
            List<char> message = new List<char>();
            Console.WriteLine("Enter alphabets in a 2 vector component form");
            int vector = 1;
            do
            {
                Console.WriteLine("Vector {0}", vector);
                char char1;
                Console.Write("Component 1: ");
                bool cont = char.TryParse(Console.ReadLine(), out char1);
                if (!cont)
                {
                    break;
                }
                Console.Write("Component 2: ");
                char char2 = char.Parse(Console.ReadLine());

                string str = char1.ToString().ToUpper();
                char1 = char.Parse(str);

                str = char2.ToString().ToUpper();
                char2 = char.Parse(str);

                message.Add(char1);
                message.Add(char2);
                vector++;
            } while (true);
            return message;
        }

        public static List<Matrix> Vectors(List<char> message)
        {
            List<Matrix> vectors = new List<Matrix>();
            for (int i = 0; i < message.Count; i += 2)
            {
                Matrix vector = new Matrix(2, 1);
                vector[1, 1] = alpha.IndexOf(message[i]);
                vector[2, 1] = alpha.IndexOf(message[i + 1]);
                vectors.Add(vector);
            }
            return vectors;
        }

        public static int Index(double num)
        {
            int val = (int)num;
            int index = 0;
            if (val < -26)
            {
                index = -(val % 26);
                return index;
            }
            if (val < 0)
            {
                index = val + 26;
                return index;
            }
            if (val < 26)
            {
                return val;
            }
            index = val % 26;
            return index;
        }
        public static List<char> Encrypt(List<char> message, List<char> keys)
        {
            List<Matrix> vectors = Vectors(message);
            List<Matrix> keyList = Vectors(keys);
            Matrix key = keyList[0];
            key = key.AugmentMatrix(keyList[1]);

            List<char> encryption = new List<char>();
            Matrix vector = new Matrix(2, 1);
            foreach (Matrix item in vectors)
            {
                vector = key.Multiply(item);
                for (int row = 1; row <= vector.NumRows; row++)
                {
                    int index = Index(vector[row, 1]);
                    encryption.Add(alpha[index]);
                }
            }
            return encryption;
        }


        public static int MultInverse(int num)
        {
            for (int i = 1; i <= 26; i++) 
            {
                if((i*num) % 26 == 1)
                {
                    return i;
                }
            }
            return num;
        }
        public static Matrix Inverse(Matrix matrix)
        {
            int det = Index(matrix.Determinant);
            double scalar = MultInverse(det);

            double temp = matrix[1, 1];
            matrix[1, 1] = matrix[2, 2];
            matrix[2, 2] = temp;

            matrix[1, 2] = Index(-matrix[1, 2]);
            matrix[2, 1] = Index(-matrix[2, 1]);

            matrix = matrix.ScalarMult(scalar);
            return matrix;
        }
        public static List<char> Decrypt(List<char> message, List<char> keys)
        {
            List<Matrix> vectors = Vectors(message);
            List<Matrix> keyList = Vectors(keys);
            Matrix key = keyList[0];
            key = key.AugmentMatrix(keyList[1]);

            key = Inverse(key);


            List<char> decryption = new List<char>();
            Matrix vector = new Matrix(2, 1);
            foreach (Matrix item in vectors)
            {
                vector = key.Multiply(item);
                for (int row = 1; row <= vector.NumRows; row++)
                {
                    int index = Index(vector[row, 1]);
                    decryption.Add(alpha[index]);
                }
            }
            return decryption;
        }


        static void Main(string[] args)
        {
            char[] messageArr = { 'L', 'O', 'F', 'M', 'I', 'M' };
            List<char> message = new List<char>(messageArr);

            char[] keyArr = { 'C', 'B', 'B', 'G' };
            List<char> key = new List<char>(keyArr);

            Console.WriteLine("Message: ");
            //List<char> encryption = Encrypt(message, key);
            foreach (char item in message)
            {
                Console.Write(item);
            }

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Decryption: ");
            List<char> decryption = Decrypt(message, key);
            foreach (char item in decryption)
            {
                Console.Write(item);
            }
        }
    }
}
