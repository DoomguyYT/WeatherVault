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
using System.Windows.Shapes;

namespace WeatherVault
{
    /// <summary>
    /// Логика взаимодействия для ApiKeyDialog.xaml
    /// </summary>
    public partial class ApiKeyDialog : Window
    {
        public string ApiKey { get; private set; } = string.Empty;
        private bool _isPasswordVisible = false;

        public ApiKeyDialog()
        {
            InitializeComponent();
            ApiKeyPasswordBox.Focus();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ApiKey = ApiKeyPasswordBox.Password.Trim();
            if (string.IsNullOrEmpty(ApiKey))
            {
                MessageBox.Show("⚠️ API-ключ не может быть пустым!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void ToggleShowPassword_Click(object sender, RoutedEventArgs e)
        {
            _isPasswordVisible = !_isPasswordVisible;

            var parent = ApiKeyPasswordBox.Parent as StackPanel;
            if (parent == null) return;

            var index = parent.Children.IndexOf(ApiKeyPasswordBox);

            if (_isPasswordVisible)
            {
                var textBox = new TextBox
                {
                    Text = ApiKeyPasswordBox.Password,
                    Width = ApiKeyPasswordBox.Width,
                    Height = ApiKeyPasswordBox.Height,
                    Padding = ApiKeyPasswordBox.Padding,
                    FontSize = ApiKeyPasswordBox.FontSize,
                    Background = ApiKeyPasswordBox.Background,
                    Foreground = ApiKeyPasswordBox.Foreground,
                    BorderBrush = ApiKeyPasswordBox.BorderBrush,
                    BorderThickness = ApiKeyPasswordBox.BorderThickness,
                };

                parent.Children.Remove(ApiKeyPasswordBox);
                parent.Children.Insert(index, textBox);
                (sender as Button).Content = "🙈";
            }
            else
            {
                var textBox = parent.Children[index] as TextBox;
                if (textBox != null)
                {
                    var password = textBox.Text;
                    parent.Children.Remove(textBox);

                    var newPasswordBox = new PasswordBox
                    {
                        Width = ApiKeyPasswordBox.Width,
                        Height = ApiKeyPasswordBox.Height,
                        Padding = ApiKeyPasswordBox.Padding,
                        FontSize = ApiKeyPasswordBox.FontSize,
                        Background = ApiKeyPasswordBox.Background,
                        Foreground = ApiKeyPasswordBox.Foreground,
                        BorderBrush = ApiKeyPasswordBox.BorderBrush,
                        BorderThickness = ApiKeyPasswordBox.BorderThickness,
                    };
                    newPasswordBox.Password = password;

                    parent.Children.Insert(index, newPasswordBox);
                    ApiKeyPasswordBox = newPasswordBox;
                    (sender as Button).Content = "👁️";
                }
            }
        }
    }
}
