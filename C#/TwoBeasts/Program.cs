using System;

namespace TwoBeasts
{
    public class Program
    {
        static void Main(string[] args)
        {
            int numberOfSteps = 20;
            Labyrinth l = new Labyrinth();

            for (int i=0; i<numberOfSteps; i++)
            {
                for (int beast = 0; beast < l.beasts.Count; beast++)
                {
                    l.beastMovement(beast);
                }
                l.printLab();
                Console.WriteLine();
            }

            Console.ReadLine();
        }
    }
}


