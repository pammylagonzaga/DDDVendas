using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Domain.Clientes;
using Vendas.Domain.Clientes.Enums;
using Vendas.Domain.Clientes.Events;
using Vendas.Domain.Clientes.ValueObjects;
using Vendas.Domain.Common.Exceptions;

namespace Vendas.Domain.Tests.Clientes;

public class ClienteTest
{
    private static NomeCompleto CriarNomeCompleto(string nome = "João Silva")
        => new(nome);
    private static Cpf CriarCpf(string numero = "12345678909")
        => new(numero);
    private static Email CriarEmail(string endereco = "joao@exemplo.com")
        => new(endereco);
    private static Telefone CriarTelefone(string numero = "11999999999")
        => new(numero);

    private static Endereco CriarEndereco(
        string cep = "01001000",
        string logradouro = "Rua A",
        string numero = "123",
        string bairro = "Centro",
        string cidade = "São Paulo",
        string estado = "SP",
        string pais = "Brasil",
        string complemento = "")
        => new(cep, logradouro, numero, bairro, cidade, estado, pais, complemento);

    private static Cliente CriarClienteValido()
        => new(
            CriarNomeCompleto(),
            CriarCpf(),
            CriarEmail(),
            CriarTelefone(),
            CriarEndereco(),
            Sexo.Masculino,
            EstadoCivil.Solteiro);


    [Fact]
    public void Construtor_ComDadosValidos_DeveCriarCliente()
    {
        // Arrange & Act
        var cliente = CriarClienteValido();
        // Assert
        cliente.Status.Should().Be(StatusCliente.Ativo);
        cliente.Sexo.Should().Be(Sexo.Masculino);
        cliente.EstadoCivil.Should().Be(EstadoCivil.Solteiro);

        cliente.Enderecos.Should().ContainSingle();
        cliente.EnderecoPrincipalId.Should().Be(cliente.Enderecos.First().Id);
    }

    [Fact]
    public void Construtor_DeveGerarEventoClienteCadastrado()
    {
        // Arrange & Act
        var cliente = CriarClienteValido();
        // Assert
        cliente.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<ClienteCadastradoEvent>();
    }

    [Theory]
    [InlineData("Nome")]
    [InlineData("Cpf")]
    [InlineData("Email")]
    [InlineData("Telefone")]

    public void Construtor_ComParametroObrigatorioNulo_DeveLancarDomainException(string campo)
    {
        NomeCompleto? nome = campo == "Nome" ? null : CriarNomeCompleto();
        Cpf? cpf = campo == "Cpf" ? null : CriarCpf();
        Email? email = campo == "Email" ? null : CriarEmail();
        Telefone? telefone = campo == "Telefone" ? null : CriarTelefone();
        Endereco endereco = CriarEndereco();

        Action act = () => new Cliente(
            nome!,
            cpf!,
            email!,
            telefone!,
            endereco);

        act.Should().Throw<DomainException>();
    }

    [Fact]

    public void AdicionarEndereco_DeveAdicionar()
    {
        var cliente = CriarClienteValido();
        var novo = CriarEndereco("01001000", "Rua A");

        cliente.AdicionarEndereco(novo);

        cliente.Enderecos.Should().HaveCount(2);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]

    public void AdicionarEndereco_ValidacaoDeNulo(bool usarNulo)
    {
        var cliente = CriarClienteValido();
        Endereco? endereco = usarNulo ? null : CriarEndereco();

        Action act = () => cliente.AdicionarEndereco(endereco!);

        if (usarNulo)
            act.Should().Throw<DomainException>();
        else
            act.Should().NotThrow();
    }

    [Fact]

    public void AdicionarEndereco_DeveAtualizrDataModificacao()
    {
        var cliente = CriarClienteValido();
        var dataAnterior = cliente.DataAtualizacao ?? DateTime.UtcNow;

        System.Threading.Thread.Sleep(5); // Garantir diferença de tempo
        cliente.AdicionarEndereco(CriarEndereco("02134000", "Rua Augusta"));
        cliente.DataAtualizacao.Should().BeAfter(dataAnterior);
    }

    [Fact]

    public void RemoverEndereco_ComSeguroEndereco_DeveRemover()
    {
        var cliente = CriarClienteValido();
        var endereco2 = CriarEndereco("02134000", "Rua Augusta");
        cliente.AdicionarEndereco(endereco2);

        cliente.RemoverEndereco(endereco2.Id);

        cliente.Enderecos.Should().HaveCount(1);
    }

    [Theory]
    [InlineData("NaoExiste")]
    [InlineData("Ultimo")]

    public void RemoverEndereco_DeveLancarExceptions(string caso)
    {
        var cliente = CriarClienteValido();
        Guid id = caso switch
        {
            "NaoExiste" => Guid.NewGuid(),
            "Ultimo" => cliente.EnderecoPrincipalId,
            _ => throw new ArgumentOutOfRangeException()
        };
        Action act = () => cliente.RemoverEndereco(id);

        act.Should().Throw<DomainException>();
    }

    [Fact]

    public void RemoverEndereco_Principal_DeveRedefinirPrincipal()
    {
        var cliente = CriarClienteValido();
        var endereco2 = CriarEndereco("02134000", "Rua Augusta");
        cliente.AdicionarEndereco(endereco2);

        cliente.RemoverEndereco(cliente.EnderecoPrincipalId);

        cliente.EnderecoPrincipalId.Should().Be(endereco2.Id);
        cliente.DomainEvents.Should().Contain(e => e is EnderecoPrincipalAlteradoEvent);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]

    public void AlterarEndereco_ValidacaoDeEndereco(bool valido)
    {
        var cliente = CriarClienteValido();
        var principal = cliente.ObterEnderecoPrincipal();

        Guid id = valido ? principal.Id : Guid.NewGuid();

        Action act = () => cliente.AlterarEndereco(
            id,
            "99999999",
            "Rua Alterada",
            "456",
            "Bairro Alterado",
            "Cidade Alterada",
            "ES",
            "Brasil");

        if (valido)
            act.Should().NotThrow();
        else
            act.Should().Throw<DomainException>();
    }

    [Fact]

    public void AlterarEndereco_DeveAlterarCampos()
    {
        var cliente = CriarClienteValido();
        var principal = cliente.ObterEnderecoPrincipal();

        cliente.AlterarEndereco(
            principal.Id,
            "99999999",
            "Rua Alterada",
            "456",
            "Bairro Alterado",
            "Cidade Alterada",
            "ES",
            "Brasil");

        principal.Logradouro.Should().Be("Rua Alterada");
    }

    //Definir Endereço Principal
    [Fact]

    public void DefinirEnderecoPrincipal_DeveDefinir()
    {
        var cliente = CriarClienteValido();
        var endereco2 = CriarEndereco("02134000", "Rua Augusta");
        cliente.AdicionarEndereco(endereco2);

        cliente.DefinirEnderecoPrincipal(endereco2.Id);

        cliente.EnderecoPrincipalId.Should().Be(endereco2.Id);
    }

    [Fact]

    public void DefinirEnderecoPrinicpal_DeveGerarEvento()
    {
        var cliente = CriarClienteValido();
        var endereco2 = CriarEndereco("02134000", "Rua Augusta");
        cliente.AdicionarEndereco(endereco2);

        cliente.DefinirEnderecoPrincipal(endereco2.Id);

        cliente.DomainEvents.Should().Contain(e => e is EnderecoPrincipalAlteradoEvent);
    }

    [Fact]

    public void ObterEnderecoPrincipal_DeveRetornarCorreto()
    {
        var cliente = CriarClienteValido();

        var principal = cliente.ObterEnderecoPrincipal();

        principal.Id.Should().Be(cliente.EnderecoPrincipalId);
    }

    [Fact]

    public void AtualizarPerfil_DeveAtualizar()
    {
        var cliente = CriarClienteValido();
        var nome = CriarNomeCompleto("Maria Souza");

        cliente.AtualizarPerfil(
            nome,
            CriarEmail("maria@ex.com"),
            CriarTelefone("21988888888"),
            Sexo.Feminino,
            EstadoCivil.Casado);

        cliente.Nome.Should().Be(nome);
        cliente.Sexo.Should().Be(Sexo.Feminino);
        cliente.EstadoCivil.Should().Be(EstadoCivil.Casado);
    }

    [Theory]
    [InlineData("Nome")]
    [InlineData("Email")]
    [InlineData("Telefone")]

    public void AtualizarPerfil_CamposNulo_DeveFalhar(string campo)
    {
        var cliente = CriarClienteValido();

        NomeCompleto? nome = campo == "Nome" ? null : CriarNomeCompleto();
        Email? email = campo == "Email" ? null : CriarEmail();
        Telefone? telefone = campo == "Telefone" ? null : CriarTelefone();

        Action act = () => cliente.AtualizarPerfil(
            nome!,
            email!,
            telefone!,
            Sexo.Masculino,
            EstadoCivil.Solteiro);

        act.Should().Throw<DomainException>();
    }

    [Fact]

    public void AtualizarPerfil_ComClienteBloqueado_DevaFalhar()
    {
        var cliente = CriarClienteValido();
        cliente.Bloquear();

        Action act = () => cliente.AtualizarPerfil(
            CriarNomeCompleto(),
            CriarEmail(),
            CriarTelefone(),
            Sexo.Masculino,
            EstadoCivil.Solteiro);

        act.Should().Throw<DomainException>();
    }

    [Fact]

    public void Bloquear_DeveBloquear()
    {
        var cliente = CriarClienteValido();

        cliente.Bloquear();

        cliente.Status.Should().Be(StatusCliente.Bloqueado);
    }

    [Fact]

    public void Bloquear_DeveGerarEvento()
        {
        var cliente = CriarClienteValido();

        cliente.Bloquear();

        cliente.DomainEvents.Should().ContainSingle(e=> e is ClienteBloqueadoEvent);
    }

    [Fact]

    public void Ativar_DeveAtivar()
    {
        var cliente = CriarClienteValido();
        cliente.Bloquear();

        cliente.Ativar();

        cliente.Status.Should().Be(StatusCliente.Ativo);
    }

    [Fact]

    public void Fluxo_Completo_DeveManterConsistencia()
    {
        var cliente = CriarClienteValido();

        var endereco2 = CriarEndereco("02134000", "Rua Augusta");
        cliente.AdicionarEndereco(endereco2);

        cliente.DefinirEnderecoPrincipal(endereco2.Id);

        cliente.EnderecoPrincipalId.Should().Be(endereco2.Id);
        cliente.Enderecos.Should().HaveCount(2);
    }
}


