using AutoMapper;
using MediatR;
using MovieTracker.Infrastructure.Interfaces;
using MovieTracker.Models.ViewModels;

namespace MovieTracker.Features.Comments;

public record GetCommentsQuery(int MovieId) : IRequest<IEnumerable<CommentVm>>;

public class GetCommentsHandler : IRequestHandler<GetCommentsQuery, IEnumerable<CommentVm>>
{
    private readonly ICommentsRepository _commentsRepository;
    private readonly IMapper _mapper;

    public GetCommentsHandler(ICommentsRepository commentsRepository, IMapper mapper)
    {
        _commentsRepository = commentsRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CommentVm>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
    {
        var comments = await _commentsRepository.GetAllCommentsForMovieAsync(request.MovieId);
        return _mapper.Map<IEnumerable<CommentVm>>(comments);
    }
}