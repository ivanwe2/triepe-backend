using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triepe.Domain.Abstractions.Providers;
using Triepe.Domain.Abstractions.Repositories;
using Triepe.Domain.Abstractions.Services;
using Triepe.Domain.Dtos.PictureDtos;
using Triepe.Domain.Exceptions;
using Triepe.Domain.Pagination;
using Triepe.Domain.RequestForms;
using Xunit;

namespace Triepe.Services.Test
{
    public class PictureServiceTests
    {
        public IPictureService _pictureService;
        public Mock<IPictureRepository> _pictureRepoMock;
        public Mock<IProductRepository> _productRepoMock;
        public Mock<IValidationProvider> _validationProviderMock;

        public Fixture fixture;

        public PictureCreateRequestForm validCreateForm, invalidCreateForm;
        public PictureUpdateRequestForm validUpdateForm, invalidUpdateForm;
        public PictureRequestDto validDto;

        public PictureServiceTests()
        {
            _pictureRepoMock = new Mock<IPictureRepository>();
            _pictureRepoMock
                .Setup(s => s.CreateAsync<PictureRequestDto, PictureResponseDto>(It.IsAny<PictureRequestDto>()))
                .ReturnsAsync(new PictureResponseDto());
            _pictureRepoMock
                .Setup(s => s.DeleteAsync(It.IsAny<Guid>()));
            _pictureRepoMock
                .Setup(s => s.GetByIdAsync<PictureResponseDto>(It.IsAny<Guid>()))
                .ReturnsAsync(new PictureResponseDto());
            _pictureRepoMock
                .Setup(s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<PictureRequestDto>()));

            _productRepoMock = new Mock<IProductRepository>();
            _productRepoMock
                .Setup(s => s.HasAnyAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            _validationProviderMock = new Mock<IValidationProvider>();

            _pictureService = new PictureService(_pictureRepoMock.Object, _validationProviderMock.Object,
                _productRepoMock.Object);

            fixture = new Fixture();

            var fileName = "test.pdf";
            var stream = new MemoryStream();

            var file = new FormFile(stream, 0, stream.Length, null, fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/pdf"
            };

            validCreateForm = new()
            {
                FileName = fileName,
                ProductId = new Guid(),
                File = file
            };

            validDto = new()
            {
                Size = validCreateForm.File.Length,
                Description = validCreateForm.FileName,
                ProductId = validCreateForm.ProductId,
                FileExtension = validCreateForm.File.ContentType,
                Bytes = new byte[10]
            };
            invalidCreateForm = new()
            {
                FileName = fileName,
                ProductId = default,
                File = null
            };
            validUpdateForm = new()
            {
                File = file
            };
            invalidUpdateForm = new()
            {
                File = null
            };
        }

        [Fact]
        public async Task CreateAsync_ValidData_ExpectedNotNull()
        {
            // Arrange
            var expected = new PictureResponseDto();
            // Act
            var result = await _pictureService.CreateAsync(validCreateForm);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expected, result);
            _pictureRepoMock.Verify(r => r.CreateAsync<PictureRequestDto, PictureResponseDto>(It.IsAny<PictureRequestDto>()), Times.Once());
        }

        [Fact]
        public async Task CreateAsync_InvalidData_ExpectedValidationException()
        {
            // Arrange
            _validationProviderMock
                .Setup(s => s.TryValidate(It.IsAny<PictureRequestDto>()))
                .Throws(new Domain.Exceptions.ValidationException(""));

            // Act
            async Task a() => await _pictureService.CreateAsync(validCreateForm);

            // Assert
            await Assert.ThrowsAsync<Domain.Exceptions.ValidationException>(a);
        }

        [Fact]
        public async Task CreateAsync_InvalidData_ProductIdDoesntExist_ExpectedNotFoundException()
        {
            // Arrange
            _productRepoMock
                .Setup(s => s.HasAnyAsync(It.IsAny<Guid>()))
                .ReturnsAsync(false);

            // Act
            async Task a() => await _pictureService.CreateAsync(invalidCreateForm);

            // Assert
            await Assert.ThrowsAsync<Domain.Exceptions.NotFoundException>(a);
        }

        [Fact]
        public async Task GetByIdAsync_ValidData_ExpectedNotNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expected = new PictureResponseDto() { Id = id };

            _pictureRepoMock
                .Setup(s => s.GetByIdAsync<PictureResponseDto>(It.IsAny<Guid>()))
                .ReturnsAsync(new PictureResponseDto() { Id = id });

            _pictureService = new PictureService(_pictureRepoMock.Object,
                _validationProviderMock.Object, null);

            // Act
            var result = await _pictureService.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expected, result);
            _pictureRepoMock.Verify(r => r.GetByIdAsync<PictureResponseDto>(id), Times.Once());
        }

        [Fact]
        public async Task GetByIdAsync_InvalidPictureId_ExpectedException()
        {
            // Act
            async Task a() => await _pictureService.GetByIdAsync(default(Guid));

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(a);
        }

        [Fact]
        public async Task GetAllAsync_ValidParameters_ExpectedEmptyCollection()
        {
            // Arrange
            var page = new PaginatedResult<PictureResponseDto>(new List<PictureResponseDto>(), 0, 0, 0);

            _pictureRepoMock
                .Setup(s => s.GetPageAsync<PictureResponseDto>(It.IsAny<int>(), It.IsAny<int>(), null))
                .ReturnsAsync(page);

            // Act
            var activities = await _pictureService.GetPaginatedResultAsync(new(), null);

            // Assert
            Assert.NotNull(activities);
            Assert.Empty(activities.Content);
            _pictureRepoMock.Verify(r => r.GetPageAsync<PictureResponseDto>(1, 10, null), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_ValidData_ExpectedInvokingRepositoryUpdateAsyncMethodOnce()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            await _pictureService.UpdateAsync(id, validUpdateForm);

            // Assert
            _pictureRepoMock.Verify(r => r.UpdateAsync<PictureRequestDto>(id, It.IsAny<PictureRequestDto>()), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_InvalidPictureId_ExpectedException()
        {
            // Act
            async Task a() => await _pictureService.UpdateAsync(default(Guid), new PictureUpdateRequestForm());

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(a);
        }

        [Fact]
        public async Task DeleteAsync_ValidData_ExpectedInvokingRepsoitoryDeleteAsyncMethodOnce()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            await _pictureService.DeleteAsync(id);

            // Assert
            _pictureRepoMock.Verify(r => r.DeleteAsync(id), Times.Once());
        }

        [Fact]
        public async Task DeleteAsync_InvalidPictureId_ExpectedException()
        {
            // Act
            async Task a() => await _pictureService.DeleteAsync(default(Guid));

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(a);
        }

    }
}
