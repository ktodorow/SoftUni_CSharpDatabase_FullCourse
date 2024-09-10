using MusicHub.Data;
using MusicHub.Data.Models;
using MusicHub.Initializer;
using System.Text;

namespace MusicHub
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            MusicHubDbContext context = new();

            DbInitializer.ResetDatabase(context);

            //Console.WriteLine(ExportAlbumsInfo(context, 7));
            Console.WriteLine(ExportSongsAboveDuration(context, 4));

        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums = context
                .Producers.FirstOrDefault(p => p.Id == producerId)
                .Albums.Select(a => new
                {
                    a.ProducerId,
                    a.Name,
                    a.ReleaseDate,
                    ProducerName = a.Producer.Name,
                    Songs = a.Songs
                        .OrderByDescending(s => s.Name)
                        .ThenBy(s => s.Writer.Name)
                        .Select(s => new
                        {
                            s.Name,
                            s.Price,
                            WriterName = s.Writer.Name
                        }),
                    TotalPrice = a.Price,
                })
                .OrderByDescending(a => a.TotalPrice).ToList();

            StringBuilder sb = new();

            foreach (var a in albums) 
            { 
                sb.AppendLine($"-AlbumName: {a.Name}");
                sb.AppendLine($"-ReleaseDate: {a.ReleaseDate:MM/dd/yyyy}");
                sb.AppendLine($"-ProducerName: {a.ProducerName}");

                if(a.Songs.Any())
                {
                    sb.AppendLine($"-Songs:");
                    int counter = 1;

                    foreach (var s in a.Songs) 
                    {
                        sb.AppendLine($"---#{counter++}");
                        sb.AppendLine($"---SongName: {s.Name}");
                        sb.AppendLine($"---Price: {s.Price:f2}");
                        sb.AppendLine($"---Writer: {s.WriterName}");
                    }
                }

                sb.AppendLine($"-AlbumPrice: {a.TotalPrice:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            //converting integer to seconds
            TimeSpan seconds = TimeSpan.FromSeconds(duration);

            var songs = context.Songs
                .AsEnumerable()
                .Where(s => s.Duration > seconds)
                .Select(s => new
                {
                    s.Name,
                    Perfomers = s.SongPerformers
                        .Select(sp => $"{sp.Performer.FirstName} {sp.Performer.LastName}")
                        .OrderBy(pName => pName)
                        .ToList(),
                    WriterName = s.Writer.Name,
                    AlbumProducerName = s.Album.Producer.Name,
                    s.Duration
                })
                .OrderBy(s => s.Name)
                .ThenBy(s => s.WriterName)
                .ToList();

            StringBuilder sb = new();

            int counter = 1;

            foreach (var s in songs)
            {
                sb.AppendLine($"-Song #{counter++}");
                sb.AppendLine($"---SongName: {s.Name}");
                sb.AppendLine($"---Writer: {s.WriterName}");

                if(s.Perfomers.Any())
                {
                    foreach (var p in s.Perfomers)
                    {
                        sb.AppendLine($"---Performer: {p}");
                    }
                }
                
                sb.AppendLine($"---AlbumProducer: {s.AlbumProducerName}");
                sb.AppendLine($"---Duration: {s.Duration:c}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}