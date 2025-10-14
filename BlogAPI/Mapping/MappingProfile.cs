using AutoMapper;
using BlogAPI.Models;
using BlogAPI.Models.DTOs.Requests;
using BlogAPI.Models.DTOs.Responses;

namespace BlogAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Entity → ResponseDtos
            CreateMap<Post, PostResponseDtos>()
            .ForMember(d => d.AuthorUsername, o => o.MapFrom(s => s.Author.Username))
            .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name))
            .ForMember(d => d.TagsCount, o => o.MapFrom(s => s.PostTags.Count))
            .ForMember(d => d.LikesCount, o => o.MapFrom(s => s.Likes.Count));

            // RequestDtos → Entity
            CreateMap<PostRequestDtos, Post>()
                .ForMember(d => d.Id, o => o.Ignore())  // o => o.Ignore() → tells AutoMapper: “Don’t try to map this field from the source”.
                .ForMember(d => d.Author, o => o.Ignore())
                .ForMember(d => d.Category, o => o.Ignore())
                .ForMember(d => d.PostTags, o => o.Ignore())
                .ForMember(d => d.Comments, o => o.Ignore())
                .ForMember(d => d.Likes, o => o.Ignore());

            CreateMap<Comment, CommentResponseDtos>()
                .ForMember(d => d.UserUsername, o => o.MapFrom(s => s.User.Username));

            CreateMap<CommentRequestDtos, Comment>()
                .ForMember(d => d.Id, o => o.Ignore()) 
                .ForMember(d => d.User, o => o.Ignore())
                .ForMember(d => d.Post, o => o.Ignore());

            CreateMap<User, RegisterResponseDtos>();

            CreateMap<RegisterRequestDtos, User>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Likes, o => o.Ignore())
                .ForMember(d => d.Posts, o => o.Ignore())
                .ForMember(d => d.Comments, o => o.Ignore());
        }
    }
}
