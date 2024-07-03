using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Interfaces;
using Server.Application.Validations;
using Server.Contracts.Abstractions.Course;
using Server.Contracts.Abstractions.Shared;
using Server.Contracts.DTOs.Course;

namespace Server.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CourseController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ICourseService _courseService;

    public CourseController(IMapper mapper, ICourseService courseService)
    {
        _mapper = mapper;
        _courseService = courseService;
    }

    [HttpPost("uploadcourse")]
    [ProducesResponseType(200, Type = typeof(Result<object>))]
    [ProducesResponseType(400, Type = typeof(Result<object>))]
    public async Task<IActionResult> UploadCourse([FromForm] CreateCourseRequest req)
    {
        var validator = new CreateCourseRequestValidator();
        var validatorResult = validator.Validate(req);
        if (validatorResult.IsValid == false)
        {
            return BadRequest(new Result<object>
            {
                Error = 1,
                Message = "Missing value!",
                Data = validatorResult.Errors.Select(x => x.ErrorMessage),
            });
        }

        var courseMapper = _mapper.Map<CreateCourseDTO>(req);
        var result = await _courseService.UploadCourse(courseMapper);

        return Ok(result);
    }

    [HttpPost("uploadcoursecontent")]
    [ProducesResponseType(200, Type = typeof(Result<object>))]
    [ProducesResponseType(400, Type = typeof(Result<object>))]
    public async Task<IActionResult> UploadCourseContent([FromForm] CreateCourseContentRequest req)
    {
        var validator = new CreateCourseRequestContentValidator();
        var validatorResult = validator.Validate(req);
        if (validatorResult.IsValid == false)
        {
            return BadRequest(new Result<object>
            {
                Error = 1,
                Message = "Missing value!",
                Data = validatorResult.Errors.Select(x => x.ErrorMessage),
            });
        }

        var courseContentMapper = _mapper.Map<CreateCourseContentDTO>(req);
        var result = await _courseService.UploadCourseContent(courseContentMapper);

        return Ok(result);
    }
}
