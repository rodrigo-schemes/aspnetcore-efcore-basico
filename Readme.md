### Escopo do Curso

#### 1. Instalação de EF CLI
    dotnet tool install --global dotnet-ef

#### 2. Criação do Application Context
- Habilitar logs do EF Core
- Opções de resiliência de conexão
- Configuração de tabela de histórico do EF Core
#### 3. Mapeamento do Modelo de Dados com Fluent API
- Detectando Propriedades não configuradas
#### 4. Migrations
##### Adiciona Migrations
    dotnet ef migrations add PrimeiraMigracao -p .\CursoEFCore\CursoEFCore.csproj
##### Gerar Script
    dotnet ef migrations script -p .\CursoEFCore\CursoEFCore.csproj -o .\CursoEFCore\PrimeiraMigracao.SQL
##### Gerar Script Idempotente
    dotnet ef migrations script -p .\CursoEFCore\CursoEFCore.csproj -o .\CursoEFCore\Idempotente.SQL -i
##### Aplicar a Migration
    dotnet ef database update -p .\CursoEFCore\CursoEFCore.csproj -v
##### Rollback de Migration
    dotnet ef database update [Migração Ponto de Retorno] -p .\CursoEFCore\CursoEFCore.csproj
##### Remover Migration
    dotnet ef migrations remove -p .\CursoEFCore\CursoEFCore.csproj
##### Aplicar Migration via código
    db.Database.Migrate();
##### Verificar Migrations Pendentes
    db.Database.GetPendingMigration();
#### 5. Operações
- Inserir Dados
- Inserir Dados de diferentes Entidades em Massa
- Inserir Dados em Lista de uma mesma Entidades
- Consultar Dados - Eager Loading
- Atualizar Dados
- Atualizar Dados Desconectado do EF Core
- Remover Dados
- Remover Dados Desconectado do EF Core



