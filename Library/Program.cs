using Library.BL.Infrastructure;
using Library.BL.Interfaces;
using Ninject;
using Serilog;


using var log = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

log.Information("test");

Injector.Inject();
var kernel = Injector.Kernel;
var authorService = kernel.Get<IAuthorService>();

authorService.Insert(new Library.DAL.DomainModel.Author
{
    FirstName = "Ion",
    LastName = "Creanga"
});