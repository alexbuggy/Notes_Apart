using NotesApart.Models;
using NotesApart.Repositories.Interfaces;

namespace NotesApart.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private NotesApartDbContext _notesapartContext;
  

        private IAlbumRepository? _AlbumRepository;

        public IAlbumRepository AlbumRepository
        {
            get
            {
                if (_AlbumRepository == null)
                {
                    _AlbumRepository = new AlbumRepository(_notesapartContext);
                }

                return _AlbumRepository;
            }
        }

        private IArtistRepository? _ArtistRepository;

        public IArtistRepository ArtistRepository
        {
            get
            {
                if (_ArtistRepository == null)
                {
                    _ArtistRepository = new ArtistRepository(_notesapartContext);
                }

                return _ArtistRepository;
            }
        }

        private ISongRepository? _SongRepository;

        public ISongRepository SongRepository
        {
            get
            {
                if (_SongRepository == null)
                {
                    _SongRepository = new SongRepository(_notesapartContext);
                }

                return _SongRepository;
            }
        }


        private IReviewRepository? _ReviewRepository;

        public IReviewRepository ReviewRepository
        {
            get
            {
                if (_ReviewRepository == null)
                {
                    _ReviewRepository = new ReviewRepository(_notesapartContext);
                }

                return _ReviewRepository;
            }
        }


        private IFavouriteAlbumRepository? _FavouriteAlbumRepository;

        public IFavouriteAlbumRepository FavouriteAlbumRepository
        {
            get
            {
                if (_FavouriteAlbumRepository == null)
                {
                    _FavouriteAlbumRepository = new FavouriteAlbumRepository(_notesapartContext);
                }

                return _FavouriteAlbumRepository;
            }
        }

        private IFavouriteSongRepository? _FavouriteSongRepository;

        public IFavouriteSongRepository FavouriteSongRepository
        {
            get
            {
                if (_FavouriteSongRepository == null)
                {
                    _FavouriteSongRepository = new FavouriteSongRepository(_notesapartContext);
                }

                return _FavouriteSongRepository;
            }
        }

        private ILikedReviewRepository? _LikedReviewRepository;

        public ILikedReviewRepository LikedReviewRepository
        {
            get
            {
                if (_LikedReviewRepository == null)
                {
                    _LikedReviewRepository = new LikedReviewRepository(_notesapartContext);
                }

                return _LikedReviewRepository;
            }
        }

        private IUserRepository? _UserRepository;

        public IUserRepository UserRepository
        {
            get
            {
                if (_UserRepository == null)
                {
                    _UserRepository = new UserRepository(_notesapartContext);
                }

                return _UserRepository;
            }
        }





        public RepositoryWrapper(NotesApartDbContext notesapartContext)
        {
            _notesapartContext = notesapartContext;
        }

        public void Save()
        {
            _notesapartContext.SaveChanges();
        }
    }
}
