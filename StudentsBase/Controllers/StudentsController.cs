using StudentsBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StudentsBase.Controllers
{
    public class StudentsController : ApiController
    {
        public IHttpActionResult GetStudents() 
        {
            using (StudentsDBEntities db = new StudentsDBEntities())
            {
                var students = db.Students.ToList();
                if (students == null) 
                {
                    return NotFound();
                }
                return Ok(students);
            }
        }

        public HttpResponseMessage GetStudent(int id) 
        {
            using (StudentsDBEntities db = new StudentsDBEntities())
            {
                var student = db.Students.FirstOrDefault(s => s.Id == id);

                if (student != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, student);
                }
                else 
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Student with Id " + id + " not found.");
                }
            }
        }

        public HttpResponseMessage Post([FromBody]Student student) 
        {
            try
            {
                using (StudentsDBEntities db = new StudentsDBEntities())
                {
                    db.Students.Add(student);
                    db.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, student);
                    message.Headers.Location = new Uri(Request.RequestUri + student.Id.ToString());
                    return message;
                }
            }
            catch (Exception ex) 
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        public HttpResponseMessage Delete(int id) 
        {
            try
            {
                using (StudentsDBEntities db = new StudentsDBEntities())
                {
                    var student = db.Students.FirstOrDefault(s => s.Id == id);

                    if (student == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Student with Id " + id + " not found to delete.");
                    }
                    else
                    {
                        db.Students.Remove(student);
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex) 
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Put(int id, [FromBody]Student student) 
        {
            try
            {
                using (StudentsDBEntities db = new StudentsDBEntities())
                {
                    var _student = db.Students.FirstOrDefault(s => s.Id == id);

                    if (_student == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Student with Id " + id.ToString() + " not found to edit.");
                    }
                    else
                    {
                        _student.FirstName = student.FirstName;
                        _student.LastName = student.LastName;
                        _student.Birthday = student.Birthday;

                        db.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, _student);
                    }
                }
            }
            catch (Exception ex) 
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
