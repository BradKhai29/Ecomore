using DataAccess.DbContexts;
using DataAccess.Entities;
using DataAccess.UnitOfWorks.Base;
using DTOs.Implementation.Categories.Incomings;
using DTOs.Implementation.Categories.Outgoings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;

        public CategoryController(IUnitOfWork<AppDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewCategoryAsync(
            [Required]
            CreateCategoryDto dto,
            CancellationToken cancellationToken)
        {
            var categoryEntity = new CategoryEntity
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
            };

            await _unitOfWork.CategoryRepository.AddAsync(categoryEntity, cancellationToken);

            await _unitOfWork.SaveChangesToDatabaseAsync(cancellationToken);

            return StatusCode(
                statusCode: StatusCodes.Status201Created,
                value: CommonResponse.Success(new GetCategoryByIdDto
                {
                    Id = categoryEntity.Id,
                    Name = dto.Name,
                }));
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateCategoryAsync(
            UpdateCategoryDto updateCategoryDto,
            CancellationToken cancellationToken)
        {
            var foundCategory = await _unitOfWork.CategoryRepository.FindByIdAsync(
                updateCategoryDto.Id,
                asNoTracking: false,
                cancellationToken);

            if (foundCategory == null)
            {
                return StatusCode(
                    statusCode: StatusCodes.Status404NotFound,
                    value: CommonResponse.Failed(messages: CommonResponse.ResourceNotExistedMessage));
            }

            foundCategory.Name = updateCategoryDto.Name;

            _unitOfWork.CategoryRepository.Update(foundCategory);

            await _unitOfWork.SaveChangesToDatabaseAsync(cancellationToken);

            return StatusCode(
                statusCode: StatusCodes.Status200OK,
                value: CommonResponse.Success());
        }

        [HttpDelete("{categoryId:guid}")]
        public async Task<IActionResult> DeleteCategoryAsync(
            Guid categoryId,
            CancellationToken cancellationToken)
        {
            var isExisted = await _unitOfWork.CategoryRepository.IsFoundByExpressionAsync(
                    findExpresison: category => category.Id == categoryId,
                    cancellationToken: cancellationToken);

            if (!isExisted)
            {
                return StatusCode(
                    statusCode: StatusCodes.Status404NotFound,
                    value: CommonResponse.Failed(messages: CommonResponse.ResourceNotExistedMessage));
            }

            _unitOfWork.CategoryRepository.Remove(new CategoryEntity { Id = categoryId });

            await _unitOfWork.SaveChangesToDatabaseAsync(cancellationToken: cancellationToken);

            return StatusCode(
                statusCode: StatusCodes.Status200OK,
                value: CommonResponse.Success());
        }
    }
}
