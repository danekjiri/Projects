using System;
namespace RobotCoordiantes
{
	public class CoordinateSpace
	{
		private enum dir
		{
			UP,
			RIGHT,
			DOWN,
			LEFT,
			FORWARD,
			DOT,
			PASS
		}
		private dir[] clockwise_dir_arr;
		private int[][] robots_directions;
		private Robot r;
		private int dir_shift;

		public CoordinateSpace()
		{
			r = new Robot();
			clockwise_dir_arr = new dir[4] { dir.UP, dir.RIGHT, dir.DOWN, dir.LEFT };
			robots_directions = new int[][] { r.UP, r.RIGHT, r.DOWN, r.LEFT };
		}

		private char readInstruction()
		{
			char ch = (char)Console.Read();
			return ch;
		}

		private dir readAndTranslateInstruction()
		{
			char instruction = (char)Console.Read();
			switch (instruction)
			{
                case 'F':
                    return dir.FORWARD;
                case '.':
                    return dir.DOT;
                case 'U':
					return dir.UP;
				case 'D':
					return dir.DOWN;
				case 'R':
					return dir.RIGHT;
				case 'L':
					return dir.LEFT;
				case '>':
					dir_shift++;
					return dir.PASS;
				case '<':
					dir_shift--;
					return dir.PASS;
				default:
					return dir.PASS;
			}
		}

		private void copyCoordinates(ref int[] from, ref int[] to)
		{
			for (int i = 0; i < from.Length; i++)
				to[i] = from[i];

			return;
		}

        private void updateDirCoordiantes(ref int[] direction, int i, int j)
        {
            int helpvar = direction[i];
            direction[i] = direction[j];
            direction[j] = -helpvar;

			return;
        }

        private void updateOrientation(dir instruction)
		{
			if (instruction == dir.PASS)
				return;

			int lenght = clockwise_dir_arr.Length;
			int shifted_direction = (lenght + (dir_shift + (int)instruction)) % lenght;
			int opossite_direction = (shifted_direction + 2) % lenght;
			instruction = clockwise_dir_arr[shifted_direction];

            switch (instruction)
            {
                case dir.UP:
					copyCoordinates(ref robots_directions[shifted_direction], ref r.FORWARD);
					//right, left not changed
					updateDirCoordiantes(ref robots_directions[opossite_direction], 2, 1);
					updateDirCoordiantes(ref robots_directions[shifted_direction], 2, 1);
                    break;
                case dir.RIGHT:
                    copyCoordinates(ref robots_directions[shifted_direction], ref r.FORWARD);
                    //up, down not changed
                    updateDirCoordiantes(ref robots_directions[opossite_direction], 0, 2);
                    updateDirCoordiantes(ref robots_directions[shifted_direction], 0, 2);
                    break;
                case dir.DOWN:
                    copyCoordinates(ref robots_directions[shifted_direction], ref r.FORWARD);
                    //right, left not changed
                    updateDirCoordiantes(ref robots_directions[opossite_direction], 1, 2);
                    updateDirCoordiantes(ref robots_directions[shifted_direction], 1, 2);
                    break;
                case dir.LEFT:
                    copyCoordinates(ref robots_directions[shifted_direction], ref r.FORWARD);
                    //up, down not changed
                    updateDirCoordiantes(ref robots_directions[opossite_direction], 2, 0);
                    updateDirCoordiantes(ref robots_directions[shifted_direction], 2, 0);
                    break;
            }
			return;
        }

        public void ExecuteInstructions()
		{
			dir instruction = readAndTranslateInstruction();
			while (instruction != dir.DOT)
			{
				if (instruction == dir.FORWARD)
					r.doStepForward();
				else
					updateOrientation(instruction);

                r.printCurrPosition();
                instruction = readAndTranslateInstruction();
            }
			Console.ReadLine();
			return;
        }

		private class Robot
		{
			//starting position
			internal int[] UP;
			internal int[] DOWN;
			internal int[] RIGHT;
			internal int[] LEFT;
			internal int[] FORWARD;
			internal int[] current_position;

            public Robot()
			{
				UP = new int[3] { 0, 1, 0 };
				DOWN = new int[3] { 0, -1, 0 };
				RIGHT = new int[3] { 1, 0, 0 };
				LEFT = new int[3] { -1, 0, 0 };
				FORWARD = new int[3] { 0, 0, 1 };
				current_position = new int[3] { 0, 0, 0 };
			}

			internal void doStepForward()
			{
				for (int i = 0; i < FORWARD.Length; i++)
					current_position[i] += FORWARD[i];

				return;
			}

			internal void printCurrPosition()
			{
				foreach (int coordinate in current_position)
				{
					Console.Write(coordinate); Console.Write(' ');
                }

                Console.Write('\n');
				return;
            }
        }
    }
}