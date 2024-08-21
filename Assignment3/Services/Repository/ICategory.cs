using Assignment3.Models;

namespace Assignment3.Services.Repository;

public interface ICategory
{
    Task<List<EventCategory>> ListCategories();
}