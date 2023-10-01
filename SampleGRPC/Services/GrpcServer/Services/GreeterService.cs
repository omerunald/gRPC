using Grpc.Core;
using GrpcServer.DataLibrary;
using Microsoft.Extensions.Logging;
using MyGRPC;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrpcServer.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }

        //public override async Task<List<MovieResponseModel>> GetMoviesList(MoviewRequestModel request, ServerCallContext context)
        //{
        //    MoviesDataLibrary moviesDataLibrary = new MoviesDataLibrary();
        //    var liste = moviesDataLibrary.GetList();
        //    List<MovieResponseModel> responseModels = new List<MovieResponseModel>();

        //    foreach (var item in liste)
        //    {
        //        var movieResponse = new MovieResponseModel
        //        {
        //            CategoryId = item.CategoryId,
        //            Code = item.Code,
        //            Description = item.Description,
        //            Id = item.Id,
        //            Rating = item.Rating,

        //        };
        //        responseModels.Add(movieResponse);
        //    }

        //    return responseModels;
        //}
    }
}