using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using RegistroDeAtendimento.Api;
using RegistroDeAtendimento.Core.Domain.Enums;
using RegistroDeAtendimento.Shared.Application.Dtos;

namespace Api.Tests.Controllers;

public class PacienteControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public PacienteControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_Deve_Retornar_Status_200(){
        var response = await _client.GetAsync("/api/paciente");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(content);
    }

    [Fact]
    public async Task GetById_Deve_Retornar_Status_200_Quando_Existir()
    {
        var pacienteDto = new CriarPacienteDto
        {
            Nome = "Maria Teste",
            DataNascimento = new DateOnly(1990, 1, 1),
            Cpf = $"{Random.Shared.Next(100000000, 999999999)}{Random.Shared.Next(0, 99):D2}",
            Sexo = SexoEnum.Feminino,
            Cep = "12345678",
            Cidade = "Cidade",
            Bairro = "Bairro",
            Logradouro = "Rua Teste",
            Complemento = "Apto 1",
            Status = StatusEnum.Ativo
        };

        var createResponse = await _client.PostAsJsonAsync("/api/paciente", pacienteDto);
        createResponse.EnsureSuccessStatusCode();
        var pacienteJson = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
        var pacienteId = pacienteJson.GetProperty("id").GetGuid();

        var response = await _client.GetAsync($"/api/paciente/{pacienteId}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains($"\"id\":\"{pacienteId.ToString().ToLower()}\"", content.ToLower());
    }

    [Fact]
    public async Task GetById_Deve_Retornar_Status_404_Quando_Id_Inexistente()
    {
        var idInexistente = Guid.NewGuid();
        var response = await _client.GetAsync($"/api/paciente/{idInexistente}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Post_Deve_Retornar_Status_201_Quando_Dados_Validos()
    {
        var pacienteDto = new CriarPacienteDto
        {
            Nome = "Novo Paciente",
            DataNascimento = new DateOnly(1985, 5, 5),
            Cpf = $"{Random.Shared.Next(100000000, 999999999)}{Random.Shared.Next(0, 99):D2}",
            Sexo = SexoEnum.Masculino,
            Cep = "87654321",
            Cidade = "Cidade",
            Bairro = "Bairro",
            Logradouro = "Rua Nova",
            Complemento = "",
            Status = StatusEnum.Ativo
        };

        var response = await _client.PostAsJsonAsync("/api/paciente", pacienteDto);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("\"id\":", content.ToLower());
    }

    [Fact]
    public async Task Post_Deve_Retornar_Status_400_Quando_Dados_Invalidos()
    {
        var pacienteDto = new CriarPacienteDto
        {
            Nome = "", 
            DataNascimento = new DateOnly(1985, 5, 5),
            Cpf = "",
            Sexo = SexoEnum.Masculino,
            Cep = "",
            Cidade = "",
            Bairro = "",
            Logradouro = "",
            Complemento = "",
            Status = StatusEnum.Ativo
        };

        var response = await _client.PostAsJsonAsync("/api/paciente", pacienteDto);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task Post_Deve_Retornar_Status_409_Quando_Cpf_Ja_Existir()
    {
        var cpfDuplicado = $"{Random.Shared.Next(100000000, 999999999)}{Random.Shared.Next(0, 99):D2}";

        var pacienteDto1 = new CriarPacienteDto
        {
            Nome = "Paciente 1",
            DataNascimento = new DateOnly(1990, 1, 1),
            Cpf = cpfDuplicado,
            Sexo = SexoEnum.Masculino,
            Cep = "12345678",
            Cidade = "Cidade",
            Bairro = "Bairro",
            Logradouro = "Rua Teste",
            Complemento = "",
            Status = StatusEnum.Ativo
        };

        var pacienteDto2 = new CriarPacienteDto
        {
            Nome = "Paciente 2",
            DataNascimento = new DateOnly(1992, 2, 2),
            Cpf = cpfDuplicado,
            Sexo = SexoEnum.Feminino,
            Cep = "87654321",
            Cidade = "Cidade",
            Bairro = "Bairro",
            Logradouro = "Rua Teste 2",
            Complemento = "",
            Status = StatusEnum.Ativo
        };

        var response1 = await _client.PostAsJsonAsync("/api/paciente", pacienteDto1);
        response1.EnsureSuccessStatusCode();

        var response2 = await _client.PostAsJsonAsync("/api/paciente", pacienteDto2);
        Assert.Equal(HttpStatusCode.Conflict, response2.StatusCode);
    }
    
    [Fact]
    public async Task Put_Deve_Retornar_Status_204_Quando_Atualizar_Com_Sucesso()
    {
        var pacienteDto = new CriarPacienteDto
        {
            Nome = "Paciente Atualizar",
            DataNascimento = new DateOnly(1992, 2, 2),
            Cpf = $"{Random.Shared.Next(100000000, 999999999)}{Random.Shared.Next(0, 99):D2}",
            Sexo = SexoEnum.Feminino,
            Cep = "12345678",
            Cidade = "Cidade",
            Bairro = "Bairro",
            Logradouro = "Rua",
            Complemento = "",
            Status = StatusEnum.Ativo
        };

        var createResponse = await _client.PostAsJsonAsync("/api/paciente", pacienteDto);
        createResponse.EnsureSuccessStatusCode();
        var pacienteJson = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
        var pacienteId = pacienteJson.GetProperty("id").GetGuid();

        var atualizarDto = new AtualizarPacienteDto
        {
            Nome = "Paciente Atualizado",
            DataNascimento = new DateOnly(1992, 2, 2),
            Cpf = pacienteDto.Cpf,
            Sexo = pacienteDto.Sexo,
            Cep = pacienteDto.Cep,
            Cidade = pacienteDto.Cidade,
            Bairro = pacienteDto.Bairro,
            Logradouro = pacienteDto.Logradouro,
            Complemento = pacienteDto.Complemento,
            Status = pacienteDto.Status
        };

        var response = await _client.PutAsJsonAsync($"/api/paciente/atualizar/{pacienteId}", atualizarDto);
        
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            Assert.Fail($"Falha no PUT: {response.StatusCode} - {error}");
        }
        
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Put_Deve_Retornar_Status_400_Quando_Dados_Invalidos()
    {
        // Cria paciente válido
        var pacienteDto = new CriarPacienteDto{
            Nome = "Paciente Teste",
            DataNascimento = new DateOnly(1995, 8, 20),
            Cpf = $"{Random.Shared.Next(100000000, 999999999)}{Random.Shared.Next(0, 99):D2}",
            Sexo = SexoEnum.Feminino,
            Cep = "12398745",
            Cidade = "Cidade",
            Bairro = "Bairro",
            Logradouro = "Rua",
            Complemento = "",
            Status = StatusEnum.Ativo
        };

        var createResponse = await _client.PostAsJsonAsync("/api/paciente", pacienteDto);
        createResponse.EnsureSuccessStatusCode();
        var pacienteJson = await createResponse.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();
        var pacienteId = pacienteJson.GetProperty("id").GetGuid();

        // Tenta atualizar com dados inválidos
        var atualizarDto = new AtualizarPacienteDto
        {
            Nome = pacienteDto.Nome,
            DataNascimento = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            Cpf = pacienteDto.Cpf,
            Sexo = pacienteDto.Sexo,
            Cep = pacienteDto.Cep,
            Cidade = pacienteDto.Cidade,
            Bairro = pacienteDto.Bairro,
            Logradouro = pacienteDto.Logradouro,
            Complemento = pacienteDto.Complemento,
            Status = pacienteDto.Status
        };

        var putResponse = await _client.PutAsJsonAsync($"/api/paciente/atualizar/{pacienteId}", atualizarDto);
        

        
        Assert.Equal(HttpStatusCode.BadRequest, putResponse.StatusCode);
    }
    
    [Fact]
    public async Task Put_Deve_Retornar_Status_404_Quando_Id_Inexistente()
    {
        var idInexistente = Guid.NewGuid();
        var atualizarDto = new AtualizarPacienteDto
        {
            Nome = "Paciente",
            DataNascimento = new DateOnly(1992, 2, 2),
            Cpf = "12345678900",
            Sexo = SexoEnum.Feminino,
            Cep = "12345678",
            Cidade = "Cidade",
            Bairro = "Bairro",
            Logradouro = "Rua",
            Complemento = "",
            Status = StatusEnum.Ativo
        };

        var response = await _client.PutAsJsonAsync($"/api/paciente/atualizar/{idInexistente}", atualizarDto);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task Put_Deve_Retornar_Status_409_Quando_Cpf_Ja_Existir()
    {
        // Cria dois pacientes com CPFs diferentes
        var cpf1 = $"{Random.Shared.Next(100000000, 999999999)}{Random.Shared.Next(0, 99):D2}";
        var cpf2 = $"{Random.Shared.Next(100000000, 999999999)}{Random.Shared.Next(0, 99):D2}";

        var pacienteDto1 = new CriarPacienteDto
        {
            Nome = "Paciente 1",
            DataNascimento = new DateOnly(1990, 1, 1),
            Cpf = cpf1,
            Sexo = SexoEnum.Masculino,
            Cep = "12345678",
            Cidade = "Cidade",
            Bairro = "Bairro",
            Logradouro = "Rua Teste",
            Complemento = "",
            Status = StatusEnum.Ativo
        };

        var pacienteDto2 = new CriarPacienteDto
        {
            Nome = "Paciente 2",
            DataNascimento = new DateOnly(1992, 2, 2),
            Cpf = cpf2,
            Sexo = SexoEnum.Feminino,
            Cep = "87654321",
            Cidade = "Cidade",
            Bairro = "Bairro",
            Logradouro = "Rua Teste 2",
            Complemento = "",
            Status = StatusEnum.Ativo
        };

        var response1 = await _client.PostAsJsonAsync("/api/paciente", pacienteDto1);
        response1.EnsureSuccessStatusCode();
        var paciente1 = await response1.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();
        var pacienteId1 = paciente1.GetProperty("id").GetGuid();

        var response2 = await _client.PostAsJsonAsync("/api/paciente", pacienteDto2);
        response2.EnsureSuccessStatusCode();
        var paciente2 = await response2.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();
        var pacienteId2 = paciente2.GetProperty("id").GetGuid();

        // Tenta atualizar o paciente 2 usando o CPF do paciente 1
        var atualizarDto = new AtualizarPacienteDto
        {
            Nome = "Paciente 2 Atualizado",
            DataNascimento = pacienteDto2.DataNascimento,
            Cpf = cpf1, // CPF já existente
            Sexo = pacienteDto2.Sexo,
            Cep = pacienteDto2.Cep,
            Cidade = pacienteDto2.Cidade,
            Bairro = pacienteDto2.Bairro,
            Logradouro = pacienteDto2.Logradouro,
            Complemento = pacienteDto2.Complemento,
            Status = pacienteDto2.Status
        };

        var putResponse = await _client.PutAsJsonAsync($"/api/paciente/atualizar/{pacienteId2}", atualizarDto);
        Assert.Equal(HttpStatusCode.Conflict, putResponse.StatusCode);
    }

    [Fact]
    public async Task Patch_Inativar_Deve_Retornar_Status_200_Quando_Paciente_Ja_Estiver_Inativo()
    {
        var pacienteDto = new CriarPacienteDto
        {
            Nome = "Paciente Já Inativo",
            DataNascimento = new DateOnly(1990, 1, 1),
            Cpf = $"{Random.Shared.Next(100000000, 999999999)}{Random.Shared.Next(0, 99):D2}",
            Sexo = SexoEnum.Masculino,
            Cep = "12345678",
            Cidade = "Cidade",
            Bairro = "Bairro",
            Logradouro = "Rua Teste",
            Complemento = "",
            Status = StatusEnum.Inativo
        };

        var createResponse = await _client.PostAsJsonAsync("/api/paciente", pacienteDto);
        createResponse.EnsureSuccessStatusCode();
        var pacienteJson = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
        var pacienteId = pacienteJson.GetProperty("id").GetGuid();

        var response = await _client.PatchAsync($"/api/paciente/inativar/{pacienteId}", null);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var message = await response.Content.ReadAsStringAsync();
        Assert.Contains("paciente já está inativo", message.ToLower());
    }
    
    [Fact]
    public async Task Patch_Inativar_Deve_Retornar_Status_204_Quando_Sucesso()
    {
        var pacienteDto = new CriarPacienteDto
        {
            Nome = "Paciente Inativar",
            DataNascimento = new DateOnly(1995, 8, 20),
            Cpf = $"{Random.Shared.Next(100000000, 999999999)}{Random.Shared.Next(0, 99):D2}",
            Sexo = SexoEnum.Feminino,
            Cep = "12398745",
            Cidade = "Cidade",
            Bairro = "Bairro",
            Logradouro = "Rua",
            Complemento = "",
            Status = StatusEnum.Ativo
        };

        var createResponse = await _client.PostAsJsonAsync("/api/paciente", pacienteDto);
        createResponse.EnsureSuccessStatusCode();
        var pacienteJson = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
        var pacienteId = pacienteJson.GetProperty("id").GetGuid();

        var response = await _client.PatchAsync($"/api/paciente/inativar/{pacienteId}", null);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Patch_Inativar_Deve_Retornar_Status_400_Quando_Id_Inexistente()
    {
        var idInexistente = Guid.NewGuid();
        var response = await _client.PatchAsync($"/api/paciente/inativar/{idInexistente}", null);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Patch_Ativar_Deve_Retornar_Status_200_Quando_Paciente_Ja_Estiver_Ativo()
    {
        var pacienteDto = new CriarPacienteDto
        {
            Nome = "Paciente Já Ativo",
            DataNascimento = new DateOnly(1995, 8, 20),
            Cpf = $"{Random.Shared.Next(100000000, 999999999)}{Random.Shared.Next(0, 99):D2}",
            Sexo = SexoEnum.Feminino,
            Cep = "12398745",
            Cidade = "Cidade",
            Bairro = "Bairro",
            Logradouro = "Rua",
            Complemento = "",
            Status = StatusEnum.Ativo
        };

        var createResponse = await _client.PostAsJsonAsync("/api/paciente", pacienteDto);
        createResponse.EnsureSuccessStatusCode();
        var pacienteJson = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
        var pacienteId = pacienteJson.GetProperty("id").GetGuid();

        var response = await _client.PatchAsync($"/api/paciente/ativar/{pacienteId}", null);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var message = await response.Content.ReadAsStringAsync();
        Assert.Contains("paciente já está ativo", message.ToLower());
    }
    
    [Fact]
    public async Task Patch_Ativar_Deve_Retornar_Status_204_Quando_Sucesso()
    {
        var pacienteDto = new CriarPacienteDto
        {
            Nome = "Paciente Ativar",
            DataNascimento = new DateOnly(1995, 8, 20),
            Cpf = $"{Random.Shared.Next(100000000, 999999999)}{Random.Shared.Next(0, 99):D2}",
            Sexo = SexoEnum.Feminino,
            Cep = "12398745",
            Cidade = "Cidade",
            Bairro = "Bairro",
            Logradouro = "Rua",
            Complemento = "",
            Status = StatusEnum.Inativo
        };

        var createResponse = await _client.PostAsJsonAsync("/api/paciente", pacienteDto);
        createResponse.EnsureSuccessStatusCode();
        var pacienteJson = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
        var pacienteId = pacienteJson.GetProperty("id").GetGuid();

        var response = await _client.PatchAsync($"/api/paciente/ativar/{pacienteId}", null);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Patch_Ativar_Deve_Retornar_Status_400_Quando_Id_Inexistente()
    {
        var idInexistente = Guid.NewGuid();
        var response = await _client.PatchAsync($"/api/paciente/ativar/{idInexistente}", null);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}