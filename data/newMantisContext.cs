using Microsoft.EntityFrameworkCore;
using newMantis.Models;

public class newMantisContext : DbContext
{
    public newMantisContext (DbContextOptions<newMantisContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
     {
         builder.Entity<UtilisateurCategorie>().HasKey(table => new {
         table.email, table.idCategorie
         });
     }

    public DbSet<Utilisateur> Utilisateur { get; set; }

    public DbSet<Categorie> Categorie { get; set; } 

    public DbSet<UtilisateurCategorie> UtilisateurCategorie {get; set; }
}