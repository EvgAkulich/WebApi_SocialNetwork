﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialNetwork_2.Database;
using SocialNetwork_2.Dto.Post;
using SocialNetwork_2.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using SocialNetwork_2.Database.DbEntities;

namespace SocialNetwork_2.Services
{
    public class PostService : IPostService
    {
        private readonly DatabaseContext _dbContext;
        private readonly IMapper _mapper;

        public PostService(DatabaseContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public GetPostDto GetPostById(int id)
        {
            var post = _dbContext.Posts.SingleOrDefault(x => x.Id == id);
            return _mapper.Map<GetPostDto>(post);
        }

        public async Task<GetPostDto> GetPostByIdAsync(int id)
        {
            var post = await _dbContext.Posts.SingleOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<GetPostDto>(post);
        }

        public List<GetPostDto> GetPostsByUserId(int userId)
        {
            var posts = _dbContext.Posts.Where(x => x.ProfileId == userId).ToList();
            return _mapper.Map<List<GetPostDto>>(posts);
        }

        public async Task<List<GetPostDto>> GetPostsByUserIdAsync(int userId)
        {
            var posts = await _dbContext.Posts.Where(x => x.ProfileId == userId).ToListAsync();
            return _mapper.Map<List<GetPostDto>>(posts);
        }

        private static void AddPostValidate(AddPostDto addPostDto)
        {
            if (addPostDto == null)
            {
                throw new ArgumentNullException(nameof(addPostDto));
            }

            if (!addPostDto.Validate())
            {
                throw new InvalidOperationException();
            }
        }

        public GetPostDto AddPost(AddPostDto addPostDto)
        {
            AddPostValidate(addPostDto);

            var post = _mapper.Map<Post>(addPostDto);
            post.Date = DateTime.Now;
            _dbContext.Posts.Add(post);
            _dbContext.SaveChanges();
            return _mapper.Map<GetPostDto>(post);

        }

        public async Task<GetPostDto> AddPostAsync(AddPostDto addPostDto)
        {
            AddPostValidate(addPostDto);

            var post = _mapper.Map<Post>(addPostDto);
            post.Date = DateTime.Now;
            await _dbContext.AddAsync(post);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<GetPostDto>(post);

        }
    }
}
