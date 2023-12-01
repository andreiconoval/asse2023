using FluentValidation;
using FluentValidation.Results;
using Library.BL.Interfaces;
using Library.DAL.Interfaces;

namespace Library.BL.Services
{
    public abstract class BaseService<T, U> : IService<T>
         where T : class
         where U : IRepository<T>
    {
        protected U _repository;
        protected IValidator<T> _validator;

        /// <summary>
        /// Ctor for base service
        /// </summary>
        /// <param name="repository">The actual repository that will fit into this class</param>
        /// <param name="validator">The fluent validation validator</param>
        public BaseService(U repository, IValidator<T> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        /// <summary>Inserts the specified entity.</summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        ///   The validation result.
        /// </returns>
        public ValidationResult Insert(T entity)
        {
            var result = _validator.Validate(entity);
            if (result.IsValid)
            {
                _repository.Insert(entity);
            }

            return result;
        }

        public ValidationResult Update(T entity)
        {
            var result = _validator.Validate(entity);
            if (result.IsValid)
            {
                _repository.Update(entity);
            }

            return result;
        }

        /// <summary>Deletes the specified entity.</summary>
        /// <param name="entity">The entity to be deleted.</param>
        public void Delete(T entity)
        {
            _repository.Delete(entity);
        }

        public T GetByID(object id)
        {
            return _repository.GetByID(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _repository.Get();
        }

        ValidationResult IService<T>.Insert(T entity)
        {
            throw new NotImplementedException();
        }

        ValidationResult IService<T>.Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
