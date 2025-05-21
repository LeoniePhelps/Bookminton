using System.Text.Json;

Console.WriteLine($"Booking Badminton at {DateTime.Now}");

using var client = new HttpClient();

var authToken = Environment.GetEnvironmentVariable("AUTH_TOKEN");

client.DefaultRequestHeaders.Add("Authorization", authToken);
client.DefaultRequestHeaders.Add("Origin", "https://bookings.better.org.uk");

var response = await client.GetAsync($"{Environment.GetEnvironmentVariable("SUGDEN_BADMINTON_URL")}2025-05-22");
var content = await response.Content.ReadAsStringAsync();

var document = JsonDocument.Parse(content);
var root = document.RootElement;


var bookableIds = root.GetProperty("data")
    .EnumerateArray()
    .Where(slot => slot.GetProperty("action_to_show").GetProperty("status").GetString() == "BOOK")
    .Select(slot => slot.GetProperty("id").GetInt32())
    .ToList();

Console.WriteLine(string.Join(", ", bookableIds));

Console.WriteLine($"Badminton Booked at {DateTime.Now}");
