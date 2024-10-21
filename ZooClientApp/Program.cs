using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

HttpClient client = new HttpClient();
string baseUrl = "http://localhost:7373/animals";

bool flag = true;

while (flag)
{
    Console.WriteLine("\nChoose an operation:");
    Console.WriteLine("1 - Get all animals");
    Console.WriteLine("2 - Get animal by ID");
    Console.WriteLine("3 - Add a new animal");
    Console.WriteLine("4 - Update an animal");
    Console.WriteLine("5 - Delete an animal");
    Console.WriteLine("6 - Exit");

    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            await GetAllAnimals();
            break;
        case "2":
            Console.Write("Enter animal ID: ");
            if (int.TryParse(Console.ReadLine(), out int getId))
            {
                await GetAnimalById(getId);
            }
            else
            {
                Console.WriteLine("Invalid ID input.");
            }
            break;
        case "3":
            Console.Write("Enter animal name: ");
            string name = Console.ReadLine();
            Console.Write("Enter animal class: ");
            string animalClass = Console.ReadLine();
            Console.Write("Is the animal male? (true/false): ");
            bool isMale = bool.Parse(Console.ReadLine());

            await AddAnimal(new Animal { Name = name, Class = animalClass, IsMale = isMale });
            break;
        case "4":
            Console.Write("Enter ID of the animal to update: ");
            if (int.TryParse(Console.ReadLine(), out int updateId))
            {
                Console.Write("Enter new animal name: ");
                string updatedName = Console.ReadLine();
                Console.Write("Enter new animal class: ");
                string updatedClass = Console.ReadLine();
                Console.Write("Is the animal male? (true/false): ");
                bool updatedIsMale = bool.Parse(Console.ReadLine());

                await UpdateAnimal(updateId, new Animal { Name = updatedName, Class = updatedClass, IsMale = updatedIsMale });
            }
            else
            {
                Console.WriteLine("Invalid ID input.");
            }
            break;
        case "5":
            Console.Write("Enter ID of the animal to delete: ");
            if (int.TryParse(Console.ReadLine(), out int deleteId))
            {
                await DeleteAnimal(deleteId);
            }
            else
            {
                Console.WriteLine("Invalid ID input.");
            }
            break;
        case "6":
            flag = false;
            Console.WriteLine("Exiting...");
            break;
        default:
            Console.WriteLine("Invalid command, please try again.");
            break;
    }
}

async Task GetAllAnimals()
{
    var response = await client.GetAsync(baseUrl);

    if (response.IsSuccessStatusCode)
    {
        var animals = await response.Content.ReadFromJsonAsync<IEnumerable<Animal>>();
        if (animals != null)
        {
            Console.WriteLine("Animals:");
            foreach (var animal in animals)
            {
                Console.WriteLine(animal.ToString());
            }
        }
        else
        {
            Console.WriteLine("No animals found.");
        }
    }
    else
    {
        string responseBody = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Error fetching animals: {response.StatusCode}");
        Console.WriteLine($"Server response: {responseBody}");
    }
}

async Task GetAnimalById(int id)
{
    var response = await client.GetAsync($"{baseUrl}/{id}");

    if (response.IsSuccessStatusCode)
    {
        var animal = await response.Content.ReadFromJsonAsync<Animal>();
        if (animal != null)
        {
            Console.WriteLine(animal.ToString());
        }
        else
        {
            Console.WriteLine($"Animal with ID {id} not found.");
        }
    }
    else
    {
        string responseBody = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Error fetching animal by ID {id}: {response.StatusCode}");
        Console.WriteLine($"Server response: {responseBody}");
    }
}

async Task AddAnimal(Animal animal)
{
    var response = await client.PostAsJsonAsync(baseUrl, animal);

    if (response.IsSuccessStatusCode)
    {
        string responseBody = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Animal successfully added: {response.StatusCode}");
        Console.WriteLine($"Server response: {responseBody}");
    }
    else
    {
        string responseBody = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Error adding animal: {response.StatusCode}");
        Console.WriteLine($"Server response: {responseBody}");
    }
}

async Task UpdateAnimal(int id, Animal animal)
{
    var response = await client.PutAsJsonAsync($"{baseUrl}/{id}", animal);

    if (response.IsSuccessStatusCode)
    {
        string responseBody = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Animal successfully updated: {response.StatusCode}");
        Console.WriteLine($"Server response: {responseBody}");
    }
    else
    {
        string responseBody = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Error updating animal: {response.StatusCode}");
        Console.WriteLine($"Server response: {responseBody}");
    }
}

async Task DeleteAnimal(int id)
{
    var response = await client.DeleteAsync($"{baseUrl}/{id}");

    if (response.IsSuccessStatusCode)
    {
        string responseBody = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Animal successfully deleted: {response.StatusCode}");
        Console.WriteLine($"Server response: {responseBody}");
    }
    else
    {
        string responseBody = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Error deleting animal: {response.StatusCode}");
        Console.WriteLine($"Server response: {responseBody}");
    }
}

public class Animal
{
    public int Id { get; set; }
    public required string? Name { get; set; }
    public required string? Class { get; set; }
    public bool IsMale { get; set; }

    override public string ToString()
    {
        string gender = IsMale ? "Male" : "Female";
        return $"Id: \"{Id}\" Name: \"{Name}\" Class: \"{Class}\" Gender: \"{gender}\"";
    }
}