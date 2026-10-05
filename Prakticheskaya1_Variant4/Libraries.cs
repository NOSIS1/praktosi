using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Prakticheskaya1_Variant4
{
    // Библиотека LibMas - базовые операции с массивом
    public static class LibMas
    {
        // Заполнить массив случайными числами
        public static int[] FillRandom(int size, int min, int max)
        {
            int[] array = new int[size];
            Random random = new Random();
            for (int i = 0; i < size; i++)
            {
                array[i] = random.Next(min, max + 1);
            }
            return array;
        }

        // Создать пустой массив
        public static int[] CreateEmpty(int size)
        {
            return new int[size];
        }

        // Очистить массив
        public static void Clear(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = 0;
            }
        }

        // Сохранить массив в файл
        public static void SaveToFile(int[] array, string filePath)
        {
            File.WriteAllLines(filePath, array.Select(x => x.ToString()));
        }

        // Загрузить массив из файла
        public static int[] LoadFromFile(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            int[] array = new int[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                array[i] = int.Parse(lines[i]);
            }
            return array;
        }
    }

    // Библиотека Lib_4 - вычисления по варианту 4
    // Задание: Ввести n целых чисел. Вычислить для чисел > 0 функцию корень из x
    public static class Lib_4
    {
        // Результат обработки одного числа
        public class Result
        {
            public int Index { get; set; }
            public int Number { get; set; }
            public string SqrtValue { get; set; }
            public string Status { get; set; }
        }

        // Обработать массив
        public static List<Result> ProcessArray(int[] array)
        {
            List<Result> results = new List<Result>();
            
            for (int i = 0; i < array.Length; i++)
            {
                Result result = new Result();
                result.Index = i;
                result.Number = array[i];
                
                if (array[i] > 0)
                {
                    double sqrt = Math.Sqrt(array[i]);
                    result.SqrtValue = sqrt.ToString("F4");
                    result.Status = "Обработано";
                }
                else
                {
                    result.SqrtValue = "-";
                    result.Status = "Не обработано";
                }
                
                results.Add(result);
            }
            
            return results;
        }

        // Получить статистику
        public static string GetStats(int[] array)
        {
            int total = array.Length;
            int processed = 0;
            
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] > 0)
                    processed++;
            }
            
            return $"Всего чисел: {total}\nОбработано (> 0): {processed}\nНе обработано (≤ 0): {total - processed}";
        }
    }
}
