using System;
using System.Collections.Generic;

namespace water_transfer
{
	public class Reader
	{

        public static int[] ReadInt() //prevede znaky na cisla
        {
            int[] inputArr = new int[6];
            int character = Console.Read();

            for (int i = 0; i < inputArr.Length; i++)
            {
                while ((character < '0') || (character > '9'))
                {
                    character = Console.Read();
                }

                int number = 0;
                while ((character >= '0') && (character <= '9'))
                {
                    number = 10 * number + (character - '0');
                    character = Console.Read();
                }
                inputArr[i] = number;
            }
            
            return inputArr;
        }

        public static int[] GetCups(int[] inputArr) //vrati celkove objemy salku
        {
            int[] cups = new int[3];
            for (int i = 0; i < (inputArr.Length / 2) ; i++)
                cups[i] = inputArr[i];

            return cups;
        }

        public static int[] GetVolumes(int[] inputArr) //vrati pocatecni objemy v salkach
        {
            int[] volumes = new int[3];
            for (int i = (inputArr.Length / 2); i < inputArr.Length; i++)
                volumes[i - (inputArr.Length / 2)] = inputArr[i];

            return volumes;
        }
    }
}

