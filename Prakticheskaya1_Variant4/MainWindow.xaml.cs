using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace Prakticheskaya1_Variant4
{
    public partial class MainWindow : Window
    {
        // Класс для отображения элемента массива
        public class ArrayItem
        {
            public int Index { get; set; }
            public int Value { get; set; }
        }

        private int[] currentArray;
        private ObservableCollection<ArrayItem> arrayItems;

        public MainWindow()
        {
            InitializeComponent();
            
            // Инициализация
            currentArray = LibMas.CreateEmpty(10);
            arrayItems = new ObservableCollection<ArrayItem>();
            
            // Заполняем случайными числами
            currentArray = LibMas.FillRandom(10, -10, 10);
            UpdateArrayDisplay();
            
            dgArray.ItemsSource = arrayItems;
            
            tbStatus.Text = "Размер массива: 10 элементов";
        }

        // Обновить отображение массива
        private void UpdateArrayDisplay()
        {
            arrayItems.Clear();
            for (int i = 0; i < currentArray.Length; i++)
            {
                arrayItems.Add(new ArrayItem { Index = i, Value = currentArray[i] });
            }
        }

        // Считать данные из таблицы
        private void ReadArrayFromGrid()
        {
            for (int i = 0; i < arrayItems.Count; i++)
            {
                currentArray[i] = arrayItems[i].Value;
            }
        }

        // Кнопка "Выполнить обработку"
        private void Process_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReadArrayFromGrid();
                
                // Обработка массива
                List<Lib_4.Result> results = Lib_4.ProcessArray(currentArray);
                
                // Отображаем результаты
                dgResults.ItemsSource = results;
                
                // Статистика
                string stats = Lib_4.GetStats(currentArray);
                MessageBox.Show("Обработка завершена!\n\n" + stats, 
                    "Результат", MessageBoxButton.OK, MessageBoxImage.Information);
                
                tbStatus.Text = "Обработка выполнена успешно";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Заполнить случайными числами
        private void FillRandom_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FillRandomDialog();
            if (dialog.ShowDialog() == true)
            {
                currentArray = LibMas.FillRandom(currentArray.Length, dialog.MinValue, dialog.MaxValue);
                UpdateArrayDisplay();
                dgResults.ItemsSource = null;
                tbStatus.Text = "Массив заполнен случайными числами";
            }
        }

        // Очистить массив
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            LibMas.Clear(currentArray);
            UpdateArrayDisplay();
            dgResults.ItemsSource = null;
            tbStatus.Text = "Массив очищен";
        }

        // Изменить размер
        private void Resize_Click(object sender, RoutedEventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox(
                "Введите новый размер массива (1-1000):", 
                "Изменение размера", 
                currentArray.Length.ToString());
            
            if (!string.IsNullOrEmpty(input) && int.TryParse(input, out int newSize))
            {
                if (newSize > 0 && newSize <= 1000)
                {
                    int[] newArray = new int[newSize];
                    int copyLength = Math.Min(currentArray.Length, newSize);
                    Array.Copy(currentArray, newArray, copyLength);
                    currentArray = newArray;
                    UpdateArrayDisplay();
                    dgResults.ItemsSource = null;
                    tbStatus.Text = $"Размер массива: {newSize} элементов";
                }
                else
                {
                    MessageBox.Show("Размер должен быть от 1 до 1000!", "Ошибка", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        // Сохранить в файл
        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReadArrayFromGrid();
                
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                dialog.Title = "Сохранить массив";
                
                if (dialog.ShowDialog() == true)
                {
                    LibMas.SaveToFile(currentArray, dialog.FileName);
                    MessageBox.Show("Массив успешно сохранен!", "Сохранение", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    tbStatus.Text = "Файл сохранен";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения: " + ex.Message, "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Открыть файл
        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                dialog.Title = "Открыть массив";
                
                if (dialog.ShowDialog() == true)
                {
                    currentArray = LibMas.LoadFromFile(dialog.FileName);
                    UpdateArrayDisplay();
                    dgResults.ItemsSource = null;
                    MessageBox.Show("Массив успешно загружен!", "Загрузка", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    tbStatus.Text = $"Размер массива: {currentArray.Length} элементов";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки: " + ex.Message, "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // О программе
        private void About_Click(object sender, RoutedEventArgs e)
        {
            string message = "Практическая работа №1\n" +
                           "Работа с одномерным массивом\n\n" +
                           "Вариант 4\n\n" +
                           "Задание:\n" +
                           "Ввести n целых чисел.\n" +
                           "Вычислить для чисел > 0 функцию √x.\n" +
                           "Результат обработки каждого числа вывести на экран.\n\n" +
                           "Разработчик: [Ваше ФИО]\n" +
                           "Группа: [Номер группы]\n" +
                           "Год: 2024";
            
            MessageBox.Show(message, "О программе", 
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Выход
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

    // Диалог для заполнения случайными числами
    public class FillRandomDialog : Window
    {
        private TextBox txtMin;
        private TextBox txtMax;
        
        public int MinValue { get; private set; }
        public int MaxValue { get; private set; }

        public FillRandomDialog()
        {
            Title = "Заполнение случайными числами";
            Width = 300;
            Height = 180;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ResizeMode = ResizeMode.NoResize;

            Grid grid = new Grid();
            grid.Margin = new Thickness(10);
            
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // Минимум
            TextBlock lblMin = new TextBlock { Text = "Минимальное значение:", Margin = new Thickness(0, 5, 0, 5) };
            Grid.SetRow(lblMin, 0);
            grid.Children.Add(lblMin);

            txtMin = new TextBox { Text = "-10", Margin = new Thickness(0, 0, 0, 10) };
            Grid.SetRow(txtMin, 1);
            grid.Children.Add(txtMin);

            // Максимум
            TextBlock lblMax = new TextBlock { Text = "Максимальное значение:", Margin = new Thickness(0, 5, 0, 5) };
            Grid.SetRow(lblMax, 2);
            grid.Children.Add(lblMax);

            txtMax = new TextBox { Text = "10", Margin = new Thickness(0, 0, 0, 10) };
            Grid.SetRow(txtMax, 3);
            grid.Children.Add(txtMax);

            // Кнопки
            StackPanel buttons = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };
            Grid.SetRow(buttons, 4);
            
            Button btnOK = new Button { Content = "OK", Width = 70, Margin = new Thickness(0, 0, 10, 0) };
            btnOK.Click += (s, e) =>
            {
                if (int.TryParse(txtMin.Text, out int min) && int.TryParse(txtMax.Text, out int max))
                {
                    if (min <= max)
                    {
                        MinValue = min;
                        MaxValue = max;
                        DialogResult = true;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Минимум не может быть больше максимума!", "Ошибка", 
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Введите корректные числа!", "Ошибка", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            };
            buttons.Children.Add(btnOK);

            Button btnCancel = new Button { Content = "Отмена", Width = 70 };
            btnCancel.Click += (s, e) => { DialogResult = false; Close(); };
            buttons.Children.Add(btnCancel);

            grid.Children.Add(buttons);
            Content = grid;
        }
    }
}
