using System;
using System.Collections.Generic;

namespace TwoBeasts
{
    public class Labyrinth
    {
        private int height;
        private int width;
        private char[,] labyrinth;
        private enum Direction //possible beast directions
        {
            UP = '^',
            DOWN = 'v',
            RIGHT = '>',
            LEFT = '<'
        }
        private Direction[] dir = new Direction[4] { Direction.UP, Direction.RIGHT, Direction.DOWN, Direction.LEFT }; //directions represented clockwise
        public List<Beast> beasts = new List<Beast>();
        private int[,] movements = new int[,]
                {
                    { -1, 0 },  // up
                    { 0, 1 },   // right
                    { 1, 0 },    // down
                    { 0, -1 }  // left
                };

        public Labyrinth()
        {
            initLab(); //create empty labyrinth
            buildLab(); //insert walls, beast, free 
        }

        private void initLab()
        {
            this.width = Reader.ReadInt();
            this.height = Reader.ReadInt();

            this.labyrinth = new char[this.height, this.width];
            return;
        }

        private void buildLab()
        {
            char character;

            for (int i = 0; i < this.height; i++)
            {
                for (int j = 0; j < this.width; j++)
                {
                    character = Reader.ReadChar();
                    fillLab(i, j, character);
                }
            }
            return;
        }

        private void fillLab(int i, int j, char character)
        {
            this.labyrinth[i, j] = character; //set array index to read one

            if (!((character == 'X') || (character == '.'))) //starting position
            {
                int direction = 0;
                switch (character)
                {
                    case '^':
                        direction = 0;
                        break;
                    case '>':
                        direction = 1;
                        break;
                    case '<':
                        direction = 3;
                        break;
                    case 'v':
                        direction = 2;
                        break;
                }
                Beast b = new Beast(direction, i, j);
                beasts.Add(b);
                this.labyrinth[i, j] = (char)b.beastCount(); //beast -> its representation given by direction when printing 
            }
            return;
        }

        public void printLab()
        {
            for (int i = 0; i < this.height; i++)
            {
                for (int j = 0; j < this.width; j++)
                {
                    if ((labyrinth[i,j] ==  'X') || (labyrinth[i,j] == '.')){
                        Console.Write(labyrinth[i, j]);
                    }
                    else
                    {
                        int direction_number = getBeastsDirection(i, j);
                        Console.Write((char)this.dir[direction_number]);
                    }
                }
                Console.WriteLine();
            }
            return;
        }

        private int getBeastsDirection(int posX, int posY)
        {
            int list_position = (int)labyrinth[posX, posY];
            Beast b = beasts[list_position];
            int direction_number = b.beast_info[0];

            return direction_number;
        }

        public void beastMovement(int beast_number)
        {
            Beast b = beasts[beast_number];
            int dir_number = b.beast_info[0];
            int prev_posX = b.beast_info[1];
            int prev_posY = b.beast_info[2];

            if (b.turnFlag == true)
            {
                b.turnFlag = false;
                b.beast_info[1] += movements[dir_number, 0]; b.beast_info[2] += movements[dir_number, 1];
                labyrinth[b.beast_info[1], b.beast_info[2]] = labyrinth[prev_posX, prev_posY];
                labyrinth[prev_posX, prev_posY] = '.';
                //todo muze se stat ze tam nekdo mezitim vleze
                return;
            }

            int turn_right_dir_number = (dir_number + 1) % 4;
            char character_on_right = labyrinth[prev_posX + movements[turn_right_dir_number, 0], prev_posY + movements[turn_right_dir_number, 1]];
            if (character_on_right == '.')
            {
                b.beast_info[0] = (b.beast_info[0] + 1) % 4;
                b.turnFlag = true;
                return;
            }

            char character_in_front = labyrinth[prev_posX + movements[dir_number, 0], prev_posY + movements[dir_number, 1]];
            if (character_in_front == '.')
            {
                b.beast_info[1] += movements[dir_number, 0]; b.beast_info[2] += movements[dir_number, 1];
                labyrinth[b.beast_info[1], b.beast_info[2]] = labyrinth[prev_posX, prev_posY];
                labyrinth[prev_posX, prev_posY] = '.';
            }
            else
            {
                b.beast_info[0] = (4 + b.beast_info[0] - 1) % 4;
                return;
            }

        }

        public class Beast
        {
            public int[] beast_info;
            static int beast_count = -1;
            public bool turnFlag;

            internal Beast(int direction, int posX, int posY)
            {
                beast_info = new int[] { direction, posX, posY };
                turnFlag = false;
                beast_count++;
            }

            public int beastCount()
            {
                return beast_count;
            }
        }
    }
}

