using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Number_Picker
{
    using System.Windows.Threading;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            SetUpNum();
            // Добавляем обработчик события KeyDown к Window
            KeyDown += Window_KeyDown;    
        }

        int currentTextValue = 0;
        double resultTime;

        private void Timer_Tick(object? sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            timeTextBlock.Text = $"Time: {(tenthsOfSecondsElapsed / 10F).ToString("0.0s")}";
            if(currentTextValue == 25)
            {
                ResultTextBlock.Text = $"Last result: {tenthsOfSecondsElapsed / 10F:0.0s}";
                resultTime = tenthsOfSecondsElapsed / 10F;
                timer.Stop();
                if(resultTime <= 30) ResultTextBlock.Text += " 😎";
                else if(resultTime > 30 && resultTime <= 60) ResultTextBlock.Text += " 😉";
                else ResultTextBlock.Text += " 😒";
            }
        }

        private void SetUpNum()
        {
            timer.Start(); // Старт таймера
            tenthsOfSecondsElapsed = 0; // десятые доли секунды
            List<int> numbers = Enumerable.Range(1, 25).ToList();
            for (int i = 0; i < numbers.Count; i++)
            {
                // Код реализует генерацию чисел от 1 до 25 в случайном порядке
                Random random = new Random();
                int randomIndex = random.Next(i, numbers.Count);
                int temp = numbers[i];
                numbers[i] = numbers[randomIndex];
                numbers[randomIndex] = temp;
            }

            for(int i = 0; i < numbers.Count; i++)
            {
                // Распределение чисел по элементам TextBlock
                if (mainGrid.Children[i] is TextBlock textBlock)
                {
                    textBlock.Text = numbers[i].ToString();
                }
            }
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // При нажатии мышкой на число, оно исчезает
                if (sender is TextBlock textBlock)
                {
                    if (int.Parse(textBlock.Text) - currentTextValue == 1)
                    {
                        textBlock.Visibility = Visibility.Hidden;
                        currentTextValue++;
                    }
                }
            
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // При нажатии Enter игра возобновляется
            if(e.Key == Key.Enter)
            {
                if (currentTextValue == 25)
                {
                    for (int i = 0; i < currentTextValue; i++)
                    {
                        if (mainGrid.Children[i] is TextBlock txtBlk)
                        {
                            txtBlk.Visibility = Visibility.Visible;
                        }
                    }
                    SetUpNum();
                    currentTextValue = 0;                   
                }
            }
        }
    }
}
