using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace LabClient
{
    public partial class MainWindow : Window
    {
        private readonly HttpClient _client = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();
            _client.BaseAddress = new Uri("http://localhost:5003/");
        }

        private async void StudentsButton_Click(object sender, RoutedEventArgs e)
        {
            TitleText.Text = "Список студентов";
            try
            {
                var response = await _client.GetAsync("api/students");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var students = JsonSerializer.Deserialize<List<Student>>(json);
                    DataGrid.ItemsSource = students;
                }
                else
                {
                    MessageBox.Show($"Ошибка сервера: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения: {ex.Message}");
            }
        }

        private async void BooksButton_Click(object sender, RoutedEventArgs e)
        {
            TitleText.Text = "Список книг";
            var response = await _client.GetAsync("api/books");
            if (response.IsSuccessStatusCode)
            {
                var books = JsonSerializer.Deserialize<List<Book>>(await response.Content.ReadAsStringAsync());
                DataGrid.ItemsSource = books;
            }
        }

        private async void RequestsButton_Click(object sender, RoutedEventArgs e)
        {
            TitleText.Text = "Очередь заявок";
            var response = await _client.GetAsync("api/requests");
            if (response.IsSuccessStatusCode)
            {
                var requests = JsonSerializer.Deserialize<List<SupportRequest>>(await response.Content.ReadAsStringAsync());
                DataGrid.ItemsSource = requests;
            }
        }

        private async void ProductsButton_Click(object sender, RoutedEventArgs e)
        {
            TitleText.Text = "Каталог товаров";
            var response = await _client.GetAsync("api/products");
            if (response.IsSuccessStatusCode)
            {
                var products = JsonSerializer.Deserialize<List<Product>>(await response.Content.ReadAsStringAsync());
                DataGrid.ItemsSource = products;
            }
        }

        private async void TasksButton_Click(object sender, RoutedEventArgs e)
        {
            TitleText.Text = "Список задач";
            var response = await _client.GetAsync("api/tasks");
            if (response.IsSuccessStatusCode)
            {
                var tasks = JsonSerializer.Deserialize<List<TaskItem>>(await response.Content.ReadAsStringAsync());
                DataGrid.ItemsSource = tasks;
            }
        }
    }

    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string GroupName { get; set; }
        public double AverageGrade { get; set; }
    }

    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class SupportRequest
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}