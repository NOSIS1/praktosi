using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace Prakticheskaya2_Matrix
{
    public partial class MainWindow : Window
    {
        private int[,] currentMatrix;
        private DataTable matrixTable;

        public MainWindow()
        {
            InitializeComponent();
            
            // Создаем матрицу по умолчанию 5x5
            CreateNewMatrix(5, 5);
            FillMatrixRandom(-10, 10);
        }

        // Создать новую матрицу
        private void CreateNewMatrix(int rows, int cols)
        {
            currentMatrix = MatrixLib.CreateEmpty(rows, cols);
            matrixTable = new DataTable();
            
            // Создаем колонки
            for (int j = 0; j < cols; j++)
            {
                matrixTable.Columns.Add($"Столбец {j + 1}", typeof(int));
            }
            
            // Создаем строки
            for (int i = 0; i < rows; i++)
            {
                DataRow row = matrixTable.NewRow();
                for (int j = 0; j < cols; j++)
                {
                    row[j] = currentMatrix[i, j];
                }
                matrixTable.Rows.Add(row);
            }
            
            dgMatrix.ItemsSource = matrixTable.DefaultView;
            tbStatus.Text = $"Матрица размером {rows}x{cols}";
        }

        // Обновить отображение матрицы
        private void UpdateMatrixDisplay()
        {
            int rows = currentMatrix.GetLength(0);
            int cols = currentMatrix.GetLength(1);
            
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrixTable.Rows[i][j] = currentMatrix[i, j];
                }
            }
        }

        // Считать данные из таблицы
        private void ReadMatrixFromGrid()
        {
            int rows = currentMatrix.GetLength(0);
            int cols = currentMatrix.GetLength(1);
            
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (int.TryParse(matrixTable.Rows[i][j].ToString(), out int value))
                    {
                        currentMatrix[i, j] = value;
                    }
                }
            }
        }

        // Заполнить случайными числами
        private void FillMatrixRandom(int min, int max)
        {
            int rows = currentMatrix.GetLength(0);
            int cols = currentMatrix.GetLength(1);
            currentMatrix = MatrixLib.FillRandom(rows, cols, min, max);
            UpdateMatrixDisplay();
        }

        // Кнопка "Создать матрицу"
        private void CreateMatrix_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CreateMatrixDialog();
            if (dialog.ShowDialog() == true)
            {
                CreateNewMatrix(dialog.Rows, dialog.Cols);
                FillMatrixRandom(-10, 10);
                tbStatus.Text = $"Создана матрица {dialog.Rows}x{dialog.Cols}";
            }
        }

        // Кнопка "Заполнить случайными"
        private void FillRandom_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FillRandomDialog();
            if (dialog.ShowDialog() == true)
            {
                FillMatrixRandom(dialog.MinValue, dialog.MaxValue);
                tbStatus.Text = "Матрица заполнена случайными числами";
            }
        }

        // Кнопка "Очистить"
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            MatrixLib.Clear(currentMatrix);
            UpdateMatrixDisplay();
            tbStatus.Text = "Матрица очищена";
        }

        // Кнопка "Найти столбец"
        private void FindColumn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReadMatrixFromGrid();
                
                int result = MatrixLib.FindFirstOddColumn(currentMatrix);
                
                string message;
                if (result > 0)
                {
                    message = $"Найден столбец №{result}, содержащий только нечетные числа!\n\n";
                    message += MatrixLib.GetColumnInfo(currentMatrix, result - 1);
                    MessageBox.Show(message, "Результат поиска", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    tbStatus.Text = $"Найден столбец №{result}";
                }
                else
                {
                    message = "Столбцов, содержащих только нечетные числа, не найдено.\nРезультат: 0";
                    MessageBox.Show(message, "Результат поиска", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    tbStatus.Text = "Столбцы не найдены";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Сохранить в файл
        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReadMatrixFromGrid();
                
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "Текстовые файлы (*.txt)|*.txt";
                dialog.Title = "Сохранить матрицу";
                
                if (dialog.ShowDialog() == true)
                {
                    MatrixLib.SaveToFile(currentMatrix, dialog.FileName);
                    MessageBox.Show("Матрица сохранена!", "Сохранение", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    tbStatus.Text = "Файл сохранен";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Открыть файл
        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Текстовые файлы (*.txt)|*.txt";
                dialog.Title = "Открыть матрицу";
                
                if (dialog.ShowDialog() == true)
                {
                    currentMatrix = MatrixLib.LoadFromFile(dialog.FileName);
                    int rows = currentMatrix.GetLength(0);
                    int cols = currentMatrix.GetLength(1);
                    CreateNewMatrix(rows, cols);
                    UpdateMatrixDisplay();
                    MessageBox.Show("Матрица загружена!", "Загрузка", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    tbStatus.Text = $"Загружена матрица {rows}x{cols}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // О программе
        private void About_Click(object sender, RoutedEventArgs e)
        {
            string message = "Работа с матрицей\n\n" +
                           "Задание:\n" +
                           "Дана целочисленная матрица размера M × N.\n" +
                           "Найти номер первого из ее столбцов,\n" +
                           "содержащих только нечетные числа.\n" +
                           "Если таких столбцов нет, то вывести 0.\n\n" +
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

    // Диалог создания матрицы
    public class CreateMatrixDialog : Window
    {
        private TextBox txtRows;
        private TextBox txtCols;
        
        public int Rows { get; private set; }
        public int Cols { get; private set; }

        public CreateMatrixDialog()
        {
            Title = "Создание матрицы";
            Width = 300;
            Height = 180;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ResizeMode = ResizeMode.NoResize;

            Grid grid = new Grid();
            grid.Margin = new Thickness(10);
            
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            TextBlock lblRows = new TextBlock { Text = "Количество строк:", Margin = new Thickness(0, 5, 0, 5) };
            Grid.SetRow(lblRows, 0);
            grid.Children.Add(lblRows);

            txtRows = new TextBox { Text = "5", Margin = new Thickness(0, 0, 0, 10) };
            Grid.SetRow(txtRows, 1);
            grid.Children.Add(txtRows);

            TextBlock lblCols = new TextBlock { Text = "Количество столбцов:", Margin = new Thickness(0, 5, 0, 5) };
            Grid.SetRow(lblCols, 2);
            grid.Children.Add(lblCols);

            txtCols = new TextBox { Text = "5", Margin = new Thickness(0, 0, 0, 10) };
            Grid.SetRow(txtCols, 3);
            grid.Children.Add(txtCols);

            StackPanel buttons = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };
            Grid.SetRow(buttons, 5);
            
            Button btnOK = new Button { Content = "OK", Width = 70, Margin = new Thickness(0, 0, 10, 0) };
            btnOK.Click += (s, e) =>
            {
                if (int.TryParse(txtRows.Text, out int rows) && int.TryParse(txtCols.Text, out int cols))
                {
                    if (rows > 0 && rows <= 20 && cols > 0 && cols <= 20)
                    {
                        Rows = rows;
                        Cols = cols;
                        DialogResult = true;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Размеры должны быть от 1 до 20!", "Ошибка", 
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

    // Диалог заполнения случайными числами
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
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            TextBlock lblMin = new TextBlock { Text = "Минимальное значение:", Margin = new Thickness(0, 5, 0, 5) };
            Grid.SetRow(lblMin, 0);
            grid.Children.Add(lblMin);

            txtMin = new TextBox { Text = "-10", Margin = new Thickness(0, 0, 0, 10) };
            Grid.SetRow(txtMin, 1);
            grid.Children.Add(txtMin);

            TextBlock lblMax = new TextBlock { Text = "Максимальное значение:", Margin = new Thickness(0, 5, 0, 5) };
            Grid.SetRow(lblMax, 2);
            grid.Children.Add(lblMax);

            txtMax = new TextBox { Text = "10", Margin = new Thickness(0, 0, 0, 10) };
            Grid.SetRow(txtMax, 3);
            grid.Children.Add(txtMax);

            StackPanel buttons = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };
            Grid.SetRow(buttons, 5);
            
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
