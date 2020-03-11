using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduardoS_300987998_A3.Models
{
    public interface IFacultyRepository
    {
        /// <summary>
        /// Retreaves a list of faculties and maps the elements asociated to them.
        /// </summary>
        IQueryable<Faculty> Faculties { get;}
        /// <summary>
        /// Add a faculty object to the database FacultyManagement
        /// </summary>
        /// <param name="faculty">
        /// Faculty object that is going to be stored.
        /// </param>
        Task Save(Faculty faculty);
        /// <summary>
        /// Delete one faculty record from the database FacultyManagement. 
        /// Object is identify by its id.
        /// </summary>
        /// <param name="id">
        /// Id of the record to be deleted
        /// </param>
        /// <returns>
        /// Returns a reference to the object deleted from the database.
        /// </returns>
        Task<Faculty> Delete(int id);
        /// <summary>
        /// Method accepts one faculty and one course and end their relationship. 
        /// The record in FacultyCourse table that joins both objects is deleted.
        /// </summary>
        /// <param name="faculty">
        /// Falculty object that contains the course.
        /// </param>
        /// <param name="course">
        /// Course object that is associated to the faculty
        /// </param>
        /// <param name="save">
        /// Optional parameter that allows to manage tha changes. Changes can be saved by the method or can be 
        /// left to be handled by the calling method. It may lead to SQL exception if it not used properly.
        /// </param>
        /// <returns>
        /// boolean confirmation if relationship between faculty and course were end successfully.
        /// </returns>
        Task RemoveCourse(int facultyId, int courseId);
    }
}
