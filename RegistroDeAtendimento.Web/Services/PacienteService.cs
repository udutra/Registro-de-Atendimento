using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using RegistroDeAtendimento.Shared.Application.Dtos;
using RegistroDeAtendimento.Shared.Application.Dtos.Responses;
using RegistroDeAtendimento.Shared.Application.Interfaces;
namespace RegistroDeAtendimento.Web.Services;

public class PacienteService(IHttpClientFactory httpClientFactory) : IPacienteService{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient(Configuration.HttpClientName);
    
    private readonly JsonSerializerOptions _options = new(){
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };
    
    public async Task<PagedResponse<List<PacienteResponseDto>>> ListarPacientesAsync(ListarPacientesDto dto){
        var queryString = ToQueryString(dto);
        var response = await _httpClient.GetAsync($"api/paciente{queryString}");

        var json = await response.Content.ReadAsStringAsync();
        var responseContent = JsonSerializer.Deserialize<PagedResponse<List<PacienteResponseDto>>>(json, _options);
        
        return responseContent ?? new PagedResponse<List<PacienteResponseDto>>(null, 500, "Erro interno no servidor");
    }
    
    public async Task<Response<PacienteResponseDto?>> ObterPacientePorIdAsync(Guid id){
        var response = await _httpClient.GetAsync($"api/paciente/{id}");

        var json = await response.Content.ReadAsStringAsync();
        var responseContent = JsonSerializer.Deserialize<Response<PacienteResponseDto?>>(json, _options);
        
        return responseContent ?? new Response<PacienteResponseDto?>(null, 500, "Erro interno no servidor");
    }
    
    public async Task<Response<PacienteResponseDto?>> CriarPacienteAsync(CriarPacienteDto dto){
        var response = await _httpClient.PostAsJsonAsync("api/paciente", dto);
        var json = await response.Content.ReadAsStringAsync();
        var responseContent = JsonSerializer.Deserialize<Response<PacienteResponseDto?>>(json, _options);
        
        if (responseContent != null){
            responseContent.Code = (int)response.StatusCode;
        }

        return responseContent ?? new Response<PacienteResponseDto?>(null, 500, "Erro interno no servidor");
    }
    
    public async Task<Response<PacienteResponseDto?>> AtualizarPacienteAsync(Guid id, AtualizarPacienteDto dto)
    {
        var httpResponse = await _httpClient.PutAsJsonAsync($"api/paciente/atualizar/{id}", dto);

        if (httpResponse.StatusCode == System.Net.HttpStatusCode.NoContent){
            
            return new Response<PacienteResponseDto?>(null, 204, "Paciente atualizado com sucesso!");
        }
        
        var json = await httpResponse.Content.ReadAsStringAsync();

        if (string.IsNullOrEmpty(json))
        {
            return new Response<PacienteResponseDto?>(null, (int)httpResponse.StatusCode, "A resposta da API foi vazia ou inválida.");
        }
    
        var responseContent = JsonSerializer.Deserialize<Response<PacienteResponseDto?>>(json, _options);
    
        return responseContent ?? new Response<PacienteResponseDto?>(null, 500, "Não foi possível interpretar a resposta do servidor.");
    }
    
    public async Task<Response<PacienteResponseDto?>> InativarPacienteAsync(Guid id){
        var response = await _httpClient.PatchAsJsonAsync<object?>($"api/paciente/inativar/{id}", null);
        var json = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            return new Response<PacienteResponseDto?>(null, (int)response.StatusCode, json);

        if (string.IsNullOrWhiteSpace(json))
            return new Response<PacienteResponseDto?>(null, (int)response.StatusCode, "Resposta vazia da API");

        var responseContent = JsonSerializer.Deserialize<Response<PacienteResponseDto?>>(json, _options);
        if (responseContent != null)
            responseContent.Code = (int)response.StatusCode;

        return responseContent ?? new Response<PacienteResponseDto?>(null, 500, "Erro ao desserializar a resposta");
    }
   
    public async Task<Response<PacienteResponseDto?>> AtivarPacienteAsync(Guid id){
        var response = await _httpClient.PatchAsJsonAsync<object?>($"api/paciente/ativar/{id}", null);
        var json = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            return new Response<PacienteResponseDto?>(null, (int)response.StatusCode, json);

        if (string.IsNullOrWhiteSpace(json))
            return new Response<PacienteResponseDto?>(null, (int)response.StatusCode, "Resposta vazia da API");

        var responseContent = JsonSerializer.Deserialize<Response<PacienteResponseDto?>>(json, _options);
        if (responseContent != null)
            responseContent.Code = (int)response.StatusCode;

        return responseContent ?? new Response<PacienteResponseDto?>(null, 500, "Erro ao desserializar a resposta");
    }
    
    private static string ToQueryString(object obj){
        var properties = from p in obj.GetType().GetProperties()
            let value = p.GetValue(obj)
            where value != null
            select $"{Uri.EscapeDataString(p.Name)}={Uri.EscapeDataString(value.ToString())}";

        return "?" + string.Join("&", properties);
    }
}