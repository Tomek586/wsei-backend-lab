
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.Extensions.Logging; // Dodane dla ILogger
using System.Collections.Generic;

namespace BackendLab01.Pages
{
    public class QuizModel : PageModel
    {
        private readonly IQuizUserService _userService;
        private readonly ILogger _logger;

        public QuizModel(IQuizUserService userService, ILogger<QuizModel> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [BindProperty]
        public string Question { get; set; }

        [BindProperty]
        public List<string> Answers { get; set; }

        [BindProperty]
        public string UserAnswer { get; set; }

        [BindProperty]
        public int QuizId { get; set; }

        [BindProperty]
        public int ItemId { get; set; }

        public void OnGet(int quizId, int itemId)
        {
            QuizId = quizId;
            ItemId = itemId;
            var quiz = _userService.FindQuizById(quizId);
            var quizItem = quiz?.Items[itemId - 1];
            Question = quizItem?.Question;
            Answers = new List<string>();
            if (quizItem is not null)
            {
                Answers.AddRange(quizItem?.IncorrectAnswers);
                Answers.Add(quizItem?.CorrectAnswer);
            }
        }

        public IActionResult OnPost()
        {
            var quiz = _userService.FindQuizById(QuizId);
            var quizItem = quiz?.Items[ItemId - 1];

            // Pobierz identyfikator u¿ytkownika z sesji lub innego Ÿród³a.
            int userId = 0; // Przyk³adowe przypisanie, zmieñ to zgodnie z twoj¹ logik¹.

            var userAnswer = new QuizItemUserAnswer(quizItem, userId, QuizId, UserAnswer);
            _userService.SaveUserAnswerForQuiz(QuizId, userId, ItemId, UserAnswer);

            if (ItemId == quiz.Items.Count)
            {
                var answers = _userService.GetUserAnswersForQuiz(QuizId, userId);
                var correctAnswers = answers.Where(answer => answer.IsCorrect());
                return RedirectToPage("Summary", new { correct = correctAnswers.Count(), total = quiz.Items.Count });
            }

            return RedirectToPage("Item", new { quizId = QuizId, itemId = ItemId + 1 });
        }
    }
}
