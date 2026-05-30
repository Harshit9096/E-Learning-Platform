using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinalProject.Controllers
{
    public class HomeController : Controller
    { DBManager db = new DBManager();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Batches()
        {
            return View();
        }
        public ActionResult Faculty() 
        {
            return View();
        }
        public ActionResult Success() 
        {
            string query = "select * from tbl_placement order by id desc";
            DataTable placement = db.executeSelect(query);
            ViewBag.placement = placement;

            return View();
        }
        public ActionResult Registration()
        {

            DataTable batches = db.executeSelect("select bid, bname from tbl_batch");
            ViewBag.batches = batches;
            return View();
        }

        [HttpPost]

        public ActionResult Registration(string name,string email,string password , long? mobno,string gender, string college,string course,int? rbatch)
        {
            string query = "insert into tbl_registerstudent values('" + name + "','" + email + "','" + password + "','"+ mobno + "','"+ gender + "','"+ college + "','"+ course + "','"+ rbatch + "','"+DateTime.Now+ "')";
            int result = db.executeInsertUpdateDelete(query);
            if (result > 0)
            {
                return Content("<script>alert(' Added Successfully');location.href='/home/Registration'</script>");

            }
            else
            {
                return Content("<script>alert('Assignment Not Added');location.href='/home/Registration'</script>");

            }
           
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]


        public ActionResult Login(string userid ,string password)
        {
            string query = "select * from tbl_registerstudent where email='" + userid + "' and password='" + password + "'   ";
            DataTable dt = db.executeSelect(query);
            if (dt.Rows.Count > 0)
            {
                Session["semail"] = dt.Rows[0][1] ;
                Session["sname"] = dt.Rows[0][0];
                Session["sbatch"] = dt.Rows[0][7];
                return RedirectToAction("Index" , "student");
            }
            else
            {
                return Content("<script>alert('Invalid id or password');location.href='/home/login'</script>");
            }
        }

        public ActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminLogin(string adminid,string password)
        {
            string query = "select * from tbl_adminlogin where adminid='" + adminid + "' and password='" + password + "'  "; 

            DataTable dt= db.executeSelect(query);
            if (dt.Rows.Count >0 )
            {
                Session["admin"] = adminid;
                return RedirectToAction("index","admin");
            }
            else
            {
                return Content("<script>alert('Id or password invaild');location.href='/home/AdminLogin'</script>");
            }
           
        }
    }
}