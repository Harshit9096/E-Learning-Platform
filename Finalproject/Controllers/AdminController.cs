using Antlr.Runtime.Misc;
using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Finalproject.Controllers
{
    [CheckSession]
    public class AdminController : Controller
    {
       


        DBManager db = new DBManager();
        // GET: Admin
   
        public ActionResult Index()
        {
          

            return View();
            }

        
        public ActionResult Category()
        {
            DataTable dt = db.executeSelect("select * from tbl_videocategory order by cid desc");
            ViewBag.Category = dt;



            return View();

        }               

        [HttpPost]
        public ActionResult Category(string catname, HttpPostedFileBase caticon)
        {
            if (catname != null && caticon != null)
            {
                String query = "insert into tbl_videocategory values('" + catname + "' , '" + caticon.FileName + "')";
                int result = db.executeInsertUpdateDelete(query);
                if (result > 0)
                {
                    //server.mapPath()  function is used to get physical location of any folder present at server 
                    caticon.SaveAs(Server.MapPath("/content/caticons/") +  caticon.FileName);
                    return Content("<script>alert('Data Added');location.href='/admin/category'</script>");
                }
                else
                {
                    return Content("<script>alert('Data Added');location.href='/admin/category'</script>");
                }

            }
            else
            {
                return Content("<script>alert('Fill all fields properly');location.href='/admin/category'</script>");
            }
        }
             

        public ActionResult AddBatch()
        {
            DataTable dt = db.executeSelect("select * from tbl_batch order by bid desc");
            ViewBag.Batch = dt;
            return View();
        }
        [HttpPost]
        
        public ActionResult AddBatch(string Batchname,DateTime Batchsdate,DateTime Batchedate,int? Totalfee,string Batchtopic,HttpPostedFileBase batchpic)
        {
            if (Batchname != null && Batchsdate != null && Batchedate != null && Totalfee != null && Batchtopic != null && batchpic != null)
            {
                string query = "insert into tbl_batch values('" + Batchname + "','" + Batchsdate.ToString("yyyy-MM-dd") + "','" + Batchedate.ToString("yyyy-MM-dd hh:mm tt") + "','" + Totalfee + "','" + Batchtopic + "','" + batchpic.FileName + "')";

                int result = db.executeInsertUpdateDelete(query);
                if (result > 0)
                {
                    batchpic.SaveAs(Server.MapPath("/content/batchpic/") + batchpic.FileName);
                    return Content("<script>alert('Batch Added Successfully');location.href='/admin/AddBatch'</script>");
                }
                else
                {
                    return Content("<script>alert('Batch Not Added');location.href='/admin/AddBatch'</script>");

                }
            }

            else
            {
                return Content("<script>alert('Fill all fields properly');location.href='/admin/addbatch'</script>");
            }

        }

       public ActionResult AddAssignment()
        {
            DataTable batches = db.executeSelect("select bid, bname from tbl_batch");
            ViewBag.batches = batches;

            DataTable dt = db.executeSelect("select * from tbl_task order by task_id desc");
            ViewBag.task = dt;
            return View();

             
        }

        [HttpPost]

        public ActionResult AddAssignment(int? cbatch,string title,string desc,HttpPostedFileBase taskpic,string author)
        {

            string query = "insert into tbl_task values('" + cbatch + "','" + title + "','" + desc + "','" + taskpic.FileName + "','"+author+"','"+DateTime.Now.ToString("yyyy-MM-dd hh:mm tt") + "')";
            int result=db.executeInsertUpdateDelete(query);
            if(result > 0)
            {
                taskpic.SaveAs(Server.MapPath("/content/taskpic/") + taskpic.FileName);
                return Content("<script>alert('Assignment Added Successfully');location.href='/admin/AddAssignment'</script>");

            }
            else
            {
                return Content("<script>alert('Assignment Not Added');location.href='/admin/AddAssignment'</script>");

            }

        }

        public ActionResult AddVideo()
        {
            DataTable batches = db.executeSelect("select bid, bname from tbl_batch");
            ViewBag.batches = batches;

            DataTable cat = db.executeSelect("select cid,cname from tbl_videocategory");
            ViewBag.cat = cat;

            DataTable dt = db.executeSelect("select * from tbl_video order by vid desc");
            ViewBag.video = dt;
            return View();

         


             }
        [HttpPost]


        public ActionResult AddVideo(int? vbatch,string videocategory, string videotitle,string videodesc,HttpPostedFileBase videopic)
        {
            string query = "insert into tbl_video values('"+ vbatch + "','"+ videocategory + "','"+ videotitle + "','"+ videodesc + "','"+ videopic.FileName + "','"+ DateTime.Now.ToString("yyyy-MM-dd hh:mm tt")+ "')";

            int result= db.executeInsertUpdateDelete(query);
            if(result > 0)
            {
                videopic.SaveAs(Server.MapPath("/content/videopic/")+ videopic.FileName);
                return Content("<script>alert('Video Added Successfully');location.href='/admin/AddVideo'</script>");
            }
            else
            {
                return Content("<script>alert('Video not Added  Successfully');location.href='/admin/AddVideo'</script>");
            }
         


        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        public ActionResult StudentManagement()
        {
            return View();
        }
        public ActionResult EnquiryManagement()
        {
            return View();
        }

        public ActionResult SubmittedTaskManagement()
        {

            string query = "select s.name,t.title,ans.ans_file, ans.id,ans.obtain_marks,ans.total_marks from tbl_submittedtask ans left join tbl_task t on ans.task_id=t. task_id left join tbl_registerstudent s on s.email=ans.email_id";
          ViewBag.ans=db.executeSelect(query);
            return View();
        }
        [HttpPost]
        public ActionResult marking(int? ansid, int? maxmarks , int? obtainmarks)
        {
            if(ansid.HasValue && maxmarks.HasValue && obtainmarks.HasValue)
            {
                string query = "update tbl_submittedtask set obtain_marks="+obtainmarks+" , total_marks="+ maxmarks + " where id="+ansid +"";
                int result = db.executeInsertUpdateDelete(query);
                if (result > 0)
                {
                    return Content("<script>alert('Marks Updated');location.href='/admin/submittedtaskmanagement'</script>");
                }
                else
                {
                    return Content("<script>alert('Marks not Updated');location.href='/admin/submittedtaskmanagement'</script>");
                }
            }
            else
            {
                return Content("<script>alert('Something going Wrong');location.href='/admin/index'</script>");
            }
        }

        public ActionResult Logout()

        {
            Session.Remove("admin");
            return RedirectToAction("adminlogin", "home");
            //return Content("<script>('Logout Successfully');location.href='/home/AdminLogin'</script>");
        }

        public ActionResult ClassLink()
        {
            DataTable batches = db.executeSelect("select bid, bname from tbl_batch");
            ViewBag.batches = batches;

            DataTable dt = db.executeSelect("select * from tbl_classlink order by lid desc");
            ViewBag.classlink = dt;
            return View();

        
        }

        [HttpPost]
        public ActionResult ClassLink(int? cbatch, string link, DateTime date, string stime, string etime, string message)
          

        {
            string query = "insert into tbl_classlink values('"+ cbatch + "','"+ link + "','"+ date.ToString("yyyy-MM-dd") + "','"+ stime + "','"+ etime + "','"+ message + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm tt") + "')";

            int result = db.executeInsertUpdateDelete(query);
            if (result > 0)
            {
               
                return Content("<script>alert('Link Added Successfully');location.href='/admin/classlink'</script>");
            }
            else
            {
                return Content("<script>alert('Link not Added  Successfully');location.href='/admin/classlink'</script>");
            }

           
        }

        public ActionResult SuccessStory()
        {
            DataTable dt = db.executeSelect("select * from tbl_placement order by id desc");
            ViewBag.successstory = dt;
            return View();


        }

        [HttpPost]
        public ActionResult SuccessStory(string name,string college,string course,string company,string post, string field, string package, HttpPostedFileBase logo, HttpPostedFileBase spic)
        {
            string query = "insert into tbl_placement values('"+ name + "','"+ college + "','"+ course + "','"+ company + "','"+ post + "','"+ field + "','"+ package + "','"+ logo.FileName + "','"+ spic.FileName + "','"+ DateTime.Now.ToString("yyyy-MM-dd hh:mm tt") + "')";

            int result = db.executeInsertUpdateDelete(query);
         
            if (result > 0)
            {
                logo.SaveAs(Server.MapPath("/content/logo/") + logo.FileName);
                spic.SaveAs(Server.MapPath("/content/spic/") + spic.FileName);
                return Content("<script>alert('Data Added Successfully');location.href='/admin/successstory'</script>");
            }
            else
            {
                return Content("<script>alert('Data not Added');location.href='/admin/successstory'</script>");
            }
            
        }

    }


    //let's create a action filter to check session, if session is not active redirect to the adminlogin

    class CheckSession : ActionFilterAttribute
    {
      public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
           
        }

       public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
           if( filterContext.HttpContext.Session["admin"]==null)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                {
                    {"Controller","Home"},
                    {"Action","adminlogin" }
                });
            }
        }
    }
}