using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TarefasAPI.Context;
using TarefasAPI.Models;

namespace TarefasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefasController(DataContext dataContext) : ControllerBase
    {
        private readonly DataContext _dataContext = dataContext;

        [HttpGet]
        [Authorize]
        // Obter todas as tarefas do usuário autenticado
        public ActionResult<IEnumerable<Tarefa>> GetTarefas()
        {
            try
            {
                string Email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
                if (Email == null) return BadRequest("Não foi possivel autenticar, por favor refaça o login.");

                IEnumerable<Tarefa> tarefas = new List<Tarefa>();

                tarefas = _dataContext.Tarefas.Where(t => t.Usuario.Email == Email);

                var result = tarefas.Select(t=> new {t.Id, t.Titulo, t.Descricao}).ToList();

                return Ok(result);
            }
            catch (Exception ex) {
                return BadRequest("Não foi possivel obter a lista de tarefas: " + ex.Message);
            }
        }

        [HttpGet("{Page}")]
        [Authorize]
        // Obter uma lista de tarefas de uma página específica
        public ActionResult<IEnumerable<Tarefa>> GetTarefasPagina(int Page)
        {
            try
            {
                string Email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
                if (Email == null) return BadRequest("Não foi possivel autenticar, por favor refaça o login.");

                int InicioPag = (Page - 1) * 10;

                var tarefas = _dataContext.Tarefas.Where(t => t.Usuario.Email == Email).Skip(InicioPag).Take(10);

                var result = tarefas.Select(t => new { t.Id, t.Titulo, t.Descricao }).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Não foi possivel obter a lista de tarefas: " + ex.Message);
            }
        }
        
        [HttpPost]
        [Authorize]
        // Cadastro de tarefa
        public ActionResult<string> CadastrarTarefa(TarefaRequest request)
        {
            try
            {
                string Email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
                if (Email == null) return BadRequest("Não foi possivel autenticar, por favor refaça o login.");
                
                Tarefa tarefa = new()
                {
                    Titulo = request.Titulo,
                    Descricao = request.Descricao,
                    Usuario = _dataContext.Usuarios.Where(u => u.Email == Email).FirstOrDefault()
                };
                
                if (tarefa.Usuario == null) return BadRequest("Usuário inválido, por favor refaça o login");
                
                _dataContext.Tarefas.Add(tarefa);
                _dataContext.SaveChanges();
                
                return Ok("Tarefa cadastrada com sucesso!");
            }
            catch(Exception ex)
            {
                return BadRequest("Não foi possivel cadastrar a tarefa: " + ex.Message);
            }
        }
        
        [HttpPut("{id}")]
        [Authorize]
        // Atualizar uma tarefa
        public ActionResult<string> AtualizarTarefa(TarefaRequest request, int id)
        {
            try
            {
                string Email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
                if (Email == null) return BadRequest("Não foi possivel autenticar, por favor refaça o login.");
                var UsuarioId = _dataContext.Usuarios.Where(u => u.Email == Email).FirstOrDefault().Id;

                var TarefaAtual = _dataContext.Tarefas.Where(t => t.Id == id && t.Usuario.Id == UsuarioId).FirstOrDefault();
                if (TarefaAtual == null) return BadRequest("Tarefa não encontrada, por favor tente novamente.");

                TarefaAtual.Titulo = request.Titulo;
                TarefaAtual.Descricao = request.Descricao;
                
                _dataContext.Tarefas.Update(TarefaAtual);
                _dataContext.SaveChanges();

                return Ok("Tarefa atualizada com sucesso!");
            }
            catch(Exception ex)
            {
                return BadRequest("Não foi possivel atualizar a tarefa: " + ex.Message);
            }
        }
        
        [HttpDelete("{id}")]
        [Authorize]
        // Remover uma tarefa
        public ActionResult<string> RemoverTarefa(int id)
        {
            try
            {
                string Email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
                if (Email == null) return BadRequest("Não foi possivel autenticar, por favor refaça o login.");

                var TarefaAtual = _dataContext.Tarefas.Where(t => t.Id == id && t.Usuario.Email == Email).FirstOrDefault();
                if (TarefaAtual == null) return BadRequest("Tarefa não encontrada, por favor tente novamente");

                _dataContext.Tarefas.Remove(TarefaAtual);
                _dataContext.SaveChanges();

                return Ok("Tarefa removida com sucesso");
            }
            catch (Exception ex) {
                return BadRequest("Não foi possivel remover sua tarefa: " + ex.Message);
            }
        }
    }
}
