using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercice02
{
    class Program
    {
        static void Main(string[] args)
        {
            string[,] grid = GenerateGrid();

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    Console.Write(grid[i,y] + "|");
                }

                Console.WriteLine();
                for (int y = 0; y < grid.GetLength(1) * 2; y++)
                {
                    Console.Write("");
                }
                Console.WriteLine();
            }
        }
        
        /// <summary>
        /// Generate the starter grid used by the player (contain  mines, honeys and count)
        /// </summary>
        /// 
        /// <returns>Une grille </returns>
        public static string[,] GenerateGrid()
        {
            int honeyPoint = 2;
            int minePoint = 1;

            Console.WriteLine("Choisissez un nombre de lignes");
            int nbLine = int.Parse(Console.ReadLine());

            Console.WriteLine("Choisissez un nombre de colonnes");
            int nbColumn = int.Parse(Console.ReadLine());

            string[,] grid = new string[nbLine, nbColumn];

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    grid[i, y] = "ND: ";
                }
            }

            var random = new Random();
            int nbHoney = random.Next(1, 3);
            int nbMine = random.Next(nbLine / 2, grid.Length / 2);

            for (int i = 0; i < nbMine; i++)
            {
                int randomLine = random.Next(0, grid.GetLength(0) - 1);
                int randomColumn = random.Next(0, grid.GetLength(1) - 1);

                if (grid[randomLine, randomColumn].Contains('M') == false)
                {
                    grid[randomLine, randomColumn] = grid[randomLine, randomColumn].Split(':')[0] + ":M";
                }
                IncrementGridCount(grid, randomLine, randomColumn, minePoint);
            }

            for (int i = 0; i < nbHoney; i++)
            {
                int randomLine = random.Next(0, grid.GetLength(0) - 1);
                int randomColumn = random.Next(0, grid.GetLength(1) - 1);

                while (grid[randomLine, randomColumn].Contains("M") == false && grid[randomLine, randomColumn].Contains("H") == false)
                {
                    randomLine = random.Next(0, grid.GetLength(0) - 1);
                    randomColumn = random.Next(0, grid.GetLength(1) - 1);

                    grid[randomLine, randomColumn] = grid[randomLine, randomColumn].Split(':')[0]+ ":H";
                    IncrementGridCount(grid, randomLine, randomColumn, honeyPoint);
                }
            }
            return grid;
        }

        /// <summary>
        /// Increment boxes around a box
        /// </summary>
        /// 
        /// <param name="grid">A grid containing the boxes</param>
        /// <param name="line">The line where they are the box</param>
        /// <param name="column">The line where they are the box</param>
        /// <param name="count">The added value</param>
        public static void IncrementGridCount(string[,] grid, int line, int column, int count)
        {
            /**
             * Si la case est sur les contours de la grille
             */
            if (line == 0 || column == 0 || line == grid.GetLength(0) - 1 || column == grid.GetLength(1) - 1)
            {
                // Si la case est un coin de la grille

                if (line == 0 && column == 0) // coin haut gauche
                {
                    Increment(grid, line, column + 1, count);
                    Increment(grid, line + 1, column + 1, count);
                    Increment(grid, line + 1, column, count);

                }
                else if (line == grid.GetLength(0) - 1 && column == 0) //coin bas gauche
                {
                    Increment(grid, line, column + 1, count);
                    Increment(grid, line - 1, column + 1, count);
                    Increment(grid, line - 1, column, count);
                }
                else if (line == 0 && column == grid.GetLength(1) - 1)// coin haut droit
                {
                    Increment(grid, line, column - 1, count);
                    Increment(grid, line + 1, column - 1, count);
                    Increment(grid, line + 1, column, count);
                }
                else if (line == grid.GetLength(0) - 1 && column == grid.GetLength(1) - 1) // coin bas droit
                {
                    Increment(grid, line, column - 1, count);
                    Increment(grid, line - 1, column - 1, count);
                    Increment(grid, line - 1, column, count);
                }

                // Si la case fait partie des contours mais n'est pas un coin
                else if (line == 0 && column != 0 && column != grid.GetLength(1) - 1) // haut
                {
                    Increment(grid, line, column + 1, count);
                    Increment(grid, line, column - 1, count);
                    Increment(grid, line + 1, column + 1, count);
                    Increment(grid, line + 1, column, count);
                    Increment(grid, line + 1, column - 1, count);
                }
                else if (line == grid.GetLength(0) - 1 && column != 0 && column != grid.GetLength(1) - 1) // bas
                {

                    Increment(grid, line, column + 1, count);
                    Increment(grid, line, column - 1, count);
                    Increment(grid, line - 1, column, count);
                    Increment(grid, line - 1, column + 1, count);
                    Increment(grid, line - 1, column - 1, count);
                }
                else if (column == 0 && line != 0 && line != grid.GetLength(0) - 1) // gauche
                {

                    Increment(grid, line - 1, column, count);
                    Increment(grid, line - 1, column + 1, count);
                    Increment(grid, line, column + 1, count);
                    Increment(grid, line + 1, column + 1, count);
                    Increment(grid, line + 1, column, count);
                }
                else if (column == grid.GetLength(1) - 1 && line != 0 && line != grid.GetLength(0) - 1) // droite
                {
                    Increment(grid, line - 1, column, count);
                    Increment(grid, line - 1, column + 1, count);
                    Increment(grid, line, column + 1, count);
                    Increment(grid, line + 1, column + 1, count);
                    Increment(grid, line + 1, column, count);
                }
            }
            /**
             * Si la case fait partie de l'intérieur de la grille
             */
            else
            {
                Increment(grid, line - 1, column - 1, count);
                Increment(grid, line, column - 1, count);
                Increment(grid, line + 1, column - 1, count);
                Increment(grid, line - 1, column, count);
                Increment(grid, line + 1, column, count);
                Increment(grid, line - 1, column + 1, count);
                Increment(grid, line, column + 1, count);
                Increment(grid, line + 1, column + 1, count);
            }
        }

        /// <summary>
        /// Increment the counter of a box
        /// </summary>
        /// 
        /// <param name="grid">The grid containing the box</param>
        /// <param name="line">The line containing the box</param>
        /// <param name="column">The column containing the box</param>
        /// <param name="count">The added value</param>
        public static void Increment(string[,] grid, int line, int column, int count)
        {

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    Console.Write(grid[i, y] + "\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine("-----------------------------------------------------");


            if (grid[line, column].Contains('H') == false && grid[line, column].Contains('M') == false)
            {
                string[] state = grid[line, column].Split(':');
                int nb;
                if (state[1].Equals(" ")) {
                    nb = count;
                    grid[line, column] = state[0] + ":" + nb.ToString();
                } 
                else
                {
                    nb = int.Parse(state[1]);
                    nb += count;

                    grid[line, column] = state[0] + ":" + nb.ToString();

                }
            }
        }

    }
}
