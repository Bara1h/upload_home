using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using upload.Models;

public class FileDbContext : DbContext
{
    public DbSet<FileMetadata> FileMetadatas { get; set; }
    public FileUploadViewModel FileUploadViewModel { get; set; }
    public List<FileMetadata> Files { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FileMetadata>().ToTable("FileMetadatas");
    }
    public FileDbContext(DbContextOptions<FileDbContext> options) : base(options) { 
    
    }
}
