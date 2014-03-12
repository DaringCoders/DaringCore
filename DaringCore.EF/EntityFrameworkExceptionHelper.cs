using System;
using System.Data.Entity.Validation;
using System.Linq;

namespace DaringCore.EF
{
    public class EntityFrameworkExceptionHelper
    {
        /// <summary>
        /// Ussage:
        /// try
        /// {
        ///  db.SaveChanges();
        /// }
        /// catch(DbEntityValidationException ex)
        /// {
        ///     string errors = GetValidationErrors(ex);
        /// }
        /// </summary>
        /// <param name="dbe">DbEntityValidationException</param>
        /// <returns>String of entity validation errors</returns>
        public static String GetValidationErrors(DbEntityValidationException dbe)
        {
            return dbe.EntityValidationErrors.SelectMany(result => result.ValidationErrors)
                      .Aggregate(String.Empty, (current, error) => current + String.Format("{0}: {1}\n", error.PropertyName, error.ErrorMessage));
        }
    }
}
