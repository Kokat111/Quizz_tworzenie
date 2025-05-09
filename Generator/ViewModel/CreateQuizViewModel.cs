using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Generator.Model;

namespace Generator.ViewModel
{
    public class CreateQuizViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<QuestionsCollection> QuestionsCollections { get; set; }
        DealWithFile dealWithFile = new DealWithFile();
        public string Title => "Tworzenie quizu";
        private int currentIndex = -1;


        private string quizName;
        public string QuizName
        {
            get => quizName;
            set
            {
                if (quizName != value)
                {
                    quizName = value;
                    OnPropertyChanged(nameof(QuizName));
                }
            }
        }

        private string question;
        public string Question
        {
            get => question;
            set
            {
                if (question != value)
                {
                    question = value;
                    OnPropertyChanged(nameof(Question));
                }
            }
        }

        private string answer1;
        public string Answer1
        {
            get => answer1;
            set
            {
                if (answer1 != value)
                {
                    answer1 = value;
                    OnPropertyChanged(nameof(Answer1));
                }
            }
        }

        private string answer2;
        public string Answer2
        {
            get => answer2;
            set
            {
                if (answer2 != value)
                {
                    answer2 = value;
                    OnPropertyChanged(nameof(Answer2));
                }
            }
        }

        private string answer3;
        public string Answer3
        {
            get => answer3;
            set
            {
                if (answer3 != value)
                {
                    answer3 = value;
                    OnPropertyChanged(nameof(Answer3));
                }
            }
        }

        private string answer4;
        public string Answer4
        {
            get => answer4;
            set
            {
                if (answer4 != value)
                {
                    answer4 = value;
                    OnPropertyChanged(nameof(Answer4));
                }
            }
        }

        private bool isCorrectAnswer1;

        public bool IsCorrectAnswer1
        {
            get => isCorrectAnswer1;
            set
            {
                if (isCorrectAnswer1 != value)
                {
                    isCorrectAnswer1 = value;
                    OnPropertyChanged(nameof(IsCorrectAnswer1));
                }
            }

        }

        private bool isCorrectAnswer2;

        public bool IsCorrectAnswer2
        {
            get => isCorrectAnswer2;
            set
            {
                if (isCorrectAnswer2 != value)
                {
                    isCorrectAnswer2 = value;
                    OnPropertyChanged(nameof(IsCorrectAnswer2));
                }
            }

        }

        private bool isCorrectAnswer3;

        public bool IsCorrectAnswer3
        {
            get => isCorrectAnswer3;
            set
            {
                if (isCorrectAnswer3 != value)
                {
                    isCorrectAnswer3 = value;
                    OnPropertyChanged(nameof(IsCorrectAnswer3));
                }
            }

        }

        private bool isCorrectAnswer4;

        public bool IsCorrectAnswer4
        {
            get => isCorrectAnswer4;
            set
            {
                if (isCorrectAnswer4 != value)
                {
                    isCorrectAnswer4 = value;
                    OnPropertyChanged(nameof(IsCorrectAnswer4));
                }
            }

        }

        private string correctAnswer;
        public string CorrectAnswer
        {
            get => correctAnswer;
            set
            {
                if (correctAnswer != value)
                {
                    correctAnswer = value;
                    OnPropertyChanged(nameof(CorrectAnswer));
                }
            }
        }

        private string answerTime;
        public string AnswerTime
        {
            get => answerTime;
            set
            {
                if (answerTime != value)
                {
                    answerTime = value;
                    OnPropertyChanged(nameof(AnswerTime));
                }
            }
        }

        public ICommand NavigateCommandRight { get; }
        public ICommand NavigateCommandLeft { get; }
        public ICommand SaveQuizCommand { get; }
        public ICommand ExitToMenuCommand { get; }


        public CreateQuizViewModel()
        {
            QuestionsCollections = new ObservableCollection<QuestionsCollection>();
            NavigateCommandRight = new RelayCommand(NavigateRight);
            NavigateCommandLeft = new RelayCommand(NavigateLeft);
            SaveQuizCommand = new RelayCommand(SaveQuiz);
            ExitToMenuCommand = new RelayCommand(ExitToMenu);

        }

        public void NavigateRight(object? parameter)
        {
            Console.WriteLine("Nawigacja w prawo");

            // Jeśli istnieje następne pytanie — przejdź do niego
            if (currentIndex + 1 < QuestionsCollections.Count)
            {
                currentIndex++;
                var nextQuestion = QuestionsCollections[currentIndex];

                Question = nextQuestion.Question;
                Answer1 = nextQuestion.Answer1;
                Answer2 = nextQuestion.Answer2;
                Answer3 = nextQuestion.Answer3;
                Answer4 = nextQuestion.Answer4;
                IsCorrectAnswer1 = nextQuestion.IsCorrectAnswer1;
                IsCorrectAnswer2 = nextQuestion.IsCorrectAnswer2;
                IsCorrectAnswer3 = nextQuestion.IsCorrectAnswer3;
                IsCorrectAnswer4 = nextQuestion.IsCorrectAnswer4;
                AnswerTime = nextQuestion.AnswerTime;
            }
            else
            {
                // Jesteśmy na końcu kolekcji — próbujemy dodać nowe pytanie
                if (!string.IsNullOrEmpty(Question) &&
                    !string.IsNullOrEmpty(Answer1) &&
                    !string.IsNullOrEmpty(Answer2) &&
                    !string.IsNullOrEmpty(Answer3) &&
                    !string.IsNullOrEmpty(Answer4))
                {
                    var newQuestion = new QuestionsCollection
                    {
                        Question = this.Question,
                        Answer1 = this.Answer1,
                        Answer2 = this.Answer2,
                        Answer3 = this.Answer3,
                        Answer4 = this.Answer4,
                        IsCorrectAnswer1 = this.IsCorrectAnswer1,
                        IsCorrectAnswer2 = this.IsCorrectAnswer2,
                        IsCorrectAnswer3 = this.IsCorrectAnswer3,
                        IsCorrectAnswer4 = this.IsCorrectAnswer4,
                        AnswerTime = this.AnswerTime
                    };

                    QuestionsCollections.Add(newQuestion);
                    currentIndex++;

                    ResetFields();
                    IsCorrectAnswer1 = false;
                    IsCorrectAnswer2 = false;
                    IsCorrectAnswer3 = false;
                    IsCorrectAnswer4 = false;
                }
                else
                {
                    Console.WriteLine("Wszystkie pola muszą zostać wypełnione.");
                }
            }
        }


        public void NavigateLeft(object? parameter)
        {
            Console.WriteLine("Nawigacja w lewo");
            if (currentIndex >= 0)  // Upewnij się, że nie wyjdziesz poza zakres
            {
                var previousQuestion = QuestionsCollections[currentIndex];
                currentIndex--;  // Przechodzimy do poprzedniego pytania


                // Ustawianie wartości formularza na dane poprzedniego pytania
                Question = previousQuestion.Question;
                Answer1 = previousQuestion.Answer1;
                Answer2 = previousQuestion.Answer2;
                Answer3 = previousQuestion.Answer3;
                Answer4 = previousQuestion.Answer4;
                IsCorrectAnswer1 = previousQuestion.IsCorrectAnswer1;
                IsCorrectAnswer2 = previousQuestion.IsCorrectAnswer2;
                IsCorrectAnswer3 = previousQuestion.IsCorrectAnswer3;
                IsCorrectAnswer4 = previousQuestion.IsCorrectAnswer4;
                AnswerTime = previousQuestion.AnswerTime;
            }
            else
            {
                Console.WriteLine("Brak poprzedniego pytania.");
            }
        }

        public void SaveQuiz(object? parameter)
        {
            // Implementacja zapisywania quizu
            Console.WriteLine("Zapisz quiz");
            dealWithFile.SaveToFile(QuestionsCollections, quizName);
        }

        public void ExitToMenu(object? parameter)
        {
            if (parameter is System.Windows.Window window)
            {
                window.Close();
            }
        }

        private void ResetFields()
        {
            // Resetowanie wartości pól
            Question = string.Empty;
            Answer1 = string.Empty;
            Answer2 = string.Empty;
            Answer3 = string.Empty;
            Answer4 = string.Empty;
            CorrectAnswer = string.Empty;
            AnswerTime = string.Empty;
        }

        // Powiadamianie o zmianach właściwości
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
