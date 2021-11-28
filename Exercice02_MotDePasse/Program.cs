using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercice02
{
    class Program
    {
        /**
         * TODO
         * 
         * Ajouter les conditions de victoires (toutes les cases découvertes + trésors aussi)
         * Ajouter un compteur de coups
         * Mettre une taille maximal et minimale pour la grille
         * 
         * Bonus : Ajouter la^possibilité de mettre des drapeaux comme dans le vrai démineur
         * Faire des niveaux de difficultés
         */
        static void Main(string[] args)
        {
            int honeyPoint = 2;
            int minePoint = 1;
            int nbLine;
            int nbColumn;

            Console.WriteLine("Choisissez un nombre de lignes :");

            //Check the type of input chain
            while (int.TryParse(Console.ReadLine(), out nbLine) == false)
            {
                Console.WriteLine("Vous devez entrer un nombre, réessayer :");
                Console.WriteLine("Choisissez un nombre de lignes :");
            }

            Console.WriteLine("Choisissez un nombre de colonnes :");

            //Check the type of input chain
            while (int.TryParse(Console.ReadLine(), out nbColumn) == false)
            {
                Console.WriteLine("Vous devez entrer un nombre, réessayer :");
                Console.WriteLine("Choisissez un nombre de colonnes :");
            }

            string[,] grid = GenerateGrid(nbLine, nbColumn, minePoint, honeyPoint);

            Console.WriteLine();
            Console.WriteLine("-----------------------");
            Console.WriteLine("Voici la grille de jeu");
            Console.WriteLine("-----------------------");
            Console.WriteLine();
            ViewGrid(grid);

            Console.WriteLine();
            Console.WriteLine("---------------------------------------------------------------------------------");
            Console.WriteLine("Le jeu commence, joyeuse chasse au trésor et puisse le sort vous être favorable !");
            Console.WriteLine("---------------------------------------------------------------------------------");
            Console.WriteLine();
            Play(grid);
        }


        /// <summary>
        /// Generate the starter grid used by the player according to a line and column number (contains  mines, honeys and count)
        ///     
        /// The grid looks like :
        /// 
        /// ND:M |ND:V |ND:V |ND:V |
        /// ------------------------
        /// ND:V |ND:7 |ND:M |ND:4 |
        /// ------------------------
        /// ND:M |ND:5 |ND:H |ND:4 |
        /// -----------------------
        /// ND:V |ND:4 |ND:3 |ND:3 |
        /// ------------------------
        /// 
        /// With 
        /// - D for Discovered
        /// - ND for Not Discovered
        /// - M for Mine
        /// - H for Honey
        /// 
        /// 
        /// </summary>
        /// <param name="nbLine"> Number line of the grid </param>
        /// <param name="nbColumn"> Number column of the grid </param>
        /// <param name="minePoint"> Number increment when the box contain a mine </param>
        /// <param name="honeyPoint"> Number increment when the box is a honey </param>
        /// <returns> The grid </returns>
        public static string[,] GenerateGrid(int nbLine, int nbColumn, int minePoint, int honeyPoint)
        {
            string[,] grid = new string[nbLine, nbColumn];
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    grid[i, y] = "DN:V";
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
                IncremmentBoxesAround(grid, randomLine, randomColumn, minePoint);
            }

            for (int i = 0; i < nbHoney; i++)
            {
                int randomLine = random.Next(0, grid.GetLength(0) - 1);
                int randomColumn = random.Next(0, grid.GetLength(1) - 1);

                while (grid[randomLine, randomColumn].Contains("M") == false && grid[randomLine, randomColumn].Contains("H") == false)
                {
                    randomLine = random.Next(0, grid.GetLength(0) - 1);
                    randomColumn = random.Next(0, grid.GetLength(1) - 1);

                    grid[randomLine, randomColumn] = grid[randomLine, randomColumn].Split(':')[0] + ":H";
                    IncremmentBoxesAround(grid, randomLine, randomColumn, honeyPoint);
                }
            }
            return grid;
        }

        /// <summary>
        /// View the grid, only box which contains D:  
        /// </summary>
        /// <param name="grid">The game grid</param>
        public static void ViewGrid(string[,] grid)
        {
            int nbLine = grid.GetLength(0);
            int nbColumn = grid.GetLength(1);

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    if (grid[i, y].Split(':')[0] == "D")
                    {
                        Console.Write(" " + grid[i, y].Split(':')[1] + " |");
                    }
                    else
                    {
                        Console.Write("   |");
                    }
                }

                Console.WriteLine();
                for (int y = 0; y < nbColumn; y++)
                {
                    Console.Write("----");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        ///     Ask the player a line and a column and discovered the box associated
        ///     If the box is a Mine it's the end of the game
        ///     If the box is a honey or a a count the game must go on
        ///     If the box is empty, the boxes around are discovered
        /// </summary>
        /// <param name="grid"> 
        ///     /-9/6*2-The game grid 
        /// </param>
        /// <returns> 
        ///     If the player win or loose 
        /// </returns>
        public static bool Play(string[,] grid)
        {
            int nbLine = grid.GetLength(0);
            int nbColumn = grid.GetLength(1);

            bool win = true;
            bool end = false;

            while (end == false)
            {
                int line, column;

                Console.WriteLine("Choisir une ligne  : ");

                //Check the type and the value of input chain, the value has to be in the array
                bool lineType = int.TryParse(Console.ReadLine(), out line);
                while ( lineType == false || line > nbLine)
                {
                    if (line > nbLine)
                    {
                        Console.WriteLine("Votre ligne se trouve en dehors de la grille, choissisez de nouveau une ligne, entre 0 et {0}", nbLine);
                    }
                    else
                    {
                        Console.WriteLine("Vous devez entrer un nombre, réessayer,");
                        Console.WriteLine("Choisir une ligne  : ");
                    }
                    lineType = int.TryParse(Console.ReadLine(), out line);
                }

                Console.WriteLine("Choisir une colonne  : ");

                //Check the type and the value of input chain, the value has to be in the array
                bool columnType = int.TryParse(Console.ReadLine(), out column);
                while (columnType == false || column > nbColumn)
                {
                    if(column > nbColumn)
                    {

                        Console.WriteLine("Votre colonne se trouve en dehors de la grille, choissisez de nouveau une colonne, entre 0 et {0}", nbColumn);
                    }
                    else
                    {
                        Console.WriteLine("Vous devez entrer un nombre, réessayer,");
                        Console.WriteLine("Choisir une colonne  : ");
                    }
                    columnType = int.TryParse(Console.ReadLine(), out column);
                }

                grid[line - 1, column - 1] = "D:" + grid[line - 1, column - 1].Split(':')[1];


                if (grid[line - 1, column - 1].Split(':')[1] == "H")
                {
                    Console.WriteLine("BRAVOOOOOOOOOOOO ! Vous avez trouvé un trésor !");
                }
                else if (grid[line - 1, column - 1].Split(':')[1] == "M")
                {
                    end = true;
                    win = false;
                }
                else if (grid[line - 1, column - 1].Split(':')[1] == "V")
                {
                    grid[line - 1, column - 1] = grid[line - 1, column - 1].Split(':')[0] + ":V";
                    MakeVisibleBoxesAround(grid, line - 1, column - 1);
                }

                ViewGrid(grid);
            }
            return win;
        }

        /// <summary>
        /// 
        /// 2 possibilité : faire deux fois cette fonction une fois pour incrémenter une autre pour rendre visible 
        /// autre possibilité: faire une fois la fonction qui renvoit un tableau avec les coordonnées puis re parcourir ce tableau pour incrementer et rendre visible 
        /// --> plus complexe parce que on parcours une première fois le tableau (les 8 cases autours) puis une seconde fois (*2) pour incrementer et rendre visible / compléxité n^2
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="line"></param>
        /// <param name="column"></param>
        /// <param name="count"></param>
        public static void IncremmentBoxesAround(string[,] grid, int line, int column, int count)
        {
            for(int i = -1; i < 2; i++)
            {
                if(line + i > 0 && line + i < grid.GetLength(0))
                {
                    for (int y = -1; y < 2; y++)
                    {
                        if (column + y > 0 && column + y < grid.GetLength(1))
                        {
                            Increment(grid, line + i, column + y, count);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// 2 possibilité : faire deux fois cette fonction une fois pour incrémenter une autre pour rendre visible 
        /// autre possibilité: faire une fois la fonction qui renvoit un tableau avec les coordonnées puis re parcourir ce tableau pour incrementer et rendre visible 
        /// --> plus complexe parce que on parcours une première fois le tableau (les 8 cases autours) puis une seconde fois (*2) pour incrementer et rendre visible / compléxité n^2
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="line"></param>
        /// <param name="column"></param>
        /// <param name="count"></param>
        public static void MakeVisibleBoxesAround(string[,] grid, int line, int column)
        {
            for (int i = -1; i < 2; i++)
            {
                if (line + i >= 0 && line + i < grid.GetLength(0))
                {
                    for (int y = -1; y < 2; y++)
                    {
                        if (column + y >= 0 && column + y < grid.GetLength(1))
                        {
                            if (grid[line + i, column + y].Split(':')[1] != "M")
                            {
                                if (grid[line + i, column + y].Split(':')[1] == "V" && grid[line + i, column + y].Split(':')[0] != "D")
                                {
                                    grid[line + i, column + y] = "D:" + 'V';
                                    MakeVisibleBoxesAround(grid, line + i, column + y);
                                }
                                else
                                {
                                    grid[line + i, column + y] = "D:" + grid[line + i, column + y].Split(':')[1];
                                }
                            }
                        }
                    }
                }
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


            if (grid[line, column].Contains('H') == false && grid[line, column].Contains('M') == false)
            {
                string[] state = grid[line, column].Split(':');
                int nb;                   
                if (state[1].Equals("V")) {
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
