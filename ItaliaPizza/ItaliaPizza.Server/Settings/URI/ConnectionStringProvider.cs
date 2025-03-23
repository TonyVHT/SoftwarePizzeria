namespace ItaliaPizza.Server.Settings.URI
{
    public static class ConnectionStringProvider
    {
        public static string GetConnectionString()
        {
            try
            {
                var connectionString = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder
                {
                    DataSource = GetEnvironmentVariable("ITALIAPIZZA_DB_SERVER"),
                    InitialCatalog = GetEnvironmentVariable("ITALIAPIZZA_DB_NAME"),
                    UserID = GetEnvironmentVariable("ITALIAPIZZA_DB_USER"),
                    Password = GetEnvironmentVariable("ITALIAPIZZA_DB_PASSWORD"),
                    ConnectTimeout = 10,
                    TrustServerCertificate = true,
                    MultipleActiveResultSets = true
                }.ConnectionString;

                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new InvalidOperationException("No se pudo obtener una cadena de conexión válida desde las variables de entorno.");
                }

                return connectionString;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error crítico al construir la cadena de conexión.", ex);
            }
        }

        private static string GetEnvironmentVariable(string variableName)
        {
            var value = Environment.GetEnvironmentVariable(variableName);

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException($"No se encontró la variable de entorno: {variableName}");
            }

            return value;
        }
    }
}
