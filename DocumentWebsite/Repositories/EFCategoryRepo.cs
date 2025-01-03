﻿using DocumentWebsite.Data;
using DocumentWebsite.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentWebsite.Repositories
{
    public class EFCategoryRepo:ICategoryRepo
    {
        private readonly ApplicationDbContext _context;

        public EFCategoryRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }
     

        public async Task AddAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.Include(c => c.Documents).FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                throw new Exception("Category not found");
            }

            foreach (var product in category.Documents)
            {
                _context.Documents.Remove(product);
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}