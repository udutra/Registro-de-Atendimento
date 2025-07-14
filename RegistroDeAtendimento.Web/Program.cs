using System.Globalization;
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

var culture = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentCulture   = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

builder.Services.AddLocalization();

Configuration.BackendUrl = builder.Configuration.GetValue<string>("BackendUrl") ?? "http://localhost:5223";

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(Configuration.BackendUrl)
});

builder.Services.AddMudServices();

builder.Services.AddHttpClient(Configuration.HttpClientName, opt =>
    opt.BaseAddress = new Uri(Configuration.BackendUrl));

builder.Services.AddScoped<CriarPacienteDtoValidator>();
builder.Services.AddScoped<CriarAtendimentoDtoValidator>();
builder.Services.AddScoped<AtualizarPacienteDtoValidator>();
builder.Services.AddScoped<AtualizarAtendimentoDtoValidator>();
builder.Services.AddTransient<IPacienteService, PacienteService>();
builder.Services.AddTransient<IAtendimentoService, AtendimentoService>();

await builder.Build().RunAsync();