using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieTracker.Models;

namespace MovieTracker.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Movie> Movies { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<CommentRating> CommentRatings { get; set; }
    
    public DbSet<UserProfile> UserProfiles { get; set; }
    
    public DbSet<FavoriteMovie> FavoriteMovies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserProfile>()
            .HasOne(up => up.AppUser)
            .WithOne()
            .HasForeignKey<UserProfile>(up => up.AppUserId);

        modelBuilder.Entity<FavoriteMovie>()
            .HasKey(fm => new { fm.UserProfileId, fm.MovieId });

        modelBuilder.Entity<FavoriteMovie>()
            .HasOne(fm => fm.UserProfile)
            .WithMany(up => up.FavoriteMovies)
            .HasForeignKey(fm => fm.UserProfileId);

        modelBuilder.Entity<FavoriteMovie>()
            .HasOne(fm => fm.Movie)
            .WithMany(m => m.FavoriteMovies) 
            .HasForeignKey(fm => fm.MovieId);
        
        modelBuilder.Entity<CommentRating>()
            .HasOne(cr => cr.Comment)
            .WithMany(c => c.CommentRatings)
            .HasForeignKey(cr => cr.CommentId)
            .OnDelete(DeleteBehavior.NoAction);

    }
}