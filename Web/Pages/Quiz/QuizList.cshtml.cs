using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BackendLab01.Pages;

public class QuizList : PageModel
{
    public List<BackendLab01.Quiz> QuizLista { get; set; }

    private readonly IQuizUserService quizUserService;

    public QuizList(IQuizUserService quizUserService)
    {
        this.quizUserService = quizUserService;
    }

    public void OnGet()
    {
        QuizLista = quizUserService.GetAllQuiz();
    }
}