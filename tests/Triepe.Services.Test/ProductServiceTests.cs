using AutoFixture;
using Moq;
using System.ComponentModel.DataAnnotations;
using Triepe.Domain.Abstractions.Providers;
using Triepe.Domain.Abstractions.Repositories;
using Triepe.Domain.Abstractions.Services;
using Triepe.Domain.Dtos.ProductDtos;
using Triepe.Domain.Pagination;
using Triepe.Domain.Providers;
using Xunit;

namespace Triepe.Services.Test
{
    public class ProductServiceTests
    {
        public IProductService _productService;
        public Mock<IProductRepository> _repoMock;
        public Mock<IValidationProvider> _validationProviderMock;

        public Fixture fixture;

        public ProductServiceTests()
        {
            _repoMock = new Mock<IProductRepository>();

            _repoMock
                .Setup(s => s.CreateAsync<ProductRequestDto, ProductResponseDto>(It.IsAny<ProductRequestDto>()))
                .ReturnsAsync(new ProductResponseDto());
            _repoMock
                .Setup(s => s.DeleteAsync(It.IsAny<Guid>()));
            _repoMock
                .Setup(s => s.GetByIdAsync<ProductResponseDto>(It.IsAny<Guid>()))
                .ReturnsAsync(new ProductResponseDto());
            _repoMock
                .Setup(s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ProductRequestDto>()));

            _validationProviderMock = new Mock<IValidationProvider>();

            _productService = new ProductService(_repoMock.Object, _validationProviderMock.Object);

            fixture = new Fixture();
        }

        [Fact]
        public async Task CreateAsync_ValidData_ExpectedNotNull()
        {
            // Arrange
            var validDto = fixture.Create<ProductRequestDto>();
            var expected = new ProductResponseDto();

            // Act
            var result = await _productService.CreateAsync(validDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expected, result);
            _repoMock.Verify(r => r.CreateAsync<ProductRequestDto, ProductResponseDto>(validDto), Times.Once());
        }

        [Fact]
        public async Task CreateAsync_InvalidData_ExpectedException()
        {
            // Arrange
            var invalidDto = new ProductRequestDto()
            {
                Name = ""
            };

            _validationProviderMock
                .Setup(s => s.TryValidate(It.IsAny<ProductRequestDto>()))
                .Throws(new Domain.Exceptions.ValidationException(""));

            // Act
            async Task a() => await _productService.CreateAsync(invalidDto);

            // Assert
            await Assert.ThrowsAsync<Domain.Exceptions.ValidationException>(a);
        }

        [Fact]
        public async Task GetByIdAsync_ValidData_ExpectedNotNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expected = new ProductResponseDto() { Id = id };

            _repoMock
                .Setup(s => s.GetByIdAsync<ProductResponseDto>(It.IsAny<Guid>()))
                .ReturnsAsync(new ProductResponseDto() { Id = id });

            _productService = new ProductService(_repoMock.Object,
                _validationProviderMock.Object);

            // Act
            var result = await _productService.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expected, result);
            _repoMock.Verify(r => r.GetByIdAsync<ProductResponseDto>(id), Times.Once());
        }

        [Fact]
        public async Task GetByIdAsync_InvalidProductId_ExpectedException()
        {
            // Act
            async Task a() => await _productService.GetByIdAsync(default(Guid));

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(a);
        }

        [Fact]
        public async Task GetAllAsync_ValidParameters_ExpectedEmptyCollection()
        {
            // Arrange
            var page = new PaginatedResult<ProductResponseDto>(new List<ProductResponseDto>(), 0, 0, 0);

            _repoMock
                .Setup(s => s.GetPageAsync<ProductResponseDto>(It.IsAny<int>(), It.IsAny<int>(), null))
                .ReturnsAsync(page);

            // Act
            var activities = await _productService.GetPaginatedResultAsync(new(), null);

            // Assert
            Assert.NotNull(activities);
            Assert.Empty(activities.Content);
            _repoMock.Verify(r => r.GetPageAsync<ProductResponseDto>(1, 10, null), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_ValidData_ExpectedInvokingRepositoryUpdateAsyncMethodOnce()
        {
            // Arrange
            var id = Guid.NewGuid();
            var validDto = fixture.Create<ProductRequestDto>();
            // Act
            await _productService.UpdateAsync(id, validDto);

            // Assert
            _repoMock.Verify(r => r.UpdateAsync<ProductRequestDto>(id, validDto), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_InvalidProductId_ExpectedException()
        {
            // Act
            async Task a() => await _productService.UpdateAsync(default(Guid), new ProductRequestDto());

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(a);
        }

        [Fact]
        public async Task DeleteAsync_ValidData_ExpectedInvokingRepsoitoryDeleteAsyncMethodOnce()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            await _productService.DeleteAsync(id);

            // Assert
            _repoMock.Verify(r => r.DeleteAsync(id), Times.Once());
        }

        [Fact]
        public async Task DeleteAsync_InvalidProductId_ExpectedException()
        {
            // Act
            async Task a() => await _productService.DeleteAsync(default(Guid));

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(a);
        }

    }
}