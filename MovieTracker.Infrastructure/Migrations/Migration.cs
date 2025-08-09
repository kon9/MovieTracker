namespace MovieTracker.Infrastructure.Migrations
{
    public abstract class Migration
    {
        public abstract int Version { get; }
        public abstract string Description { get; }
        public abstract string UpScript { get; }
        public abstract string DownScript { get; }
    }
}

