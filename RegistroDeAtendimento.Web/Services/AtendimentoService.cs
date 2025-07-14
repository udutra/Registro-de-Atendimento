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
    
    public Task<PagedResponse<List<AtendimentoResponseDto>>> ListarAtendimentosAsync(ListarAtendimentosDto dto){
        throw new NotImplementedException();
    }
    public Task<Response<AtendimentoResponseDto?>> ObterAtendimentoPorIdAsync(Guid id){
        throw new NotImplementedException();
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
    public Task<Response<AtendimentoResponseDto?>> AtualizarAtendimentoAsync(Guid id, AtualizarAtendimentoDto dto){
        throw new NotImplementedException();
    }
    public Task<Response<AtendimentoResponseDto?>> InativarAtendimentoAsync(Guid id){
        throw new NotImplementedException();
    }
    public Task<Response<AtendimentoResponseDto?>> AtivarAtendimentoAsync(Guid id){
        throw new NotImplementedException();
    }
}