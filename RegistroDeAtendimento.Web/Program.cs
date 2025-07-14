using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using RegistroDeAtendimento.Shared.Application.Interfaces;
using RegistroDeAtendimento.Shared.Application.Validators;
using RegistroDeAtendimento.Web;
using RegistroDeAtendimento.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// ✅ Define o valor ANTES de usar
Configuration.BackendUrl = builder.Configuration.GetValue<string>("BackendUrl") ?? "http://localhost:5223";

// ✅ Usa BackendUrl agora que já está preenchida
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(Configuration.BackendUrl)
});

builder.Services.AddMudServices();

// HttpClient nomeado (opcional, se estiver usando IHttpClientFactory)
builder.Services.AddHttpClient(Configuration.HttpClientName, opt =>
    opt.BaseAddress = new Uri(Configuration.BackendUrl));

builder.Services.AddScoped<CriarPacienteDtoValidator>();
builder.Services.AddTransient<IPacienteService, PacienteService>();

await builder.Build().RunAsync();