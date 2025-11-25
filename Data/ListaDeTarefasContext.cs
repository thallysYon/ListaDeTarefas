using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ListaDeTarefas.Models;

namespace ApplicationDbContext.Data
{
    public class ListaDeTarefasContext : DbContext
    {
        public ListaDeTarefasContext (DbContextOptions<ListaDeTarefasContext> options)
            : base(options)
        {
        }

        public DbSet<ListaDeTarefas.Models.CategoriaModel> CategoriaModel { get; set; } = default!;
        public DbSet<ListaDeTarefas.Models.TarefaModel> TarefaModel { get; set; } = default!;
    }
}
