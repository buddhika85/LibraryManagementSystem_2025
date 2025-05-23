﻿using Core.Enums;

namespace Core.DTOs
{
    public class BookWithAuthorsDto
    {
        // Book
        public int BookId { get; set; }
        public required string BookTitle { get; set; }
        public required BookGenre BookGenre { get; set; }
        public required string BookGenreStr { get; set; }      
        public required DateTime BookPublishedDate { get; set; }
        public string BookPublishedDateStr { get; set; } = string.Empty;
        public required string BookPictureUrl { get; set; }

        public bool IsAvailable { get; set; }

        // authors
        public IReadOnlyList<AuthorDto> AuthorList { get; set; } = [];

        public string AuthorListStr => string.Join(", ", AuthorList.Select(x => x.Name));
    }
}
