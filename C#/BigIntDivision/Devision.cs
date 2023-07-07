using System;
namespace BigInDevision
{
	public class Devision
	{
        private int divident_cypher_pointer;
        private int[] divident;
        private int devisor;
		private int[] result = new int[100];
		private int result_cypher_pointer;
		private int numbers_after_decimal = 6;

		public Devision()
		{
			divident = Reader.ReadDiv();
			devisor = Reader.ReadDev();
		}

        private void printNumber()
        {
            for (int i = 0; i < result_cypher_pointer; i++) //counted part
            {
                if (result[i] == 10)
                    Console.Write('.');
                else
                    Console.Write(result[i]);
            }

            while (numbers_after_decimal != 0) //fill with zeros if less than 6 decimals after point
            {
                if (numbers_after_decimal == 6) //whole number result
                    Console.Write('.');
                Console.Write(0);
                numbers_after_decimal--;
            }
            Console.Write('\n'); //newline
        }

        private bool divIsBigger(int div)
        {
            if (div >= devisor)
                return true;
            return false;
        }

        private void devisionAfterDecimalPoint(int div)
		{
            result[result_cypher_pointer] = 10; //10 is decimal point
            result_cypher_pointer++;
            int numberOfDecimal = 6;
            div *= 10;

            for (int i = 0; i < numberOfDecimal; i++)
			{
				numbers_after_decimal--;
				div = extractingCurrDev(div);
				div *= 10;
				if (div == 0) //cannot do division anymore - leading zeros
					break;
            }
        }

		private int extractingCurrDev(int div) //one step of devision
		{
			int preres;

            if (divIsBigger(div) == true)
            {
                preres = div / devisor;
                result[result_cypher_pointer] = preres;
                result_cypher_pointer++;
                div = div - (devisor * preres);
            }
            else
            {
                result[result_cypher_pointer] = 0; //div is smaller
                result_cypher_pointer++;
            }

			return div;
        }

		private void greaterZero(int div) //division untill while result or decimal point
		{
            while (true)
            {
                div = extractingCurrDev(div);
                if (divident_cypher_pointer + 1 == divident.Length)
                {
                    if (div == 0)
                    {
                        break;
                    }
                    else
                    {
                        devisionAfterDecimalPoint(div);
                        break;
                    }
                }
                    
                div *= 10;
                divident_cypher_pointer++;
                div += divident[divident_cypher_pointer];
            }
        }

        private void findStartingPos(int div = 0) //borrow as mych digits as it is larger than devisor or start computing with 0. ...
        {
            div += divident[divident_cypher_pointer];
            if (Reader.div_value < devisor)
            {
                result[result_cypher_pointer] = 0;
                result_cypher_pointer++;
                devisionAfterDecimalPoint(Reader.div_value);
            }
            else
            {
                while (divIsBigger(div) == false)
                {
                    divident_cypher_pointer++;
                    div *= 10;
                    div += divident[divident_cypher_pointer];

                }
                greaterZero(div);
            }

            return;
        }

        public void DoDevision()
        {
            findStartingPos();
            printNumber();

            return;
        }
	}
}