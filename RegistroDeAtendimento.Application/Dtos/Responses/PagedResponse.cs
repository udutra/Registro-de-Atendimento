using System.Text.Json.Serialization;

namespace RegistroDeAtendimento.Application.Dtos.Responses;

public class PagedResponse<TData> : Response<TData>{
    [JsonConstructor]
    public PagedResponse(TData? data, int totalCount, int currentPage = ConfigurationResponse.DefaultCurrentPage,
        int pageSize = ConfigurationResponse.DefaultPageSize) : base(data){
        Data = data;
        TotalCount = totalCount;
        CurrentPage = currentPage;
        PageSize = pageSize;
    }

    public PagedResponse(TData? data, int code = ConfigurationResponse.DefaultStatusCode, string? message = null) :
        base(data, code, message){ }

    public int CurrentPage{ get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public int PageSize{ get; set; } = ConfigurationResponse.DefaultPageSize;
    public int TotalCount{ get; set; }
}