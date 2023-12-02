using AutoMapper;
using MediatR;
using MovieTracker.Infrastructure.Interfaces;
using MovieTracker.Models.ViewModels;

namespace MovieTracker.Features.Comments;

public record GetCommentQuery(int Id) : IRequest<CommentVm>;

public class GetCommentHandler : IRequestHandler<GetCommentQuery, CommentVm>
{
    private readonly ICommentsRepository _commentsRepository;
    private readonly IMapper _mapper;

    public GetCommentHandler(ICommentsRepository commentsRepository, IMapper mapper)
    {
        _commentsRepository = commentsRepository;
        _mapper = mapper;
    }

    public async Task<CommentVm> Handle(GetCommentQuery request, CancellationToken cancellationToken)
    {
        var comment = await _commentsRepository.GetByIdAsync(request.Id);
        return _mapper.Map<CommentVm>(comment);
    }
}