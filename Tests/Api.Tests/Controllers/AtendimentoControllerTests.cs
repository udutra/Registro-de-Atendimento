using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using RegistroDeAtendimento.Api;
using RegistroDeAtendimento.Core.Domain.Enums;
using RegistroDeAtendimento.Shared.Application.Dtos;

namespace Api.Tests.Controllers;

public class AtendimentoControllerTests(CustomWebApplicationFactory<Program> factory) : IClassFixture<CustomWebApplicationFactory<Program>>{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetAll_Deve_Retornar_Status_200(){
        var response = await _client.GetAsync("/api/atendimento");
    
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(content);
    }
    
    [Fact]
    public async Task GetById_Deve_Retornar_Status_200_Quando_Existir(){
        var pacienteDto = new CriarPacienteDto(){
            Nome = "João Silva",
            DataNascimento = new DateOnly(1999, 1, 1),
            Cpf = "12345678900",
            Sexo = SexoEnum.Masculino,
            Cep = "12345678",
            Cidade = "Porto Alegre",
            Bairro = "Centro",
            Logradouro = "Rua das Flores, 123",
            Complemento = "Ap 101",
            Status = StatusEnum.Ativo
        };

        var createPacienteResponse = await _client.PostAsJsonAsync("/api/paciente", pacienteDto);

        if (!createPacienteResponse.IsSuccessStatusCode){
            var message = await createPacienteResponse.Content.ReadAsStringAsync();
            Assert.Fail($"Falha ao tentar criar paciente para teste: {message}");
        }

        var pacienteCriado = await createPacienteResponse.Content.ReadFromJsonAsync<JsonElement>();
        var pacienteId = pacienteCriado.GetProperty("id").GetGuid();

        var atendimentoDto = new CriarAtendimentoDto(){
            PacienteId = pacienteId,
            DataHora = DateTime.Now.AddDays(-1),
            Descricao = "Teste de atendimento",
            Status = 0
        };

        var createAtendimentoResponse = await _client.PostAsJsonAsync("/api/atendimento", atendimentoDto);

        if (!createAtendimentoResponse.IsSuccessStatusCode)
        {
            var message = await createAtendimentoResponse.Content.ReadAsStringAsync();
            Assert.Fail($"Falha ao tentar criar atendimento: {message}");
        }

        var atendimentoCriado = await createAtendimentoResponse.Content.ReadFromJsonAsync<JsonElement>();
        var atendimentoId = atendimentoCriado.GetProperty("id").GetGuid();
        
        var getResponse = await _client.GetAsync($"/api/atendimento/{atendimentoId}");

        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        var content = await getResponse.Content.ReadAsStringAsync();
        Assert.Contains($"\"id\":\"{atendimentoId.ToString().ToLower()}\"", content.ToLower());
    }
    
    [Fact]
    public async Task GetById_Deve_Retornar_Status_404_Quando_Id_Inexistente(){
        var idInexistente = Guid.NewGuid();
        var response = await _client.GetAsync($"/api/atendimento/{idInexistente}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task Post_Deve_Retornar_Status_201_Quando_Dados_Validos(){

        var pacienteDto = new CriarPacienteDto(){
            Nome = "João Silva",
            DataNascimento = new DateOnly(1999, 1, 1),
            Cpf = $"{Random.Shared.Next(100000000, 999999999)}{Random.Shared.Next(0, 99):D2}",
            Sexo = SexoEnum.Masculino,
            Cep = "12345678",
            Cidade = "Porto Alegre",
            Bairro = "Centro",
            Logradouro = "Rua das Flores, 123",
            Complemento = "Ap 101",
            Status = StatusEnum.Ativo
        };
        
        var createPacienteResponse = await _client.PostAsJsonAsync("/api/paciente", pacienteDto);
        
        if (!createPacienteResponse.IsSuccessStatusCode){
            var message = await createPacienteResponse.Content.ReadAsStringAsync();
            Assert.Fail($"Falha ao tentar criar paciente para teste: {message}");
        }
        
        var pacienteCriado = await createPacienteResponse.Content.ReadFromJsonAsync<JsonElement>();
        var pacienteId = pacienteCriado.GetProperty("id").GetGuid();
        
        var atendimentoDto = new CriarAtendimentoDto(){
            PacienteId = pacienteId,
            DataHora = DateTime.Now.AddDays(-1),
            Descricao = "Consulta de rotina",
            Status = 0
        };
        
        var response = await _client.PostAsJsonAsync("/api/atendimento", atendimentoDto);
        
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("\"id\":", content.ToLower());
    }

    [Fact]
    public async Task Post_Deve_Retornar_Status_400_Quando_Dados_Invalidos(){
        var pacienteDto = new CriarPacienteDto(){
            Nome = "João Silva",
            DataNascimento = new DateOnly(1999, 1, 1),
            Cpf = $"{Random.Shared.Next(100000000, 999999999)}{Random.Shared.Next(0, 99):D2}",
            Sexo = SexoEnum.Masculino,
            Cep = "12345678",
            Cidade = "Porto Alegre",
            Bairro = "Centro",
            Logradouro = "Rua das Flores, 123",
            Complemento = "Ap 101",
            Status = StatusEnum.Ativo
        };
        
        var createPacienteResponse = await _client.PostAsJsonAsync("/api/paciente", pacienteDto);
        
        if (!createPacienteResponse.IsSuccessStatusCode){
            var message = await createPacienteResponse.Content.ReadAsStringAsync();
            Assert.Fail($"Falha ao tentar criar paciente para teste: {message}");
        }
        
        var pacienteCriado = await createPacienteResponse.Content.ReadFromJsonAsync<JsonElement>();
        var pacienteId = pacienteCriado.GetProperty("id").GetGuid();
        
        var atendimentoDto = new CriarAtendimentoDto(){
            PacienteId = pacienteId,
            DataHora = DateTime.Now.AddDays(1),
            Descricao = "Consulta de rotina",
            Status = 0
        };
        
        var response = await _client.PostAsJsonAsync("/api/atendimento", atendimentoDto);
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_Deve_Retornar_Status_404_Quando_Id_Usuario_Nao_Existe(){
        var atendimentoDto = new CriarAtendimentoDto(){
            PacienteId = Guid.NewGuid(),
            DataHora = DateTime.Now,
            Descricao = "Teste com dados inválidos",
            Status = 0
        };
        
        var response = await _client.PostAsJsonAsync("/api/atendimento", atendimentoDto);
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task Put_Deve_Retornar_Status_204_Quando_Atualizar_Com_Sucesso(){
        var pacienteDto = new CriarPacienteDto(){
            Nome = "José Santos",
            DataNascimento = new DateOnly(1985, 3, 10),
            Cpf = $"{Random.Shared.Next(100000000, 999999999)}{Random.Shared.Next(0, 99):D2}",
            Sexo = SexoEnum.Masculino,
            Cep = "45678912",
            Cidade = "Rio de Janeiro",
            Bairro = "Copacabana",
            Logradouro = "Av Atlântica, 500",
            Complemento = "Bloco A",
            Status = StatusEnum.Ativo
        };

        var createPacienteResponse = await _client.PostAsJsonAsync("/api/paciente", pacienteDto);
        
        if (!createPacienteResponse.IsSuccessStatusCode){
            var message = await createPacienteResponse.Content.ReadAsStringAsync();
            Assert.Fail($"Falha ao tentar criar paciente para teste: {message}");
        }
        
        var pacienteCriado = await createPacienteResponse.Content.ReadFromJsonAsync<JsonElement>();
        var pacienteId = pacienteCriado.GetProperty("id").GetGuid();
        
        var atendimentoDto = new CriarAtendimentoDto(){
            PacienteId = pacienteId,
            DataHora = DateTime.Now.AddDays(-2),
            Descricao = "Atendimento para atualização",
            Status = StatusEnum.Ativo
        };
        
        var createAtendimentoResponse = await _client.PostAsJsonAsync("/api/atendimento", atendimentoDto);
        
        if (!createAtendimentoResponse.IsSuccessStatusCode){
            var message = await createAtendimentoResponse.Content.ReadAsStringAsync();
            Assert.Fail($"Falha ao tentar criar atendimento: {message}");
        }
        
        var atendimentoCriado = await createAtendimentoResponse.Content.ReadFromJsonAsync<JsonElement>();
        var atendimentoId = atendimentoCriado.GetProperty("id").GetGuid();
        

        var atualizarDto = new AtualizarAtendimentoDto(){
            PacienteId = pacienteId,
            DataHora = DateTime.Now.AddDays(-3),
            Descricao = "Descrição atualizada"
        };
        
        var response = await _client.PutAsJsonAsync($"/api/atendimento/atualizar/{atendimentoId}", atualizarDto);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            Assert.Fail($"Falha no PUT: {response.StatusCode} - {error}");
        }

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Put_Deve_Retornar_Status_404_Quando_Id_Inexistente(){
        var idInexistente = Guid.NewGuid();
        
        var atualizarDto = new AtualizarAtendimentoDto(){
            PacienteId = Guid.NewGuid(),
            DataHora = DateTime.Now,
            Descricao = "Tentativa de atualização"
        };
        
        var response = await _client.PutAsJsonAsync($"/api/atendimento/atualizar/{idInexistente}", atualizarDto);
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task Put_Deve_Retornar_Status_400_Quando_Enviar_Dados_Invalidos(){
        var pacienteDto = new CriarPacienteDto(){
            Nome = "José Santos",
            DataNascimento = new DateOnly(1985, 3, 10),
            Cpf = $"{Random.Shared.Next(100000000, 999999999)}{Random.Shared.Next(0, 99):D2}",
            Sexo = SexoEnum.Masculino,
            Cep = "45678912",
            Cidade = "Rio de Janeiro",
            Bairro = "Copacabana",
            Logradouro = "Av Atlântica, 500",
            Complemento = "Bloco A",
            Status = StatusEnum.Ativo
        };

        var createPacienteResponse = await _client.PostAsJsonAsync("/api/paciente", pacienteDto);
        createPacienteResponse.EnsureSuccessStatusCode();
        var pacienteCriado = await createPacienteResponse.Content.ReadFromJsonAsync<JsonElement>();
        var pacienteId = pacienteCriado.GetProperty("id").GetGuid();

        var atendimentoDto = new CriarAtendimentoDto(){
            PacienteId = pacienteId,
            DataHora = DateTime.Now.AddDays(-2),
            Descricao = "Atendimento para atualização",
            Status = StatusEnum.Ativo
        };

        var createAtendimentoResponse = await _client.PostAsJsonAsync("/api/atendimento", atendimentoDto);
        createAtendimentoResponse.EnsureSuccessStatusCode();
        var atendimentoCriado = await createAtendimentoResponse.Content.ReadFromJsonAsync<JsonElement>();
        var atendimentoId = atendimentoCriado.GetProperty("id").GetGuid();

        var atualizarDto = new AtualizarAtendimentoDto(){
            PacienteId = pacienteId,
            DataHora = DateTime.Now.AddDays(3),
            Descricao = ""
        };

        var response = await _client.PutAsJsonAsync($"/api/atendimento/atualizar/{atendimentoId}", atualizarDto);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("data", content.ToLower());
    }
    
    [Fact]
    public async Task Patch_Deve_Retornar_Status_200_Quando_Atendimento_Ja_Estiver_Inativo(){
        var pacienteDto = new CriarPacienteDto{
            Nome = "Paciente Inativo",
            DataNascimento = new DateOnly(1990, 1, 1),
            Cpf = $"{Random.Shared.Next(100000000, 999999999)}{Random.Shared.Next(0, 99):D2}",
            Sexo = SexoEnum.Masculino,
            Cep = "87654321",
            Cidade = "Cidade X",
            Bairro = "Bairro Y",
            Logradouro = "Rua Z",
            Complemento = "",
            Status = StatusEnum.Ativo
        };

        var pacienteResponse = await _client.PostAsJsonAsync("/api/paciente", pacienteDto);
        pacienteResponse.EnsureSuccessStatusCode();
        var pacienteJson = await pacienteResponse.Content.ReadFromJsonAsync<JsonElement>();
        var pacienteId = pacienteJson.GetProperty("id").GetGuid();

        var atendimentoDto = new CriarAtendimentoDto{
            PacienteId = pacienteId,
            DataHora = DateTime.Now.AddDays(-2),
            Descricao = "Consulta já inativa",
            Status = StatusEnum.Inativo
        };

        var atendimentoResponse = await _client.PostAsJsonAsync("/api/atendimento", atendimentoDto);
        atendimentoResponse.EnsureSuccessStatusCode();
        var atendimentoJson = await atendimentoResponse.Content.ReadFromJsonAsync<JsonElement>();
        var atendimentoId = atendimentoJson.GetProperty("id").GetGuid();

        var response = await _client.PatchAsync($"/api/atendimento/inativar/{atendimentoId}", null);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var message = await response.Content.ReadAsStringAsync();
        Assert.Contains("atendimento ja está inativo.", message.ToLower());
    }
    
    [Fact]
    public async Task Patch_Deve_Retornar_Status_204_Quando_Inativar_Com_Sucesso(){
        var pacienteDto = new CriarPacienteDto{
            Nome = "Paciente Teste",
            DataNascimento = new DateOnly(1990, 1, 1),
            Cpf = $"{Random.Shared.Next(100000000, 999999999)}{Random.Shared.Next(0, 99):D2}",
            Sexo = SexoEnum.Masculino,
            Cep = "12345678",
            Cidade = "Cidade Teste",
            Bairro = "Bairro Teste",
            Logradouro = "Rua Teste",
            Complemento = "Apto 1",
            Status = StatusEnum.Ativo
        };

        var pacienteResponse = await _client.PostAsJsonAsync("/api/paciente", pacienteDto);
        pacienteResponse.EnsureSuccessStatusCode();
        var pacienteJson = await pacienteResponse.Content.ReadFromJsonAsync<JsonElement>();
        var pacienteId = pacienteJson.GetProperty("id").GetGuid();

        var atendimentoDto = new CriarAtendimentoDto{
            PacienteId = pacienteId,
            DataHora = DateTime.Now.AddDays(-1),
            Descricao = "Consulta 1",
            Status = StatusEnum.Ativo
        };

        var atendimentoResponse = await _client.PostAsJsonAsync("/api/atendimento", atendimentoDto);
        atendimentoResponse.EnsureSuccessStatusCode();
        var atendimentoJson = await atendimentoResponse.Content.ReadFromJsonAsync<JsonElement>();
        var atendimentoId = atendimentoJson.GetProperty("id").GetGuid();

        var response = await _client.PatchAsync($"/api/atendimento/inativar/{atendimentoId}", null);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            Assert.Fail($"Falha no PATCH: {response.StatusCode} - {error}");
        }
        
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
    
    
    [Fact]
    public async Task Patch_Deve_Retornar_Status_400_Quando_Atendimento_Nao_Existir_No_Inativar(){
        var idInexistente = Guid.NewGuid();
        var response = await _client.PatchAsync($"/api/atendimento/inativar/{idInexistente}", null);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("não encontrado", content.ToLower());
    }
    
    [Fact]
    public async Task Patch_Deve_Retornar_Status_200_Quando_Atendimento_Ja_Estiver_Ativo(){
        var pacienteDto = new CriarPacienteDto{
            Nome = "Paciente Já Ativo",
            DataNascimento = new DateOnly(1980, 5, 20),
            Cpf = $"{Random.Shared.Next(100000000, 999999999)}{Random.Shared.Next(0, 99):D2}",
            Sexo = SexoEnum.Feminino,
            Cep = "98765432",
            Cidade = "Cidade B",
            Bairro = "Bairro B",
            Logradouro = "Rua B",
            Complemento = "Bloco B",
            Status = StatusEnum.Ativo
        };

        var pacienteResponse = await _client.PostAsJsonAsync("/api/paciente", pacienteDto);
        var pacienteContent = await pacienteResponse.Content.ReadAsStringAsync();

        if (!pacienteResponse.IsSuccessStatusCode)
            Assert.Fail($"Erro ao criar paciente: {pacienteResponse.StatusCode} - {pacienteContent}");

        var pacienteJson = JsonDocument.Parse(pacienteContent).RootElement;
        var pacienteId = pacienteJson.GetProperty("id").GetGuid();

        var atendimentoDto = new CriarAtendimentoDto
        {
            PacienteId = pacienteId,
            DataHora = DateTime.Now.AddDays(-3),
            Descricao = "Consulta já ativa",
            Status = StatusEnum.Ativo
        };

        var atendimentoResponse = await _client.PostAsJsonAsync("/api/atendimento", atendimentoDto);
        var atendimentoContent = await atendimentoResponse.Content.ReadAsStringAsync();

        if (!atendimentoResponse.IsSuccessStatusCode)
            Assert.Fail($"Erro ao criar atendimento: {atendimentoResponse.StatusCode} - {atendimentoContent}");

        var atendimentoJson = JsonDocument.Parse(atendimentoContent).RootElement;
        var atendimentoId = atendimentoJson.GetProperty("id").GetGuid();

        var response = await _client.PatchAsync($"/api/atendimento/ativar/{atendimentoId}", null);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var message = await response.Content.ReadAsStringAsync();
        Assert.Contains("atendimento ja está ativado", message.ToLower());
    }
    
    [Fact]
    public async Task Patch_Deve_Retornar_Status_204_Quando_Ativar_Com_Sucesso(){
        var pacienteDto = new CriarPacienteDto{
            Nome = "Paciente Ativo",
            DataNascimento = new DateOnly(1990, 1, 1),
            Cpf = $"{Random.Shared.Next(100000000, 999999999)}{Random.Shared.Next(0, 99):D2}",
            Sexo = SexoEnum.Masculino,
            Cep = "12345678",
            Cidade = "Cidade A",
            Bairro = "Bairro A",
            Logradouro = "Rua A",
            Complemento = "",
            Status = StatusEnum.Ativo
        };

        var pacienteResponse = await _client.PostAsJsonAsync("/api/paciente", pacienteDto);
        pacienteResponse.EnsureSuccessStatusCode();
        var pacienteJson = await pacienteResponse.Content.ReadFromJsonAsync<JsonElement>();
        var pacienteId = pacienteJson.GetProperty("id").GetGuid();

        var atendimentoDto = new CriarAtendimentoDto{
            PacienteId = pacienteId,
            DataHora = DateTime.Now.AddDays(-2),
            Descricao = "Consulta para ativar",
            Status = StatusEnum.Inativo
        };

        var atendimentoResponse = await _client.PostAsJsonAsync("/api/atendimento", atendimentoDto);
        atendimentoResponse.EnsureSuccessStatusCode();
        var atendimentoJson = await atendimentoResponse.Content.ReadFromJsonAsync<JsonElement>();
        var atendimentoId = atendimentoJson.GetProperty("id").GetGuid();

        var response = await _client.PatchAsync($"/api/atendimento/ativar/{atendimentoId}", null);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
    
    [Fact]
    public async Task Patch_Deve_Retornar_Status_400_Quando_Atendimento_Nao_Existir_No_Ativar(){
        var idInexistente = Guid.NewGuid();
        var response = await _client.PatchAsync($"/api/atendimento/ativar/{idInexistente}", null);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("não encontrado", content.ToLower());
    }
}