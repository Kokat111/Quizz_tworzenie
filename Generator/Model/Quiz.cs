using System;
using System.Collections.Generic;

namespace Generator.Model
{
    public class Quiz
    {
        public string QuizName { get; set; }
        public List<QuestionsCollection> Questions { get; set; }
    }
}
