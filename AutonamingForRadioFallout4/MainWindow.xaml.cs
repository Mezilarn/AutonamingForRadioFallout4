using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace AutonamingForRadioFallout4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string folderPath;

        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void AllFilesCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MaxFilesTextBox.IsEnabled = false;
        }

        private void AllFilesCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            MaxFilesTextBox.IsEnabled = true;
        }

        private void SelectPathFolderButton_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog ofd = new() { IsFolderPicker = true };
            if (ofd.ShowDialog() == CommonFileDialogResult.Ok)
            {
                //Получить путь до папки
                folderPath = ofd.FileName;
                //Вывести название папки
                PathFolderTextBox.Text = System.IO.Path.GetFileName(folderPath);
            }
        }

        private void RenameFilesButton_Click(object sender, RoutedEventArgs e)
        {
            if (folderPath == null)
            {
                MessageBox.Show("Вы не выбрали папку!");
            }
            else
            {
                //Получить все файлы в папке
                string[] files = Directory.GetFiles(folderPath);
                if (AllFilesCheckBox.IsChecked == true)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        //Получить директорию файла, объединить с i и расширением файлы
                        string newPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(files[i]), (i + 1).ToString() + System.IO.Path.GetExtension(files[i]));
                        //Заменить файл
                        File.Move(files[i], newPath);
                    }
                    MessageBox.Show("Готово");
                }
                else if (AllFilesCheckBox.IsChecked == false)
                {
                    if (Int32.Parse(MaxFilesTextBox.Text) > files.Length)
                    {
                        MessageBox.Show($"Вы ввели число больше, чем файлов в папке!\nВы ввели {Int32.Parse(MaxFilesTextBox.Text)}, в папке их всего {files.Length}");
                    }
                    else
                    {
                        for (int i = 0; i < Int32.Parse(MaxFilesTextBox.Text); i++)
                        {
                            //Получить директорию файла, объединить с i и расширением файлы
                            string newPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(files[i]), (i + 1).ToString() + System.IO.Path.GetExtension(files[i]));
                            //Заменить файл
                            File.Move(files[i], newPath);
                        }
                        MessageBox.Show("Готово");
                    }
                }
            }
        }
        
        private void MaxFilesTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //Ввод только цифр
            e.Handled = !int.TryParse(e.Text, out int result);
        }
    }
}
