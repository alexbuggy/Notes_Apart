namespace NotesApart.Repositories.Interfaces
{
    public interface IRepositoryWrapper
    {
        IAlbumRepository AlbumRepository { get; }
        IArtistRepository ArtistRepository { get; }
        ISongRepository SongRepository { get; }
        IReviewRepository ReviewRepository { get; }
        IFavouriteAlbumRepository FavouriteAlbumRepository { get; }
        IFavouriteSongRepository FavouriteSongRepository { get; }
        ILikedReviewRepository LikedReviewRepository { get; }
        IUserRepository UserRepository { get; }
        void Save();
    }
}
