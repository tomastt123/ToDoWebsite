namespace ToDoAppASP.Models
{
    public class Filters
    {
        public Filters(string filterstring)
        {
            FilterString = filterstring ?? "all-all-all-all";
            string[] filters = FilterString.Split('-');

            if (filters.Length >= 4)
            {
                CategoryId = filters[0];
                Due = filters[1];
                StatusId = filters[2];
                Priority = filters[3];
            }
            else
            {
                CategoryId = "all";
                Due = "all";
                StatusId = "all";
                Priority = "all";
            }
        }

        public string FilterString { get; }

        public string CategoryId { get; }
        public string Due { get; }
        public string StatusId { get; }
        public string Priority { get; }

        public bool HasCategory => CategoryId.ToLower() != "all";
        public bool HasDue => Due.ToLower() != "all";
        public bool HasStatus => StatusId.ToLower() != "all";
        public bool HasPriority => Priority.ToLower() != "all";

        public static Dictionary<string, string> DueFilterValues =>
            new Dictionary<string, string>
            {
                {"future", "Future" },
                {"past", "Past" },
                {"today", "Today" }
            };

        public bool IsPast => Due.ToLower() == "past";
        public bool IsFuture => Due.ToLower() == "future";
        public bool IsToday => Due.ToLower() == "today";

        public ToDo.Priority? PriorityEnum
        {
            get
            {
                if (Enum.TryParse<ToDo.Priority>(Priority, true, out var priority))
                {
                    return priority;
                }
                return null;
            }
        }
    }
}
