namespace Repository.Base;

using Entity.Animal;
using Microsoft.Identity.Client;

public abstract class BaseZooRepository{
    public abstract Task<Animal> GetAnimalByIdAsync(int id);
    public abstract Task<IEnumerable<Animal>> GetAllAnimalsAsync();
    public abstract Task<bool> AddAnimalAsync(Animal animal);
    public abstract Task<bool> UpdateAnimalAsync(Animal animal);
    public abstract Task<bool> DeleteAnimalAsync(int id);
}