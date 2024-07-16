using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using MvcMovie.Controllers;
using MvcMovie.Data;
using MvcMovie.Models;
using Xunit;

namespace MvcMovie.Tests
{
    public class MovieControllerTests
    {
        [Fact]
        public async Task Index_ReturnsAViewResult_WithMovieGenreViewModel()
        {
            // Arrange
            var mockContext = new Mock<MvcMovieContext>();
            var mockMovies = new List<Movie>
            {
                new Movie { Title = "Movie1", Genre = "Genre1" },
                new Movie { Title = "Movie2", Genre = "Genre2" },
                new Movie { Title = "Movie3", Genre = "Genre2" }
            }.AsQueryable();

            var mockDbSetMovies = new Mock<DbSet<Movie>>();
            mockDbSetMovies.As<IQueryable<Movie>>().Setup(m => m.Provider).Returns(mockMovies.Provider);
            mockDbSetMovies.As<IQueryable<Movie>>().Setup(m => m.Expression).Returns(mockMovies.Expression);
            mockDbSetMovies.As<IQueryable<Movie>>().Setup(m => m.ElementType).Returns(mockMovies.ElementType);
            mockDbSetMovies.As<IQueryable<Movie>>().Setup(m => m.GetEnumerator()).Returns(mockMovies.GetEnumerator());

            mockContext.Setup(c => c.Movie).ReturnsDbSet(mockDbSetMovies.Object);

            var controller = new MoviesController(mockContext.Object);

            // Act
            var result = await controller.Index(null, null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<MovieGenreViewModel>(viewResult.ViewData.Model);
            Assert.Equal(3, model.Movies.Count());
        }
    }
}