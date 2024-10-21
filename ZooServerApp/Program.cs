using System.Net;
using System.Text.Json;
using Entity.Animal;
using Repository.ZooEntityFrameworkRepository;

const int port = 7070;
HttpListener httpListener = new HttpListener();

httpListener.Prefixes.Add($"http://*:{port}/");
httpListener.Start();

Console.WriteLine($"HTTP Server started on 'http://localhost:{port}/'");


var repository = new ZooEntityFrameworkRepository();

while (true)
{
    try
    {
        var context = await httpListener.GetContextAsync();

        // Use using statements to ensure streams are properly disposed
        var reader = new StreamReader(context.Request.InputStream);
        var writer = new StreamWriter(context.Response.OutputStream);
        
        var requestBodyStr = await reader.ReadToEndAsync();

            Console.WriteLine(context.Request.RawUrl);
            context.Response.ContentType = "application/json";
            var methodType = context.Request.HttpMethod;
            var normalizedRawUrl = context.Request.RawUrl?.Trim().ToLower() ?? "/";
            var rawUrlItems = normalizedRawUrl.Split("/", StringSplitOptions.RemoveEmptyEntries);

            if (rawUrlItems.Length == 0)
            {
                await writer.WriteLineAsync("Welcome to Zoo API!");
                await writer.FlushAsync();
                context.Response.StatusCode = (int)HttpStatusCode.OK;
            }
            else if (rawUrlItems[0] == "animals")
            {
                switch (methodType)
                {
                case "GET":
    if (rawUrlItems.Length == 2 && int.TryParse(rawUrlItems[1], out int animalId))
    {
        var animal = await repository.GetAnimalByIdAsync(animalId);
        if (animal != null)
        {
            var jsonResponse = JsonSerializer.Serialize(animal);
            await writer.WriteLineAsync(jsonResponse);  // Send the animal to the client
            context.Response.StatusCode = (int)HttpStatusCode.OK;
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
        }
    }
    else
    {
        var allAnimals = await repository.GetAllAnimalsAsync();
        var jsonResponse = JsonSerializer.Serialize(allAnimals);
        await writer.WriteLineAsync(jsonResponse);  // Send all animals to the client
        context.Response.StatusCode = (int)HttpStatusCode.OK;
    }
    await writer.FlushAsync();
    break;

case "POST":
    Animal newAnimal = null;
    try
    {
        newAnimal = JsonSerializer.Deserialize<Animal>(requestBodyStr);
    }
    catch (JsonException)
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        await writer.WriteLineAsync("Invalid JSON format.");
        await writer.FlushAsync();
        break;
    }

    if (newAnimal != null)
    {
        var result = await repository.AddAnimalAsync(newAnimal);
        if (result)
        {
            await writer.WriteLineAsync("Success: Animal added.");
            context.Response.StatusCode = (int)HttpStatusCode.Created;
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }
    else
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }
    await writer.FlushAsync();
    break;

case "PUT":
    if (rawUrlItems.Length == 2 && int.TryParse(rawUrlItems[1], out animalId))
    {
        Animal updatedAnimal = null;
        try
        {
            updatedAnimal = JsonSerializer.Deserialize<Animal>(requestBodyStr);
        }
        catch (JsonException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await writer.WriteLineAsync("Invalid JSON format.");
            await writer.FlushAsync();
            break;
        }

        if (updatedAnimal != null)
        {
            var result = await repository.UpdateAnimalAsync(updatedAnimal);
            if (result)
            {
                await writer.WriteLineAsync("Success: Animal updated.");
                context.Response.StatusCode = (int)HttpStatusCode.OK;
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        }
    }
    else
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }
    await writer.FlushAsync();
    break;

case "DELETE":
    if (rawUrlItems.Length == 2 && int.TryParse(rawUrlItems[1], out animalId))
    {
        var result = await repository.DeleteAnimalAsync(animalId);
        if (result)
        {
            await writer.WriteLineAsync("Success: Animal deleted.");
            context.Response.StatusCode = (int)HttpStatusCode.OK;
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
        }
    }
    else
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }
    await writer.FlushAsync();
    break;

                }
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
        } catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }

    }
