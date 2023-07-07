using System;
using System.Collections.Generic;

namespace water_transfer
{
    public class History
    {
        public Dictionary<int, int> history;

		public History()
		{
			history = new Dictionary<int, int>();
		}


        public void LookAndStore(int[] currSituation, int numberOfFillings)
        {
            int[] helpCurrSituation = new int[3];

            if (numberOfFillings > 0)
            {
                for (int help = 0; help < helpCurrSituation.Length; help++)
                    helpCurrSituation[help] = currSituation[help];
                Model.q.Enqueue(helpCurrSituation); //poprve vzdy hodi do sdilene fronty

                for (int i = 0; i < currSituation.Length; i++) 
                {
                    Console.Write("{0} ",currSituation[i]);
                    if (history.ContainsKey(currSituation[i]) == false) //pokud nejaky objem vidi poprve
                        history.Add(helpCurrSituation[i], numberOfFillings);
                }
                Console.Write("\n");
            }

            else //prvnotni inicializace
            {
                Model.q.Enqueue(currSituation);
                for (int i = 0; i < currSituation.Length; i++)
                {
                    if (history.ContainsKey(currSituation[i]) == false)
                        history.Add(currSituation[i], numberOfFillings);
                }
            }
        }
    }
}

