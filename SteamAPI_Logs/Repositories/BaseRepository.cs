﻿using SteamAPI.Context;
using SteamAPI.Interfaces;

namespace SteamAPI.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly InMemoryContext
            _context;

        public BaseRepository(InMemoryContext inMemoryContext)
        {
            _context = inMemoryContext;
        }

        public Task<int> Delete(int key)
        {
            return Task.Run(() =>
            {
                var entity = _context.Find<T>(key);

                if (entity == null)
                {
                    throw new Exception("ID inexistente.");
                }

                _context.Remove(entity);
                _context.SaveChanges();
                return key;
            });
        }

        public Task<IQueryable<T>> Get(int page, int maxResults)
        {
            return Task.Run(() =>
            {
                var data = _context.Set<T>().AsQueryable().Skip((page-1) * maxResults).Take(maxResults);
                return data.Any() ? data : new List<T>().AsQueryable();
            });
        }

        public Task<T?> GetByKey(int key)
        {
            return Task.Run(() =>
            {
                return _context.Find<T>(key);
            });
        }

        public Task<T> Insert(T entity)
        {
            return Task.Run(() =>
            {
                _context.Add(entity);
                _context.SaveChanges();
                return entity;
            });
        }

        public Task<T> Update(int key, T entity)
        {
            return Task.Run(() =>
            {
                _context.Update(entity);
                _context.SaveChanges();
                return entity;
            });
        }
    }
}
