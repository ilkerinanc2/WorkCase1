using System;
using System.Collections.Generic;
using System.Linq;


/// <summary>
/// İlker İnanç
/// Hepsiburada Case Study Mars Rover
/// 
/// Important Notes : 
/// /// Application has no limitations for rover count.
/// /// As long as you continue to enter application will receive the information.
/// /// If you will not enter more information, press enter for continue.
/// 
/// </summary>
namespace HepsiburadaCase1
{
    class Program
    {        
        static void Main(string[] args)
        {
            #region Set upper bound

            string[] boundCoordsArr = Console.ReadLine().Split();
            Coordinates boundCoords = new Coordinates(int.Parse(boundCoordsArr[0]), int.Parse(boundCoordsArr[1]), "");

            #endregion

            List<Coordinates> roverCoordinatesList = new List<Coordinates>(); // Keeps return values for console (Final coordinates of rovers).
            string[] coordinates = Console.ReadLine().ToUpper().Split();

            while(coordinates.Length == 3)
            {
                Coordinates roverCoords = fillCoordsFromArray(coordinates);

                string moveOrders = Console.ReadLine().ToUpper();

                processAllOrders(roverCoords, moveOrders, boundCoords);

                roverCoordinatesList.Add(roverCoords);

                coordinates = Console.ReadLine().ToUpper().Split();
            }

            roverCoordinatesList.ForEach(coor => Console.WriteLine(string.Join(" ", coor.X, coor.Y, coor.H)));
            
            Console.ReadLine();
        }

        /// <summary>
        /// Processes all orders one by one.
        /// Controls bounds for every step to avoid overflow.
        /// </summary>
        /// <param name="roverCoords"></param>
        /// <param name="moveOrders"></param>
        /// <param name="boundCoords"></param>
        private static void processAllOrders(Coordinates roverCoords, string moveOrders, Coordinates boundCoords)
        {
            Dictionary<string, int> directionDict = getDirectionDict();

            foreach (char order in moveOrders)
            {
                updateCoordsAndHeadings(roverCoords, order, directionDict);

                #region Right upper bound controls
                if (roverCoords.X > boundCoords.X)
                    roverCoords.X = boundCoords.X;
                if (roverCoords.Y > boundCoords.Y)
                    roverCoords.Y = boundCoords.Y;
                #endregion

                #region Left bottom bound controls
                if (roverCoords.X < 0)
                    roverCoords.X = 0;
                if (roverCoords.Y < 0)
                    roverCoords.Y = 0;
                #endregion
            }
        }

        /// <summary>
        /// Updates coordinates and headings for given order.        
        /// </summary>
        /// <param name="roverCoords"></param>
        /// <param name="order"></param>
        /// <param name="directionDict"></param>
        private static void updateCoordsAndHeadings(Coordinates roverCoords, char order, Dictionary<string, int> directionDict)
        {
            // DirectionDict has four values. With enumerating them, easily finds their next direction with modulo.
            // Enum can also be used.            

            int directionCode = directionDict[roverCoords.H];            

            switch (order)
            {
                case 'L': // Just rotation changes, needs negativity control when turns left.
                    directionCode--;
                    if (directionCode < 0)
                        directionCode += 4;
                    updateHeading(roverCoords, directionDict, directionCode);
                    break;
                case 'R': // Just rotation changes
                    directionCode++;
                    updateHeading(roverCoords, directionDict, directionCode);
                    break;
                case 'M': // Just position changes
                    updateXY(roverCoords);
                    break;
            }
        }

        /// <summary>
        /// Moves the rover by a grid according to its heading.        
        /// </summary>
        /// <param name="roverCoords"></param>
        private static void updateXY(Coordinates roverCoords)
        {
            switch (roverCoords.H)
            {
                case "N":
                    roverCoords.Y++;
                    break;
                case "E":
                    roverCoords.X++;
                    break;
                case "S":
                    roverCoords.Y--;
                    break;
                case "W":
                    roverCoords.X--;
                    break;
            }
        }

        /// <summary>
        /// Updates coordinates with given direction code.        
        /// </summary>
        /// <param name="roverCoords"></param>
        /// <param name="directionDict"></param>
        /// <param name="directionCode"></param>
        private static void updateHeading(Coordinates roverCoords, Dictionary<string, int> directionDict, int directionCode)
        {
            roverCoords.H = directionDict.FirstOrDefault(v => v.Value == directionCode % 4).Key;
        }

        /// <summary>
        /// Returns a dictionary that holds directions and their codes.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, int> getDirectionDict()
        {
            return new Dictionary<string, int>() { { "N", 0 }, { "E", 1 }, { "S", 2 }, { "W", 3 } };
        }

        /// <summary>
        /// Fills Coordinates object with coordinates array that has taken and splitted from console.
        /// </summary>
        /// <param name="coordinates"></param>
        /// <returns></returns>
        private static Coordinates fillCoordsFromArray(string[] coordinates)
        {
            return new Coordinates(int.Parse(coordinates[0]), int.Parse(coordinates[1]), coordinates[2]);
        }
    }

    /// <summary>
    /// Class for keeping coordinates in one place.
    /// </summary>
    public class Coordinates
    {
        public Coordinates(int x, int y, string h)
        {
            X = x;
            Y = y;
            H = h;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public string H { get; set; } // Heading
    }
}
