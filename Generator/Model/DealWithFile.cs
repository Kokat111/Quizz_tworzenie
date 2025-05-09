using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text.Json;
using System.Collections.ObjectModel;

namespace Generator.Model
{
    class DealWithFile
    {
        CezarEncryption cezar = new CezarEncryption();
        public ObservableCollection<QuestionsCollection> QuestionsCollections = new ObservableCollection<QuestionsCollection>();
        public string userDocumentsPath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public string databasePath = "C:\\Quizy\\QUIZY.db";

        // Zmieniona metoda SaveToFile, która teraz zapisuje ObservableCollection
        public void SaveToFile(ObservableCollection<QuestionsCollection> questions, string quizName)
        {
            // Serializowanie ObservableCollection do formatu JSON
            string json = JsonSerializer.Serialize(questions);

            // Szyfrowanie danych
            string encryptedJson = cezar.EncryptText(json);

            // Otwieranie połączenia z bazą danych SQLite
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={databasePath};Version=3;"))
            {
                connection.Open();

                // Przygotowanie zapytania SQL do dodania do bazy danych
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    // Zapytanie SQL do dodania nowego quizu do tabeli Quizzes
                    command.CommandText = "INSERT INTO Quizzes (QuizName, EncryptedJson) VALUES (@quizName, @encryptedJson)";
                    command.Parameters.AddWithValue("@quizName", quizName); // Dynamiczne ustawienie nazwy quizu
                    command.Parameters.AddWithValue("@encryptedJson", encryptedJson); // Szyfrowane dane quizu

                    // Wykonanie zapytania SQL
                    command.ExecuteNonQuery();
                }
            }
        }

        // Możesz dodać metodę do ładowania quizu z bazy danych
        public ObservableCollection<QuestionsCollection> LoadFromFile(string quizName)
        {
            ObservableCollection<QuestionsCollection> loadedQuestions = new ObservableCollection<QuestionsCollection>();

            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={databasePath};Version=3;"))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand("SELECT EncryptedJson FROM Quizzes WHERE QuizName = @quizName", connection))
                {
                    command.Parameters.AddWithValue("@quizName", quizName);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string encryptedJson = reader["EncryptedJson"].ToString();
                            string decryptedJson = cezar.DecryptText(encryptedJson);
                            var questions = JsonSerializer.Deserialize<ObservableCollection<QuestionsCollection>>(decryptedJson);

                            if (questions != null)
                            {
                                foreach (var question in questions)
                                {
                                    loadedQuestions.Add(question);
                                }
                            }
                        }
                    }
                }
            }

            return loadedQuestions;
        }

        public List<string> GetAllQuizNames()
        {
            var quizNames = new List<string>();

            using (var connection = new SQLiteConnection($"Data Source={databasePath};Version=3;"))
            {
                connection.Open();
                using (var command = new SQLiteCommand("SELECT QuizName FROM Quizzes", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        quizNames.Add(reader.GetString(0));
                    }
                }
            }

            return quizNames;
        }
    }
}
