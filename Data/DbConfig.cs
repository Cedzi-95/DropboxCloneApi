public static class DbConfig
{
    // Using ?? to have a default value, if none has been set in the dev environment. 
    static readonly string Host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
    static readonly int Port = int.Parse(Environment.GetEnvironmentVariable("DB_PORT") ?? "5432");
    static readonly string Database = Environment.GetEnvironmentVariable("DB_NAME") ?? "dropboxclone";
    static readonly string Username = Environment.GetEnvironmentVariable("DB_USER") ?? "postgres";
    static readonly string Password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "password";

    public static readonly string connectionString =
    $"""
    Host={Host};Port={Port};Database={Database};Username={Username};Password={Password};
    """;
}