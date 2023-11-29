using CursoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CursoEFCore.Data;

public class ApplicationContext : DbContext
{
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    
    private static readonly ILoggerFactory Logger = LoggerFactory.Create(log => log.AddConsole());
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseLoggerFactory(Logger)
            .EnableSensitiveDataLogging() // exibe dados sensíveis no Log
            .UseSqlServer("Server=localhost,1433;Database=CursoEFCore;User Id=sa;Password=P@ssword!;TrustServerCertificate=true",
                opt => opt.EnableRetryOnFailure(
                    maxRetryCount: 2,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorNumbersToAdd: null)); //Default: maxRetryCount: 6 / maxRetryDelay: 1 min

        //opt.MigrationsHistoryTable("tabela_migracoes"): Customizar o nome da tabela de migrações
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
        MapearPropriedadesEsquecidas(modelBuilder);
    }

    private void MapearPropriedadesEsquecidas(ModelBuilder modelBuilder)
    {
        //Obtem todas as entidades
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            //Obtem as propriedades do tipo string
            var properties = entity.GetProperties().Where(p => p.ClrType == typeof(string));

            foreach (var property in properties)
            {
                //Se o tipo da coluna for nulo e não tiver Max Length definido
                if (string.IsNullOrEmpty(property.GetColumnType())
                    && !property.GetMaxLength().HasValue)
                {
                    //Seta um tipo padrão para o campo
                    property.SetColumnType("VARCHAR(100)");
                }
            }
        }
    }
}