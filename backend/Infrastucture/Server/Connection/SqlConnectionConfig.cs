using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Connections;

namespace Backend.Infrastucture.Server.Connection
{
  internal static class SqlConnectionConfig
  {
    static string DataSource = string.Empty;
    static string User = string.Empty;
    static string Password = string.Empty;
    static bool WindowAuth;
    static bool Encrypt;
    static string InitialCatalog = string.Empty;
    static bool Initialized = false;

    private static void InitializeConfig()
    {
      var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "SQL.json");

      if (!File.Exists(jsonPath))
        throw new FileNotFoundException("JSON file with database connection configuration not found.");

      var jsonStr = File.ReadAllText(jsonPath);
      var jsonObject = JsonNode.Parse(jsonStr)?.AsObject();

      if (jsonObject is null)
        throw new JsonException("Failed to read json file.");

      if (jsonObject.ContainsKey("DataSource"))
        DataSource = jsonObject["DataSource"]?.ToString() ?? "";

      if (jsonObject.ContainsKey("User"))
        User = jsonObject["User"]?.ToString() ?? "";

      if (jsonObject.ContainsKey("Password"))
        Password = jsonObject["Password"]?.ToString() ?? "";

      if (jsonObject.ContainsKey("WindowAuth"))
        WindowAuth = jsonObject["WindowAuth"]?.GetValue<bool>() ?? false;

      if (jsonObject.ContainsKey("Encrypt"))
        Encrypt = jsonObject["Encrypt"]?.GetValue<bool>() ?? false;

      if (jsonObject.ContainsKey("InitialCatalog"))
        InitialCatalog = jsonObject["InitialCatalog"]?.ToString() ?? "";

      Initialized = true;
    }

    internal static string ReturnConnectionString(string? database = null)
    {
      if (!Initialized)
        InitializeConfig();

      if (!Initialized)
        throw new ConnectionAbortedException("Error while trying to build the connection string.");

      string connection = "";

      connection += $"Data Source={DataSource};";
      connection += $"Initial Catalog={(database is null ? InitialCatalog : database)};";

      if (WindowAuth)
      {
        connection += "Integrated Security=True";
      }
      else
      {
        connection += $"User Id={User};";
        connection += $"Password={Password};";
      }

      connection += "Pooling=True;";
      connection += "TrustServerCertificate=True;";

      connection += $"Encrypt={(Encrypt ? "True" : "False")};";

      return connection;
    }
  }
}
