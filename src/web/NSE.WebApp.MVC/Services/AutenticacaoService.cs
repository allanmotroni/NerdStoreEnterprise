﻿using NSE.WebApp.MVC.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services
{
    public class AutenticacaoService : Service, IAutenticacaoService
    {
        private readonly HttpClient _httpClient;

        public AutenticacaoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UsuarioRespostaLogin> Login(UsuarioLogin usuarioLogin)
        {
            var loginContent = new StringContent(JsonSerializer.Serialize(usuarioLogin), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:44363/api/identidade/autenticar", loginContent);

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            if (!TratarErrosResponse(response))
            {
                var resposta = await response.Content.ReadAsStringAsync();
                var usuarioRespostaLogin = new UsuarioRespostaLogin
                {
                    ResponseResult = JsonSerializer.Deserialize<ResponseResult>(resposta, options)
                };

                return usuarioRespostaLogin;
            }

            return JsonSerializer.Deserialize<UsuarioRespostaLogin>(await response.Content.ReadAsStringAsync(), options);
        }

        public async Task<UsuarioRespostaLogin> Registro(UsuarioRegistro usuarioRegistro)
        {
            var loginContent = new StringContent(JsonSerializer.Serialize(usuarioRegistro), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:44363/api/identidade/nova-conta", loginContent);

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            if (!TratarErrosResponse(response))
            {
                return new UsuarioRespostaLogin
                {
                    ResponseResult = JsonSerializer.Deserialize<ResponseResult>(await response.Content.ReadAsStringAsync(), options)
                };
            }

            return JsonSerializer.Deserialize<UsuarioRespostaLogin>(await response.Content.ReadAsStringAsync(), options);
        }
    }
}
