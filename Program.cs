using System;
using System.Collections;
using System.Globalization;
using System.Text;

namespace DS_Proje1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("------------Random Matrice Generation-------------\n");
            GenerateRandomPoints(10, 100, 100);
            Console.WriteLine("--------------K-Nearest Neighbors--------------\n");
            KNN(2, -1.102, 10.2324, 1.6213, -1.8482);

            Console.ReadKey();
        }

        static void GenerateRandomPoints(int n, int height, int width)
        {
            var random = new Random();

            var coordinates = new double[2, n];
            for (var i = 0; i < n; i++)
            {
                double randomRow = random.Next(height);  //value of "y"
                double randomColumn = random.Next(width);  //value of "x"

                coordinates[0, i] = randomColumn;
                coordinates[1, i] = randomRow;
            }

            Console.WriteLine("Printing out the coordinates of {0} random points: ", n);
            for (var i = 0; i < coordinates.GetLength(0); i++)
            {
                for (var j = 0; j < coordinates.GetLength(1); j++)
                {
                    if (j == 0 && i == 0) Console.Write("x: ");
                    if (j == 0 && i == 1) Console.Write("y: ");

                    Console.Write(coordinates[i, j] + " ");
                }
                Console.WriteLine();
            }

            var distanceMatrix = new double[n, n];

            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < n; j++)
                    distanceMatrix[i, j] =
                        Distance(coordinates[0, i], coordinates[1, i],
                            coordinates[0, j], coordinates[1, j]);
            }

            Console.WriteLine("\n" + "Printing out the symmetric Distance Matrix of {0} points: ", n);
            for (var i = 0; i < distanceMatrix.GetLength(0); i++)
            {
                for (var j = 0; j < distanceMatrix.GetLength(1); j++)
                {
                    Console.Write(Math.Round(distanceMatrix[i, j], 1) + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();

            double Distance(double x1, double y1, double x2, double y2)
            {
                return Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
            }
        }

        static void KNN(int k, double V1_varyans, double V2_çarpıklık, double V3_basıklık, double V4_entropi)
        {
            var dataSize = System.IO.File.ReadAllLines(@"data_banknote_authentication.txt").Length;
            var text = System.IO.File.ReadAllText(@"data_banknote_authentication.txt");
            var lines = System.IO.File.ReadAllLines(@"data_banknote_authentication.txt");

            var dataMatrix = new double[dataSize, dataSize];
            var i = 0;
            foreach (var row in text.Split('\n'))
            {
                var j = 0;
                foreach (var column in row.Split(','))
                {
                    dataMatrix[i, j] = double.Parse(column, CultureInfo.InvariantCulture);
                    j++;
                }
                i++;
            }

            var margin = new double[dataSize];
            var counterfeit = new int[dataSize];

            for (var j = 0; j < dataMatrix.GetLength(0); j++)
            {
                var eucledianDistance =
                    Math.Sqrt(Math.Pow(V1_varyans - dataMatrix[j, 0], 2) +
                              Math.Pow(V2_çarpıklık - dataMatrix[j, 1], 2) +
                              Math.Pow(V3_basıklık - dataMatrix[j, 2], 2) +
                              Math.Pow(V4_entropi - dataMatrix[j, 3], 2));

                margin[j] = eucledianDistance;
                counterfeit[j] = (int)dataMatrix[j, 4];
            }

            Array.Sort(margin, counterfeit); //Sorting by keys and items

            var notCounterfeit = 0;
            var isCounterfeit = 0;
            for (var j = 0; j < k; j++)
            {
                switch (counterfeit[j])
                {
                    case 0:
                        isCounterfeit++;
                        break;
                    case 1:
                        notCounterfeit++;
                        break;
                }
            }

            for (var j = 0; j < k; j++)
            {
                Console.Write("k{0}: ", j + 1);
                Console.Write("Distance: " + Math.Round(margin[j], 4) + " ");
                Console.Write("Class: " + counterfeit[j] + "\n");
            }

            if (isCounterfeit > notCounterfeit)
                Console.WriteLine("\nSpecimen is counterfeit.\n");
            else
                Console.WriteLine("\nSpecimen is genuine.\n");


            var testListOfOnes = new ArrayList();
            var testListOfZeros = new ArrayList();
            foreach (var line in lines)
            {
                if (line.EndsWith("0"))
                    testListOfZeros.Add(line);

                if (line.EndsWith("1"))
                    testListOfOnes.Add(line);
            }
            testListOfOnes.RemoveRange(0, testListOfOnes.Count - 100); //the last 100 genuine specimens
            testListOfZeros.RemoveRange(0, testListOfZeros.Count - 100); //the last 100 counterfeit specimens

            Console.OutputEncoding = Encoding.UTF8; //to include Turkish phonetics
            Console.WriteLine("\n-------------------Veriseti-------------------\n");
            Console.WriteLine("Varyans - Çarpıklık - Basıklık - Entropi - Sınıf\n");
            foreach (var line in lines)
                Console.WriteLine(line);
        }

    }
}
