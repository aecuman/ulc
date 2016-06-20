using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using DHTMLX.Scheduler;
using DHTMLX.Common;
using DHTMLX.Scheduler.Data;
using DHTMLX.Scheduler.Controls;

using ULC.Models;
namespace ULC.Controllers
{
    public class CalendarController : Controller
    {
        public ActionResult Index()
        {
            //Being initialized in that way, scheduler will use CalendarController.Data as a the datasource and CalendarController.Save to process changes
            var scheduler = new DHXScheduler(this);
            scheduler.BeforeInit.Add(string.Format("initResponsive({0})", scheduler.Name));

            scheduler.InitialDate = DateTime.Now;
            scheduler.InitialView = "day";

            scheduler.LoadData = true;
            scheduler.EnableDataprocessor = true;
            
            //
           

            return View(scheduler);
        }

        public ContentResult Data()
        {
            return new SchedulerAjaxData(new ULCSchedContainer().Activities);
        }

        public ActionResult Save(Activity updatedEvent, FormCollection formData)
        {
            var action = new DataAction(formData);
            var context = new ULCSchedContainer();

            try
            {
                switch (action.Type)
                {
                    case DataActionTypes.Insert: // your Insert logic
                        context.Activities.Add(updatedEvent);
                        break;
                    case DataActionTypes.Delete: // your Delete logic
                        updatedEvent = context.Activities.SingleOrDefault(ev => ev.Id == updatedEvent.Id);
                        context.Activities.Remove(updatedEvent);
                        break;
                    default:// "update" // your Update logic
                        updatedEvent = context.Activities.SingleOrDefault(
                        ev => ev.Id == updatedEvent.Id);
                        UpdateModel(updatedEvent);
                        break;
                }
                context.SaveChanges();
                action.TargetId = updatedEvent.Id;
            }
            catch (Exception a)
            {
                action.Type = DataActionTypes.Error;
            }
            return (new AjaxSaveResponse(action));
        }
    }
}