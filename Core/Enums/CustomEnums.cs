namespace Core.Enums
{
    public enum BookGenre
    {
        None = 0,
        Fiction = 1,
        NonFiction,
        Drama,
        Adventure,
        Horror,
        Fantasy,
        ScienceFiction,
        Mystery,
        Historical,
        Poetry
    }

    public enum UserRoles
    {
        Admin = 100,
        Staff = 1,
        Member = 2
    }

    public enum BorrowalStatus
    {
        Available = 1,
        Out = 2,
        Returned = 3
    }
}
