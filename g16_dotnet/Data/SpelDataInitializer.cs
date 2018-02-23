namespace g16_dotnet.Data
{
    public static class SpelDataInitializer
    {

        public static void InitializeData(ApplicationDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }
}
