using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using Server.Application.Interfaces;
using Server.Contracts.Abstractions.Shared;
using Server.Contracts.DTOs.Course;
using Server.Contracts.Settings;
using Server.Domain.Entities;

namespace Server.Application.Services;

public class CourseService : ICourseService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CloudinarySetting _cloudinarySetting;
    private readonly Cloudinary _cloudinary;
    public CourseService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<CloudinarySetting> cloudinaryConfig)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        //_cloudinarySetting = cloudinarySetting;

        var account = new Account(cloudinaryConfig.Value.CloudName,
            cloudinaryConfig.Value.ApiKey,
            cloudinaryConfig.Value.ApiSecret);

        _cloudinary = new Cloudinary(account);
        _cloudinarySetting = cloudinaryConfig.Value;
    }

    public async Task<Result<object>> UploadCourse(CreateCourseDTO courseDto)
    {
        string fileExtension = Path.GetExtension(courseDto.ThumbNail.FileName);
        string newFileName = $"{courseDto.Title}_{courseDto.Id}_{fileExtension}";
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(newFileName, courseDto.ThumbNail.OpenReadStream()),
            Folder = _cloudinarySetting.Folder,
        };
        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        if(uploadResult?.StatusCode != System.Net.HttpStatusCode.OK)
        {
            return new Result<object>
            {
                Error = 1,
                Message = "Course creation failed, please try again!",
                Data = null
            };
        }

        var imageUrl = uploadResult.Uri.AbsoluteUri;
        var publicId = uploadResult.PublicId;

        courseDto.ThumbNailUrl = imageUrl;
        courseDto.ThumbNailId = publicId;

        var courseMapper = _mapper.Map<Course>(courseDto);

        await _unitOfWork.courseRepository.AddAsync(courseMapper);
        var result = await _unitOfWork.SaveChangeAsync();

        return new Result<object>
        {
            Error = result > 0 ? 0 : 1,
            Message = result > 0 ? "Create course successfully" : "Create course fail",
            Data = uploadResult
        };
    }

    public async Task<Result<object>> UploadCourseContent(CreateCourseContentDTO courseContentDto)
    {
        string fileExtension = Path.GetExtension(courseContentDto.Video.FileName);
        string newFileName = $"{courseContentDto.Title}_{courseContentDto.Id}_{fileExtension}";
        var uploadParams = new VideoUploadParams
        {
            File = new FileDescription(newFileName, courseContentDto.Video.OpenReadStream()),
            Folder = _cloudinarySetting.Folder,
        };
        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        if (uploadResult?.StatusCode != System.Net.HttpStatusCode.OK)
        {
            return new Result<object>
            {
                Error = 1,
                Message = "Course creation failed, please try again!",
                Data = null
            };
        }


        var videoUrl = uploadResult.Uri.AbsoluteUri;
        var publicId = uploadResult.PublicId;

        courseContentDto.ContentUrl = videoUrl;
        courseContentDto.ContentId = publicId;
        courseContentDto.ContentTime = "";

        var courseContentMapper = _mapper.Map<CourseContent>(courseContentDto);

        await _unitOfWork.courseContentRepository.AddAsync(courseContentMapper);
        var result = await _unitOfWork.SaveChangeAsync();

        return new Result<object>
        {
            Error = result > 0 ? 0 : 1,
            Message = result > 0 ? "Create course content successfully" : "Create course content fail",
            Data = courseContentDto
        };
    }
}
