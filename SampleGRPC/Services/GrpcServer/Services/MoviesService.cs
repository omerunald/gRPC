using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcServer.DataLibrary;
using Microsoft.Extensions.Logging;
using MyGRPC;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrpcServer.Services
{
    public class MoviesService : Movies.MoviesBase
    {
        private readonly ILogger<MoviesService> _logger;

        public MoviesService(ILogger<MoviesService> logger)
        {
            _logger = logger;
        }

        public override Task<MovieResponseModel> GetMoviesById(MoviewRequestModel request, ServerCallContext context)
        {
            MoviesDataLibrary moviesDataLibrary = new MoviesDataLibrary();
            var dataModel = moviesDataLibrary.GetMovieById(request.Id);

            return Task.FromResult(dataModel);
        }

        public override async Task GetMoviesFirst(Empty request, IServerStreamWriter<MovieResponseModel> responseStream, ServerCallContext context)
        {
            MoviesDataLibrary moviesDataLibrary = new MoviesDataLibrary();
            foreach (var item in moviesDataLibrary.GetList())
            {
                await responseStream.WriteAsync(item);
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }


        public override async Task GetMoviesRangeList(Empty request, IServerStreamWriter<MovieListModel> responseStream, ServerCallContext context)
        {
            MoviesDataLibrary moviesDataLibrary = new MoviesDataLibrary();
            MovieListModel moviesListModel = new MovieListModel();
            moviesListModel.Movies.AddRange(moviesDataLibrary.GetList());

            await responseStream.WriteAsync(moviesListModel);
        }



        public override async Task GetMoviesList(Empty empty, IServerStreamWriter<MovieResponseModel> responseStream, ServerCallContext context)
        {
            MoviesDataLibrary moviesDataLibrary = new MoviesDataLibrary();
            var liste = moviesDataLibrary.GetList();

            foreach (var item in liste)
            {
                var movieResponse = new MovieResponseModel
                {
                    CategoryId = item.CategoryId,
                    Code = item.Code,
                    Description = item.Description,
                    Id = item.Id,
                    Rating = item.Rating,

                };
                await responseStream.WriteAsync(movieResponse);

            }
        }


        public override async Task GetMovies(Empty request, IServerStreamWriter<MovieListModel> responseStream, ServerCallContext context)
        {
            MoviesDataLibrary moviesDataLibrary = new MoviesDataLibrary();

            MovieListModel os = new MovieListModel();
            os.Movies.AddRange(moviesDataLibrary.GetList());

            await responseStream.WriteAsync(os);
        }

        public override async Task<MovieListModel> SetMovies(IAsyncStreamReader<MoviewRequestModel> requestStream, ServerCallContext context)
        {
            MoviesDataLibrary moviesDataLibrary = new MoviesDataLibrary();

            var response = new MovieListModel();
            await foreach (var item in requestStream.ReadAllAsync())
            {
                var dataModel = moviesDataLibrary.GetMovieById(item.Id);
                if (dataModel != null)
                    response.Movies.Add(dataModel);
            }

            return response;
        }

        public override async Task SetGetMovies(IAsyncStreamReader<MoviewRequestModel> requestStream, IServerStreamWriter<MovieResponseModel> responseStream, ServerCallContext context)
        {
            MoviesDataLibrary moviesDataLibrary = new MoviesDataLibrary();

            await foreach (var item in requestStream.ReadAllAsync())
            {
                Console.WriteLine(requestStream.Current.Id + " - Client >> Server");

                var dataModel = moviesDataLibrary.GetMovieById(item.Id);
                if (dataModel != null)
                    await responseStream.WriteAsync(dataModel);

                string msg = dataModel != null ? dataModel.Code : "Data Bulunamadı";

                Console.WriteLine(msg + " - Server >> Client");
            }
        }
    }
}