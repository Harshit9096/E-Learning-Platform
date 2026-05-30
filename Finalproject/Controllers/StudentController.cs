using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Finalproject.Controllers
{
    [CheckStudentSession]
    public class StudentController : Controller
    {
        DBManager db = new DBManager();
        // GET: Student
        public ActionResult Index()
        {
            string email = Session["semail"].ToString();
            string query = "select * from tbl_registerstudent where email='" + email + "'";
            ViewBag.profile = db.executeSelect(query);

            int batch = Convert.ToInt32(Session["sbatch"]);
            string q1 = "select top 1 * from tbl_classlink where batch=" + batch + " order by lid desc";
            ViewBag.link=db.executeSelect(q1);
            return View();
          
        }
        public ActionResult Assignment()
        {
            if (Session["sbatch"] != null)
            {
                int batch = Convert.ToInt32(Session["sbatch"]);
                string email = Session["semail"].ToString();
                string query = "select task.*,ans.id from tbl_task task left join tbl_submittedtask ans on task.task_id=ans.task_id and ans.email_id='" + email + "' where task.batch_id='" + batch + "' order by id desc";
                DataTable task = db.executeSelect(query);
                ViewBag.task = task;
                return View();
            }
            else
            {
                return Content("<script>alert('Error occured. Please login again');location.href='/home/login'</script>");
            }

        }

        [HttpPost]
        public ActionResult Assignment(int? tid, HttpPostedFileBase tfile)

        {
            string email = Session["semail"].ToString();
            string query = "insert into tbl_submittedtask values('" + tid + "','" + email + "','" + tfile.FileName + "',0,0,'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "')";
            int result = db.executeInsertUpdateDelete(query);
            if (result > 0)
            {
                tfile.SaveAs(Server.MapPath("/content/answerfile/") + tfile.FileName);


                return Content("<script>alert('Your Task submitted');location.href='/student/assignment'</script>");
            }
            else
            {

                return Content("<script>alert('Please try again');location.href='/student/assignment'</script>");
            }
        }

        public ActionResult Lecture()
        {
            string query = "select * from tbl_videocategory order by cid desc";
            ViewBag.vcat = db.executeSelect(query);

            return View();
        }
        public ActionResult Videos(int? cid)
        {
            if (Session["sbatch"] != null && cid.HasValue)
            {
                int batch = Convert.ToInt32(Session["sbatch"]);
                string query = "select * from tbl_video where batch_id=" + batch + " and cat_id=" + cid + "order by vid desc";
                ViewBag.Video = db.executeSelect(query);
                return View();
            }
            else
            {
                return Content("<script>alert('Error occured. Please choose category');location.href='/student/lecture'</script>");
            }
        }

        public ActionResult UploadedAssignment()
        {
            return View();
        }


        public ActionResult MyProfile()
        {
            string email = Session["semail"].ToString();
            string query = "select * from tbl_registerstudent where email='" + email + "'";
            ViewBag.profile = db.executeSelect(query);
            return View();
        }
        [HttpPost]
        public ActionResult MyProfile(string name, long mobno, string college)
        {
            string email = Session["semail"].ToString();
            string query = "update tbl_registerstudent set name='" + name + "', mobileno='" + mobno + "',college='" + college + "' where email='" + email + "'   ";
            int result = db.executeInsertUpdateDelete(query);
            if (result > 0)
            {
                return Content("<script>alert('Profile  Updated. Please try again..');location.href='/student/myprofile'</script>");

            }

            else
            {
                return Content("<script>alert('few error. Please try again..');location.href='/student/myprofile'</script>");

            }
        }

        public ActionResult ChangePassword()
        {

            return View();
        }
        [HttpPost]

        public ActionResult ChangePassword(string opass, string npass, string cnpass)
        {
            if (npass.Equals(cnpass))
            {
                string email = Session["semail"].ToString();
                string query = "update tbl_registerstudent set password='" + npass + "' where email='" + email + "' and password='" + opass + "'";
                int result = db.executeInsertUpdateDelete(query);
                if (result > 0)
                {
                    Session.RemoveAll();
                    return Content("<script>alert('Password updates .Please login again');location.href='/home/login'</script>");
                }
                else
                {
                    return Content("<script>alert('old password not matched.');location.href='/student/ChangePassword'</script>");
                }
            }
            else
            {
                return Content("<script>alert('New Password and Confirm Password not matched');location.href='/student/ChangePassword'</script>");
            }


        }


        public ActionResult Logout()
        {
            Session.RemoveAll();
            return RedirectToAction("login", "home");

        }
    }

    class CheckStudentSession : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["semail"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                {
                    {"Controller","Home"},
                    {"Action","login" }
                });
            }
        }
    }

}