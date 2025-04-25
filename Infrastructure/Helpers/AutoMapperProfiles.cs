using AutoMapper;
using Core.DTOs;
using Core.Entities;

namespace Infrastructure.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Author, AuthorDto>().ForMember(dest => dest.DateOfBirthStr, opt => opt.MapFrom(src => FindShortDateString(src.DateOfBirth)));

            CreateMap<Book, BookWithAuthorsDto>()
                    .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Title))
                    .ForMember(dest => dest.BookGenre, opt => opt.MapFrom(src => src.Genre))
                    .ForMember(dest => dest.BookGenreStr, opt => opt.MapFrom(src => src.Genre.ToString()))
                    .ForMember(dest => dest.BookPublishedDate, opt => opt.MapFrom(src => src.PublishedDate))
                    .ForMember(dest => dest.BookPublishedDateStr, opt => opt.MapFrom(src => FindShortDateString(src.PublishedDate)))
                    .ForMember(dest => dest.BookPictureUrl, opt => opt.MapFrom(src => src.PictureUrl))
                    .ForMember(dest => dest.AuthorList, opt => opt.MapFrom(src => src.Authors));

            CreateMap<BookSaveDto, Book>();
            CreateMap<Book, BookSaveDto>().ForMember(dest => dest.AuthorIds, opt => opt.MapFrom(src => src.Authors.Select(x => x.Id)));

            CreateMap<AppUser, UserInfoDto>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
        }

       

        private string FindShortDateString(DateTime dateOfBirth)
        {
            try
            {
                return dateOfBirth.ToShortDateString();
            }
            catch (Exception)
            {

                return string.Empty;
            }
        }
    }
}
