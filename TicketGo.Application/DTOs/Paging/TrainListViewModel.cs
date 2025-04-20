using TicketGo.Domain.Entities;

namespace TicketGo.Application.DTOs.Paging
{
    public class TrainListViewModel
    {
        public IEnumerable<Train> Trains { get; set; } = new List<Train>();
        public IEnumerable<TrainRoute> TrainRoutes { get; set; } = new List<TrainRoute>();
        public IEnumerable<Coach> coaches { get; set; } = new List<Coach>();

        public PagingSearch PagingInfo { get; set; } = new PagingSearch();


        //public List<string> StartPoints {  get; set; }
        //public List<string> EndPoints { get; set; }
    }
}
