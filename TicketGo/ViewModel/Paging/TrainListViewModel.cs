using System.Collections.Generic;
using DO_AN.Models; // Đổi namespace thành tên của bạn

namespace DO_AN.ViewModel.Paging
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
