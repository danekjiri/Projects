using System;
using System.Collections.Generic;

namespace water_transfer
{
	public class Model
	{
		public Situation s;
		public static Queue<int[]> q;
		Queue<int[]> helpQ;

		public Model()
		{
            //int[] inputArr = { 4, 1, 1, 1, 1, 1 };

            int[] inputArr = Reader.ReadInt();
            s = new Situation(Reader.GetCups(inputArr), Reader.GetVolumes(inputArr));
            q = new Queue<int[]>();
            helpQ = new Queue<int[]>();

			helpQ.Enqueue(Reader.GetVolumes(inputArr));
        }

        public void StartCount() //hlavni vypocet
		{
			int[] currSituation = new int[3];
			int numberOfFillings = 0;
			int queueCount = 1;

			while (queueCount > 0)
			{
				for (int i = 0; i < queueCount; i++)
				{
                    currSituation = helpQ.Dequeue();
                    s.TryToFill(currSituation, numberOfFillings); //pokusi se rozlit vodu do ostatnich - validni prida do fronty
                }

                numberOfFillings++; //dalsi kolo prelivani
				queueCount = q.Count;

				for (int i = 0; i < queueCount; i++) //presypani pomocne fronty do while fronty
				{
					currSituation = q.Dequeue();
					helpQ.Enqueue(currSituation);
				}
			}

			s.PrintResult(); //vrati na vystup vsechny mozne kombinace
        }
    }
}

