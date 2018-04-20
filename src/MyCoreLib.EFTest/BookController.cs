using EF.Data;
using EF.Entity;
using EF.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EF.Web.Controllers
{
    public class BookController : Controller
    {
        private EFDbContext db;
        public BookController()
        {
            db = new EFDbContext();
        }
        #region 列表
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(db.Set<Book>().ToList());
        }
        #endregion

        #region AddBook
        /// <summary>
        /// 添加Book
        /// </summary>
        /// <returns></returns>
        public ActionResult AddBook()
        {
            PublisherModel model = new PublisherModel();
            List<Publisher> listPublisher = db.Set<Publisher>().ToList();

            foreach (var item in listPublisher)
            {
                model.PublisherList.Add(new SelectListItem()
                {
                    Text = item.PublisherName,
                    Value = item.ID.ToString()

                });
            }

            ViewBag.PublishedList = model.PublisherList;


            return View();
        }

        /// <summary>
        /// 添加Book
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddBook([Bind(Include = "BookName,BookAuthor,BookPrice,AddedDate,ModifiedDate,PublisherId")] Book model)
        {
            Book addBook = new Book()
            {
                ID = GuidHelper.GetGuid(),
                AddedDate = model.AddedDate,
                BookAuthor = model.BookAuthor,
                BookName = model.BookName,
                BookPrice = model.BookPrice,
                ModifiedDate = model.ModifiedDate,
                PublisherId = Request["PublishedName"] != null ? new Guid(Request["PublishedName"].ToString()) : Guid.Empty
                //这里因为出版商我用的是另外的Model，视图中使用模型绑定只能用一个Model，所以修改和添加只能这样搞了。

            };
            db.Entry(addBook).State = EntityState.Added;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

        #region UpdateBook
        /// <summary>
        /// 修改Book
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public ActionResult UpdateBook(Guid bookId)
        {
            PublisherModel model = new PublisherModel();
            List<Publisher> listPublisher = db.Set<Publisher>().ToList();

            foreach (var item in listPublisher)
            {
                model.PublisherList.Add(new SelectListItem()
                {
                    Text = item.PublisherName,
                    Value = item.ID.ToString()

                });
            }

            ViewBag.PublishedList = model.PublisherList;



            Book bookModel = db.Set<Book>().Where(s => s.ID == bookId).FirstOrDefault();
            return View(bookModel);
        }

        /// <summary>
        /// 修改Book
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateBook([Bind(Include = "ID,BookName,BookAuthor,BookPrice,AddedDate,ModifiedDate,PublisherId")] Book model)  //注意这里一定别忘记绑定 ID列哦
        {
            Book bookModel = db.Set<Book>().Where(s => s.ID == model.ID).FirstOrDefault();

            if (bookModel != null)
            {
                Book updatemodel = new Book()
                {
                    AddedDate = model.AddedDate,
                    BookAuthor = model.BookAuthor,
                    ID = model.ID,
                    ModifiedDate = model.ModifiedDate,
                    BookName = model.BookName,
                    BookPrice = model.BookPrice,
                    PublisherId = new Guid(Request["PublishedName"].ToString())//这里因为出版商我用的是另外的Model，视图中使用模型绑定只能用一个Model，所以修改和添加只能这样搞了。
                };
                db.Entry(bookModel).CurrentValues.SetValues(updatemodel);  //保存的另外一种方式
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }

            #region 保存的方式二
            //db.Entry(model).State = EntityState.Modified;
            //db.SaveChanges();
            //return RedirectToAction("Index"); 
            #endregion
        }
        #endregion

        #region DeleteBook
        public ActionResult DeleteBook(Guid bookId)
        {
            Book model = db.Set<Book>().Where(s => s.ID == bookId).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteBook(Guid bookId, FormCollection form)
        {
            Book model = db.Set<Book>().Where(s => s.ID == bookId).FirstOrDefault();
            db.Entry(model).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

    }
}
