using FluentValidation;
using FluentValidation.Results;
using Library.BL.Interfaces;
using Library.DAL.Interfaces;
using Microsoft.Extensions.Logging;

namespace Library.BL.Services
{
    public abstract class BaseService<T, U> : IService<T>
         where T : class
         where U : IRepository<T>
    {
        protected U _repository;
        protected IValidator<T> _validator;
        protected ILogger _logger;

        /// <summary>
        /// Ctor for base service
        /// </summary>
        /// <param name="repository">The actual repository that will fit into this class</param>
        /// <param name="validator">The fluent validation validator</param>
        public BaseService(U repository, IValidator<T> validator, ILogger logger)
        {
            _repository = repository;
            _validator = validator;
            _logger = logger;
        }

        /// <summary>Inserts the specified entity.</summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        ///   The validation result.
        /// </returns>
        public virtual ValidationResult Insert(T entity)
        {
            try
            {
                var result = _validator.Validate(entity);
                if (result.IsValid)
                {
                    _repository.Insert(entity);
                    _logger.LogInformation($"Add new {nameof(T)}");
                }
                else
                {
                    _logger.LogInformation($"Cannot add new {nameof(T)}, entity is invalid");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Log on error add");
                throw;
            }

        }

        public virtual ValidationResult Update(T entity)
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
        public virtual void Delete(T entity)
        {
            _repository.Delete(entity);
        }

        /// <summary>
        /// Get entity based on Id
        /// </summary>
        /// <param name="id"> Id of entity</param>
        /// <returns></returns>
        public virtual T GetByID(object id)
        {
            return _repository.GetByID(id);
        }

        /// <summary>
        /// Get all entities
        /// </summary>
        /// <returns>Return list of entities</returns>
        public virtual IEnumerable<T> GetAll()
        {
            return _repository.Get();
        }
    }
}
