using System.Collections.Generic;
using System.Reflection.Emit;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace NotesApart.Models
{
    public class NotesApartDbContext : IdentityDbContext<User>
    {
        public NotesApartDbContext(DbContextOptions<NotesApartDbContext> options) : base(options) { }



        public DbSet<Artist> Artists { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<FavouriteSong> FavouriteSongs { get; set; }
        public DbSet<FavouriteAlbum> FavouriteAlbums { get; set; }
        public DbSet<LikedReview> LikedReviews { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);




            modelBuilder.Entity<Album>()
                .HasOne(a => a.Artist)
                .WithMany(ar => ar.Albums)
                .HasForeignKey(a => a.ArtistId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Song>()
                .HasOne(s => s.Album)
                .WithMany(a => a.Songs)
                .HasForeignKey(s => s.AlbumId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Song>()
                .HasOne(s => s.Artist)
                .WithMany(a => a.Songs)
                .HasForeignKey(s => s.ArtistId)
                .OnDelete(DeleteBehavior.Restrict);

          
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Album)
                .WithMany(a => a.Reviews)
                .HasForeignKey(r => r.AlbumId)
                .OnDelete(DeleteBehavior.Restrict);

            
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Song)
                .WithMany(s => s.Reviews)
                .HasForeignKey(r => r.SongId)
                .OnDelete(DeleteBehavior.Restrict);




       
            modelBuilder.Entity<FavouriteSong>()
                .HasKey(fs => new { fs.UserId, fs.SongId });

            modelBuilder.Entity<FavouriteSong>()
                .HasOne(fs => fs.Song)
                .WithMany(s => s.FavouriteSongs)
                .HasForeignKey(fs => fs.SongId)
                .OnDelete(DeleteBehavior.Restrict);






            modelBuilder.Entity<FavouriteAlbum>()
                .HasKey(fa => new { fa.UserId, fa.AlbumId });

            modelBuilder.Entity<FavouriteAlbum>()
                .HasOne(fa => fa.Album)
                .WithMany(a => a.FavouriteAlbums)
                .HasForeignKey(fa => fa.AlbumId)
                .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<Review>()
            .HasOne(r => r.User)
             .WithMany(u => u.Reviews)
             .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FavouriteSong>()
                .HasOne(fs => fs.User)
                .WithMany(u => u.FavoriteSongs)
                .HasForeignKey(fs => fs.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FavouriteAlbum>()
                .HasOne(fa => fa.User)
                .WithMany(u => u.FavoriteAlbums)
                .HasForeignKey(fa => fa.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LikedReview>()
                .HasOne(lr => lr.User)
                .WithMany(u => u.LikedReviews)
                .HasForeignKey(lr => lr.UserId)
                .OnDelete(DeleteBehavior.Cascade);




            modelBuilder.Entity<LikedReview>()
                .HasKey(lr => new { lr.UserId, lr.ReviewId });

            modelBuilder.Entity<LikedReview>()
                .HasOne(lr => lr.Review)
                .WithMany(r => r.LikedByUsers)
                .HasForeignKey(lr => lr.ReviewId)
                .OnDelete(DeleteBehavior.Restrict);





            modelBuilder.Entity<Review>()
                .HasCheckConstraint("CK_Review_SongOrAlbum",
                    "(SongId IS NOT NULL AND AlbumId IS NULL) OR (SongId IS NULL AND AlbumId IS NOT NULL)");

        }
    }
}
