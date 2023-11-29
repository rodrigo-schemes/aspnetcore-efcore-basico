using CursoEFCore.Data;
using CursoEFCore.Domain;
using CursoEFCore.ValueObjects;
using Microsoft.EntityFrameworkCore;

InserirDados();
InserirDadosEmMassa();
InserirClientesEmMassa();
ConsultarDados();
CadastrarPedido();
ConsultarPedidoCarregamentoAdiantado();
AtualizarDados();
AtualizarDadosComNovaInstanciaDeObjeto();
AtualizarDadosDesconectado();
RemoverRegistro();
RemoverRegistroDeFormaDesconectada();


static void RemoverRegistroDeFormaDesconectada()
{
    using var db = new ApplicationContext();

    var codigoCliente = CadastrarCliente();

    var cliente = new Cliente
    {
        Id = codigoCliente
    };
    
    db.Clientes.Remove(cliente);
    db.SaveChanges();
}

static void RemoverRegistro()
{
    using var db = new ApplicationContext();

    var codigoCliente = CadastrarCliente();

    var cliente = db.Clientes.Find(codigoCliente);

    db.Clientes.Remove(cliente);
    db.SaveChanges();
}

static void AtualizarDadosDesconectado()
{
    using var db = new ApplicationContext();

    var codigoCliente = CadastrarCliente();

    var cliente = new Cliente
    {
        Id = codigoCliente
    };
    
    var clienteDesconectado = new
    {
        Nome = "Cliente Desconectado 2",
        Telefone = "1111"
    };

    // O attach fará o carregamento da entidade no EFCore
    db.Attach(cliente);
    
    // Atribuição do objeto para o EFCore rastrear
    db.Entry(cliente).CurrentValues.SetValues(clienteDesconectado);

    db.SaveChanges();
}

static void AtualizarDadosComNovaInstanciaDeObjeto()
{ 
    using var db = new ApplicationContext();

    var codigoCliente = CadastrarCliente();
    
    //O find busca pela PK
    var cliente = db.Clientes.Find(codigoCliente);
    
    var clienteDesconectado = new
    {
        Nome = "Cliente Desconectado",
        Telefone = "999999999"
    };

    db.Entry(cliente).CurrentValues.SetValues(clienteDesconectado);

    db.SaveChanges();
}

static void AtualizarDados()
{
    using var db = new ApplicationContext();

    var codigoCliente = CadastrarCliente();

    //O find busca pela PK
    var cliente = db.Clientes.Find(codigoCliente);

    cliente.Nome = "Cliente Alterado Passo 2";

    // O update irá sobreescrever todas as propriedades do objeto
    //db.Clientes.Update(cliente);
    
    // Ao chamar o SaveChanges direto, o EfCore irá alterar somente os atributos modificados
    db.SaveChanges();
}

static void ConsultarPedidoCarregamentoAdiantado()
{
    using var db = new ApplicationContext();
    
    //Eager Loading - O Include fará o carregamento da entidade PedidosItens. ThenInclude carregará Produtos
    var pedidos = db.Pedidos
        .Include(x => x.Itens)
            .ThenInclude(x => x.Produto)
        .ToList();
    
    Console.WriteLine(pedidos.Count);
}

static int CadastrarCliente()
{
    using var db = new ApplicationContext();

    var cliente = new Cliente
    {
        Nome = "Nome do Cliente",
        Telefone = "123456789",
        Cidade = "Cidade do Cliente",
        Estado = "UF",
        CEP = "12345123"
    };

    db.Clientes.Add(cliente);

    db.SaveChanges();

    return cliente.Id;
}

static void CadastrarPedido()
{
    using var db = new ApplicationContext();

    var cliente = db.Clientes.FirstOrDefault();
    var produto = db.Produtos.FirstOrDefault();

    var pedido = new Pedido
    {
        ClienteId = cliente.Id,
        IniciadoEm = DateTime.Now,
        FinalizadoEm = DateTime.Now,
        Observacao = "Pedido Teste",
        Status = StatusPedido.Analise,
        TipoFrete = TipoFrete.SemFrete,
        Itens = new List<PedidoItem>
        {
            new PedidoItem
            {
                ProdutoId = produto.Id,
                Desconto = 0,
                Quantidade = 1,
                Valor = 10
            }
        }
    };

    db.Pedidos.Add(pedido);
    db.SaveChanges();
}

static void ConsultarDados()
{
    using var db = new ApplicationContext();
    //var consultaPorSintaxe = (from c in db.Clientes where c.Id > 0 select c).ToList();
    
    //Ao utilizar o AsNoTracking, não será realizado a consulta em memória e sim direto no banco
    //var consultaPorMetodo = db.Clientes.AsNoTracking().Where(x => x.Id > 0).ToList();
    
    var consultaPorMetodo = db.Clientes
        .Where(x => x.Id > 0)
        .OrderBy(x => x.Id)
        .ToList();

    foreach (var cliente in consultaPorMetodo)
    {
        Console.WriteLine($"Consultando Cliente: {cliente.Id}");
        
        //Ao executar a busca por Find, será verificado primeiro o dado em memória
        //db.Clientes.Find(cliente.Id);
        
        //Ao executar a busca por FirstOrDefault, será executada a query no banco
        db.Clientes.FirstOrDefault(x => x.Id == cliente.Id);
    }
}

static void InserirClientesEmMassa()
{
    var listaDeClientes = new List<Cliente>
    {
        new()
        {
            Nome = "Cliente 1",
            CEP = "12345678",
            Cidade = "Cidade",
            Estado = "SP",
            Telefone = "11123456789"
        },
        new()
        {
            Nome = "Cliente 2",
            CEP = "87654321",
            Cidade = "Cidade",
            Estado = "SP",
            Telefone = "11987654321"
        }
    };
    
    using var db = new ApplicationContext();
    db.Clientes.AddRange(listaDeClientes);

    var registros = db.SaveChanges();
    
    Console.WriteLine($"Total Registros: {registros}");
}

static void InserirDadosEmMassa()
{
    var produto = new Produto
    {
        Descricao = "Produto Teste",
        CodigoBarras = "1234567891231",
        Valor = 10m,
        TipoProduto = TipoProduto.MercadoriaParaRevenda,
        Ativo = true
    };

    var cliente = new Cliente
    {
        Nome = "Augusto Heitor Benício Cavalcanti",
        CEP = "69060770",
        Cidade = "Manaus",
        Estado = "AM",
        Telefone = "92991422706"
    };
    
    using var db = new ApplicationContext();
    db.AddRange(produto, cliente);

    var registros = db.SaveChanges();
    
    Console.WriteLine($"Total Registros: {registros}");

}

static void InserirDados()
{
    var produto = new Produto
    {
        Descricao = "Produto Teste",
        CodigoBarras = "1234567891231",
        Valor = 10m,
        TipoProduto = TipoProduto.MercadoriaParaRevenda,
        Ativo = true
    };

    using var db = new ApplicationContext();
    db.Produtos.Add(produto);

    var registros = db.SaveChanges();
    Console.WriteLine($"Total Registros: {registros}");
}