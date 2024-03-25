using ApplicationCore.Interfaces.Repository;
using BackendLab01;

namespace Infrastructure.Memory;
public static class SeedData
{
    public static void Seed(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var provider = scope.ServiceProvider;
            var quizRepo = provider.GetService<IGenericRepository<Quiz, int>>();
            var quizItemRepo = provider.GetService<IGenericRepository<QuizItem, int>>();

            var question1 = new QuizItem(1, "Co jest stolicą Polski?",
                new List<string>() { "Kraków", "Kielce", "Sopot" }, "Warszawa");
            var question2 = new QuizItem(2, "Ile to jest 2+2*2",
                new List<string>() { "8", "2", "3" }, "6");
            var question3 = new QuizItem(3, "Co jest stolicą Albanii?",
                new List<string>() { "Dobczyce", "Split", "Rzym" }, "Tirana");

            var quiz1 = new Quiz(1, new List<QuizItem>() { question1, question2, question3 }, "quiz1");



            var question4 = new QuizItem(4, "Stolicą Austrii jest:",
                new List<string>() { "Australia", "Wrocław", "Warszawa" }, "Wiedeń");
            var question5 = new QuizItem(5, "W którym roku był chrzest Polski?",
                new List<string>() { "2014", "1025", "1234" }, "966");
            var question6 = new QuizItem(6, "Bitwa pod Grunwaldem była w roku ...",
                new List<string>() { "1990", "2115", "966" }, "1410");
            var quiz2 = new Quiz(2, new List<QuizItem>() { question4, question5, question6 }, "quiz2");

            quizItemRepo.Add(question4);
            quizItemRepo.Add(question5);
            quizItemRepo.Add(question6);
            quizItemRepo.Add(question1);
            quizItemRepo.Add(question2);
            quizItemRepo.Add(question3);



            quizRepo.Add(quiz1);
            quizRepo.Add(quiz2);
        }
    }
}