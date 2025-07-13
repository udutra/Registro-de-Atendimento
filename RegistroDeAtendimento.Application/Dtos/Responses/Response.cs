using System.Text.Json.Serialization;

namespace RegistroDeAtendimento.Application.Dtos.Responses;

public class Response<TData>{
    public int Code;

    [JsonConstructor]
    public Response(){
        Code = ConfigurationResponse.DefaultStatusCode;
    }

    public Response(TData? data, int code = ConfigurationResponse.DefaultStatusCode, string? message = null){
        Code = code;
        Data = data;
        Message = message;
    }

    public TData? Data{ get; set; }
    public string? Message{ get; set; }

    [JsonIgnore] public bool IsSuccess => Code is >= 200 and <= 299;
}