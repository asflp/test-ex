using Npgsql;
using WebServerHB.Entities;

namespace WebServerHB.HelpEntities;

public class DatabaseManager
{
    private const string ConnectionString =
        $"Host=localhost;" +
        $"Port=5432;" +
        $"Database=Test;" +
        $"Username=postgres;" +
        $"Password=12345678";
    
    private readonly NpgsqlConnection _connection = new(ConnectionString);
    
    private const string TablePeople = "people";
    
    public async Task AddPerson(People people)
    {
        await _connection.OpenConnectionIfClosed();
        
        string commandText = $"INSERT INTO {TablePeople} (people_id, name, date, category, user_id, image) " +
                             $"VALUES (@people_id, @name, @date, @category, @user_id, @image)";
        await using(var cmd = new NpgsqlCommand(commandText, _connection))
        {
            cmd.Parameters.AddWithValue("people_id", people.Id);
            cmd.Parameters.AddWithValue("name", people.Name);
            cmd.Parameters.AddWithValue("category", people.Category);
            cmd.Parameters.AddWithValue("date", DateTime.Parse(people.Date));
            cmd.Parameters.AddWithValue("user_id", Guid.Parse(people.UserId));
            cmd.Parameters.AddWithValue("image", people.Image);

            await cmd.ExecuteNonQueryAsync();
        }
        
        await _connection.CloseAsync();
    }
    
    private People ReadPerson(NpgsqlDataReader reader)
    {
        var peopleId = reader["people_id"] as Guid? ?? default;
        var name = reader["name"] as string;
        var category = reader["category"] as string;
        var date = reader["date"] as string;
        var userId = reader["user_id"] as string;
        var image = reader["image"] as string;

        People question = new People()
        {
            Id = peopleId,
            Name = name!,
            Category = category!,
            Date = date!,
            UserId = userId!,
            Image = image!
        };
        return question;
    }
    
    public async Task UpdatePerson(string peopleId, People people)
    {
        await _connection.OpenConnectionIfClosed();
        
        var commandText = $@"UPDATE {TablePeople}
                SET name = @name, date = @date, image = @image
                WHERE people_id = {Guid.Parse(peopleId)}";

        await using (var cmd = new NpgsqlCommand(commandText, _connection))
        {
            cmd.Parameters.AddWithValue("name", people.Name);
            cmd.Parameters.AddWithValue("date", DateOnly.Parse(people.Date));
            cmd.Parameters.AddWithValue("image", people.Image);

            await cmd.ExecuteNonQueryAsync();
        }
        
        await _connection.CloseAsync();
    }
    
    public async Task DeletePerson(string peopleId)
    {
        await _connection.OpenConnectionIfClosed();
        
        string commandText = $"DELETE FROM {TablePeople} WHERE people_id = @people_id";
        await using (var cmd = new NpgsqlCommand(commandText, _connection))
        {
            cmd.Parameters.AddWithValue("people_id", Guid.Parse(peopleId));
            await cmd.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
        }
    }
}