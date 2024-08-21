using Assignment3.Models;
using Assignment3.Services.Repository;
using Microsoft.EntityFrameworkCore;

namespace Assignment3.Services.Implement;

public class CategoryService : ICategory
{
    private PRN221_BL5_Assignment3Context db;

    public CategoryService(PRN221_BL5_Assignment3Context db)
    {
        this.db = db;
    }

    public async Task<List<EventCategory>> ListCategories()
    {
        return await db.EventCategories.ToListAsync();
    }
}