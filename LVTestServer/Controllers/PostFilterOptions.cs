using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace LVTestServer.Controllers
{
    
    public class PostFilterOptions
    {
        public DateTime? DateMin { get; set; }
        public DateTime DateMax { get; set; } = DateTime.Now;
        internal bool IsValid => DateMin is null || DateMin < DateMax;
        public bool SortDescending { get; set; }
    }
}