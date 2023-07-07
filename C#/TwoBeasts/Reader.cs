using System;

namespace TwoBeasts
{
    public static class Reader
    {

        public static int ReadInt()
        {
            int read_number;
            read_number = Convert.ToInt32(Console.ReadLine());

            return read_number;
        }

        public static char ReadChar()
        {
            char read_char;
            read_char = (char)Console.Read();
            if (read_char == '\n') //if new line characters read
                read_char = (char)Console.Read();
            if (read_char == '\r')
                read_char = (char)Console.Read();

            return read_char;
        }
    }
}

