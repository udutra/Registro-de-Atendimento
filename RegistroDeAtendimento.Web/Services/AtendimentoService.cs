using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using RegistroDeAtendimento.Core.Domain.Entities;
using RegistroDeAtendimento.Shared.Application.Dtos;
using RegistroDeAtendimento.Shared.Application.Dtos.Responses;
using RegistroDeAtendimento.Shared.Application.Interfaces;

namespace RegistroDeAtendimento.Web.Services;

public class AtendimentoService(IHttpClientFactory httpClientFactory) : IAtendimentoService{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient(Configuration.HttpClientName);
    
    private readonly JsonSerializerOptions _options = new(){
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };
    
    public async Task<PagedResponse<List<AtendimentoResponseDto>>> ListarAtendimentosAsync(ListarAtendimentosDto dto){
        var queryString = ToQueryString(dto);
        var response = await _httpClient.GetAsync($"api/atendimento{queryString}");

        var json = await response.Content.ReadAsStringAsync();
        var responseContent = JsonSerializer.Deserialize<PagedResponse<List<AtendimentoResponseDto>>>(json, _options);
        
        return responseContent ?? new PagedResponse<List<AtendimentoResponseDto>>(null, 500, "Erro interno no servidor");
    }
    public async Task<Response<AtendimentoResponseDto?>> ObterAtendimentoPorIdAsync(Guid id){
        var response = await _httpClient.GetAsync($"api/atendimento/{id}");

        var json = await response.Content.ReadAsStringAsync();
        var responseContent = JsonSerializer.Deserialize<Response<AtendimentoResponseDto?>>(json, _options);
        
        return responseContent ?? new Response<AtendimentoResponseDto?>(null, 500, "Erro interno no servidor");
    }
    public async Task<Response<AtendimentoResponseDto?>> CriarAtendimentoAsync(CriarAtendimentoDto dto){
        var response = await _httpClient.PostAsJsonAsync("api/atendimento", dto);
        var json = await response.Content.ReadAsStringAsync();
        var responseContent = JsonSerializer.Deserialize<Response<AtendimentoResponseDto?>>(json, _options);
        
        if (responseContent != null){
            responseContent.Code = (int)response.StatusCode;
        }

        return responseContent ?? new Response<AtendimentoResponseDto?>(null, 500, "Erro interno no servidor");
    }
    
    public async Task<Response<AtendimentoResponseDto?>> AtualizarAtendimentoAsync(Guid id, AtualizarAtendimentoDto dto){
        var httpResponse = await _httpClient.PutAsJsonAsync($"api/atendimento/atualizar/{id}", dto);

        if (httpResponse.StatusCode == System.Net.HttpStatusCode.NoContent){
            
            return new Response<AtendimentoResponseDto?>(null, 204, "Atendimento atualizado com sucesso!");
        }
        
        var json = await httpResponse.Content.ReadAsStringAsync();

        if (string.IsNullOrEmpty(json))
        {
            return new Response<AtendimentoResponseDto?>(null, (int)httpResponse.StatusCode, "A resposta da API foi vazia ou inválida.");
        }
    
        var responseContent = JsonSerializer.Deserialize<Response<AtendimentoResponseDto?>>(json, _options);
    
        return responseContent ?? new Response<AtendimentoResponseDto?>(null, 500, "Não foi possível interpretar a resposta do servidor.");
    }
    
    public async Task<Response<AtendimentoResponseDto?>> InativarAtendimentoAsync(Guid id){
        var response = await _httpClient.PatchAsJsonAsync<object?>($"api/atendimento/inativar/{id}", null);
        var json = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            return new Response<AtendimentoResponseDto?>(null, (int)response.StatusCode, json);

        if (string.IsNullOrWhiteSpace(json))
            return new Response<AtendimentoResponseDto?>(null, (int)response.StatusCode, "Resposta vazia da API");

        var responseContent = JsonSerializer.Deserialize<Response<AtendimentoResponseDto?>>(json, _options);
        if (responseContent != null)
            responseContent.Code = (int)response.StatusCode;

        return responseContent ?? new Response<AtendimentoResponseDto?>(null, 500, "Erro ao desserializar a resposta");
    }
    
    public async Task<Response<AtendimentoResponseDto?>> AtivarAtendimentoAsync(Guid id){
        var response = await _httpClient.PatchAsJsonAsync<object?>($"api/atendimento/ativar/{id}", null);
        var json = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            return new Response<AtendimentoResponseDto?>(null, (int)response.StatusCode, json);

        if (string.IsNullOrWhiteSpace(json))
            return new Response<AtendimentoResponseDto?>(null, (int)response.StatusCode, "Resposta vazia da API");

        var responseContent = JsonSerializer.Deserialize<Response<AtendimentoResponseDto?>>(json, _options);
        if (responseContent != null)
            responseContent.Code = (int)response.StatusCode;

        return responseContent ?? new Response<AtendimentoResponseDto?>(null, 500, "Erro ao desserializar a resposta");
    }
    
    private static string ToQueryString(object obj)
    {
        var properties = from p in obj.GetType().GetProperties()
            let value = p.GetValue(obj)
            where value != null
            select $"{Uri.EscapeDataString(p.Name)}={Uri.EscapeDataString(FormatValue(value))}";

        return "?" + string.Join("&", properties);

        static string FormatValue(object value)
        {
            return value switch
            {
                DateTime dt => dt.ToString("yyyy-MM-ddTHH:mm:ss"),
                DateOnly d => d.ToString("yyyy-MM-dd"),
                _ => value.ToString() ?? string.Empty
            };
        }
    }
}