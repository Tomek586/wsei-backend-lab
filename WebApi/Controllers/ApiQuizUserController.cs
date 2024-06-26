﻿using ApplicationCore.Exeptions;
using BackendLab01;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTO;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("/api/v1/users/quizzes")]
    public class ApiQuizUserController : ControllerBase
    {
        private IQuizUserService _service;

        public ApiQuizUserController(IQuizUserService service)
        {
            _service = service;
        }

        // METODA 1
        [HttpGet]
        [Route("{id}")]
        public ActionResult<QuizDTO> FindQuizById(int id)
        {
            var quiz = _service.FindQuizById(id);

            return quiz is null ? NotFound() : QuizDTO.Of(quiz);
        }

        // METODA 2 (PROBLEM)
        [HttpGet]
        [Route("{quizId}/items/{itemId}/answers")]
        public ActionResult SaveAnswer(int quizId, int itemId, AnswerDTO dto)
        {
            try
            {
                _service.SaveUserAnswerForQuiz(quizId, 1, itemId, dto.Answer);
                // Utwórz odpowiedni adres URL dla nowo utworzonej odpowiedzi
                var locationUri = new Uri($"/{quizId}/items/{itemId}/answers", UriKind.Relative);
                // Przekaz obiekt locationUri do metody Created()
                return Created(locationUri, null);
            }
            catch (DuplicateAnswerException ex)
            {
                return new BadRequestObjectResult(new
                {
                    //Error: e.Message
                });
            }
        }

        // METODA 3
        [HttpGet]
        [Route("{quizId}/answers")]
        public ActionResult<object> ReturnFeedback(int quizId)
        {
            int correct = _service.CountCorrectAnswersForQuizFilledByUser(quizId, 1);

            return new
            {
                CorrectAnswers = correct,
                QuizId = quizId,
                UserId = 1
            };
        }




    }
}