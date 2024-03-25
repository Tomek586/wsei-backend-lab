using BlazorApp;
using System;
using System.Collections.Generic;
using System.Linq;

public class UserService : IUserService
{
    private readonly Dictionary<string, string> _userConnections = new Dictionary<string, string>();

    public void Add(string connectionId, string username)
    {
        // Dodaj użytkownika do słownika
        _userConnections[username] = connectionId;
    }

    public void RemoveByName(string username)
    {
        // Usuń użytkownika ze słownika
        _userConnections.Remove(username);
    }

    public string GetConnectionIdByName(string username)
    {
        // Pobierz connectionId użytkownika z podanym username
        return _userConnections.GetValueOrDefault(username);
    }

    public IEnumerable<(string ConnectionId, string Username)> GetAll()
    {
        // Zwróć wszystkie pary connectionId i username
        return _userConnections.Select(kv => (kv.Value, kv.Key)).ToList();
    }
}
