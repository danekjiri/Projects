using System;

namespace BigInDevision
{
    public static class Reader
    {
        static int dev_cyphers = 0;
        const int ASCII_int_value = 48;
        public static int div_value;

        public static int[] ReadDiv()
        {
            char[] read_number;
            read_number = Console.ReadLine().ToArray();


            int[] cypher_arr = new int[read_number.Length];

            for (int i = 0; i < read_number.Length; i++)
            {
                div_value *= 10;
                cypher_arr[i] = (int)read_number[i] - ASCII_int_value;
                div_value += cypher_arr[i];
            }

            return cypher_arr;
        }

        public static int ReadDev()
        {
            int devisor = 0;
            char read_ch = (char)Console.Read();
            while (read_ch != '\n' &&  read_ch != '\r')
            {
                if (devisor != 0)
                    devisor *= 10;
                devisor += (int)read_ch - ASCII_int_value;
                dev_cyphers++;
                read_ch = (char)Console.Read();
            }

            return devisor;
        }
    }
}

/*
 //IMPLEMENTATION WITH NEGATIVE/NOT NATURAL NUMBERS
 if (read_number[0] == '-')
            {
                sign = true;
                sign_shift = 1;
            }
  int[] cypher_arr = new int[read_number.Length - sign_shift];
            for (int i = 0; i < read_number.Length - sign_shift; i++)
            {
                if (read_number[i + sign_shift] != '.')
                    cypher_arr[i] = (int)read_number[i + sign_shift] - 48;
                else
                    cypher_arr[i] = 10; // 11=>decimal point

            }

            return cypher_arr;
 */
