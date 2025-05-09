using Generator.Model;
using Generator.View;
using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows.Input;

namespace Generator.ViewModel
{
    public class SelectQuizViewModel : BaseViewModel
    {
        private DealWithFile dealWithFile = new DealWithFile();
        public ObservableCollection<QuestionsCollection> Questions { get; set; } = new();
        public ObservableCollection<string> QuizNames { get; } = new ObservableCollection<string>();

        private string? _selectedQuizName;
        public string? SelectedQuizName
        {
            get => _selectedQuizName;
            set
            {
                if (_selectedQuizName != value)
                {
                    _selectedQuizName = value;
                    OnPropertyChanged();
                    // Powiadamiamy, że stan przycisku może się zmienić  
                    (LoadQuizCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public ICommand LoadQuizCommand { get; }
        public ICommand LoadQuizNamesCommand { get; }

        public event Action<string>? QuizSelected;

        private readonly string dbPath = "C:\\Quizy\\QUIZY.db";

        public SelectQuizViewModel()
        {
            LoadQuizCommand = new RelayCommand(LoadSelectedQuizCommand, CanExecuteLoadSelectedQuiz);
            LoadQuizNames();
        }

        // Metoda do załadowania quizu  
        private void LoadSelectedQuizCommand(object? parameter)
        {
            LoadSelectedQuizData();  // Załaduj dane quizu

            // Otwórz nowe okno z wczytanym quizem
            var createQuizWindow = new CreateQuizWindow();

            // Tworzenie nowego ViewModel dla CreateQuizWindow i przypisanie danych
            var createQuizViewModel = new CreateQuizViewModel();
            createQuizViewModel.QuizName = SelectedQuizName;  // Możesz przekazać dodatkowe dane tutaj

            // Tworzenie nowych obiektów QuestionsCollection i dodawanie ich do ObservableCollection
            var dealWithFile = new DealWithFile();
            var loadedQuestions = dealWithFile.LoadFromFile(SelectedQuizName);

            // Tworzymy kolejne obiekty QuestionsCollection na podstawie załadowanych danych
            foreach (var question in loadedQuestions)
            {
                var newQuestion = new QuestionsCollection(
                    question.Question,
                    question.Answer1,
                    question.Answer2,
                    question.Answer3,
                    question.Answer4,
                    question.IsCorrectAnswer1,
                    question.IsCorrectAnswer2,
                    question.IsCorrectAnswer3,
                    question.IsCorrectAnswer4,
                    question.AnswerTime
                );

                createQuizViewModel.QuestionsCollections.Add(newQuestion);
            }

            createQuizWindow.DataContext = createQuizViewModel;
            createQuizWindow.Show();  // Wyświetlenie okna

            NotifyQuizSelected();  // Wywołanie zdarzenia, że quiz został załadowany
        }


        // Metoda sprawdzająca, czy można wykonać załadowanie quizu  
        private bool CanExecuteLoadSelectedQuiz(object? parameter)
        {
            //return !string.IsNullOrEmpty(SelectedQuizName);
            return true;
        }

        // Metoda odpowiedzialna za załadowanie danych wybranego quizu  
        private void LoadSelectedQuizData()
        {
            if (string.IsNullOrEmpty(SelectedQuizName)) return;

            // Wczytanie tylko wybranego quizu  
            try
            {
                using var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;");
                conn.Open();

                using var cmd = new SQLiteCommand("SELECT DISTINCT QuizName FROM Quizzes WHERE QuizName = @QuizName", conn);
                cmd.Parameters.AddWithValue("@QuizName", SelectedQuizName);

                using var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // Możesz tu zrobić coś z wybranym quizem, np. przypisać do zmiennej, pobrać pytania, itp.  
                    string quizName = reader.GetString(0);
                    Console.WriteLine($"Załadowano quiz: {quizName}");
                }
                else
                {
                    Console.WriteLine("Nie znaleziono wybranego quizu.");
                }
            }
            catch (Exception ex)
            {
                // Logowanie błędu  
                Console.WriteLine($"Błąd podczas wczytywania wybranego quizu: {ex.Message}");
            }
        }

        // Metoda wywołująca zdarzenie, informująca, że quiz został załadowany  
        private void NotifyQuizSelected()
        {
            if (!string.IsNullOrEmpty(SelectedQuizName))
            {
                QuizSelected?.Invoke(SelectedQuizName);
            }
        }

        // Ładowanie wszystkich quizów  
        private void LoadQuizNames()
        {
            QuizNames.Clear();

            try
            {
                using var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;");
                conn.Open();

                using var cmd = new SQLiteCommand("SELECT DISTINCT QuizName FROM Quizzes", conn);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string quizName = reader.GetString(0);
                    if (!string.IsNullOrWhiteSpace(quizName))
                        QuizNames.Add(quizName);
                }
            }
            catch (Exception ex)
            {
                // Tu możesz dodać logowanie lub komunikat o błędzie  
                Console.WriteLine($"Błąd podczas wczytywania nazw quizów: {ex.Message}");
            }
        }
    }
}
