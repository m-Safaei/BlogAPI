using BlogAPI.Core.Domain.Entities;
using BlogAPI.Core.DTO.Blog;

namespace BlogAPI.Core.Mappers;

public static class BlogMappers
    {
        public static BlogDto ToBlogDto(this Blog blog)
        {
            return new BlogDto()
            {
                Id = blog.Id,
                Title = blog.Title,
                Content = blog.Content,
                CreateDate = blog.CreateDate,
            };
        }

        public static Blog ToBlog(this CreateBlogRequestDto createBlogRequestDto)
        {
            return new Blog()
            {
                Title = createBlogRequestDto.Title,
                Content = createBlogRequestDto.Content,
            };
        }
    }

