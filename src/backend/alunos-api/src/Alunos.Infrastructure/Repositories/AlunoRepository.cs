using Alunos.Domain.Entities;
using Alunos.Domain.Interfaces;
using Alunos.Domain.ValueObjects;
using Alunos.Infrastructure.Data;
using Core.Data;
using Core.Utils;
using Microsoft.EntityFrameworkCore;

namespace Alunos.Infrastructure.Repositories;

public class AlunoRepository(AlunoDbContext context) : IAlunoRepository
{
    public IUnitOfWork UnitOfWork => context;

    public async Task AdicionarAsync(Aluno aluno)
    {
        await context.Alunos.AddAsync(aluno);
    }

    public async Task AtualizarAsync(Aluno aluno)
    {
        aluno.AtualizarDataModificacao();
        context.Alunos.Update(aluno);
        await Task.CompletedTask;
    }

    public async Task<Aluno> ObterPorIdAsync(Guid alunoId, bool noTracked = true)
    {
        var query = context.Alunos.AsQueryable();

        if (noTracked)
        {
            query = query.AsNoTracking();
        }

        return await query
            .Include(a => a.MatriculasCursos)
            .ThenInclude(m => m.Certificado)
            .FirstOrDefaultAsync(a => a.CodigoUsuarioAutenticacao == alunoId);
    }

    public async Task<Aluno> ObterPorEmailAsync(string email, bool noTracked = true)
    {
        var query = context.Alunos.AsQueryable(); 

        if (noTracked)
        {
            query = query.AsNoTracking();
        }

        return await query.Include(a => a.MatriculasCursos).FirstOrDefaultAsync(a => a.Email == email);
    }

    public async Task<Aluno> ObterPorCodigoUsuarioAsync(Guid codigoUsuario, bool noTracked = true)
    {
        var query = context.Alunos.AsQueryable();

        if (noTracked)
        {
            query = query.AsNoTracking();
        }

        return await query
            .Include(a => a.MatriculasCursos)
            .ThenInclude(m => m.Certificado)
            .FirstOrDefaultAsync(a => a.CodigoUsuarioAutenticacao == codigoUsuario);
    }

    public async Task AdicionarMatriculaCursoAsync(MatriculaCurso matriculaCurso)
    {
        await context.MatriculasCursos.AddAsync(matriculaCurso);
    }

    public async Task AdicionarCertificadoMatriculaCursoAsync(Certificado certificado)
    {
        await context.Certificados.AddAsync(certificado);
    }

    public async Task<MatriculaCurso?> ObterMatriculaPorIdAsync(Guid matriculaId, bool noTracked = true)
    {
        var query = context.MatriculasCursos.AsQueryable();

        if (noTracked)
        {
            query = query.AsNoTracking();
        }

        return await query
            .Include(m => m.HistoricoAprendizado)
            .Include(m => m.Certificado)
            .FirstOrDefaultAsync(m => m.Id == matriculaId);
    }

    public async Task AtualizarEstadoHistoricoAprendizadoAsync(HistoricoAprendizado historicoAntigo, HistoricoAprendizado historicoNovo)
    {
        context.AtualizarEstadoValueObject(historicoAntigo, historicoNovo);
        await Task.CompletedTask;
    }

    public void Dispose()
    {
        context?.Dispose();
        GC.SuppressFinalize(this);
    }
}
