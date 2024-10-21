using Entity.Animal;
using Repository.Base;
namespace Repository.ZooEntityFrameworkRepository;
using Data.ZooDbContext;
public class ZooEntityFrameworkRepository : BaseZooRepository
{
    public override async Task<bool> AddAnimalAsync(Animal animal){
        await Task.Run(()=>{
            var dbcontext = new ZooDbContext();
            new ZooDbContext().Database.EnsureCreated();

            dbcontext.Animals.Add(animal);

            
            return dbcontext.SaveChanges();
        });
        return false;
    }

    public override async Task<bool> DeleteAnimalAsync(int id)
    {
        await Task.Run(()=>{
            var dbcontext = new ZooDbContext();
            new ZooDbContext().Database.EnsureCreated();
            var newAnimal = new Animal{
                Id = 3,
                Name = "Unknown",
                Class = "Unkown",
                IsMale = false
            };
            dbcontext.Animals.Remove(newAnimal);

            
            return dbcontext.SaveChanges();
        });
        return false;
    }

    public override async Task<IEnumerable<Animal>> GetAllAnimalsAsync()
    {
        await Task.Run(()=>{
            var dbcontext = new ZooDbContext();
            new ZooDbContext().Database.EnsureCreated();
            var allAnimals = dbcontext.Animals.ToList();
            
            
        });
        return default;
    }

    public override async Task<Animal> GetAnimalByIdAsync(int id)
    {
        await Task.Run(()=>{
            var dbcontext = new ZooDbContext();
            new ZooDbContext().Database.EnsureCreated();
            var animalbyId = dbcontext.Animals.Where(animal => animal.Id ==id);
            return animalbyId;
        });
        return default;
    }

    public override async Task<bool> UpdateAnimalAsync(Animal animal)
    {
        await Task.Run(()=>{
            var dbcontext = new ZooDbContext();
            new ZooDbContext().Database.EnsureCreated();
            var foundAnimal = dbcontext.Animals.FirstOrDefault(anm => anm.Id == animal.Id);
            foundAnimal.Name = animal.Name;
            foundAnimal.Class = animal.Class;
            foundAnimal.IsMale = animal.IsMale;
            dbcontext.Animals.Update(foundAnimal);

            
            return dbcontext.SaveChanges();
        });
        return false;
    }
}