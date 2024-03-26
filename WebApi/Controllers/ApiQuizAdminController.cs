using AutoMapper;
using BackendLab01;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebApi.DTO;

namespace WebApi.Controllers
{

    public class NewQuizDto
    {
        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string? Title { get; set; }
    }

    [ApiController]
    [Route("api/v1/admin/quizzes")]
    public class ApiQuizAdminController : ControllerBase
    {
        private readonly IQuizAdminService _adminService;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;

        public ApiQuizAdminController(IQuizAdminService adminService, LinkGenerator linkGenerator, IMapper mapper)
        {
            _adminService = adminService;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
        }

        //[HttpPost]
        //public ActionResult<object> AddQuiz(LinkGenerator link, NewQuizDto dto)
        //{
        //    var quiz = _adminService.AddQuiz(new NewQuizDto() { Title = dto.Title, });
        //    return Created(
        //        link.GetPathByAction(
        //            HttpContext,
        //            nameof(GetQuiz),         // nazwa metody kontrolera zwracająca quiz
        //            null,                    // kontroler, null oznacza bieżący
        //            new { quiId = quiz.Id }),// parametry ścieżki, id utworzonego quiz
        //        quiz
        //    );
        //}
        //Zakladajac ze nie ma pytan w quizie
        [HttpPost]
        public ActionResult<object> AddQuiz(NewQuizDto dto)
        {
            // Zakładając, że na ten moment lista pytań dla nowego quizu jest pusta
            var quiz = _adminService.AddQuiz(dto.Title, new List<QuizItem>());
            var quizUrl = _linkGenerator.GetPathByAction(HttpContext, nameof(GetQuiz), null, new { quizId = quiz.Id });
            return Created(quizUrl, quiz);
        }

        [HttpGet]
        [Route("{quizId}")]
        public ActionResult<Quiz> GetQuiz(int quizId)
        {
            var quiz = _adminService.FindAllQuizzes().FirstOrDefault(q => q.Id == quizId);
            return quiz is null ? NotFound() : quiz;
        }

        [HttpPatch]
        [Route("{quizId}")]
        [Consumes("application/json-patch+json")]
        public ActionResult<Quiz> AddQuizItem(int quizId, JsonPatchDocument<Quiz>? patchDoc)
        {
            var quiz = _adminService.FindAllQuizzes().FirstOrDefault(q => q.Id == quizId);
            if (quiz is null || patchDoc is null)
            {
                return NotFound(new
                {
                    error = $"Quiz width id {quizId} not found"
                });
            }
            int previousCount = quiz.Items.Count;
            patchDoc.ApplyTo(quiz, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (previousCount < quiz.Items.Count)
            {
                QuizItem item = quiz.Items[^1];
                quiz.Items.RemoveAt(quiz.Items.Count - 1);
                _adminService.AddQuizItemToQuiz(quizId, item);
            }
            return Ok(_adminService.FindAllQuizzes().FirstOrDefault(q => q.Id == quizId));
        }
    }
}