using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TarefasAPI.Models;
using System.Text.RegularExpressions;
using TarefasAPI.Context;
using Microsoft.AspNetCore.Authorization;

namespace TarefasAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(DataContext dataContext) : ControllerBase
    {
        private readonly DataContext _dataContext = dataContext;

        public class Result
        {
            public string Token { get; set; } = string.Empty;
            public string Nome { get; set; } = string.Empty;
        };

        // Cadastro
        [HttpPost("cadastro")]
        public ActionResult<Usuario> Cadastro(UsuarioRequest request)
        {
            try
            {
                Usuario user = new();
                if (!ValidarSenha(request.Senha)) return BadRequest("A senha precisa conter pelo menos seis caracteres, uma letra maiúscula, uma letra minúscula, um número e um máximo de dezesseis carácteres.");

                string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Senha);

                user.Nome = request.Nome;
                user.Email = request.Email;
                user.PasswordHash = passwordHash;

                _dataContext.Usuarios.Add(user);
                _dataContext.SaveChanges();

                return Ok("Usuário cadastrado com sucesso!");
            }
            catch
            {
                return BadRequest("Não foi possível realizar o cadastro, tente cadastrar outro email.");
            }
        }

        // Login
        [HttpPost("login")]
        public ActionResult<Result> Login(LoginRequest request)
        {
            var user = _dataContext.Usuarios.Where(u => u.Email == request.Email).FirstOrDefault();
            if (user == null) return BadRequest("Usuário não encontrado");
            
            if(!BCrypt.Net.BCrypt.Verify(request.Senha, user.PasswordHash)) return BadRequest("Usuário ou senha inválidos.");
            
            Result result = new ()
            {
                Nome = user.Nome,
                Token = CriarToken(user)
            };
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        public ActionResult<string> Ping()
        {
           return Ok("Pong!");
        }

        // Validar senha com Regex
        private static bool ValidarSenha(string Senha)
        {
            var Numero = new Regex(@"[0-9]+");
            var CaixaAlta = new Regex(@"[A-Z]+");
            var CaixaBaixa = new Regex(@"[a-z]+");
            var Simbolo = new Regex(@"[!@#$%^&*(),.?\"":{ }|<>]");
            var QuantidadeChar = new Regex(@".{6,16}");

            return Numero.IsMatch(Senha) && CaixaAlta.IsMatch(Senha) &&
                   CaixaBaixa.IsMatch(Senha) && Simbolo.IsMatch(Senha) &&
                   QuantidadeChar.IsMatch(Senha);
        }

        // Criar Token JWT
        private static string CriarToken(Usuario user)
        {
            List<Claim> claims =
            [
                new Claim(ClaimTypes.Name, user.Nome),
                new Claim(ClaimTypes.Email, user.Email)
            ];

            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Settings.Key));
            var Credencial = new SigningCredentials(Key, SecurityAlgorithms.HmacSha512Signature);
            var SecurityToken = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(2), signingCredentials: Credencial);

            var jwt = new JwtSecurityTokenHandler().WriteToken(SecurityToken);

            return jwt;
        }
    }
}
