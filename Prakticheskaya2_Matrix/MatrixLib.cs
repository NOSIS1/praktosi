using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Prakticheskaya2_Matrix
{
    // Библиотека для работы с матрицами
    public static class MatrixLib
    {
        // Создать пустую матрицу
        public static int[,] CreateEmpty(int rows, int cols)
        {
            return new int[rows, cols];
        }

        // Заполнить матрицу случайными числами
        public static int[,] FillRandom(int rows, int cols, int min, int max)
        {
            int[,] matrix = new int[rows, cols];
            Random random = new Random();
            
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix[i, j] = random.Next(min, max + 1);
                }
            }
            
            return matrix;
        }

        // Очистить матрицу
        public static void Clear(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = 0;
                }
            }
        }

        // Сохранить матрицу в файл
        public static void SaveToFile(int[,] matrix, string filePath)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            
            List<string> lines = new List<string>();
            lines.Add($"{rows} {cols}");
            
            for (int i = 0; i < rows; i++)
            {
                string line = "";
                for (int j = 0; j < cols; j++)
                {
                    line += matrix[i, j] + " ";
                }
                lines.Add(line.Trim());
            }
            
            File.WriteAllLines(filePath, lines);
        }

        // Загрузить матрицу из файла
        public static int[,] LoadFromFile(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            string[] size = lines[0].Split(' ');
            int rows = int.Parse(size[0]);
            int cols = int.Parse(size[1]);
            
            int[,] matrix = new int[rows, cols];
            
            for (int i = 0; i < rows; i++)
            {
                string[] values = lines[i + 1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < cols; j++)
                {
                    matrix[i, j] = int.Parse(values[j]);
                }
            }
            
            return matrix;
        }

        // Проверить, является ли число нечетным
        public static bool IsOdd(int number)
        {
            return number % 2 != 0;
        }

        // Проверить, содержит ли столбец только нечетные числа
        public static bool IsColumnOdd(int[,] matrix, int colIndex)
        {
            int rows = matrix.GetLength(0);
            
            for (int i = 0; i < rows; i++)
            {
                if (!IsOdd(matrix[i, colIndex]))
                {
                    return false;
                }
            }
            
            return true;
        }

        // Найти номер первого столбца с нечетными числами
        // Возвращает номер столбца (1-based) или 0 если не найден
        public static int FindFirstOddColumn(int[,] matrix)
        {
            int cols = matrix.GetLength(1);
            
            for (int j = 0; j < cols; j++)
            {
                if (IsColumnOdd(matrix, j))
                {
                    return j + 1; // Возвращаем номер столбца (нумерация с 1)
                }
            }
            
            return 0; // Не найден
        }

        // Получить информацию о столбце
        public static string GetColumnInfo(int[,] matrix, int colIndex)
        {
            int rows = matrix.GetLength(0);
            string info = $"Столбец {colIndex + 1}:\n";
            
            for (int i = 0; i < rows; i++)
            {
                int value = matrix[i, colIndex];
                string type = IsOdd(value) ? "нечетное" : "четное";
                info += $"  [{i}] = {value} ({type})\n";
            }
            
            return info;
        }
    }
}
