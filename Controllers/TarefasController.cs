using System.Linq;
using System.Threading.Tasks;
using GerenciadorDeTarefas.Data;
using GerenciadorDeTarefas.Models;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class TarefasController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TarefasController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTarefa(int id)
    {
        var tarefa = await _context.Tarefas.FindAsync(id);
        if (tarefa == null)
            return NotFound();
        return Ok(tarefa);
    }

    [HttpGet("ObterTodos")]
    public IActionResult ObterTodos()
    {
        var tarefas = _context.Tarefas.ToList();
        return Ok(tarefas);
    }

    [HttpGet("ObterPorTitulo")]
    public IActionResult ObterPorTitulo(string titulo)
    {
        var tarefas = _context.Tarefas.Where(t => t.Titulo.Contains(titulo)).ToList();
        return Ok(tarefas);
    }

    [HttpGet("ObterPorData")]
    public IActionResult ObterPorData(DateTime data)
    {
        var tarefas = _context.Tarefas.Where(t => t.Data.Date == data.Date).ToList();
        return Ok(tarefas);
    }

    [HttpGet("ObterPorStatus")]
    public IActionResult ObterPorStatus(StatusTarefa status)
    {
        var tarefas = _context.Tarefas.Where(t => t.Status == status).ToList();
        return Ok(tarefas);
    }

    [HttpPost]
    public async Task<IActionResult> CriarTarefa(Tarefa tarefa)
    {
        _context.Tarefas.Add(tarefa);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTarefa), new { id = tarefa.Id }, tarefa);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarTarefa(int id, Tarefa tarefaAtualizada)
    {
        var tarefa = await _context.Tarefas.FindAsync(id);
        if (tarefa == null)
            return NotFound();

        tarefa.Titulo = tarefaAtualizada.Titulo;
        tarefa.Descricao = tarefaAtualizada.Descricao;
        tarefa.Data = tarefaAtualizada.Data;
        tarefa.Status = tarefaAtualizada.Status;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarTarefa(int id)
    {
        var tarefa = await _context.Tarefas.FindAsync(id);
        if (tarefa == null)
            return NotFound();

        _context.Tarefas.Remove(tarefa);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
