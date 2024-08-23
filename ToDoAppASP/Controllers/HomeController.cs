using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoAppASP.Models;
using System.Linq;
using System;

namespace ToDoAppASP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ToDoContext context;

        public HomeController(ToDoContext ctx) => context = ctx;

        public IActionResult Index(string id)
        {
            var filters = new Filters(id);
            ViewBag.Filters = filters;

            ViewBag.Categories = context.Categories.ToList();
            ViewBag.Statuses = context.Statuses.ToList();
            ViewBag.DueFilters = Filters.DueFilterValues;
            ViewBag.Priorities = Enum.GetValues(typeof(ToDo.Priority))
                                     .Cast<ToDo.Priority>()
                                     .Select(p => new { Value = p.ToString(), Text = p.ToString() });

            IQueryable<ToDo> query = context.ToDos
                .Include(t => t.Category)
                .Include(t => t.Status);

            if (filters.HasCategory)
            {
                query = query.Where(t => t.CategoryId == filters.CategoryId);
            }

            if (filters.HasStatus)
            {
                query = query.Where(t => t.StatusId == filters.StatusId);
            }

            if (filters.HasDue)
            {
                var today = DateTime.Today;
                if (filters.IsPast)
                {
                    query = query.Where(t => t.DueDate < today);
                }
                else if (filters.IsFuture)
                {
                    query = query.Where(t => t.DueDate > today);
                }
                else if (filters.IsToday)
                {
                    query = query.Where(t => t.DueDate == today);
                }
            }

            if (filters.HasPriority)
            {
                if (Enum.TryParse<ToDo.Priority>(filters.Priority, out var priority))
                {
                    query = query.Where(t => t.TaskPriority == priority);
                }
            }

            var tasks = query.OrderBy(t => t.DueDate).ToList();
            return View(tasks);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Categories = context.Categories.ToList();
            ViewBag.Statuses = context.Statuses.ToList();
            ViewBag.Priorities = Enum.GetValues(typeof(ToDo.Priority))
                                     .Cast<ToDo.Priority>()
                                     .Select(p => new { Value = p.ToString(), Text = p.ToString() });

            var task = new ToDo { StatusId = "open" };
            return View(task);
        }

        [HttpPost]
        public IActionResult Add(ToDo task)
        {
            if (ModelState.IsValid)
            {
                if (!task.DueDate.HasValue)
                {
                    task.DueDate = DateTime.Today; 
                }

                var calculatedDueDate = CalculateNextDueDate(task);

                if (calculatedDueDate.HasValue)
                {
                    task.DueDate = calculatedDueDate.Value;
                }

                context.ToDos.Add(task);
                context.SaveChanges();

                if (task.RecurrenceFrequency != null)
                {
                    CreateRecurringTasks(task);
                }

                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Categories = context.Categories.ToList();
                ViewBag.Statuses = context.Statuses.ToList();
                ViewBag.Priorities = Enum.GetValues(typeof(ToDo.Priority))
                                         .Cast<ToDo.Priority>()
                                         .Select(p => new { Value = p.ToString(), Text = p.ToString() });

                return View(task);
            }
        }

        [HttpPost]
        public IActionResult Filter(string categoryFilter, string dueFilter, string statusFilter, string priorityFilter)
        {
            string filterString = $"{categoryFilter}-{dueFilter}-{statusFilter}-{priorityFilter}";
            return RedirectToAction("Index", new { id = filterString });
        }

        [HttpPost]
        public IActionResult MarkComplete([FromRoute] string id, ToDo selected)
        {
            selected = context.ToDos.Find(selected.Id);

            if (selected != null)
            {
                selected.StatusId = "done";
                context.SaveChanges();
            }
            return RedirectToAction("Index", new { id = id });
        }

        [HttpPost]
        public IActionResult DeleteComplete(string id)
        {
            var toDelete = context.ToDos.Where(t => t.StatusId == "done").ToList();

            foreach (var task in toDelete)
            {
                context.ToDos.Remove(task);
            }
            context.SaveChanges();

            return RedirectToAction("Index", new { id = id });
        }

        private DateTime? CalculateNextDueDate(ToDo task)
        {
            if (task.RecurrenceFrequency == null || !task.DueDate.HasValue)
                return null;

            DateTime nextDueDate = task.DueDate.Value;

            switch (task.RecurrenceFrequency.ToLower())
            {
                case "daily":
                    nextDueDate = nextDueDate.AddDays(task.RecurrenceInterval ?? 1);
                    break;
                case "weekly":
                    nextDueDate = nextDueDate.AddDays(7 * (task.RecurrenceInterval ?? 1));
                    break;
                case "monthly":
                    nextDueDate = nextDueDate.AddMonths(task.RecurrenceInterval ?? 1);
                    break;
                default:
                    break;
            }

            if (task.RecurrenceEndDate.HasValue && nextDueDate > task.RecurrenceEndDate.Value)
                return null;

            return nextDueDate;
        }

        private void CreateRecurringTasks(ToDo task)
        {
            DateTime? nextDueDate = CalculateNextDueDate(task);

            while (nextDueDate.HasValue && (!task.RecurrenceEndDate.HasValue || nextDueDate <= task.RecurrenceEndDate))
            {
                var newTask = new ToDo
                {
                    Description = task.Description,
                    DueDate = nextDueDate,
                    CategoryId = task.CategoryId,
                    StatusId = task.StatusId,
                    TaskPriority = task.TaskPriority,
                    RecurrenceFrequency = task.RecurrenceFrequency,
                    RecurrenceInterval = task.RecurrenceInterval,
                    RecurrenceEndDate = task.RecurrenceEndDate
                };

                context.ToDos.Add(newTask);
                context.SaveChanges();

                nextDueDate = CalculateNextDueDate(newTask);
            }
        }
    }
}
