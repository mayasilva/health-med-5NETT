﻿using Core.Entity;
using Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class MedicoRepository : IMedicoRepository
    {
        protected ApplicationDbContext _context;
        protected DbSet<Medico> _dbSet;

        public MedicoRepository(ApplicationDbContext contexto)
        {
            _context = contexto;
            _dbSet = _context.Set<Medico>();
        }

        public void Alterar(Medico entidade)
        {
            _dbSet.Update(entidade);
            _context.SaveChanges();
        }

        public void Cadastrar(Medico entidade)
        {
            _dbSet.Add(entidade);
            _context.SaveChanges();
        }

        public void Deletar(string crm)
        {
            _dbSet.Remove(ObterPorCrm(crm));
            _context.SaveChanges();
        }

        public Medico? ObterAgendaPorCrm(string crm)
        {
            return _dbSet
                .Include(m => m.Agendas)
                .FirstOrDefault(entity => entity.Crm == crm);
        }

        public Medico ObterPorCrm(string crm)
            => _dbSet.FirstOrDefault(entity => entity.Crm == crm);


        public IList<Medico> ObterTodos(string? especialidade)
        {
            if(string.IsNullOrEmpty(especialidade))
            {
                return _dbSet.ToList();
            }
            
            return _dbSet.Where(m => m.Especialidade == especialidade).ToList();
        }
    }
}
