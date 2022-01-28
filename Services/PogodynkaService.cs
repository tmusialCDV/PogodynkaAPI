namespace PogodynkaAPI.Services
{
    public interface IPogodynkaService
    {
        Pogodynka Create(CreatePogodynkaDto dto);
        void Delete(int id);
        List<PogodynkaDto> GetAll();
    }

    public class PogodynkaService : IPogodynkaService
    {
        private readonly PogodynkaDbContext dbContext;
        private readonly IUserContextService userContextService;

        public PogodynkaService(PogodynkaDbContext dbContext, IUserContextService userContextService)
        {
            this.dbContext = dbContext;
            this.userContextService = userContextService;
        }

        public List<PogodynkaDto> GetAll()
        {
            var data = dbContext.PogodynkaData.Where(p => p.CreatedById == userContextService.GetUserId);
            var mappedData = data.Select(r => new PogodynkaDto()
            {
                Id = r.Id,
                TempC = r.TempC,
                TempF = r.TempF,
                DateTime = r.DateTime,
                Description = r.Description,
                ImageData = r.ImageData
            }).OrderByDescending(r => r.DateTime).ToList();

            return mappedData;
        }

        public Pogodynka Create(CreatePogodynkaDto dto)
        {
            var data = new Pogodynka();

            if(dto.TempC == null) data.TempC = (int)((dto.TempF - 32) * 0.55);
            else data.TempC = (int)dto.TempC;


            if (dto.TempF == null) data.TempF = (int)((dto.TempC * 1.8) + 32);
            else data.TempF = (int)dto.TempF;
            
            data.DateTime = DateTime.Now;
            data.Description = dto.Description;
            data.ImageData = dto.ImageData;
            data.CreatedById = userContextService.GetUserId;

            dbContext.PogodynkaData.Add(data);
            dbContext.SaveChanges();

            return data;
        }

        public void Delete(int id)
        {
            var data = dbContext.PogodynkaData.FirstOrDefault(r => r.Id == id);
            if (data is null) throw new NotFoundException("Data not found");
            if (data.CreatedById != userContextService.GetUserId) throw new ForbidException();

            dbContext.PogodynkaData.Remove(data);
            dbContext.SaveChanges();
        }
    }
}
