using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UserDocuments.Models;

namespace UserDocuments.Services
{
    public class DocumentStorageService
    {
        private readonly string _root;
        private readonly UserDocsDbContext _db;

        public DocumentStorageService(IConfiguration cfg, UserDocsDbContext db)
        {
            _root = cfg["Storage:Root"]!;
            _db = db;
        }

        public async Task<string> SaveAsync(
            Guid userId,
            string originalFileName,
            string mimeType,
            Action<string> saveToPath)
        {
            var fileId = Guid.NewGuid().ToString("N");
            var ext = Path.GetExtension(originalFileName);
            var rel = Path.Combine("users", userId.ToString(),
                                   DateTime.UtcNow.ToString("yyyy"),
                                   DateTime.UtcNow.ToString("MM"));
            var dir = Path.Combine(_root, rel);
            Directory.CreateDirectory(dir);

            var safeName = fileId + ext;
            var fullPath = Path.Combine(dir, safeName);

            // usa la callback per salvare fisicamente
            try { 
                saveToPath(fullPath); 
            }
            catch {

            }

            // ottieni dimensione dal file creato
            var fi = new FileInfo(fullPath);

            _db.UserDocs.Add(new UserDoc
            {
                Id = fileId,
                UserId = userId,
                FileNameOriginal = Path.GetFileName(originalFileName),
                StoragePath = Path.Combine(rel, safeName).Replace('\\', '/'),
                Size = fi.Length,
                MimeType = mimeType,
                CreatedAtUtc = DateTime.UtcNow
            });
            await _db.SaveChangesAsync();
            return fullPath;
        }

        public async Task<Stream> GetAsync(Guid userId, string fileId)
        {
            var f = await _db.UserDocs
                             .Where(x => x.UserId == userId && x.Id == fileId)
                             .SingleAsync();

            var fullPath = Path.Combine(_root, f.StoragePath);
            return new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public async Task<(Stream Stream, string MimeType, string FileNameOriginal)> GetForDownloadAsync(Guid userId, string fileId)
        {
            var f = await _db.UserDocs
                             .Where(x => x.UserId == userId && x.Id == fileId)
                             .SingleAsync();

            var fullPath = Path.Combine(_root, f.StoragePath.Replace('/', Path.DirectorySeparatorChar));
            var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return (stream, f.MimeType, f.FileNameOriginal);
        }

        public async Task DeleteAsync(Guid userId, string fileId)
        {
            var doc = await _db.UserDocs.FirstOrDefaultAsync(d => d.Id == fileId);
            if (doc == null) return;

            var fullPath = Path.Combine(_root, doc.StoragePath.Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(fullPath))
                File.Delete(fullPath);

            _db.UserDocs.Remove(doc);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserDoc>> ListAsync(Guid userId, int limit)
        {
            return await _db.UserDocs
                .AsNoTracking()
                .Where(d => d.UserId == userId)
                .OrderByDescending(d => d.CreatedAtUtc)
                .Take(limit)
                .ToListAsync();
        }

    }

}
