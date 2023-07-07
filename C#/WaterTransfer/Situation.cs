using System;
using System.Collections.Generic;

namespace water_transfer
{
	public class Situation
	{
		int[] cups;
        public History h;
        public List<string> prevSituations;



        public Situation(int[] inputCups, int[] cupsVolumes)
		{
			cups = inputCups;
			h = new History();
            prevSituations = new List<string>(); //pole obsahujici id minulych kombinaci
        }

        public void TryToFill(int[] currSituation, int numberOfFillings)
		{
			int[] helpCurrSituation = new int[3];
			int[] currentSituation = currSituation;

            for (int x = 0; x < currentSituation.Length; x++) //vysyp momentalni situaci do pomocne, aby se nezmenila
                helpCurrSituation[x] = currentSituation[x];
            
            if (numberOfFillings > 0)
            {
                for (int i = 0; i < currentSituation.Length; i++)
                {
                    for (int j = 0; j < currentSituation.Length; j++)
                    {
                        if ((i != j) && ((currentSituation[i] > 0) && (currentSituation[j] < cups[j]))) //neleje-li sama do sebe nebo do plneho ci z prazdneho
                        {
                            for (int x = 0; x < currentSituation.Length; x++)
                                helpCurrSituation[x] = currentSituation[x];

                            while ((helpCurrSituation[i] > 0) && (helpCurrSituation[j] < cups[j])) //postupne prelej tekutinu
                            {
                                helpCurrSituation[i]--;
                                helpCurrSituation[j]++;
                            }
                            if (prevSituations.Contains(createId(helpCurrSituation)) == false) //zkoumame pokud se jedna o novou kombinaci
                                h.LookAndStore(helpCurrSituation, numberOfFillings);
                        }
                    }
                }

                prevSituations.Add(createId(currentSituation));
            }

            else //pri prvni iniciali
				h.LookAndStore(helpCurrSituation, numberOfFillings);
            
        }

        string createId(int[] helpCurrSituation)
        {
            return Convert.ToString((helpCurrSituation[0], helpCurrSituation[1], helpCurrSituation[2])); //pseudohash pro hledani
        }


        public void PrintResult()
		{
			foreach (KeyValuePair<int, int> dict in new SortedDictionary<int, int>(h.history)) //vypis serazeneho slovniku dle klicu
				Console.Write("{0}:{1} ", dict.Key, dict.Value);
        }
	}
}

